using System.Data;

namespace ScrapeEdit
{
    public partial class SettingsControlPanel : UserControl
    {
        AppSettings appSettings;
        ScreenScraperApi ssa;

        public SettingsControlPanel(AppSettings appSet, ScreenScraperApi ssa)
        {
            appSettings = appSet;
            this.ssa = ssa;
            InitializeComponent();
            SetupHandlers_Download();
            SetupHandlers_GameList();

            ProcessChecks_Download();
            ProcessValues_GameList();


            Setup_Global();
            SetupHandlers_Scrape();
            ProcessValues_Scrape();
            ProcessValues_Login();
            ProcessValues_Directory();

        }

        public event Action OnCloseRequested;
        public event Action<bool[]>? OnCloseRequestedWithInfo;
        private bool _ReloadTreeView = false;
        private bool _Reload_SEDIR = false;
        private void btn_SCP_Close_Click(object sender, EventArgs e)
        {
            appSettings.Save();
            OnCloseRequested?.Invoke();
            OnCloseRequestedWithInfo?.Invoke( new bool[]
            { _ReloadTreeView, _Reload_SEDIR } );
            
        }

        #region DownloadSettings
        //DownloadSettings Start
        private void SetupHandlers_Download()
        {
            // Pair each checkbox with its event handler.
            var checkboxHandlers = new (CheckBox checkbox, EventHandler handler)[]
            {
                // Main Thumb settings handlers
                (chk_DLS_box_2d, (s, e) => UpdateSettingAndMainThumb(s, e, value => DownloadSettings.Download_box_2D = value)),
                (chk_DLS_box_2d_back, (s, e) => UpdateSettingAndMainThumb(s, e, value => DownloadSettings.Download_box_2D_back = value)),
                (chk_DLS_box_2d_side, (s, e) => UpdateSettingAndMainThumb(s, e, value => DownloadSettings.Download_box_2D_side = value)),
                (chk_DLS_box_3d, (s, e) => UpdateSettingAndMainThumb(s, e, value => DownloadSettings.Download_box_3D = value)),
                (chk_DLS_box_texture, (s, e) => UpdateSettingAndMainThumb(s, e, value => DownloadSettings.Download_box_texture = value)),
                (chk_DLS_fanart, (s, e) => UpdateSettingAndMainThumb(s, e, value => DownloadSettings.Download_fanart = value)),
                (chk_DLS_mixrbv1, (s, e) => UpdateSettingAndMainThumb(s, e, value => DownloadSettings.Download_mixrbv1 = value)),
                (chk_DLS_mixrbv2, (s, e) => UpdateSettingAndMainThumb(s, e, value => DownloadSettings.Download_mixrbv2 = value)),
                (chk_DLS_ss, (s, e) => UpdateSettingAndMainThumb(s, e, value => DownloadSettings.Download_ss = value)),
                (chk_DLS_sstitle, (s, e) => UpdateSettingAndMainThumb(s, e, value => DownloadSettings.Download_sstitle = value)),
                (chk_DLS_steamgrid, (s, e) => UpdateSettingAndMainThumb(s, e, value => DownloadSettings.Download_steamgrid = value)),
                (chk_DLS_themehs, (s, e) => UpdateSettingAndMainThumb(s, e, value => DownloadSettings.Download_themehs = value)),

                // Video settings handlers
                (chk_DLS_video, (s, e) => UpdateSettingAndVideo(s, e, value => DownloadSettings.Download_video = value)),
                (chk_DLS_video_normalized, (s, e) => UpdateSettingAndVideo(s, e, value => DownloadSettings.Download_video_normalized = value)),

                // Marquee settings handlers
                (chk_DLS_wheel, (s, e) => UpdateSettingAndMarquee(s, e, value => DownloadSettings.Download_wheel = value)),
                (chk_DLS_wheel_hd, (s, e) => UpdateSettingAndMarquee(s, e, value => DownloadSettings.Download_wheel_hd = value)),
                (chk_DLS_wheel_carbon, (s, e) => UpdateSettingAndMarquee(s, e, value => DownloadSettings.Download_wheel_carbon = value)),
                (chk_DLS_wheel_steel, (s, e) => UpdateSettingAndMarquee(s, e, value => DownloadSettings.Download_wheel_steel = value)),
                (chk_DLS_screenmarquee, (s, e) => UpdateSettingAndMarquee(s, e, value => DownloadSettings.Download_screenmarquee = value)),
                (chk_DLS_screenmarqueesmall, (s, e) => UpdateSettingAndMarquee(s, e, value => DownloadSettings.Download_screenmarqueesmall = value)),

                // Setting without toggling additional options.
                (chk_DLS_manuel, (s, e) => DownloadSettings.Downlaod_manuel = ((CheckBox)s).Checked)
            };

            foreach (var (chk, handler) in checkboxHandlers)
            {
                chk.CheckedChanged += handler;
            }
        }

