using System.Xml;

namespace ScrapeEdit
{
    public class SerializableGameListSettings
    { 
        public string MainImage { get; set; } = "box-2D";
        public string Thumbnail {  get; set; } = "box-3D";
        public string Marquee { get; set; } = "wheel";
        public string Video { get; set; } = "video-normalized";
        public bool Manual { get; set; } = true;

        public void Update()
        { 
            MainImage = GameListSettings.MainImage;
            Thumbnail = GameListSettings.Thumbnail;
            Marquee = GameListSettings.Marquee;
            Video = GameListSettings.Video;
            Manual = GameListSettings.Manual;
        }
    }
    public static class GameListSettings
    {
        public static string MainImage { get; set; } = "box-2D";
        public static string MainImageXML()
        { 
            return XML_ImageLocations[MainImage];
        }

        public static string Thumbnail { get; set; } = "box-3D";

        public static string ThumbnailXML()
        {
            return XML_ImageLocations[Thumbnail];
        }

        public static string Marquee { get; set; } = "wheel";

        public static string MarqueeXML()
        {
            return XML_ImageLocations[Marquee];
        }

        public static string Video { get; set; } = "video-normalized";

        public static string VideoXML()
        {
            return XML_ImageLocations[Video];
        }

        public static bool Manual { get; set; } = true;

        public static List<string> Main_Thumb_Options = new List<string>();

        public static List<string> Add_MainThumb_Options(string entry)
        {
            if (!Main_Thumb_Options.Contains(entry))
                Main_Thumb_Options.Add(entry);

            return Main_Thumb_Options;
        }
        public static List<string> Remove_MainThumb_Options(string entry)
        {
            if (Main_Thumb_Options.Contains(entry))
                Main_Thumb_Options.RemoveAt(Main_Thumb_Options.IndexOf(entry));

            return Main_Thumb_Options;
        }

        public static List<string> Marquee_Options = new List<string>();
        public static List<string> Add_Marquee_Options(string entry)
        {
            if (!Marquee_Options.Contains(entry))
                Marquee_Options.Add(entry);

            return Marquee_Options;
        }
        public static List<string> Remove_Marquee_Options(string entry)
        {
            if (Marquee_Options.Contains(entry))
                Marquee_Options.RemoveAt(Marquee_Options.IndexOf(entry));

            return Marquee_Options;
        }
        public static List<string> Video_Options = new List<string>();
        public static List<string> Add_Video_Option(string entry)
        {
            if (!Video_Options.Contains(entry)) // add it
            {
                Video_Options.Add(entry);
            }

            return Video_Options;
        }
        public static List<string> Remove_Video_Option(string entry)
        {
            if (Video_Options.Contains(entry)) // remove it
            {
                Video_Options.RemoveAt(Video_Options.IndexOf(entry));
            }

            return Video_Options;
        }

        public static readonly HashSet<string> IgnoredExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".m3u", ".txt", ".db", ".cfg" // Add more extensions as needed
        };


        public static XmlWriterSettings XMLSettings()
        {
            return new XmlWriterSettings
            {
                Indent = true,             // Ensures each node appears on a new line
                IndentChars = "    ",       // Use spaces for indentation
                NewLineChars = "\r\n",      // Windows-style new line
                NewLineHandling = NewLineHandling.Replace,
                Encoding = new System.Text.UTF8Encoding(false) // Prevents BOM (Byte Order Mark)
            };
        }

        static Dictionary<string, string> XML_ImageLocations = new Dictionary<string, string>()
        {
            { "none", "none"},
            {"sstitle", $"//jeu/medias/sstitle[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"ss", $"//jeu/medias/ss[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"fanart" ,$"//jeu/medias/fanart[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"video" ,$"//jeu/medias/video[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"video-normalized" ,$"//jeu/medias/video-normalized[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"themehs"  ,$"//jeu/medias/themehs[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"screenmarquee"  ,$"//jeu/medias/screenmarquee[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"screenmarqueesmall"  ,$"//jeu/medias/screenmarqueesmall[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"manuel"  ,$"//jeu/medias/manuel[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"steamgrid" ,$"//jeu/medias/steamgrid[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"wheel" ,$"//jeu/medias/wheel[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"wheel-hd" ,$"//jeu/medias/wheel-hd[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"wheel-carbon"  ,$"//jeu/medias/wheel-carbon[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"wheel-steel"  ,$"//jeu/medias/wheel-steel[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"box-2D" ,$"//jeu/medias/box-2D[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"box-2D-side" ,$"//jeu/medias/box-2D-side[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"box-2D-back" ,$"//jeu/medias/box-2D-back[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"box-texture" ,$"//jeu/medias/box-texture[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"box-3D" ,$"//jeu/medias/box-3D[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"support-texture" ,$"//jeu/medias/support-texture[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"support-2D" ,$"//jeu/medias/support-2D[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"bezel-16-9" ,$"//jeu/medias/bezel-16-9[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"mixrbv1", $"//jeu/medias/mixrbv1[@region='{GlobalDefaults.DefaultRegionAbrv}']"},
            {"mixrbv2", $"//jeu/medias/mixrbv2[@region='{GlobalDefaults.DefaultRegionAbrv}']"},

        };
    }

}
