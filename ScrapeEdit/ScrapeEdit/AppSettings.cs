using System.Xml.Serialization;

namespace ScrapeEdit
{
    public static class SessionSettings
    { 
        public static string UserName { get; set; } = "";
        public static string Password { get; set; } = "";
        public static string RomDirectory { get; set; } = "";
        public static string SettingsFolder { get; set; } = "";
        public static string SEDirectory { 
            get 
            { 
                return Path.Combine(SettingsFolder, "Cache"); 
            } 
        }
        public static string SEDirectory_ROM
        {
            get 
            { 
                return Path.Combine(SEDirectory, "roms"); 
            }
        }
        public static bool editingInProgress { get; set; } = false;


    }

    [XmlRoot("appSettings")]
    public class AppSettings
    {
        private static string SettingsFolder =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ScrapeEdit");

        private static string SettingsFilePath => Path.Combine(SettingsFolder, "appSettings.xml");

        [XmlElement("romDirectory")]
        public string RomDirectory { get; set; }

        [XmlElement("seDirectory")]
        public string SEDirectory { get; set; } = Path.Combine(SettingsFolder,"Cache");

        [XmlElement("userName")]
        public string UserName {get;set;}

        [XmlElement("passWord")]
        public string Password 
        {get;set;}

        [XmlElement]
        public bool WorkingCreds = false;

        public void SetUserName(string userName)
        {
            UserName = userName;
            SessionSettings.UserName = userName;
        }
        public void SetPassword(string password)
        {
            Password = password;
            SessionSettings.Password = password;
        }
        public void SetRomDir(string filePath)
        {
            RomDirectory = filePath;
            SessionSettings.RomDirectory = filePath;
            Save();
        }
        public void SetSEDir(string filePath)
        { 
            SEDirectory = filePath; 
            SessionSettings.SettingsFolder = filePath;
            Save();
        }

        [XmlElement("globalSettings")]
        public SerializableGlobalSettings GlobalSettingsData { get; set; } = new SerializableGlobalSettings();

        [XmlElement("scrapeSettings")]
        public SerializableScrapeSettings ScrapeSettingsData { get; set; } = new SerializableScrapeSettings();

        [XmlElement("downloadSettings")]
        public SerializableDownloadSettings DownloadSettingsData { get; set; } = new SerializableDownloadSettings();

        [XmlElement("gamelistSettings")]
        public SerializableGameListSettings GameListSettingsData { get; set; } = new SerializableGameListSettings();

        bool settingsMissing = false;

