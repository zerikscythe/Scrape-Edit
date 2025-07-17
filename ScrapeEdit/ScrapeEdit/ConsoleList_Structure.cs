using Microsoft.VisualBasic.Logging;
using ScapeEdit;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace ScrapeEdit
{
    public class ConsoleList
    {
        public List<ScrapedConsole> ScrapedConsoles = new List<ScrapedConsole>();

        //do not load and remake list if file already exists...

        public ConsoleList()
        {
            LoadXML();
        }

        public void LoadXML()
        {
            try
            {
                var doc = new XmlDocument();
                string filePath = Path.Combine(SessionSettings.SettingsFolder, "ss_consoles.xml");

                doc.Load(filePath); 

                var systemNodes = doc.SelectNodes("Data//systeme");

                if (systemNodes != null)
                {
                    foreach (XmlNode node in systemNodes)
                    {
                        var scrapedConsole = new ScrapedConsole
                        {
                            Id = GetInt(node["id"]),
                            ParentId = GetInt(node["parentid"]),
                            Name = GetPreferredName(node),
                            ShortName = node.SelectSingleNode("noms/nom_recalbox")?.InnerText ?? "",
                            Aliases = node.SelectSingleNode("noms/noms_commun")?.InnerText.Split(',') ?? [],
                            Manufacturer = node["compagnie"]?.InnerText,
                            Type = node["type"]?.InnerText,
                            YearStart = GetIntNullable(node["datedebut"]),
                            YearEnd = GetIntNullable(node["datefin"]),
                            Extensions = node["extensions"]?.InnerText.Split(',') ?? [],
                            RomType = node["romtype"]?.InnerText,
                            SupportType = node["supporttype"]?.InnerText
                        };


                        // Parse <medias> section
                        var mediasNode = node.SelectSingleNode("medias");
                        if (mediasNode != null)
                        {
                            foreach (XmlNode mediaNode in mediasNode.SelectNodes("media"))
                            {
                                var asset = new MediaAsset
                                {
                                    Type = mediaNode.Attributes["type"]?.InnerText ?? "",
                                    Region = mediaNode.Attributes["region"]?.InnerText ?? "wor",
                                    Format = mediaNode.Attributes["format"]?.InnerText ?? "",
                                    Url = mediaNode.InnerText.Trim(),
                                    CRC = mediaNode.Attributes["crc"]?.InnerText ?? "",
                                    MD5 = mediaNode.Attributes["md5"]?.InnerText ?? "",
                                    SHA1 = mediaNode.Attributes["sha1"]?.InnerText ?? "",
                                    Version = mediaNode.Attributes["version"]?.InnerText ?? "",
                                    ExtraAttributes = string.Join(";", mediaNode.Attributes
                                        .Cast<XmlAttribute>()
                                        .Where(attr => attr.Name is not ("type" or "region" or "format" or "crc" or "md5" or "sha1" or "version"))
                                        .Select(attr => $"{attr.Name}={attr.Value}"))
                                };

                                scrapedConsole.Media.AddMedia(asset.Type, asset);
                            }
                        }


                        ScrapedConsoles.Add(scrapedConsole);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing XML: {ex.Message}");
            }
        }

        public ScrapedConsole? GetConsoleById(int id)
        {
            return ScrapedConsoles.FirstOrDefault(c => c.Id == id);
        }

        private string GetPreferredName(XmlNode node)
        {
            string region = GlobalDefaults.DefaultRegion?.ToUpperInvariant() ?? "";

            var nomsNode = node.SelectSingleNode("noms");

            if (nomsNode == null)
                return "Unknown";

            // Try based on region
            switch (region)
            {
                case "USA":
                case "US":
                    return nomsNode.SelectSingleNode("nom_us")?.InnerText
                        ?? nomsNode.SelectSingleNode("nom_eu")?.InnerText
                        ?? nomsNode.SelectSingleNode("nom_jp")?.InnerText
                        ?? "Unknown";

                case "EU":
                case "EUR":
                    return nomsNode.SelectSingleNode("nom_eu")?.InnerText
                        ?? nomsNode.SelectSingleNode("nom_us")?.InnerText
                        ?? nomsNode.SelectSingleNode("nom_jp")?.InnerText
                        ?? "Unknown";

                case "JP":
                case "JPN":
                    return nomsNode.SelectSingleNode("nom_jp")?.InnerText
                        ?? nomsNode.SelectSingleNode("nom_us")?.InnerText
                        ?? nomsNode.SelectSingleNode("nom_eu")?.InnerText
                        ?? "Unknown";

                default:
                    return nomsNode.SelectSingleNode("nom_eu")?.InnerText
                        ?? nomsNode.SelectSingleNode("nom_us")?.InnerText
                        ?? nomsNode.SelectSingleNode("nom_jp")?.InnerText
                        ?? "Unknown";
            }
        }



        private int GetInt(XmlNode? node) =>
            int.TryParse(node?.InnerText, out var val) ? val : 0;

        private int? GetIntNullable(XmlNode? node) =>
            int.TryParse(node?.InnerText, out var val) ? val : null;


    }


    public class ScrapedConsole
    {
        
        public int Id { get; set; }
        public int ParentId { get; set; } = 0;         // id_parent
        public string TreeName { get; set; } = "";      //treeNodeName
        public string Name { get; set; }                 // nom_eu
        public string ShortName { get; set; }            // nom_recalbox
        public string[] Aliases { get; set; }            // noms_commun
        public string Manufacturer { get; set; }
        public string Type { get; set; }                 // Console, Ordinateur, etc.
        public int? YearStart { get; set; }
        public int? YearEnd { get; set; }
        public string[] Extensions { get; set; }
        public string RomType { get; set; }
        public string SupportType { get; set; }

        public ConsoleMedia Media { get; set; } = new(); // All media grouped

        private string cachePath;

        public string[] DesiredConsoleMediaTypes = new[]
        {
            "icon",
            "controller",
            "logo-monochrome",
            "illustration"
        };

        private Bitmap _mainImg = null;
        private Bitmap _consoleImg = null;
        private Bitmap _controllerImg = null;
        private Bitmap _iconImg = null;
        public Bitmap MainImg 
        {
            get 
            {
                if (_mainImg != null)
                    return _mainImg;
                else 
                    return GetMediaImg("logo-monochrome");
            }
            set 
            { 
                _mainImg = value; 
            }
        }
        public Bitmap ConsoleImg
        {
            get
            {
                if (_consoleImg != null)
                    return _consoleImg;
                else
                    return GetMediaImg("illustration");
            }
            set
            {
                _consoleImg = value;
            }
        }
        public Bitmap ControllerImg
        {
            get
            {
                if (_controllerImg != null)
                    return _controllerImg;
                else
                    return GetMediaImg("controller");
            }
            set
            {
                _controllerImg = value;
            }
        }
        public Bitmap IconImg
        {
            get
            {
                if (_iconImg != null)
                    return _iconImg;
                else
                    return GetMediaImg("icon");
            }
            set
            {
                _iconImg = value;
            }
        }

        public Bitmap GetMediaImg(string typeOf)
        {
            cachePath = Path.Combine(SessionSettings.SEDirectory, "Consoles", TreeName);

            if (!CheckForMedia(typeOf))
                return null;

            // Build a search pattern like: "TreeName-logo-svg.*"
            string searchPattern = $"{TreeName}-{typeOf}.*";

            string[] matches;
            try
            {
                matches = Directory.GetFiles(cachePath, searchPattern);
            }
            catch
            {
                return null; // Cache path invalid or inaccessible
            }

            if (matches.Length == 0)
                return null;

            try
            {
                // Use a memory copy so the file isn't locked externally
                using var fs = new FileStream(matches[0], FileMode.Open, FileAccess.Read);
                return new Bitmap(fs);
            }
            catch
            {
                return null;
            }
        }

        private bool CheckForMedia(string type)
        {
            
            return Media.MediaByType.ContainsKey(type) && Media.MediaByType[type].Count > 0;
        }

    }

    public class ConsoleMedia
    {
        public Dictionary<string, List<MediaAsset>> MediaByType { get; set; } = new();

        public void AddMedia(string type, MediaAsset asset)
        {
            if (!MediaByType.ContainsKey(type))
                MediaByType[type] = new List<MediaAsset>();

            MediaByType[type].Add(asset);
        }

        public MediaAsset? GetPreferred(string type, string preferredRegion = "wor")
        {
            if (!MediaByType.TryGetValue(type, out var assets))
                return null;

            return assets.FirstOrDefault(m => m.Region == preferredRegion)
                ?? assets.FirstOrDefault();
        }

    }

    public class MediaAsset
    {
        public string Type { get; set; }
        public string Region { get; set; }
        public string Format { get; set; }
        public string Url { get; set; }
        public string CRC { get; set; }
        public string MD5 { get; set; }
        public string SHA1 { get; set; }
        public string Version { get; set; }
        public string ExtraAttributes { get; set; } // fallback for unknown attrs
    }
}
