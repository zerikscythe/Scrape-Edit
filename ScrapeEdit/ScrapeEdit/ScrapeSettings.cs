namespace ScrapeEdit
{
    public static class ScrapeSettings
    {
        public static bool UseDummyHash { get; set; } = false;
        public static bool RenameRoms { get; set; } = false;
        public static bool AskForClarity { get; set; } = false;
        public static bool UseCRC32 { get; set; } = false;
        public static bool UseMD5 { get; set; } = true;
        public static bool UseSHA1 { get; set; } = false;
        public static bool useCached_XML { get; set; } = true;
        public static int X_Days 
        {
            get { return SessionSettings.X_Days; }
            set { SessionSettings.X_Days = value; } 
        }
        public static int MaxThreads 
        {
            get { return SessionSettings.MaxThreads; }
            set { SessionSettings.MaxThreads = value; }
        }
    }

    public class SerializableScrapeSettings
    {
        public bool RenameRoms { get; set; } = false;
        public bool AskForClarity { get; set; } = false;
        public bool UseCRC32 { get; set; } = false;
        public bool UseMD5 { get; set; } = true;
        public bool UseSHA1 { get; set; } = false;

        public bool useCached_XML { get; set; } = true;

        public int X_Days { get; set; } = 30;
        public int MaxThreads { get; set; } = 5;

        public void Update()
        { 
            RenameRoms = ScrapeSettings.RenameRoms;
            AskForClarity = ScrapeSettings.AskForClarity;
            UseCRC32 = ScrapeSettings.UseCRC32;
            UseMD5 = ScrapeSettings.UseMD5;
            UseSHA1 = ScrapeSettings.UseSHA1;
            useCached_XML = ScrapeSettings.useCached_XML;
            X_Days = ScrapeSettings.X_Days;
            MaxThreads = ScrapeSettings.MaxThreads;
        }
    }
}
