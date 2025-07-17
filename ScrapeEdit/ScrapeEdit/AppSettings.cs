using System.IO;
using System.Xml.Serialization;

namespace ScrapeEdit
{
    public static class SessionSettings
    { 
        public static string UserName { get; set; } = "";
        public static string Password { get; set; } = "";
        public static string RomDirectory { get; set; } = "";
        public static string SettingsFolder { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ScrapeEdit");
        public static string SEDirectory { 
            get 
            { 
                return SettingsFolder + "\\Cache"; 
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
        public static bool CheckForUpdateOnStartup { get; set; } = true;
        public static int MaxThreads { get; set; } = 1;
        public static int X_Days { get; set; } = 1;

    }

    [XmlRoot("appSettings")]
    public class AppSettings
    {
        [XmlElement("SettingsFolder")]
        public string SettingsFolder
        {
            get => SessionSettings.SettingsFolder;
            set => SessionSettings.SettingsFolder = value;
        }

        private string SettingsFilePath => Path.Combine(this.SettingsFolder, "appSettings.xml");

        //private static string SettingsFilePath => Path.Combine(SettingsFolder, "appSettings.xml");

        [XmlElement("romDirectory")]
        public string RomDirectory {
            get { return SessionSettings.RomDirectory; }
            set { SessionSettings.RomDirectory = value; }
        
        }

        //[XmlElement("seDirectory")]
        //public string SEDirectory
        //{
        //    get { return SessionSettings.SEDirectory; }
        //    set { SessionSettings.SettingsFolder = Directory.GetParent(value).ToString(); }

        //}
        [XmlElement("userName")]
        public string UserName 
        { get { return SessionSettings.UserName;  }
            set { SessionSettings.UserName = value; } 
        }

        [XmlElement("passWord")]
        public string Password {
            get { return SessionSettings.Password;  }
            set { SessionSettings.Password = value; } 
        }

        [XmlElement]
        public bool WorkingCreds = false;

        [XmlElement("CheckForUpdateOnStartup")]
        public bool CheckForUpdateOnStartup
        {
            get
            {
                return SessionSettings.CheckForUpdateOnStartup;
            }
            set 
            {
                SessionSettings.CheckForUpdateOnStartup = value;
            }
        }
        [XmlElement("MaxThreads")]

        public int MaxThreads
        { 
            get { return SessionSettings.MaxThreads; } 
            set { SessionSettings.MaxThreads = value; } 
        }

        [XmlElement("X_Days")]
        public int X_Days
        {
            get { return SessionSettings.X_Days; }
            set { SessionSettings.X_Days = value; }
        }

        public void SetUserName(string userName)
        {
            //UserName = userName;
            SessionSettings.UserName = userName;
        }
        public void SetPassword(string password)
        {
            //Password = password;
            SessionSettings.Password = password;
        }
        public void SetRomDir(string filePath)
        {
            //RomDirectory = filePath;
            SessionSettings.RomDirectory = filePath;
            Save();
        }
        public void SetSettingsDir(string filePath)
        { 
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

            if(firstLoad)
            {
                firstLoad = false;
                Load();
            }
        }

        private bool firstLoad = false;
        public void Load()//string[] Load()
        {
            //string[] reply = new string[2];

            if (!File.Exists(SettingsFilePath))
            {
                firstLoad = true;
                Save(); 
            }
            else
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(AppSettings));
                    using (var reader = new StreamReader(SettingsFilePath))
                    {
                        var settings = (AppSettings)serializer.Deserialize(reader);



                        //this.RomDirectory = settings.RomDirectory;
                        ////this.SEDirectory = settings.SEDirectory;
                        //this.UserName = settings.UserName;
                        //this.Password = settings.Password;
                        this.WorkingCreds = settings.WorkingCreds;

                        SessionSettings.UserName = settings.UserName;
                        SessionSettings.Password = settings.Password;
                        SessionSettings.RomDirectory = settings.RomDirectory;
                        SessionSettings.SettingsFolder = settings.SettingsFolder;
                        SessionSettings.CheckForUpdateOnStartup = settings.CheckForUpdateOnStartup;

                        if (!Directory.Exists(SessionSettings.SEDirectory))
                        {
                            Directory.CreateDirectory(SessionSettings.SEDirectory);
                            Directory.CreateDirectory(SessionSettings.SEDirectory_ROM);
                        }

                        // Apply loaded values to the static ScrapeSettings class
                        ScrapeSettings.RenameRoms = settings.ScrapeSettingsData.RenameRoms;
                        ScrapeSettings.AskForClarity = settings.ScrapeSettingsData.AskForClarity;
                        ScrapeSettings.UseCRC32 = settings.ScrapeSettingsData.UseCRC32;
                        ScrapeSettings.UseMD5 = settings.ScrapeSettingsData.UseMD5;
                        ScrapeSettings.UseSHA1 = settings.ScrapeSettingsData.UseSHA1;
                        ScrapeSettings.useCached_XML = settings.ScrapeSettingsData.useCached_XML;
                        ScrapeSettings.X_Days = settings.ScrapeSettingsData.X_Days;
                        ScrapeSettings.MaxThreads = settings.ScrapeSettingsData.MaxThreads;

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
