using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;


namespace ScapeEdit
{

    [XmlRoot("gameList")]
    public class GameList
    {

        [XmlElement("game")]
        public List<ScrapedGame> ScrapedGames = new List<ScrapedGame>();

        public void ConvertGLXmlToScrapedGame(string xmlData)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xmlData);

                var gameNodes = doc.SelectNodes("//game");

                if (gameNodes != null)
                {
                    foreach (XmlNode gameNode in gameNodes)
                    {
                        var scrapedGame = new ScrapedGame
                        {
                            Id = int.TryParse(gameNode.Attributes["id"]?.Value, out var id) ? id : 0,
                            Name = gameNode.SelectSingleNode("name")?.InnerText ?? "",
                            Developer = gameNode.SelectSingleNode("developer")?.InnerText ?? "",
                            Publisher = gameNode.SelectSingleNode("publisher")?.InnerText ?? "",
                            ReleaseDate = gameNode.SelectSingleNode("releasedate")?.InnerText ?? "",
                            Rating = double.TryParse(gameNode.SelectSingleNode("rating")?.InnerText, out var rating) ? rating : 0.0,
                            Path = gameNode.SelectSingleNode("path")?.InnerText ?? "",
                            Players = gameNode.SelectSingleNode("players")?.InnerText ?? "",
                            Genre = gameNode.SelectSingleNode("genre")?.InnerText ?? "",
                            Description = gameNode.SelectSingleNode("desc")?.InnerText ?? "",
                            Image = gameNode.SelectSingleNode("image")?.InnerText ?? "",
                            Thumbnail = gameNode.SelectSingleNode("thumbnail")?.InnerText ?? "",
                            Marquee = gameNode.SelectSingleNode("marquee")?.InnerText ?? "",
                            Video = gameNode.SelectSingleNode("video")?.InnerText ?? "",
                            Manual = gameNode.SelectSingleNode("manual")?.InnerText ?? "",
                            Region = gameNode.SelectSingleNode("region")?.InnerText ?? "",
                            Hidden = bool.TryParse(gameNode.SelectSingleNode("hidden")?.InnerText, out var hidden) ? hidden : false,
                            Favorite = bool.TryParse(gameNode.SelectSingleNode("favorite")?.InnerText, out var favorite) ? favorite : false
                        };

                        ScrapedGames.Add(scrapedGame);
                    }
                }
            }
            catch
            { }

        }


        public string CreateNewGameListFromScrapedGames()
        {
            // Create the root element.
            XElement rootElement = new XElement("gameList");

            // For each scraped game, generate its XML entry and add it to the root.
            foreach (var game in ScrapedGames)
            {
                // Assume each ScrapedGame has a method that returns a valid <game> XML string.
                string gameXmlString = game.CreateGameListXML_Entry();

                try
                {
                    // Parse the string into an XElement.
                    XElement gameElement = XElement.Parse(gameXmlString);
                    rootElement.Add(gameElement);
                }
                catch (Exception ex)
                {
                    // Log or handle parsing errors as needed.
                    Console.WriteLine("Error parsing game XML: " + ex.Message);
                }
            }

            // Instead of using rootElement.ToString(), create an XmlWriter with settings that force LF newlines.
            var settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineChars = "\n",       // Set newlines to UNIX-style LF.
                OmitXmlDeclaration = true   // Consistent with your existing configuration.
            };

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    rootElement.WriteTo(xmlWriter);
                    xmlWriter.Flush();
                }
                return stringWriter.ToString();
            }
        }

        public void AddGame(ScrapedGame sg)
        {
            ScrapedGame sgX = ScrapedGames.Find(x => x.Path == sg.Path);
            if (sgX != null)
                ScrapedGames.Remove(sgX);

            ScrapedGames.Add(sg);
        }


        public GameList LoadGameListXML(string XMLfilePath)
        {
            try
            {
                string xml;
                using (var reader = new StreamReader(XMLfilePath))
                {
                    xml = reader.ReadToEnd();
                }

                GameList gameList = new GameList();
                gameList.ConvertGLXmlToScrapedGame(xml); // Assuming this method populates the GameList

                return gameList;
            }
            catch (IOException ex)
            {
                throw new ApplicationException("Error reading the file: " + XMLfilePath);
            }
        }
    }

    

    [XmlRoot("game")]
    public class ScrapedGame
    {


        [XmlElement("image")]
        public string Image { get; set; }

        [XmlElement("video")]
        public string Video { get; set; }

        [XmlElement("manual")]
        public string Manual { get; set; }
        [XmlElement("marquee")]
        public string Marquee { get; set;}

        [XmlElement("thumbnail")]
        public string Thumbnail { get; set;}

        [XmlAttribute("id")]
        public int Id { get; set; }

        // This property tells the serializer whether to include the Id attribute.
        [XmlIgnore]
        public bool IdSpecified
        {
            get { return Id != 0; }
        }

        string _path;
        [XmlElement("path")]
        public string Path {
            get { return _path; }
            set {
                    
                    _path = value.Replace("./", "");
                    _path = "./"+_path.Replace("\\", "/"); 
                } 
        }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("desc")]
        public string Description { get; set; }

        [XmlElement("rating")]
        public double Rating { get; set; }

        public string ParsedReleaseDate
        {
            get { return ParseDateCodes(ReleaseDate); }
            
        }

        string _releaseDate = "";
        [XmlElement("releasedate")]
        public string ReleaseDate 
        { 
            get { return _releaseDate; } 
            set { _releaseDate = EncodeDateCodes(value); } 
        }


        [XmlElement("developer")]
        public string Developer { get; set; }

        [XmlElement("publisher")]
        public string Publisher { get; set; }

        [XmlElement("genre")]
        public string Genre { get; set; }

        [XmlElement("players")]
        public string Players { get; set; }

        [XmlElement("region")]
        public string Region { get; set; }

        [XmlElement("favorite")]
        public bool Favorite { get; set; }

        [XmlElement("hidden")]
        public bool Hidden { get; set; }

        [XmlElement("CRC")]
        public string CRC32 { get; set; }

        [XmlElement("MD5")]
        public string MD5 { get; set; }

        [XmlElement("SHA1")]
        public string SHA1 { get; set; }

        public bool HashMatch { get; set; } = false;

        public string NormalizePath(string path)
        {
            return path.Replace("./", "").Replace("/", "\\");
        }

        public string CreateGameListXML_Entry()
        {
            var serializer = new XmlSerializer(typeof(ScrapedGame));

            // Create empty namespaces to prevent default xmlns attributes
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            // Configure XmlWriter to omit the XML declaration
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true, // optional: set to false if you don't need indentation
                NewLineChars = "\n"
            };

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    serializer.Serialize(xmlWriter, this, ns);
                }
                return stringWriter.ToString();
            }
        }

        string EncodeDateCodes(string formattedDate)
        {
            if (string.IsNullOrWhiteSpace(formattedDate))
                return string.Empty;

            // If the string is just a year (4 characters), assume it's already "original"
            if (formattedDate.Length == 4)
                return formattedDate;

            // Try to parse the date in "MM/dd/yyyy" format.
            if (DateTime.TryParseExact(formattedDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                // Return the date in the original layout.
                // Since the original layout expected time, we append a default time ("T000000")
                return dt.ToString("yyyyMMdd'T'000000", CultureInfo.InvariantCulture);
            }
            // Try to parse the date in "yyyy-MM-dd" format.
            if (DateTime.TryParseExact(formattedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt2))
            {
                // Return the date in the original layout.
                // Since the original layout expected time, we append a default time ("T000000")
                return dt2.ToString("yyyyMMdd'T'000000", CultureInfo.InvariantCulture);
            }
            // If parsing fails, return the original string.
            return formattedDate;
        }
        string ParseDateCodes(string date)
        {
            if (date == "Unknown")
                return null;

            string reply = "";

            if (date.Contains('T'))
                reply = DateTime.ParseExact(date, "yyyyMMddTHHmmss", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            else if (date.Length == 4)//only year?
                reply = date;
            else if (date != "")
                reply = DateTime.Parse(date).ToString("MM/dd/yyyy");


            return reply;
        }

        public ScrapedGame Clone(string relativePath)
        {
            return new ScrapedGame
            {
                Image = this.Image,
                Video = this.Video,
                Manual = this.Manual,
                Marquee = this.Marquee,
                Thumbnail = this.Thumbnail,
                Id = this.Id,
                Path = relativePath,
                Name = this.Name,
                Description = this.Description,
                Rating = this.Rating,
                ReleaseDate = this.ParsedReleaseDate, // Optional: keep original or parsed?
                Developer = this.Developer,
                Publisher = this.Publisher,
                Genre = this.Genre,
                Players = this.Players,
                Region = this.Region,
                CRC32 = this.CRC32,
                MD5 = this.MD5,
                SHA1 = this.SHA1,
                Favorite = this.Favorite,
                Hidden = this.Hidden
            };
        }


    }
}


