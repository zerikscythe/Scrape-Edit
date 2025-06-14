using ScapeEdit;
using System.Xml;

namespace ScrapeEdit
{
    public static class GameListManager
    {
        public static Dictionary<string, GameList> gameLists = new Dictionary<string, GameList>();

        public static ScrapedGame ConvertSSXmlToScrapedGame(string xmlData, TreeNodeDetail node = null)
        {
            var scrapedGame = new ScrapedGame();

            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xmlData);

                scrapedGame.Id = int.TryParse(doc.SelectSingleNode("//jeu")?.Attributes["id"]?.Value, out var id) ? id : 0;
                scrapedGame.Name = doc.SelectSingleNode($"//jeu/noms/nom[@region='{GlobalDefaults.DefaultRegionAbrv}']")?.InnerText
                                     ?? doc.SelectSingleNode($"//jeu/noms/nom[@region='wor']")?.InnerText
                                     ?? node.FileName;
                scrapedGame.Developer = doc.SelectSingleNode("//jeu/developpeur")?.InnerText ?? "Unknown";
                scrapedGame.Publisher = doc.SelectSingleNode("//jeu/editeur")?.InnerText ?? "Unknown";
                scrapedGame.ReleaseDate = doc.SelectSingleNode("//jeu/dates/date")?.InnerText ?? "Unknown";
                scrapedGame.Rating = double.TryParse(doc.SelectSingleNode("//jeu/note")?.InnerText, out var rating) ? rating : 0.0;
                scrapedGame.Path = doc.SelectSingleNode("//jeu/rom/romfilename")?.InnerText ?? "Unknown";
                scrapedGame.Players = doc.SelectSingleNode("//jeu/joueurs")?.InnerText ?? "1";
                scrapedGame.Genre = doc.SelectSingleNode("//jeu/genres/genre")?.InnerText ?? "Unknown";
                scrapedGame.Description = doc.SelectSingleNode($"//jeu/synopsis/synopsis[@langue='{GlobalDefaults.DefaultLangAbrv}']")?.InnerText
                                          //?? doc.SelectSingleNode("//jeu/synopsis/synopsis[@langue='en']")?.InnerText
                                          ?? "No synopsis available";
                scrapedGame.Region = doc.SelectSingleNode("//jeu/regions/region")?.InnerText ?? "??";


            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error interpreting XML data into ScrapedGame object.", ex);
            }