        /// <summary>
        /// Updates a setting via the provided setter and applies MainThumb toggling.
        /// </summary>
        private void UpdateSettingAndMainThumb(object sender, EventArgs e, Action<bool> setter)
        {
            var chk = sender as CheckBox;
            setter(chk.Checked);
            ToggleGameListOption(chk,
                text => { GameListSettings.Add_MainThumb_Options(text); },
                text => { GameListSettings.Remove_MainThumb_Options(text); });
            UpdateMainThumb();
        }

        /// <summary>
        /// Updates a setting via the provided setter and applies Marquee toggling.
        /// </summary>
        private void UpdateSettingAndMarquee(object sender, EventArgs e, Action<bool> setter)
        {
            var chk = sender as CheckBox;
            setter(chk.Checked);
            ToggleGameListOption(chk,
                text => { GameListSettings.Add_Marquee_Options(text); },
                text => { GameListSettings.Remove_Marquee_Options(text); });
            UpdateMarquee();
        }

        /// <summary>
        /// Updates a setting via the provided setter and applies Video toggling.
        /// </summary>
        private void UpdateSettingAndVideo(object sender, EventArgs e, Action<bool> setter)
        {
            var chk = sender as CheckBox;
            setter(chk.Checked);
            ToggleGameListOption(chk,
                text => { GameListSettings.Add_Video_Option(text); },
                text => { GameListSettings.Remove_Video_Option(text); });

            UpdateVideo();
        }

        /// <summary>
        /// Generic method to either add or remove a game list option based on the CheckBox's state.
        /// </summary>
        private void ToggleGameListOption(CheckBox chk, Action<string> addOption, Action<string> removeOption)
        {
            if (chk.Checked)
                addOption(chk.Text);
            else
                removeOption(chk.Text);
        }

        private void btn_DLS_All_Click(object sender, EventArgs e)
        {
            foreach (Control c in gb_SCP_Download.Controls)
            {
                if (c is CheckBox chk && chk.Text != "Clear")
                {
                    chk.Checked = true;
                }
            }
        }
        private bool clearing = false;
        private void btn_DLS_Clear_Click(object sender, EventArgs e)
        {
            clearing = true;

            foreach (Control c in gb_SCP_Download.Controls)
            {
                if (c is CheckBox chk)
                {
                    chk.Checked = false;
                }
            }

            clearing = false;
        }

        public void ProcessChecks_Download()
        {
            // Synchronize UI checkboxes with the current settings.
            chk_DLS_box_2d.Checked = DownloadSettings.Download_box_2D;
            chk_DLS_box_2d_back.Checked = DownloadSettings.Download_box_2D_back;
            chk_DLS_box_2d_side.Checked = DownloadSettings.Download_box_2D_side;
            chk_DLS_box_3d.Checked = DownloadSettings.Download_box_3D;
            chk_DLS_box_texture.Checked = DownloadSettings.Download_box_texture;
            chk_DLS_fanart.Checked = DownloadSettings.Download_fanart;
            chk_DLS_manuel.Checked = DownloadSettings.Downlaod_manuel;
            chk_DLS_mixrbv1.Checked = DownloadSettings.Download_mixrbv1;
            chk_DLS_mixrbv2.Checked = DownloadSettings.Download_mixrbv2;
            chk_DLS_screenmarquee.Checked = DownloadSettings.Download_screenmarquee;
            chk_DLS_screenmarqueesmall.Checked = DownloadSettings.Download_screenmarqueesmall;
            chk_DLS_ss.Checked = DownloadSettings.Download_ss;
            chk_DLS_sstitle.Checked = DownloadSettings.Download_sstitle;
            chk_DLS_steamgrid.Checked = DownloadSettings.Download_steamgrid;
            chk_DLS_themehs.Checked = DownloadSettings.Download_themehs;
            chk_DLS_video.Checked = DownloadSettings.Download_video;
            chk_DLS_video_normalized.Checked = DownloadSettings.Download_video_normalized;
            chk_DLS_wheel.Checked = DownloadSettings.Download_wheel;
            chk_DLS_wheel_hd.Checked = DownloadSettings.Download_wheel_hd;
            chk_DLS_wheel_carbon.Checked = DownloadSettings.Download_wheel_carbon;
            chk_DLS_wheel_steel.Checked = DownloadSettings.Download_wheel_steel;
        }

