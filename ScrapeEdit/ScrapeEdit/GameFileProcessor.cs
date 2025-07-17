using ScrapeEdit.Properties;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace ScrapeEdit
{
    public static class GameFileProcessor
    {
        //private static string seRomDir;
        private static ScreenScraperApi ssa;
        private static Dictionary<string, string> ValidEXT;

        public static bool SetValuesFirst(ScreenScraperApi _ssa, Dictionary<string, string> _validExt)
        {
            if (string.IsNullOrEmpty(SessionSettings.SEDirectory_ROM))
            {
                MessageBox.Show("Please select a location for the ScrapeEdit Cache", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            ssa = _ssa;
            ValidEXT = _validExt;
            Directory.CreateDirectory(SessionSettings.SEDirectory_ROM);
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

                string xmlData = await GetXmlDataForNodeAsync(node, downloadMedia);
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
                string currentFileName = node.FileNameFull;
                string scrapedFileName = GetXML_FileName(new XmlDocument { InnerXml = xmlData }, node);

                bool renameNeeded = ScrapeSettings.RenameRoms &&
                                    !string.Equals(currentFileName, scrapedFileName, StringComparison.OrdinalIgnoreCase);

                if (ScrapeSettings.RenameRoms && renameNeeded)
                {
                    try
                    {
                        RenameRomFile(node, scrapedFileName);
                        //update the UI to reflex the upcoming changes
                        onRenamed(scrapedFileName);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"Cannot rename ROM: {ex.Message}", "Rename Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }
                    report(50, "Hash Found - File Renamed!");
                }

                cancellationToken.ThrowIfCancellationRequested();
                report(60, "Finalizing and downloading…");
                var completedNode = await FinalizeGameMediaUpdate(
                    node, 
                    xmlData, 
                    cancellationToken);

                report(100, "Scrape completed successfully!");


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
                ? Path.Combine(SessionSettings.SEDirectory_ROM, node.Tag_ConsoleName)
                : Path.Combine(SessionSettings.SEDirectory_ROM, node.Tag_ConsoleName, node.Tag_SubDirPath);
            Directory.CreateDirectory(cacheDir);
            return cacheDir;
        }

        

        private static async Task<string> GetXmlDataForNodeAsync(
            TreeNodeDetail node,
            bool downloadMedia = true)
                {
                    //string originalName = node.FileNameFull;
                    string originalCachePath = Path.Combine(GetCacheDir(node), node.FileNameFull + ".xml");

                    string xmlData = null;

                    // 2) Load from cache if it exists and is younger than 30 days
                    if (ScrapeSettings.useCached_XML &&
                        File.Exists(originalCachePath) &&
                        AgeOfCachedDataLessThan(originalCachePath))
                    {
                        xmlData = await File.ReadAllTextAsync(originalCachePath);
                        downloadMedia = false;
                    }
                    else
                    {
                        // 3) Attempt fresh scrape using filename-based lookup
                        xmlData = await ssa.GenerateXmlDataAsync(node);
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

                        if (string.IsNullOrWhiteSpace(title)) return null;

                        xmlData = await ssa.GenerateXmlDataAsync(
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

            string originalCachePath = Path.Combine(GetCacheDir(node), node.FileNameFull + ".xml");
            xmlData = SetXML_FileName(doc, GetXML_FileName(doc, node));
            
            XML_Writer.Write(xmlData, originalCachePath);

            return xmlData;
        }

        private static string SetXML_FileName(XmlDocument doc, string romFileName)
        {
            XmlNode romNode = doc.SelectSingleNode("//jeu/rom/romfilename");

            if (romNode == null)
            {
                var romContainer = doc.SelectSingleNode("//jeu/rom") ?? doc.CreateElement("rom");
                if (romContainer.ParentNode == null)
                    doc.SelectSingleNode("//jeu")?.AppendChild(romContainer);
                var newRomFileNode = doc.CreateElement("romfilename");
                newRomFileNode.InnerText = romFileName;
                romContainer.AppendChild(newRomFileNode);
            }
            else 
                romNode.InnerText = romFileName;

            return doc.OuterXml;
        }

        private static string GetXML_FileName(XmlDocument doc, TreeNodeDetail node = null)
        {
            string fileName = "";
            if (ScrapeSettings.RenameRoms) 
            { 
                string fileHash = node.Tag_Hash;
                string fileHashType = Hash.ReturnHashType(fileHash);

                var match = doc.SelectNodes("//jeu/roms/rom")
                    .Cast<XmlNode>()
                    .FirstOrDefault(r => string.Equals(r.Attributes[fileHashType]?.Value, fileHash, StringComparison.OrdinalIgnoreCase));
                fileName = match?.Attributes["romfilename"]?.Value?.Trim();
            }

            if (string.IsNullOrEmpty(fileName))
            {
                // Fallback to the first romfilename node if no hash match found
                fileName = node.FileNameFull;//
            }

            return fileName;
        }


        private static void RenameRomFile(TreeNodeDetail node, string newFileName)
        {
            
            string originalFilename = node.FileNameFull;
            string og_FN_Short = Path.GetFileNameWithoutExtension(originalFilename);

            string newFullPath = Path.Combine(node.Tag_ConsolePath, newFileName);

            if (!string.Equals(node.Tag_FullPath, newFullPath, StringComparison.OrdinalIgnoreCase))
            {
                if (File.Exists(newFullPath))
                    throw new IOException($"A file named '{newFileName}' already exists.");

                File.Move(node.Tag_FullPath, newFullPath);
                node.Tag_FullPath = newFullPath;
            }

            GameListManager.PurgeOldMediaFiles(node, og_FN_Short);
        }

        private static async Task<TreeNodeDetail> FinalizeGameMediaUpdate(
            TreeNodeDetail node,
            string xmlData,
            CancellationToken ct
        )
        {

            await MediaDownloader.DownloadMediaAsync(
                node,
                xmlData,
                DownloadSettings.DownloadFilesTypes(),
                ct
            );

            node.Game = GameListManager.ConvertSSXmlToScrapedGame(xmlData, node);

            return node;
        }
        private static (string title, string consoleID)? PromptForGameInfo(TreeNodeDetail node)
        {
            (string title, string consoleID)? result = null;

            if (Application.OpenForms.Count > 0)
            {
                var mainForm = Application.OpenForms[0];
                if (mainForm.InvokeRequired)
                {
                    mainForm.Invoke(new MethodInvoker(() =>
                    {
                        result = ShowPromptDialog(node);
                    }));
                }
                else
                {
                    result = ShowPromptDialog(node);
                }
            }

            return result;
        }

        private static (string title, string consoleID)? ShowPromptDialog(TreeNodeDetail node)
        {
            var consoleDict = ConsoleIDHandler.consoleIds
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.FirstOrDefault() ?? "");

            using (var form = new Form_ClarifyGameInfo(node.FileName, consoleDict))
            {
                // Preselect the default console
                if (form.Controls["cboConsole"] is ComboBox cb)
                {
                    foreach (var item in cb.Items)
                    {
                        var pair = (KeyValuePair<string, string>)item;
                        if (pair.Value == node.Tag_ConsoleID)
                        {
                            cb.SelectedItem = item;
                            break;
                        }
                    }
                }

                if (form.ShowDialog() == DialogResult.OK)
                {
                    return (form.EnteredTitle, form.SelectedConsoleID);
                }
            }

            return null;
        }

        private static bool AgeOfCachedDataLessThan(string filePath)
        {
            int fileAge = 0;
            bool reply = false;

            if (File.Exists(filePath))
            {
                DateTime creationTime = File.GetCreationTime(filePath);
                TimeSpan age = DateTime.Now - creationTime;
                fileAge = (int)age.TotalDays;
            }
            
            if(fileAge < SessionSettings.X_Days)
                reply = true;

            return reply;
        }
    }
}
