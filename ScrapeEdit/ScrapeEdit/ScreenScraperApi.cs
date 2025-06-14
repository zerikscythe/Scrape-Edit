using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace ScrapeEdit
{
    /// <summary>
    /// Should you want to run this code, you need to set your own credentials in a _DEV_KEY class.
    ///     public static class _DEV_KEY
    ////    {
    ////        public static string _devid = "";
    ////        public static string _devpassword = "";
    ////        public static string _softname = "";
    ////    }
    /// Or overrid the values below directly.


    public class ScreenScraperApi
    {
        private const string BaseUrl = "https://api.screenscraper.fr/api2/jeuInfos.php";

        private string _ssid;
        private string _sspassword;
        HttpClient httpClient = new HttpClient();
        public static bool goodCredentials = false;

        public string DevID
        {
            get { return _DEV_KEY._devid; }
        }
        public string DevPassword
        {
            get { return _DEV_KEY._devpassword; }
        }
        public string SoftName
        {
            get { return _DEV_KEY._softname; }
        }

        public void LoadCredentials(string user, string pwrd)
        {
            _ssid = user;//lines[3].Trim(),
            _sspassword = pwrd; //lines[4].Trim()
        }

        public string GenerateXmlData(TreeNodeDetail node, string userInputName = "", string userConsoleID = "", bool retry = false)//, string romtype = "rom", string romnom = null, long? romtaille = null)
        {
            string originalFilePath = node.Tag_FullPath;
            //string hash = "";
            if (!retry)
            {
                if (ScrapeSettings.UseCRC32)
                    Hash.Compute_CRC32(node);
                else if (ScrapeSettings.UseMD5)
                    Hash.Compute_MD5(node);
                else if (ScrapeSettings.UseSHA1)
                    Hash.Compute_SHA1(node);
            }
            try
            {
                var url = "";
                if (!retry)
                    url = GenerateUriFromFilename(node.FileName, node.Tag_ConsoleID);
                else
                {
                    if (!string.IsNullOrEmpty(userInputName))
                        url = GenerateUriFromFilename(userInputName, userConsoleID);
                    else
                        return null;
                }

                var response = httpClient.GetAsync(url).Result;

                response.EnsureSuccessStatusCode(); // Throws exception if the status code is not 2xx

                return response.Content.ReadAsStringAsync().Result; // Return the XML data
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("404"))
            {
                return "404";
            }
            catch (Exception ex)
            {
                if (retry == true)
                {
                    return null;
                }
                else
                {
                    return "400";
                }
            }
        }

        private string GenerateUriFromFilename(string fileName, string systemeid)//, string romtype = "rom", long? romtaille = null)
        {
            var queryParams = new List<string>
            {
                $"devid={Uri.EscapeDataString(DevID)}",
                $"devpassword={Uri.EscapeDataString(DevPassword)}",
                $"softname={Uri.EscapeDataString(SoftName)}",
                $"ssid={Uri.EscapeDataString(_ssid)}",
                $"sspassword={Uri.EscapeDataString(_sspassword)}",
                "output=xml",
                $"systemeid={systemeid}",
                $"romnom={Uri.EscapeDataString(fileName)}",
            };

            var queryString = string.Join("&", queryParams);
            return $"{BaseUrl}?{queryString}";
        }

        public async Task<bool> TestCredentialsAsync(string _un, string _pw)
        {
            bool reply = false;
            string url;
            var queryParams = new List<string>
                {
                    $"devid={Uri.EscapeDataString(DevID)}",
                    $"devpassword={Uri.EscapeDataString(DevPassword)}",
                    $"softname={Uri.EscapeDataString(SoftName)}",
                    $"ssid={Uri.EscapeDataString(_un)}",
                    $"sspassword={Uri.EscapeDataString(_pw)}",
                    "output=xml",
                    $"systemeid=1",
                    $"romnom=Sonic the Hedgehog",
                };

            var queryString = string.Join("&", queryParams);
            url = $"{BaseUrl}?{queryString}";

            try
            {
                //var response = httpClient.GetAsync(url).Result;
                var response = await httpClient.GetAsync(url); // Make it async

                response.EnsureSuccessStatusCode(); // Throws exception if the status code is not 2xx
                string data = response.Content.ReadAsStringAsync().Result; // Return the XML data

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);

                SSUserInfo user = new SSUserInfo
                {
                    Id = doc.SelectSingleNode("//ssuser/id")?.InnerText,
                    NumId = int.Parse(doc.SelectSingleNode("//ssuser/numid")?.InnerText ?? "0"),
                    Niveau = int.Parse(doc.SelectSingleNode("//ssuser/niveau")?.InnerText ?? "0"),
                    MaxRequestsPerDay = int.Parse(doc.SelectSingleNode("//ssuser/maxrequestsperday")?.InnerText ?? "0"),
                    RequestsToday = int.Parse(doc.SelectSingleNode("//ssuser/requeststoday")?.InnerText ?? "0"),
                    LastVisit = DateTime.TryParse(doc.SelectSingleNode("//ssuser/datedernierevisite")?.InnerText, out var dt) ? dt : null
                };

                reply = user.NumId > 0 ? true : false;


                //reply = response.StatusCode == System.Net.HttpStatusCode.OK;

            }
            catch (HttpRequestException ex) when (ex.Message.Contains("404"))
            {
                reply = false;
            }
            catch (Exception ex)
            {
                reply = false;
            }

            return reply;
        }

        public async Task<bool> TestCredentialsAsync()
        {
            bool reply = false;
            string url;
            var queryParams = new List<string>
                {
                    $"devid={Uri.EscapeDataString(DevID)}",
                    $"devpassword={Uri.EscapeDataString(DevPassword)}",
                    $"softname={Uri.EscapeDataString(SoftName)}",
                    $"ssid={Uri.EscapeDataString(_ssid)}",
                    $"sspassword={Uri.EscapeDataString(_sspassword)}",
                    "output=xml",
                    $"systemeid=1",
                    $"romnom=Sonic the Hedgehog",
                };

            var queryString = string.Join("&", queryParams);
            url = $"{BaseUrl}?{queryString}";

            try
            {
                var response = await httpClient.GetAsync(url); // Make it async

                response.EnsureSuccessStatusCode(); // Throws exception if the status code is not 2xx
                reply = response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("404"))
            {
                reply = false;
            }
            catch (Exception ex)
            {
                reply = false;
            }

            return reply;
        }

    }

    public class SSUserInfo
    {
        public string Id { get; set; }
        public int NumId { get; set; }
        public int Niveau { get; set; }
        public int MaxRequestsPerDay { get; set; }
        public int RequestsToday { get; set; }
        public DateTime? LastVisit { get; set; }
    }
    [XmlRoot("Data")]
    public class ScraperData
    {
        [XmlElement("jeu")]
        public GameData Game { get; set; }
    }

    public class GameData
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("romid")]
        public int RomId { get; set; }

        [XmlElement("systeme")]
        public SystemElement System { get; set; }

        [XmlElement("editeur")]
        public CompanyElement Publisher { get; set; }

        [XmlElement("developpeur")]
        public CompanyElement Developer { get; set; }

        [XmlElement("joueurs")]
        public int Players { get; set; }

        [XmlElement("note")]
        public int Note { get; set; }

        [XmlArray("noms")]
        [XmlArrayItem("nom")]
        public List<LocalizedName> Names { get; set; }

        [XmlArray("synopsis")]
        [XmlArrayItem("synopsis")]
        public List<LocalizedSynopsis> SynopsisList { get; set; }

        [XmlArray("classifications")]
        [XmlArrayItem("classification")]
        public List<Classification> Classifications { get; set; }

        [XmlArray("dates")]
        [XmlArrayItem("date")]
        public List<LocalizedDate> ReleaseDates { get; set; }

        [XmlArray("genres")]
        [XmlArrayItem("genre")]
        public List<LocalizedGenre> Genres { get; set; }

        [XmlArray("modes")]
        [XmlArrayItem("mode")]
        public List<LocalizedMode> Modes { get; set; }

        [XmlArray("roms")]
        [XmlArrayItem("rom")]
        public List<RomInfo> Roms { get; set; }

        [XmlArray("medias")]
        [XmlArrayItem("media")]
        public List<MediaFile> MediaFiles { get; set; }

        [XmlArray("roms")]
        [XmlArrayItem("rom")]
        public List<RomSummary> RomMatches { get; set; }

    }

    public class SystemElement
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlText]
        public string Name { get; set; }
    }

    public class CompanyElement
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlText]
        public string Name { get; set; }
    }

    public class LocalizedName
    {
        [XmlAttribute("region")]
        public string Region { get; set; }

        [XmlText]
        public string Name { get; set; }
    }

    public class LocalizedSynopsis
    {
        [XmlAttribute("langue")]
        public string Language { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    public class Classification
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public class LocalizedDate
    {
        [XmlAttribute("region")]
        public string Region { get; set; }

        [XmlText]
        public string Date { get; set; }
    }

    public class LocalizedGenre
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("langue")]
        public string Language { get; set; }

        [XmlText]
        public string Genre { get; set; }
    }

    public class LocalizedMode
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("langue")]
        public string Language { get; set; }

        [XmlText]
        public string Mode { get; set; }
    }

    public class RomInfo
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("romfilename")]
        public string FileName { get; set; }

        [XmlAttribute("rommd5")]
        public string MD5 { get; set; }

        [XmlAttribute("romsha1")]
        public string SHA1 { get; set; }

        [XmlAttribute("romcrc")]
        public string CRC { get; set; }

        [XmlAttribute("romsize")]
        public long Size { get; set; }
    }
    public class MediaFile
    {
        [XmlAttribute("parent")]
        public string Parent { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("region")]
        public string Region { get; set; }

        [XmlAttribute("format")]
        public string Format { get; set; }

        [XmlAttribute("crc")]
        public string CRC { get; set; }

        [XmlAttribute("md5")]
        public string MD5 { get; set; }

        [XmlAttribute("sha1")]
        public string SHA1 { get; set; }

        [XmlAttribute("size")]
        public long Size { get; set; }

        [XmlAttribute("organisation")]
        public string Organisation { get; set; }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlIgnore]
        public bool IdSpecified => Id != 0;


        [XmlAttribute("subparent")]
        public string SubParent { get; set; }

        [XmlText]
        public string Url { get; set; }
    }

    public class RomSummary
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("romfilename")]
        public string FileName { get; set; }

        [XmlAttribute("romsize")]
        public long Size { get; set; }

        [XmlAttribute("romcrc")]
        public string CRC { get; set; }

        [XmlAttribute("rommd5")]
        public string MD5 { get; set; }

        [XmlAttribute("romsha1")]
        public string SHA1 { get; set; }

        // Optional attributes (e.g., alt, best, etc.)
        [XmlAttribute("alt")]
        public int Alt { get; set; }

        [XmlAttribute("best")]
        public int Best { get; set; }

        [XmlAttribute("demo")]
        public int Demo { get; set; }

        [XmlAttribute("beta")]
        public int Beta { get; set; }

        [XmlAttribute("proto")]
        public int Proto { get; set; }

        [XmlAttribute("hack")]
        public int Hack { get; set; }

        [XmlAttribute("trad")]
        public int Trad { get; set; }

        [XmlAttribute("unl")]
        public int Unl { get; set; }

        [XmlAttribute("netplay")]
        public int Netplay { get; set; }

        [XmlAttribute("romcloneof")]
        public int RomCloneOf { get; set; }
    }


    public static class ScraperXmlParser
    {
        public static ScraperData Deserialize(string xmlContent)
        {
            var serializer = new XmlSerializer(typeof(ScraperData));
            using var reader = new StringReader(xmlContent);
            return (ScraperData)serializer.Deserialize(reader);
        }
    }


    public static class XmlGameLoader
    {
        public static ScraperData LoadFromXml(string filePath)
        {
            var serializer = new XmlSerializer(typeof(ScraperData));
            using var stream = new FileStream(filePath, FileMode.Open);
            return (ScraperData)serializer.Deserialize(stream);
        }
    }
}
