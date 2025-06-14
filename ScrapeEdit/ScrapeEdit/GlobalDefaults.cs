namespace ScrapeEdit
{
    public class SerializableGlobalSettings()
    {
        public  string DefaultLanguage { get; set; }
        public  string DefaultRegion { get; set; }

        public void Update()
        {
            DefaultLanguage = GlobalDefaults.DefaultLanguage;
            DefaultRegion = GlobalDefaults.DefaultRegion;
        }

    }
    public static class GlobalDefaults
    {
        public static List<string> GetAllLanguages() 
        {
            List<string> reply = new List<string>();
            foreach (KeyValuePair<string,string> pair in LanguageIDs)
            {
                reply.Add(pair.Key);
            }
            return reply;
        }
        public static List<string> GetAllRegions()
        {
            List<string> reply = new List<string>();
            foreach (KeyValuePair<string, string> pair in RegionIDs)
            {
                reply.Add(pair.Key);
            }
            return reply;
        }
        public static string DefaultLanguage { get; set; } = "English";
        public static string DefaultLangAbrv 
        {
            get 
            {
                return LanguageIDs[DefaultLanguage];
            } 
        }
        static Dictionary<string, string> LanguageIDs = new Dictionary<string, string>()
        {
            { "English", "en"},
            { "Japananese", "jp"},
            { "German", "de"},
            { "French", "fr"},
            { "Italian", "it" },
            { "Portugese", "pt"},


        };
        public static string DefaultRegion { get; set; } = "USA";
        public static string DefaultRegionAbrv
        {
            get
            {
                return RegionIDs[DefaultRegion];
            }
        
        }
        static Dictionary<string, string> RegionIDs = new Dictionary<string, string>()
        {
            { "USA", "us"},
            { "Japan", "jp"},
            { "Europe", "eu"},
            { "Germany", "de"},
            { "France", "fr"},
            { "Italy", "it" },
            { "Portugal", "pt"},
            { "World", "wor"}

        };

    }
}
