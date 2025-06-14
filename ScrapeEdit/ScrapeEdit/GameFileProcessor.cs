using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ScrapeEdit
{
    public static class GameFileProcessor
    {
        private static string seRomDir;
        private static ScreenScraperApi ssa;
        private static Dictionary<string, string> ValidEXT;

        public static bool SetValuesFirst(string _seRomDir, ScreenScraperApi _ssa, Dictionary<string, string> _validExt)
        {
            if (string.IsNullOrEmpty(_seRomDir))
            {
                MessageBox.Show("Please select a location for the ScrapeEdit Cache", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            seRomDir = _seRomDir;
            ssa = _ssa;
            ValidEXT = _validExt;
            Directory.CreateDirectory(_seRomDir);
            return true;
        }

        public static async Task<TreeNodeDetail> ProcessSingleGameFileAsync(
            TreeNodeDetail node,
            Action<int, string> report,
            Action<string> onRenamed,
            CancellationToken cancellationToken
        )
        {
            try
            {
                report(0, "Starting scrape...");
                cancellationToken.ThrowIfCancellationRequested();

                bool downloadMedia = true;

                //ScraperData testData = ScraperXmlParser.Deserialize(GetXmlDataForNode(node, ref downloadMedia));

                string xmlData = GetXmlDataForNode(node, ref downloadMedia);
                if (xmlData == null)
                {
                    report(100, "Scrape failed");
                    return null;
                }

                report(10, "Parsing metadata...");
                cancellationToken.ThrowIfCancellationRequested();

                // Ensure romfilename and path are set based on rules (hash matching, fallback, etc.)
                xmlData = ProcessNameChange(node, xmlData);
                report(25, "Parsing metadata... Finished!");

                // Determine if a rename is needed
                string currentName = node.FileName + node.FileExtension;
                string scrapedName = GetRomNameFromXML(new XmlDocument { InnerXml = xmlData });

                bool renameNeeded = ScrapeSettings.RenameRoms &&
                                    !string.IsNullOrWhiteSpace(scrapedName) &&
                                    !string.Equals(currentName, scrapedName, StringComparison.OrdinalIgnoreCase);

                string updatedXml = xmlData;
                string originalFilename = currentName;

                if (renameNeeded)
                {
                    try
                    {
                        originalFilename = RenameRomFileAndUpdateXml(node, xmlData, out updatedXml);

                        if (onRenamed != null)
                            onRenamed(node.Tag_FullPath);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"Cannot rename ROM: {ex.Message}", "Rename Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }
                }

                cancellationToken.ThrowIfCancellationRequested();
                report(60, "Finalizing and downloading…");
                var completedNode = await FinalizeGameMediaUpdate(node, updatedXml, renameNeeded ? originalFilename : null, cancellationToken);

                report(100, "Scrape completed successfully!");
                Thread.Sleep(500); // Allow UI to update before returning

                return completedNode;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (Exception ex)
            {
                report(100, $"Error: {ex.Message}");
                return null;
            }
        }

        public static string GetCacheDir(TreeNodeDetail node)
        {
            // Determine cache directory based on console and optional subdirectory
            string cacheDir = string.IsNullOrEmpty(node.Tag_SubDirPath)
                ? Path.Combine(seRomDir, node.Tag_ConsoleName)
                : Path.Combine(seRomDir, node.Tag_ConsoleName, node.Tag_SubDirPath);
            Directory.CreateDirectory(cacheDir);
            return cacheDir;
        }

        private static string GetXmlDataForNode(
    TreeNodeDetail node,
    //ref bool useHash,
    ref bool downloadMedia)
        {
            //string originalName = node.FileNameFull;
            string originalCachePath = Path.Combine(GetCacheDir(node), node.FileNameFull + ".xml");

            string xmlData = null;

            // 2) Load from cache if it exists and is younger than 30 days
            if (ScrapeSettings._30Day &&
                File.Exists(originalCachePath) &&
                AgeOfCachedData(originalCachePath) <= 30)
            {
                xmlData = File.ReadAllText(originalCachePath);
                downloadMedia = false;
            }
            else
            {
                // 3) Attempt fresh scrape using filename-based lookup
                xmlData = ssa.GenerateXmlData(node);
            }

            //deleted 4

            // 5) If clarity is enabled and no data was fetched, prompt user for manual title
            if ((xmlData == "400" || xmlData == "404") && ScrapeSettings.AskForClarity)
            {
                string title = node.FileName;
                string ConsoleID = node.Tag_ConsoleID;

                var result = PromptForGameInfo(node);
                if (result != null)
                {
                    title = result.Value.title;
                    ConsoleID = result.Value.consoleID;
                    // retry logic with new values
                }

                //string title = PromptForGameTitle(node.FileName);

                if (string.IsNullOrWhiteSpace(title)) return null;

                xmlData = ssa.GenerateXmlData(
                    node,
                    title,
                    ConsoleID,
                    true
                );
                if (xmlData == null) return null;
            }

            return xmlData;
        }


        public static string ProcessNameChange(TreeNodeDetail node, string xmlData)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmlData);

            string originalName = node.FileNameFull;
            string originalCachePath = Path.Combine(GetCacheDir(node), node.FileNameFull + ".xml");
            bool useHash = ScrapeSettings.RenameRoms;
            string scrapedName = GetRomNameFromXML(doc);
            string hash = node.Tag_Hash;

            if (useHash && !string.IsNullOrWhiteSpace(hash))
            {
                string hashType = Hash.ReturnHashType(hash); // Already returns "romcrc", "rommd5", or "romsha1"

                var matchedRom = doc.SelectNodes("//jeu/roms/rom")
                    .Cast<XmlNode>()
                    .FirstOrDefault(r =>
                        string.Equals(r.Attributes[hashType]?.Value, hash, StringComparison.OrdinalIgnoreCase));

                if (matchedRom != null)
                {
                    string matchedRomName = matchedRom.Attributes["romfilename"]?.Value?.Trim();
                    if (!string.IsNullOrEmpty(matchedRomName))
                    {
                        scrapedName = matchedRomName;
                        node.Tag_HashMatchFound = true;
                    }
                }
                else
                {
                    scrapedName = originalName;
                }
            }

            xmlData = SetRomNameInXML(doc, originalName, scrapedName);

            using (var writer = XmlWriter.Create(originalCachePath, new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineOnAttributes = false,
                Encoding = Encoding.UTF8
            }))
            {
                doc.Save(writer);
            }

            return xmlData;
        }

        private static string SetRomNameInXML(XmlDocument doc, string originalName,string scrapedName)
        {
            // 6) Update the <romfilename> and <path> nodes in the XML
            XmlNode romNode = doc.SelectSingleNode("//jeu/rom/romfilename");
            if (romNode != null)
                romNode.InnerText = scrapedName;
            XmlNode pathNode = doc.SelectSingleNode("//jeu/path");
            if (pathNode != null)
                pathNode.InnerText = "./" + scrapedName;
            // 7) Return the modified XML as a string
            return doc.OuterXml;
        }

        private static string GetRomNameFromXML(XmlDocument doc)
        {

            // 7) Extract the initially proposed ROM name from the response
            XmlNode romNode = doc.SelectSingleNode("//jeu/rom/romfilename");
            string scrapedName = romNode?.InnerText?.Trim();
            return scrapedName;
        }
       
        private static string RenameRomFileAndUpdateXml(TreeNodeDetail node, string xmlData, out string updatedXml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmlData);

            string newFilename = doc.SelectSingleNode("//jeu/rom/romfilename")?.InnerText?.Trim();
            if (string.IsNullOrEmpty(newFilename))
            {
                string fileHash = node.Tag_Hash;
                string fileHashType = Hash.ReturnHashType(fileHash);

                var match = doc.SelectNodes("//jeu/roms/rom")
                    .Cast<XmlNode>()
                    .FirstOrDefault(r => string.Equals(r.Attributes[fileHashType]?.Value, fileHash, StringComparison.OrdinalIgnoreCase));
                newFilename = match?.Attributes["romfilename"]?.Value?.Trim();
            }

            string originalFilename = node.FileName + node.FileExtension;
            if (string.IsNullOrEmpty(newFilename))
                newFilename = originalFilename;

            XmlNode romNode = doc.SelectSingleNode("//jeu/rom/romfilename");
            if (romNode != null)
            {
                romNode.InnerText = newFilename;
            }
            else
            {
                var romContainer = doc.SelectSingleNode("//jeu/rom") ?? doc.CreateElement("rom");
                if (romContainer.ParentNode == null)
                    doc.SelectSingleNode("//jeu")?.AppendChild(romContainer);
                var newRomFileNode = doc.CreateElement("romfilename");
                newRomFileNode.InnerText = newFilename;
                romContainer.AppendChild(newRomFileNode);
            }

            XmlNode pathNode = doc.SelectSingleNode("//jeu/path");
            if (pathNode != null)
            {
                pathNode.InnerText = "./" + newFilename;
            }

            string newFullPath = Path.Combine(node.Tag_ConsolePath, newFilename);
            if (!string.Equals(node.Tag_FullPath, newFullPath, StringComparison.OrdinalIgnoreCase))
            {
                if (File.Exists(newFullPath))
                    throw new IOException($"A file named '{newFilename}' already exists.");
                File.Move(node.Tag_FullPath, newFullPath);
                node.Tag_FullPath = newFullPath;
            }

            updatedXml = doc.OuterXml;
            return originalFilename;
        }

        private static async Task<TreeNodeDetail> FinalizeGameMediaUpdate(
            TreeNodeDetail node,
            string xmlData,
            string oldFilename,
            CancellationToken ct
        )
        {
            string relativeXml = node.Tag_ConsoleRomPath.TrimStart('\\', '/') + ".xml";
            string xmlOutputPath = Path.Combine(seRomDir, node.Tag_ConsoleName, relativeXml);
            Directory.CreateDirectory(Path.GetDirectoryName(xmlOutputPath));

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData);
            using (var writer = XmlWriter.Create(xmlOutputPath, new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineOnAttributes = false,
                Encoding = Encoding.UTF8
            }))
            {
                doc.Save(writer);
            }

            //File.WriteAllText(xmlOutputPath, xmlData);

            await MediaDownloader.DownloadMediaAsync(
                node,
                seRomDir,
                xmlData,
                DownloadSettings.DownloadFilesTypes(),
                ct,
                oldFilename
            );

            GameListManager.MoveMediaToLocalDir(node, seRomDir);
            node.Game = GameListManager.ConvertSSXmlToScrapedGame(xmlData, node);
            GameListManager.UpdateGameListEntry(node);
            return node;
        }

        private static (string title, string consoleID)? PromptForGameInfo(TreeNodeDetail node)
        {
            var consoleDict = ConsoleIDHandler.consoleIds
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.FirstOrDefault() ?? "");

            using (var form = new Form_ClarifyGameInfo(node.FileName, consoleDict))
            {
                string defaultConsoleID = node.Tag_ConsoleID;

                // Preselect the default console
                ComboBox cb = form.Controls["cboConsole"] as ComboBox;
                foreach (var item in cb.Items)
                {
                    var pair = (KeyValuePair<string, string>)item;
                    if (pair.Value == defaultConsoleID)
                    {
                        cb.SelectedItem = item;
                        break;
                    }
                }

                if (form.ShowDialog() == DialogResult.OK)
                {
                    return (form.EnteredTitle, form.SelectedConsoleID);
                }
                return null;
            }
        }

        private static int AgeOfCachedData(string filePath)
        {
            if (File.Exists(filePath))
            {
                DateTime creationTime = File.GetCreationTime(filePath);
                TimeSpan age = DateTime.Now - creationTime;
                return (int)age.TotalDays;
            }
            return 0;
        }
    }
}