            return scrapedGame;
        }





        public static void UpdateGameListEntry(TreeNodeDetail node)
        {
            //var consoleNode = ReturnConsoleNodeFromChild(node);

            var consoleKey = node.Tag_ConsoleName;
                //Path.GetFileName(consoleNode?.Tag_ConsolePath)?.ToLowerInvariant();

            if (consoleKey != null && gameLists.TryGetValue(consoleKey, out var gameList))
            {
                gameList.AddGame(node.Game);
                gameList.ScrapedGames.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase));
            }
        }
        public static void UpdateGameListEntry(TreeNodeDetail[] nodes)
        {
            foreach (TreeNodeDetail node in nodes)
                if (node.Game != null)
                    gameLists[node.Tag_ConsoleName].ScrapedGames.Add(node.Game);

        }
        public static void PurgeOldMediaFiles(TreeNodeDetail node, string fileName = "")
        {
            string[] mediaTypes = { "image", "marquee", "thumb", "video", "manual" };
            foreach (string mediaType in mediaTypes)
            {
                // folder and extension logic as before...
                string folder = mediaType switch
                {
                    "video" => "videos",
                    "manual" => "manuals",
                    _ => "images"
                };
                string ext = mediaType switch
                {
                    "video" => ".mp4",
                    "manual" => ".pdf",
                    _ => ".png"
                };

                string path = Path.Combine(
                    node.Tag_ConsolePath,
                    folder,
                   (fileName != "") ? $"{fileName}-{mediaType}{ext}" : $"{node.FileName}-{mediaType}{ext}"
                );
                if (File.Exists(path))
                {
                    try { File.Delete(path); }
                    catch (IOException) { /* log or ignore */ }
                }
            }
        }
        public static void MoveMediaToLocalDir(
        TreeNodeDetail node, string seDir, string oldFileName = null)
        {
            // purge the old art first (if renaming occurred)
            if (!string.IsNullOrEmpty(oldFileName)
                && !oldFileName.Equals(node.FileName, StringComparison.OrdinalIgnoreCase))
            {
                PurgeOldMediaFiles(node, oldFileName);
            }

            // then purge any existing art for the _new_ filename
            PurgeOldMediaFiles(node);

            string art_Image;
            string art_Marquee;
            string art_Thumbnail;
            string art_Video;
            string art_Manual;


            if (node.Tag_SubDirPath != null)
            {
                //move img files to local folder
                art_Image = Path.Combine(
                   seDir,
                   node.Tag_ConsoleName,
                   node.Tag_SubDirPath,
                   GameListSettings.MainImage,
                   node.FileName + "-" + GameListSettings.MainImage + ".png");

                art_Marquee = Path.Combine(
                   seDir,
                   node.Tag_ConsoleName,
                                   node.Tag_SubDirPath,
                   GameListSettings.Marquee,
                   node.FileName + "-" + GameListSettings.Marquee + ".png");

                art_Thumbnail = Path.Combine(
                   seDir,
                   node.Tag_ConsoleName,
                                   node.Tag_SubDirPath,
                   GameListSettings.Thumbnail,
                   node.FileName + "-" + GameListSettings.Thumbnail + ".png");

                art_Video = Path.Combine(
                   seDir,
                   node.Tag_ConsoleName,
                                  node.Tag_SubDirPath,
                   GameListSettings.Video,
                   node.FileName + "-" + GameListSettings.Video + ".mp4");

                art_Manual = Path.Combine(
                   seDir,
                   node.Tag_ConsoleName,
                   node.Tag_SubDirPath,
                   "manuel",
                   node.FileName + "-" + "manuel.pdf");
            }
            else
            {
                //move img files to local folder
                art_Image = Path.Combine(
                   seDir,
                   node.Tag_ConsoleName,
                   GameListSettings.MainImage,
                   node.FileName + "-" + GameListSettings.MainImage + ".png");

                art_Marquee = Path.Combine(
                   seDir,
                   node.Tag_ConsoleName,
                   GameListSettings.Marquee,
                   node.FileName + "-" + GameListSettings.Marquee + ".png");

                art_Thumbnail = Path.Combine(
                   seDir,
                   node.Tag_ConsoleName,
                   GameListSettings.Thumbnail,
                   node.FileName + "-" + GameListSettings.Thumbnail + ".png");

                art_Video = Path.Combine(
                   seDir,
                   node.Tag_ConsoleName,
                   GameListSettings.Video,
                   node.FileName + "-" + GameListSettings.Video + ".mp4");

                art_Manual = Path.Combine(
                   seDir,
                   node.Tag_ConsoleName,
                   "manuel",
                   node.FileName + "-" + "manuel.pdf");

            }

            string Img_Dest = Path.Combine(
                node.Tag_ConsolePath,
                "images\\" +
                node.FileName + "-image.png");

            string Marq_Dest = Path.Combine(
                node.Tag_ConsolePath,
                "images\\" +
                node.FileName + "-marquee.png");

            string Thumb_Dest = Path.Combine(
                node.Tag_ConsolePath,
                "images\\" +
                node.FileName + "-thumb.png");

            string Video_Dest = Path.Combine(
                node.Tag_ConsolePath,
                "videos\\" +
                node.FileName + "-video.mp4");

            string Manual_dest = Path.Combine(
                node.Tag_ConsolePath,
                "manuals\\" +
                node.FileName + "-manual.pdf");

            Directory.CreateDirectory(Path.Combine(node.Tag_ConsolePath, "images\\"));
            Directory.CreateDirectory(Path.Combine(node.Tag_ConsolePath, "videos\\"));
            Directory.CreateDirectory(Path.Combine(node.Tag_ConsolePath, "manuals\\"));

            if (File.Exists(art_Image))
                File.Copy(art_Image, Img_Dest, true);
            if (File.Exists(art_Marquee))
                File.Copy(art_Marquee, Marq_Dest, true);
            if (File.Exists(art_Thumbnail))
                File.Copy(art_Thumbnail, Thumb_Dest, true);
            if (File.Exists(art_Video))
                File.Copy(art_Video, Video_Dest, true);
            if (File.Exists(art_Manual))
                File.Copy(art_Manual, Manual_dest, true);

            //PopulateDisplayArea();
            //LoadSGImages();
        }
        public static void WriteGameListToFile(TreeNodeDetail node)
        {
            string xmlPath = Path.Combine(node.Tag_ConsolePath, "gamelist.xml");

            if (!gameLists.TryGetValue(node.Tag_ConsoleName, out var gameList))
                return;

            if (File.Exists(xmlPath))
                File.Delete(xmlPath);

            string xmlData = gameList.CreateNewGameListFromScrapedGames();
            File.WriteAllText(xmlPath, xmlData);
        }

        public static void PostProcess(TreeNodeDetail node, string seDir)
        {
            MoveMediaToLocalDir(node, seDir);
            node.Game = SetImageLinks(node);
            UpdateGameListEntry(node);
        }

        private static ScrapedGame SetImageLinks(TreeNodeDetail node)
        {
            ScrapedGame sg = node.Game;
            string fileName = node.FileName;

            sg.Image = FormatPath("./images", fileName, "image.png");
            sg.Marquee = FormatPath("./images", fileName, "marquee.png");
            sg.Thumbnail = FormatPath("./images", fileName, "thumb.png");

            sg.Video = GameListSettings.Video != "none"
                ? FormatPath("./videos", fileName, "video.mp4")
                : "";

            sg.Manual = GameListSettings.Manual
                ? FormatPath("./manuals", fileName, "manual.pdf")
                : "";

            return sg;
        }

        private static string FormatPath(string directory, string fileName, string fileType)
        {
            return Path.Combine(directory, $"{fileName}-{fileType}").Replace("\\", "/");
        }
    }
}