        //DownloadSettings End
        #endregion downloadsettings

        #region GameListSettings
        public void UpdateMainThumb()
        {
            cb_GLS_MainImage.Items.Clear();
            cb_GLS_Thumb.Items.Clear();

            // Sort and add MainThumb options.
            var sortedMainThumbOptions = GameListSettings.Main_Thumb_Options.OrderBy(option => option).ToArray();
            if (sortedMainThumbOptions == null || sortedMainThumbOptions.Length == 0)
            {
                cb_GLS_MainImage.Items.Add("No Main Thumb Options Available");
                cb_GLS_MainImage.Text = "No Main Thumb Options Available";
                cb_GLS_Thumb.Items.Add("No Main Thumb Options Available");
                cb_GLS_Thumb.Text = "No Main Thumb Options Available";
                return;
            }

            cb_GLS_MainImage.Items.AddRange(sortedMainThumbOptions);
            cb_GLS_MainImage.Text = GameListSettings.MainImage;

            cb_GLS_Thumb.Items.AddRange(sortedMainThumbOptions);
            cb_GLS_Thumb.Text = GameListSettings.Thumbnail;

        }

        void UpdateMarquee()
        {
            cb_GLS_Marquee.Items.Clear();
            // Sort and add Marquee options.
            var sortedMarqueeOptions = GameListSettings.Marquee_Options.OrderBy(option => option).ToArray();
            if (sortedMarqueeOptions == null || sortedMarqueeOptions.Length == 0)
            {
                cb_GLS_Marquee.Items.Add("No Marquee Options Available");
                cb_GLS_Marquee.Text = "No Marquee Options Available";
                return;
            }

            cb_GLS_Marquee.Items.AddRange(sortedMarqueeOptions);
            cb_GLS_Marquee.Text = GameListSettings.Marquee;
        }
        void UpdateVideo()
        {
            cb_GLS_Video.Items.Clear();
            // Sort and add Video options.
            var sortedVideoOptions = GameListSettings.Video_Options.OrderBy(option => option).ToArray();
            if(sortedVideoOptions == null || sortedVideoOptions.Length == 0)
            {
                cb_GLS_Video.Items.Add("No Video Options Available");
                cb_GLS_Video.Text = "No Video Options Available";
                return;
            }

            cb_GLS_Video.Items.AddRange(sortedVideoOptions);
            cb_GLS_Video.Text = GameListSettings.Video;
        }

        public void ProcessValues_GameList()
        {
            UpdateMainThumb();
            UpdateMarquee();
            UpdateVideo();

            // Sync the manual checkbox.
            chk_GLS_Manual.Checked = GameListSettings.Manual;
        }

        void SetupHandlers_GameList()
        {
            // Use lambda expressions directly in the event subscription to update properties.
            cb_GLS_MainImage.SelectedIndexChanged += (s, e) =>
                GameListSettings.MainImage = cb_GLS_MainImage.Text;

            cb_GLS_Thumb.SelectedIndexChanged += (s, e) =>
                GameListSettings.Thumbnail = cb_GLS_Thumb.Text;

            cb_GLS_Marquee.SelectedIndexChanged += (s, e) =>
                GameListSettings.Marquee = cb_GLS_Marquee.Text;

            cb_GLS_Video.SelectedIndexChanged += (s, e) =>
                GameListSettings.Video = cb_GLS_Video.Text;

            chk_GLS_Manual.CheckedChanged += (s, e) =>
                GameListSettings.Manual = chk_GLS_Manual.Checked;
        }