        public void Save()
        {
            try
            {
                Directory.CreateDirectory(SettingsFolder); // Make sure the directory exists

                GlobalSettingsData.Update();
                DownloadSettingsData.Update();
                GameListSettingsData.Update();
                ScrapeSettingsData.Update();

                if (settingsMissing)
                    SessionSettings.SettingsFolder = SettingsFolder;


                var serializer = new XmlSerializer(typeof(AppSettings));
                using (var writer = new StreamWriter(SettingsFilePath))
                {
                    serializer.Serialize(writer, this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save settings: {ex.Message}");
            }
        }

        public void Load()//string[] Load()
        {
            //string[] reply = new string[2];

            if (!File.Exists(SettingsFilePath))
                Save();
            else
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(AppSettings));
                    using (var reader = new StreamReader(SettingsFilePath))
                    {
                        var settings = (AppSettings)serializer.Deserialize(reader);

                        if (!Directory.Exists(SEDirectory))
                        {
                            Directory.CreateDirectory(settings.SEDirectory);
                            Directory.CreateDirectory(settings.SEDirectory+"\\roms\\");

                        }

                        this.RomDirectory = settings.RomDirectory;
                        this.SEDirectory = settings.SEDirectory;
                        this.UserName = settings.UserName;
                        this.Password = settings.Password;
                        this.WorkingCreds = settings.WorkingCreds;

                        SessionSettings.UserName = settings.UserName;
                        SessionSettings.Password = settings.Password;
                        SessionSettings.RomDirectory = settings.RomDirectory;
                        SessionSettings.SettingsFolder = SettingsFolder; // Static settings folder path

                        // Apply loaded values to the static ScrapeSettings class
                        ScrapeSettings.RenameRoms = settings.ScrapeSettingsData.RenameRoms;
                        ScrapeSettings.AskForClarity = settings.ScrapeSettingsData.AskForClarity;
                        ScrapeSettings.UseCRC32 = settings.ScrapeSettingsData.UseCRC32;
                        ScrapeSettings.UseMD5 = settings.ScrapeSettingsData.UseMD5;
                        ScrapeSettings.UseSHA1 = settings.ScrapeSettingsData.UseSHA1;
                        ScrapeSettings._30Day = settings.ScrapeSettingsData._30Day;

                        GlobalDefaults.DefaultLanguage = settings.GlobalSettingsData.DefaultLanguage;
                        GlobalDefaults.DefaultRegion = settings.GlobalSettingsData.DefaultRegion;

                        DownloadSettings.Download_sstitle = settings.DownloadSettingsData.Download_sstitle;
                        DownloadSettings.Download_ss = settings.DownloadSettingsData.Download_ss;
                        DownloadSettings.Download_fanart = settings.DownloadSettingsData.Download_fanart;
                        DownloadSettings.Download_video = settings.DownloadSettingsData.Download_video;
                        DownloadSettings.Download_video_normalized = settings.DownloadSettingsData.Download_video_normalized;
                        DownloadSettings.Download_themehs = settings.DownloadSettingsData.Download_themehs;
                        DownloadSettings.Download_screenmarquee = settings.DownloadSettingsData.Download_screenmarquee;
                        DownloadSettings.Download_screenmarqueesmall = settings.DownloadSettingsData.Download_screenmarqueesmall;
                        DownloadSettings.Downlaod_manuel = settings.DownloadSettingsData.Downlaod_manuel;
                        DownloadSettings.Download_steamgrid = settings.DownloadSettingsData.Download_steamgrid;
                        DownloadSettings.Download_wheel = settings.DownloadSettingsData.Download_wheel;
                        DownloadSettings.Download_wheel_hd = settings.DownloadSettingsData.Download_wheel_hd;
                        DownloadSettings.Download_wheel_carbon = settings.DownloadSettingsData.Download_wheel_carbon;
                        DownloadSettings.Download_wheel_steel = settings.DownloadSettingsData.Download_wheel_steel;
                        DownloadSettings.Download_box_2D = settings.DownloadSettingsData.Download_box_2D;
                        DownloadSettings.Download_box_2D_side = settings.DownloadSettingsData.Download_box_2D_side;
                        DownloadSettings.Download_box_2D_back = settings.DownloadSettingsData.Download_box_2D_back;
                        DownloadSettings.Download_box_texture = settings.DownloadSettingsData.Download_box_texture;
                        DownloadSettings.Download_box_3D = settings.DownloadSettingsData.Download_box_3D;
                        DownloadSettings.Download_support_texture = settings.DownloadSettingsData.Download_support_texture;
                        DownloadSettings.Downlaod_support_2D = settings.DownloadSettingsData.Downlaod_support_2D;
                        DownloadSettings.Download_bezel_16_9 = settings.DownloadSettingsData.Download_bezel_16_9;
                        DownloadSettings.Download_mixrbv1 = settings.DownloadSettingsData.Download_mixrbv1;
                        DownloadSettings.Download_mixrbv2 = settings.DownloadSettingsData.Download_mixrbv2;

                        // ✅ Ensure GameListSettingsData is updated from the loaded settings
                        this.GameListSettingsData = settings.GameListSettingsData ?? new SerializableGameListSettings();

                        // ✅ Now apply to GameListSettings
                        GameListSettings.MainImage = GameListSettingsData.MainImage;
                        GameListSettings.Thumbnail = GameListSettingsData.Thumbnail;
                        GameListSettings.Marquee = GameListSettingsData.Marquee;
                        GameListSettings.Video = GameListSettingsData.Video;
                        GameListSettings.Manual = GameListSettingsData.Manual;
                    }
                }
                catch
                {
                    MessageBox.Show("Error with appSettings.xml file.");
                }
            }

            //return reply;
        }

    }
}
