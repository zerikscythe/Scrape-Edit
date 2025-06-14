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

        public static bool _30Day { get; set; } = true;
    }

    public class SerializableScrapeSettings
    {
        public bool RenameRoms { get; set; } = false;
        public bool AskForClarity { get; set; } = false;
        public bool UseCRC32 { get; set; } = false;
        public bool UseMD5 { get; set; } = true;
        public bool UseSHA1 { get; set; } = false;

        public bool _30Day { get; set; } = true;

        public void Update()
        { 
            RenameRoms = ScrapeSettings.RenameRoms;
            AskForClarity = ScrapeSettings.AskForClarity;
            UseCRC32 = ScrapeSettings.UseCRC32;
            UseMD5 = ScrapeSettings.UseMD5;
            UseSHA1 = ScrapeSettings.UseSHA1;
            _30Day = ScrapeSettings._30Day;
        }
    }
}