        #endregion GameListSettings

        #region GlobalSettings
        private void Setup_Global()
        {
            cb_Global_lang.Items.AddRange(GlobalDefaults.GetAllLanguages().ToArray());
            cb_Global_region.Items.AddRange(GlobalDefaults.GetAllRegions().ToArray());
            cb_Global_lang.Text = GlobalDefaults.DefaultLanguage;
            cb_Global_region.Text = GlobalDefaults.DefaultRegion;
            cb_Global_lang.TextChanged += Cb_Global_lang_TextChanged;
            cb_Global_region.TextChanged += Cb_Global_region_TextChanged;

        }

        private void Cb_Global_region_TextChanged(object? sender, EventArgs e)
        {
            GlobalDefaults.DefaultRegion = cb_Global_region.Text;
        }

        private void Cb_Global_lang_TextChanged(object? sender, EventArgs e)
        {
            GlobalDefaults.DefaultLanguage = cb_Global_lang.Text;
        }
        #endregion GlobalSettings

        #region Scrape Settings
        public void SetupHandlers_Scrape()
        {
            chk_SS_CRC32.CheckedChanged += Chk_SS_CRC32_CheckedChanged;
            chk_SS_MD5.CheckedChanged += Chk_SS_MD5_CheckedChanged;
            chk_SS_Rename.CheckedChanged += Cb_SS_Rename_CheckedChanged;
            chk_SS_Prompt.CheckedChanged += Cb_SS_Prompt_CheckedChanged;
            chk_SS_SHA1.CheckedChanged += Chk_SS_SHA1_CheckedChanged;
            chk_SS_30Day.CheckedChanged += Chk_SS_30Day_CheckedChanged;
        }

        private void Chk_SS_30Day_CheckedChanged(object? sender, EventArgs e)
        {
            ScrapeSettings._30Day = chk_SS_30Day.Checked;
        }

        private bool suppressEvents = false; // Flag to prevent event recursion

        private void Chk_SS_CRC32_CheckedChanged(object? sender, EventArgs e)
        {
            if (suppressEvents) return;

            suppressEvents = true;
            if (chk_SS_CRC32.Checked)
            {
                chk_SS_MD5.Checked = false;
                chk_SS_SHA1.Checked = false;

                ScrapeSettings.UseCRC32 = true;
                ScrapeSettings.UseMD5 = false;
                ScrapeSettings.UseSHA1 = false;
            }
            suppressEvents = false;
        }

        private void Chk_SS_MD5_CheckedChanged(object? sender, EventArgs e)
        {
            if (suppressEvents) return;

            suppressEvents = true;
            if (chk_SS_MD5.Checked)
            {
                chk_SS_CRC32.Checked = false;
                chk_SS_SHA1.Checked = false;

                ScrapeSettings.UseCRC32 = false;
                ScrapeSettings.UseMD5 = true;
                ScrapeSettings.UseSHA1 = false;
            }
            suppressEvents = false;
        }

        private void Chk_SS_SHA1_CheckedChanged(object? sender, EventArgs e)
        {
            if (suppressEvents) return;

            suppressEvents = true;
            if (chk_SS_SHA1.Checked)
            {
                chk_SS_MD5.Checked = false;
                chk_SS_CRC32.Checked = false;

                ScrapeSettings.UseCRC32 = false;
                ScrapeSettings.UseMD5 = false;
                ScrapeSettings.UseSHA1 = true;
            }
            suppressEvents = false;
        }

        private void Cb_SS_Prompt_CheckedChanged(object? sender, EventArgs e)
        {
            ScrapeSettings.AskForClarity = chk_SS_Prompt.Checked;
        }

        private void Cb_SS_Rename_CheckedChanged(object? sender, EventArgs e)
        {
            ScrapeSettings.RenameRoms = chk_SS_Rename.Checked;
        }

        void ProcessValues_Scrape()
        {
            chk_SS_Rename.Checked = ScrapeSettings.RenameRoms;
            chk_SS_Prompt.Checked = ScrapeSettings.AskForClarity;
            chk_SS_MD5.Checked = ScrapeSettings.UseMD5;
            chk_SS_CRC32.Checked = ScrapeSettings.UseCRC32;
            chk_SS_30Day.Checked = ScrapeSettings._30Day;
        }


        #endregion Scrape Settings

        #region Login

        bool RetryCredSave = false;
        private async void btn_SSL_Save_Click(object sender, EventArgs e)
        {
            string UserName = tb_SSL_Username.Text;
            string Password = tb_SSL_Password.Text;

            // Validate (Example: Ensure fields are not empty)
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (testResult)
            {
                appSettings.SetUserName(UserName);
                appSettings.SetPassword(Password);
                appSettings.WorkingCreds = true;
                appSettings.Save();
            }
            else
            {
                if (!RetryCredSave)
                {
                    RetryCredSave = true;
                    bool result = await TestCredentialsAsync_Internal();
                    if (result)
                        btn_SSL_Save_Click(sender, e);
                    return;
                }
                else
                    MessageBox.Show("Please try a different username and/or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task<bool> TestCredentialsAsync_Internal()
        {
            string UserName = tb_SSL_Username.Text;
            string Password = tb_SSL_Password.Text;

            // Validate input
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            lbl_SSL_TestResult.Text = "Status: Testing...";
            lbl_SSL_TestResult.BackColor = Color.LightYellow;

            bool result = await ssa.TestCredentialsAsync(UserName, Password);
            testResult = result; // update global flag
            UpdatePassFail(result);

            return result;
        }

        public void ProcessValues_Login()
        {
            if (!string.IsNullOrEmpty(SessionSettings.UserName))
                tb_SSL_Username.Text = SessionSettings.UserName;
            if (!string.IsNullOrEmpty(SessionSettings.Password))
                tb_SSL_Password.Text = SessionSettings.Password;

            UpdatePassFail(appSettings.WorkingCreds);


        }

        bool testResult = false;
        private async void btn_SSL_LoginTest_Click(object sender, EventArgs e)
        {
            await TestCredentialsAsync_Internal();
        }

        void UpdatePassFail(bool test)
        {
            lbl_SSL_TestResult.Text = test ? "Status: Valid" : "Status: Invalid";
            lbl_SSL_TestResult.BackColor = test ? Color.LightGreen : Color.LightCoral;
        }

        #endregion Login

        #region Directory

        public void ProcessValues_Directory()
        {
            lbl_DIR_Root.Text = SessionSettings.RomDirectory;
            lbl_DIR_Cache.Text = SessionSettings.SettingsFolder;
        }

        private void btn_DIR_Root_Click(object sender, EventArgs e)
        {
            
            string folder = GetUserDir();
            if (!string.IsNullOrWhiteSpace(folder))
            {
                if (folder != SessionSettings.RomDirectory) //user can setup new location for roms
                {
                    var result = MessageBox.Show("A new ROM dir has been selected.\nROM structure will now be refreshed! \nPress NO to cancel change!", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        appSettings.SetRomDir(folder);
                        _ReloadTreeView = true;
                        ProcessValues_Directory();
                    }
                    else
                    { 
                        
                    }    
                }
            }
        }
        private void btn_DIR_SE_Click(object sender, EventArgs e)
        {
            string folder = GetUserDir();
            if (!string.IsNullOrWhiteSpace(folder))
            {
                if (folder != SessionSettings.RomDirectory) //user can setup new location for roms
                {
                    _Reload_SEDIR = true;
                    appSettings.SetSEDir(folder);
                    ProcessValues_Directory();
                }
            }
        }
        string GetUserDir()
        {

            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return null;


                if (string.IsNullOrEmpty(dialog.SelectedPath))
                    return null;

                return dialog.SelectedPath;

            }
        }

        private void btn_DIR_AppDir_Click(object sender, EventArgs e)
        {
            string appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ScrapeEdit");

            //open the directory in explorer
            System.Diagnostics.Process.Start("explorer.exe", appDir);
        }
        #endregion Directory

    }
}
