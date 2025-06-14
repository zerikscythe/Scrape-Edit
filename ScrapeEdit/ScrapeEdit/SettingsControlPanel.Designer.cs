using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ScrapeEdit
{
    partial class SettingsControlPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btn_SCP_Close = new Button();
            chk_DLS_sstitle = new CheckBox();
            chk_DLS_ss = new CheckBox();
            chk_DLS_fanart = new CheckBox();
            chk_DLS_video = new CheckBox();
            chk_DLS_video_normalized = new CheckBox();
            chk_DLS_themehs = new CheckBox();
            chk_DLS_screenmarquee = new CheckBox();
            chk_DLS_screenmarqueesmall = new CheckBox();
            chk_DLS_manuel = new CheckBox();
            chk_DLS_steamgrid = new CheckBox();
            chk_DLS_wheel = new CheckBox();
            chk_DLS_wheel_carbon = new CheckBox();
            chk_DLS_wheel_steel = new CheckBox();
            chk_DLS_box_2d = new CheckBox();
            chk_DLS_box_2d_side = new CheckBox();
            chk_DLS_box_2d_back = new CheckBox();
            chk_DLS_box_texture = new CheckBox();
            chk_DLS_box_3d = new CheckBox();
            chk_DLS_mixrbv1 = new CheckBox();
            chk_DLS_mixrbv2 = new CheckBox();
            gb_SCP_Download = new GroupBox();
            btn_DLS_All = new Button();
            btn_DLS_Clear = new Button();
            chk_DLS_wheel_hd = new CheckBox();
            chk_SS_Rename = new CheckBox();
            chk_SS_Prompt = new CheckBox();
            chk_SS_CRC32 = new CheckBox();
            chk_SS_MD5 = new CheckBox();
            chk_SS_SHA1 = new CheckBox();
            chk_SS_30Day = new CheckBox();
            gb_SCP_Scrape = new GroupBox();
            tb_SSL_Username = new TextBox();
            tb_SSL_Password = new TextBox();
            lbl_SSL_Username = new Label();
            lbl_SSL_Password = new Label();
            btn_SSL_LoginTest = new Button();
            gb_SCP_Login = new GroupBox();
            lbl_SSL_TestResult = new Label();
            cb_GLS_MainImage = new ComboBox();
            cb_GLS_Thumb = new ComboBox();
            cb_GLS_Marquee = new ComboBox();
            cb_GLS_Video = new ComboBox();
            chk_GLS_Manual = new CheckBox();
            lbl_GLS_MainImage = new Label();
            lbl_GLS_Thumb = new Label();
            lbl_GLS_Marquee = new Label();
            lbl_GLS_Video = new Label();
            gb_SCP_GameList = new GroupBox();
            cb_Global_region = new ComboBox();
            cb_Global_lang = new ComboBox();
            lbl_Global_region = new Label();
            lbl_Global_lang = new Label();
            gb_SCP_Global = new GroupBox();
            gb_SCP_Directory = new GroupBox();
            btn_DIR_AppDir = new Button();
            lbl_DIR_Cache = new Label();
            lbl_DIR_Root = new Label();
            btn_SCP_Cache = new Button();
            btn_DIR_Root = new Button();
            btn_SSL_Save = new Button();
            gb_SCP_Download.SuspendLayout();
            gb_SCP_Scrape.SuspendLayout();
            gb_SCP_Login.SuspendLayout();
            gb_SCP_GameList.SuspendLayout();
            gb_SCP_Global.SuspendLayout();
            gb_SCP_Directory.SuspendLayout();
            SuspendLayout();
            // 
            // btn_SCP_Close
            // 
            btn_SCP_Close.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btn_SCP_Close.Location = new Point(1020, 474);
            btn_SCP_Close.Name = "btn_SCP_Close";
            btn_SCP_Close.Size = new Size(75, 23);
            btn_SCP_Close.TabIndex = 0;
            btn_SCP_Close.Text = "Close";
            btn_SCP_Close.UseVisualStyleBackColor = true;
            btn_SCP_Close.Click += btn_SCP_Close_Click;
            // 
            // chk_DLS_sstitle
            // 
            chk_DLS_sstitle.AutoSize = true;
            chk_DLS_sstitle.Location = new Point(6, 20);
            chk_DLS_sstitle.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_sstitle.Name = "chk_DLS_sstitle";
            chk_DLS_sstitle.Size = new Size(56, 19);
            chk_DLS_sstitle.TabIndex = 1;
            chk_DLS_sstitle.Text = "sstitle";
            chk_DLS_sstitle.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_ss
            // 
            chk_DLS_ss.AutoSize = true;
            chk_DLS_ss.Location = new Point(6, 40);
            chk_DLS_ss.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_ss.Name = "chk_DLS_ss";
            chk_DLS_ss.Size = new Size(36, 19);
            chk_DLS_ss.TabIndex = 3;
            chk_DLS_ss.Text = "ss";
            chk_DLS_ss.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_fanart
            // 
            chk_DLS_fanart.AutoSize = true;
            chk_DLS_fanart.Location = new Point(6, 60);
            chk_DLS_fanart.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_fanart.Name = "chk_DLS_fanart";
            chk_DLS_fanart.Size = new Size(57, 19);
            chk_DLS_fanart.TabIndex = 4;
            chk_DLS_fanart.Text = "fanart";
            chk_DLS_fanart.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_video
            // 
            chk_DLS_video.AutoSize = true;
            chk_DLS_video.Location = new Point(6, 80);
            chk_DLS_video.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_video.Name = "chk_DLS_video";
            chk_DLS_video.Size = new Size(55, 19);
            chk_DLS_video.TabIndex = 5;
            chk_DLS_video.Text = "video";
            chk_DLS_video.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_video_normalized
            // 
            chk_DLS_video_normalized.AutoSize = true;
            chk_DLS_video_normalized.Location = new Point(6, 100);
            chk_DLS_video_normalized.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_video_normalized.Name = "chk_DLS_video_normalized";
            chk_DLS_video_normalized.Size = new Size(119, 19);
            chk_DLS_video_normalized.TabIndex = 6;
            chk_DLS_video_normalized.Text = "video-normalized";
            chk_DLS_video_normalized.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_themehs
            // 
            chk_DLS_themehs.AutoSize = true;
            chk_DLS_themehs.Location = new Point(6, 120);
            chk_DLS_themehs.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_themehs.Name = "chk_DLS_themehs";
            chk_DLS_themehs.Size = new Size(72, 19);
            chk_DLS_themehs.TabIndex = 7;
            chk_DLS_themehs.Text = "themehs";
            chk_DLS_themehs.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_screenmarquee
            // 
            chk_DLS_screenmarquee.AutoSize = true;
            chk_DLS_screenmarquee.Location = new Point(6, 160);
            chk_DLS_screenmarquee.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_screenmarquee.Name = "chk_DLS_screenmarquee";
            chk_DLS_screenmarquee.Size = new Size(107, 19);
            chk_DLS_screenmarquee.TabIndex = 8;
            chk_DLS_screenmarquee.Text = "screenmarquee";
            chk_DLS_screenmarquee.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_screenmarqueesmall
            // 
            chk_DLS_screenmarqueesmall.AutoSize = true;
            chk_DLS_screenmarqueesmall.Location = new Point(6, 200);
            chk_DLS_screenmarqueesmall.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_screenmarqueesmall.Name = "chk_DLS_screenmarqueesmall";
            chk_DLS_screenmarqueesmall.Size = new Size(135, 19);
            chk_DLS_screenmarqueesmall.TabIndex = 9;
            chk_DLS_screenmarqueesmall.Text = "screenmarqueesmall";
            chk_DLS_screenmarqueesmall.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_manuel
            // 
            chk_DLS_manuel.AutoSize = true;
            chk_DLS_manuel.Location = new Point(6, 140);
            chk_DLS_manuel.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_manuel.Name = "chk_DLS_manuel";
            chk_DLS_manuel.Size = new Size(66, 19);
            chk_DLS_manuel.TabIndex = 10;
            chk_DLS_manuel.Text = "manuel";
            chk_DLS_manuel.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_steamgrid
            // 
            chk_DLS_steamgrid.AutoSize = true;
            chk_DLS_steamgrid.Location = new Point(6, 180);
            chk_DLS_steamgrid.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_steamgrid.Name = "chk_DLS_steamgrid";
            chk_DLS_steamgrid.Size = new Size(79, 19);
            chk_DLS_steamgrid.TabIndex = 11;
            chk_DLS_steamgrid.Text = "steamgrid";
            chk_DLS_steamgrid.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_wheel
            // 
            chk_DLS_wheel.AutoSize = true;
            chk_DLS_wheel.Location = new Point(6, 220);
            chk_DLS_wheel.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_wheel.Name = "chk_DLS_wheel";
            chk_DLS_wheel.Size = new Size(57, 19);
            chk_DLS_wheel.TabIndex = 12;
            chk_DLS_wheel.Text = "wheel";
            chk_DLS_wheel.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_wheel_carbon
            // 
            chk_DLS_wheel_carbon.AutoSize = true;
            chk_DLS_wheel_carbon.Location = new Point(6, 260);
            chk_DLS_wheel_carbon.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_wheel_carbon.Name = "chk_DLS_wheel_carbon";
            chk_DLS_wheel_carbon.Size = new Size(99, 19);
            chk_DLS_wheel_carbon.TabIndex = 13;
            chk_DLS_wheel_carbon.Text = "wheel-carbon";
            chk_DLS_wheel_carbon.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_wheel_steel
            // 
            chk_DLS_wheel_steel.AutoSize = true;
            chk_DLS_wheel_steel.Location = new Point(6, 280);
            chk_DLS_wheel_steel.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_wheel_steel.Name = "chk_DLS_wheel_steel";
            chk_DLS_wheel_steel.Size = new Size(86, 19);
            chk_DLS_wheel_steel.TabIndex = 14;
            chk_DLS_wheel_steel.Text = "wheel-steel";
            chk_DLS_wheel_steel.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_box_2d
            // 
            chk_DLS_box_2d.AutoSize = true;
            chk_DLS_box_2d.Location = new Point(6, 300);
            chk_DLS_box_2d.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_box_2d.Name = "chk_DLS_box_2d";
            chk_DLS_box_2d.Size = new Size(65, 19);
            chk_DLS_box_2d.TabIndex = 15;
            chk_DLS_box_2d.Text = "box-2D";
            chk_DLS_box_2d.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_box_2d_side
            // 
            chk_DLS_box_2d_side.AutoSize = true;
            chk_DLS_box_2d_side.Location = new Point(6, 320);
            chk_DLS_box_2d_side.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_box_2d_side.Name = "chk_DLS_box_2d_side";
            chk_DLS_box_2d_side.Size = new Size(91, 19);
            chk_DLS_box_2d_side.TabIndex = 16;
            chk_DLS_box_2d_side.Text = "box-2D-side";
            chk_DLS_box_2d_side.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_box_2d_back
            // 
            chk_DLS_box_2d_back.AutoSize = true;
            chk_DLS_box_2d_back.Location = new Point(6, 340);
            chk_DLS_box_2d_back.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_box_2d_back.Name = "chk_DLS_box_2d_back";
            chk_DLS_box_2d_back.Size = new Size(95, 19);
            chk_DLS_box_2d_back.TabIndex = 17;
            chk_DLS_box_2d_back.Text = "box-2D-back";
            chk_DLS_box_2d_back.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_box_texture
            // 
            chk_DLS_box_texture.AutoSize = true;
            chk_DLS_box_texture.Location = new Point(6, 360);
            chk_DLS_box_texture.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_box_texture.Name = "chk_DLS_box_texture";
            chk_DLS_box_texture.Size = new Size(88, 19);
            chk_DLS_box_texture.TabIndex = 18;
            chk_DLS_box_texture.Text = "box-texture";
            chk_DLS_box_texture.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_box_3d
            // 
            chk_DLS_box_3d.AutoSize = true;
            chk_DLS_box_3d.Location = new Point(6, 380);
            chk_DLS_box_3d.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_box_3d.Name = "chk_DLS_box_3d";
            chk_DLS_box_3d.Size = new Size(65, 19);
            chk_DLS_box_3d.TabIndex = 19;
            chk_DLS_box_3d.Text = "box-3D";
            chk_DLS_box_3d.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_mixrbv1
            // 
            chk_DLS_mixrbv1.AutoSize = true;
            chk_DLS_mixrbv1.Location = new Point(6, 400);
            chk_DLS_mixrbv1.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_mixrbv1.Name = "chk_DLS_mixrbv1";
            chk_DLS_mixrbv1.Size = new Size(69, 19);
            chk_DLS_mixrbv1.TabIndex = 20;
            chk_DLS_mixrbv1.Text = "mixrbv1";
            chk_DLS_mixrbv1.UseVisualStyleBackColor = true;
            // 
            // chk_DLS_mixrbv2
            // 
            chk_DLS_mixrbv2.AutoSize = true;
            chk_DLS_mixrbv2.Location = new Point(6, 420);
            chk_DLS_mixrbv2.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_mixrbv2.Name = "chk_DLS_mixrbv2";
            chk_DLS_mixrbv2.Size = new Size(69, 19);
            chk_DLS_mixrbv2.TabIndex = 21;
            chk_DLS_mixrbv2.Text = "mixrbv2";
            chk_DLS_mixrbv2.UseVisualStyleBackColor = true;
            // 
            // gb_SCP_Download
            // 
            gb_SCP_Download.Controls.Add(btn_DLS_All);
            gb_SCP_Download.Controls.Add(btn_DLS_Clear);
            gb_SCP_Download.Controls.Add(chk_DLS_wheel_hd);
            gb_SCP_Download.Controls.Add(chk_DLS_screenmarqueesmall);
            gb_SCP_Download.Controls.Add(chk_DLS_sstitle);
            gb_SCP_Download.Controls.Add(chk_DLS_ss);
            gb_SCP_Download.Controls.Add(chk_DLS_fanart);
            gb_SCP_Download.Controls.Add(chk_DLS_video);
            gb_SCP_Download.Controls.Add(chk_DLS_wheel_steel);
            gb_SCP_Download.Controls.Add(chk_DLS_mixrbv2);
            gb_SCP_Download.Controls.Add(chk_DLS_wheel_carbon);
            gb_SCP_Download.Controls.Add(chk_DLS_video_normalized);
            gb_SCP_Download.Controls.Add(chk_DLS_wheel);
            gb_SCP_Download.Controls.Add(chk_DLS_mixrbv1);
            gb_SCP_Download.Controls.Add(chk_DLS_steamgrid);
            gb_SCP_Download.Controls.Add(chk_DLS_themehs);
            gb_SCP_Download.Controls.Add(chk_DLS_manuel);
            gb_SCP_Download.Controls.Add(chk_DLS_box_3d);
            gb_SCP_Download.Controls.Add(chk_DLS_screenmarquee);
            gb_SCP_Download.Controls.Add(chk_DLS_box_texture);
            gb_SCP_Download.Controls.Add(chk_DLS_box_2d);
            gb_SCP_Download.Controls.Add(chk_DLS_box_2d_back);
            gb_SCP_Download.Controls.Add(chk_DLS_box_2d_side);
            gb_SCP_Download.Location = new Point(483, 9);
            gb_SCP_Download.Margin = new Padding(3, 2, 3, 2);
            gb_SCP_Download.Name = "gb_SCP_Download";
            gb_SCP_Download.Padding = new Padding(3, 2, 3, 2);
            gb_SCP_Download.Size = new Size(234, 488);
            gb_SCP_Download.TabIndex = 24;
            gb_SCP_Download.TabStop = false;
            gb_SCP_Download.Text = "Media Download Settings";
            // 
            // btn_DLS_All
            // 
            btn_DLS_All.Location = new Point(153, 436);
            btn_DLS_All.Name = "btn_DLS_All";
            btn_DLS_All.Size = new Size(75, 23);
            btn_DLS_All.TabIndex = 26;
            btn_DLS_All.Text = "Select All";
            btn_DLS_All.UseVisualStyleBackColor = true;
            btn_DLS_All.Click += btn_DLS_All_Click;
            // 
            // btn_DLS_Clear
            // 
            btn_DLS_Clear.Location = new Point(153, 465);
            btn_DLS_Clear.Name = "btn_DLS_Clear";
            btn_DLS_Clear.Size = new Size(75, 23);
            btn_DLS_Clear.TabIndex = 25;
            btn_DLS_Clear.Text = "Clear";
            btn_DLS_Clear.UseVisualStyleBackColor = true;
            btn_DLS_Clear.Click += btn_DLS_Clear_Click;
            // 
            // chk_DLS_wheel_hd
            // 
            chk_DLS_wheel_hd.AutoSize = true;
            chk_DLS_wheel_hd.Location = new Point(6, 240);
            chk_DLS_wheel_hd.Margin = new Padding(3, 2, 3, 2);
            chk_DLS_wheel_hd.Name = "chk_DLS_wheel_hd";
            chk_DLS_wheel_hd.Size = new Size(76, 19);
            chk_DLS_wheel_hd.TabIndex = 24;
            chk_DLS_wheel_hd.Text = "wheel-hd";
            chk_DLS_wheel_hd.UseVisualStyleBackColor = true;
            // 
            // chk_SS_Rename
            // 
            chk_SS_Rename.AutoSize = true;
            chk_SS_Rename.Location = new Point(6, 83);
            chk_SS_Rename.Name = "chk_SS_Rename";
            chk_SS_Rename.Size = new Size(153, 19);
            chk_SS_Rename.TabIndex = 0;
            chk_SS_Rename.Text = "Rename Roms by HASH";
            chk_SS_Rename.UseVisualStyleBackColor = true;
            // 
            // chk_SS_Prompt
            // 
            chk_SS_Prompt.AutoSize = true;
            chk_SS_Prompt.Location = new Point(6, 104);
            chk_SS_Prompt.Name = "chk_SS_Prompt";
            chk_SS_Prompt.Size = new Size(171, 19);
            chk_SS_Prompt.TabIndex = 1;
            chk_SS_Prompt.Text = "Prompt for Name Resolver?";
            chk_SS_Prompt.UseVisualStyleBackColor = true;
            // 
            // chk_SS_CRC32
            // 
            chk_SS_CRC32.AutoSize = true;
            chk_SS_CRC32.Location = new Point(6, 20);
            chk_SS_CRC32.Name = "chk_SS_CRC32";
            chk_SS_CRC32.Size = new Size(83, 19);
            chk_SS_CRC32.TabIndex = 2;
            chk_SS_CRC32.Text = "Use CRC32";
            chk_SS_CRC32.UseVisualStyleBackColor = true;
            // 
            // chk_SS_MD5
            // 
            chk_SS_MD5.AutoSize = true;
            chk_SS_MD5.Location = new Point(6, 41);
            chk_SS_MD5.Name = "chk_SS_MD5";
            chk_SS_MD5.Size = new Size(73, 19);
            chk_SS_MD5.TabIndex = 3;
            chk_SS_MD5.Text = "Use MD5";
            chk_SS_MD5.UseVisualStyleBackColor = true;
            // 
            // chk_SS_SHA1
            // 
            chk_SS_SHA1.AutoSize = true;
            chk_SS_SHA1.Location = new Point(6, 62);
            chk_SS_SHA1.Name = "chk_SS_SHA1";
            chk_SS_SHA1.Size = new Size(77, 19);
            chk_SS_SHA1.TabIndex = 4;
            chk_SS_SHA1.Text = "Use SHA1";
            chk_SS_SHA1.UseVisualStyleBackColor = true;
            // 
            // chk_SS_30Day
            // 
            chk_SS_30Day.AutoSize = true;
            chk_SS_30Day.Location = new Point(6, 125);
            chk_SS_30Day.Name = "chk_SS_30Day";
            chk_SS_30Day.Size = new Size(170, 19);
            chk_SS_30Day.TabIndex = 5;
            chk_SS_30Day.Text = "SS Data <30 days use cache";
            chk_SS_30Day.UseVisualStyleBackColor = true;
            // 
            // gb_SCP_Scrape
            // 
            gb_SCP_Scrape.Controls.Add(chk_SS_CRC32);
            gb_SCP_Scrape.Controls.Add(chk_SS_30Day);
            gb_SCP_Scrape.Controls.Add(chk_SS_Rename);
            gb_SCP_Scrape.Controls.Add(chk_SS_SHA1);
            gb_SCP_Scrape.Controls.Add(chk_SS_Prompt);
            gb_SCP_Scrape.Controls.Add(chk_SS_MD5);
            gb_SCP_Scrape.Location = new Point(277, 9);
            gb_SCP_Scrape.Name = "gb_SCP_Scrape";
            gb_SCP_Scrape.Size = new Size(200, 166);
            gb_SCP_Scrape.TabIndex = 25;
            gb_SCP_Scrape.TabStop = false;
            gb_SCP_Scrape.Text = "Scrape Settings";
            // 
            // tb_SSL_Username
            // 
            tb_SSL_Username.Location = new Point(12, 44);
            tb_SSL_Username.Name = "tb_SSL_Username";
            tb_SSL_Username.Size = new Size(242, 23);
            tb_SSL_Username.TabIndex = 0;
            // 
            // tb_SSL_Password
            // 
            tb_SSL_Password.Location = new Point(12, 97);
            tb_SSL_Password.Name = "tb_SSL_Password";
            tb_SSL_Password.Size = new Size(242, 23);
            tb_SSL_Password.TabIndex = 1;
            // 
            // lbl_SSL_Username
            // 
            lbl_SSL_Username.AutoSize = true;
            lbl_SSL_Username.Location = new Point(12, 21);
            lbl_SSL_Username.Name = "lbl_SSL_Username";
            lbl_SSL_Username.Size = new Size(148, 15);
            lbl_SSL_Username.TabIndex = 2;
            lbl_SSL_Username.Text = "ScreenScraper.fr Username";
            // 
            // lbl_SSL_Password
            // 
            lbl_SSL_Password.AutoSize = true;
            lbl_SSL_Password.Location = new Point(12, 74);
            lbl_SSL_Password.Name = "lbl_SSL_Password";
            lbl_SSL_Password.Size = new Size(145, 15);
            lbl_SSL_Password.TabIndex = 3;
            lbl_SSL_Password.Text = "ScreenScraper.fr Password";
            // 
            // btn_SSL_LoginTest
            // 
            btn_SSL_LoginTest.Location = new Point(12, 129);
            btn_SSL_LoginTest.Name = "btn_SSL_LoginTest";
            btn_SSL_LoginTest.Size = new Size(58, 29);
            btn_SSL_LoginTest.TabIndex = 4;
            btn_SSL_LoginTest.Text = "Test";
            btn_SSL_LoginTest.UseVisualStyleBackColor = true;
            btn_SSL_LoginTest.Click += btn_SSL_LoginTest_Click;
            // 
            // gb_SCP_Login
            // 
            gb_SCP_Login.Controls.Add(btn_SSL_Save);
            gb_SCP_Login.Controls.Add(lbl_SSL_TestResult);
            gb_SCP_Login.Controls.Add(lbl_SSL_Username);
            gb_SCP_Login.Controls.Add(btn_SSL_LoginTest);
            gb_SCP_Login.Controls.Add(tb_SSL_Username);
            gb_SCP_Login.Controls.Add(lbl_SSL_Password);
            gb_SCP_Login.Controls.Add(tb_SSL_Password);
            gb_SCP_Login.Location = new Point(3, 9);
            gb_SCP_Login.Name = "gb_SCP_Login";
            gb_SCP_Login.Size = new Size(268, 166);
            gb_SCP_Login.TabIndex = 26;
            gb_SCP_Login.TabStop = false;
            gb_SCP_Login.Text = "Login Settings";
            // 
            // lbl_SSL_TestResult
            // 
            lbl_SSL_TestResult.AutoSize = true;
            lbl_SSL_TestResult.Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_SSL_TestResult.Location = new Point(76, 136);
            lbl_SSL_TestResult.Name = "lbl_SSL_TestResult";
            lbl_SSL_TestResult.Size = new Size(102, 15);
            lbl_SSL_TestResult.TabIndex = 5;
            lbl_SSL_TestResult.Text = "Status: Unknown";
            // 
            // cb_GLS_MainImage
            // 
            cb_GLS_MainImage.FormattingEnabled = true;
            cb_GLS_MainImage.Items.AddRange(new object[] { "none", "sstitle", "ss", "fanart", "themehs", "steamgrid", "wheel", "wheel_carbon", "wheel_steel", "box_2D", "box_2D_side", "box_2D_back", "box_texture", "box_3D", "mixrbv1", "mixrbv2" });
            cb_GLS_MainImage.Location = new Point(6, 35);
            cb_GLS_MainImage.Name = "cb_GLS_MainImage";
            cb_GLS_MainImage.Size = new Size(151, 23);
            cb_GLS_MainImage.TabIndex = 1;
            // 
            // cb_GLS_Thumb
            // 
            cb_GLS_Thumb.FormattingEnabled = true;
            cb_GLS_Thumb.Items.AddRange(new object[] { "none", "sstitle", "ss", "fanart", "themehs", "steamgrid", "wheel", "wheel_carbon", "wheel_steel", "box_2D", "box_2D_side", "box_2D_back", "box_texture", "box_3D", "mixrbv1", "mixrbv2" });
            cb_GLS_Thumb.Location = new Point(6, 77);
            cb_GLS_Thumb.Name = "cb_GLS_Thumb";
            cb_GLS_Thumb.Size = new Size(151, 23);
            cb_GLS_Thumb.TabIndex = 2;
            // 
            // cb_GLS_Marquee
            // 
            cb_GLS_Marquee.FormattingEnabled = true;
            cb_GLS_Marquee.Items.AddRange(new object[] { "none", "screenmarquee", "screenmarqueesmall" });
            cb_GLS_Marquee.Location = new Point(6, 122);
            cb_GLS_Marquee.Name = "cb_GLS_Marquee";
            cb_GLS_Marquee.Size = new Size(151, 23);
            cb_GLS_Marquee.TabIndex = 3;
            // 
            // cb_GLS_Video
            // 
            cb_GLS_Video.FormattingEnabled = true;
            cb_GLS_Video.Items.AddRange(new object[] { "no", "video", "video-normalized" });
            cb_GLS_Video.Location = new Point(6, 165);
            cb_GLS_Video.Name = "cb_GLS_Video";
            cb_GLS_Video.Size = new Size(151, 23);
            cb_GLS_Video.TabIndex = 4;
            // 
            // chk_GLS_Manual
            // 
            chk_GLS_Manual.AutoSize = true;
            chk_GLS_Manual.Location = new Point(6, 194);
            chk_GLS_Manual.Name = "chk_GLS_Manual";
            chk_GLS_Manual.Size = new Size(91, 19);
            chk_GLS_Manual.TabIndex = 5;
            chk_GLS_Manual.Text = "Link Manual";
            chk_GLS_Manual.UseVisualStyleBackColor = true;
            // 
            // lbl_GLS_MainImage
            // 
            lbl_GLS_MainImage.AutoSize = true;
            lbl_GLS_MainImage.Location = new Point(6, 19);
            lbl_GLS_MainImage.Name = "lbl_GLS_MainImage";
            lbl_GLS_MainImage.Size = new Size(70, 15);
            lbl_GLS_MainImage.TabIndex = 6;
            lbl_GLS_MainImage.Text = "Main Image";
            // 
            // lbl_GLS_Thumb
            // 
            lbl_GLS_Thumb.AutoSize = true;
            lbl_GLS_Thumb.Location = new Point(6, 61);
            lbl_GLS_Thumb.Name = "lbl_GLS_Thumb";
            lbl_GLS_Thumb.Size = new Size(64, 15);
            lbl_GLS_Thumb.TabIndex = 7;
            lbl_GLS_Thumb.Text = "Thumbnail";
            // 
            // lbl_GLS_Marquee
            // 
            lbl_GLS_Marquee.AutoSize = true;
            lbl_GLS_Marquee.Location = new Point(6, 104);
            lbl_GLS_Marquee.Name = "lbl_GLS_Marquee";
            lbl_GLS_Marquee.Size = new Size(54, 15);
            lbl_GLS_Marquee.TabIndex = 8;
            lbl_GLS_Marquee.Text = "Marquee";
            // 
            // lbl_GLS_Video
            // 
            lbl_GLS_Video.AutoSize = true;
            lbl_GLS_Video.Location = new Point(6, 148);
            lbl_GLS_Video.Name = "lbl_GLS_Video";
            lbl_GLS_Video.Size = new Size(37, 15);
            lbl_GLS_Video.TabIndex = 9;
            lbl_GLS_Video.Text = "Video";
            // 
            // gb_SCP_GameList
            // 
            gb_SCP_GameList.Controls.Add(lbl_GLS_MainImage);
            gb_SCP_GameList.Controls.Add(lbl_GLS_Video);
            gb_SCP_GameList.Controls.Add(cb_GLS_MainImage);
            gb_SCP_GameList.Controls.Add(lbl_GLS_Marquee);
            gb_SCP_GameList.Controls.Add(cb_GLS_Thumb);
            gb_SCP_GameList.Controls.Add(lbl_GLS_Thumb);
            gb_SCP_GameList.Controls.Add(cb_GLS_Marquee);
            gb_SCP_GameList.Controls.Add(cb_GLS_Video);
            gb_SCP_GameList.Controls.Add(chk_GLS_Manual);
            gb_SCP_GameList.Location = new Point(723, 9);
            gb_SCP_GameList.Name = "gb_SCP_GameList";
            gb_SCP_GameList.Size = new Size(177, 233);
            gb_SCP_GameList.TabIndex = 27;
            gb_SCP_GameList.TabStop = false;
            gb_SCP_GameList.Text = "GameList Settings";
            // 
            // cb_Global_region
            // 
            cb_Global_region.FormattingEnabled = true;
            cb_Global_region.Location = new Point(6, 34);
            cb_Global_region.Name = "cb_Global_region";
            cb_Global_region.Size = new Size(177, 23);
            cb_Global_region.TabIndex = 1;
            // 
            // cb_Global_lang
            // 
            cb_Global_lang.FormattingEnabled = true;
            cb_Global_lang.Location = new Point(6, 76);
            cb_Global_lang.Name = "cb_Global_lang";
            cb_Global_lang.Size = new Size(177, 23);
            cb_Global_lang.TabIndex = 2;
            // 
            // lbl_Global_region
            // 
            lbl_Global_region.AutoSize = true;
            lbl_Global_region.Location = new Point(6, 18);
            lbl_Global_region.Name = "lbl_Global_region";
            lbl_Global_region.Size = new Size(44, 15);
            lbl_Global_region.TabIndex = 3;
            lbl_Global_region.Text = "Region";
            // 
            // lbl_Global_lang
            // 
            lbl_Global_lang.AutoSize = true;
            lbl_Global_lang.Location = new Point(6, 60);
            lbl_Global_lang.Name = "lbl_Global_lang";
            lbl_Global_lang.Size = new Size(59, 15);
            lbl_Global_lang.TabIndex = 4;
            lbl_Global_lang.Text = "Language";
            // 
            // gb_SCP_Global
            // 
            gb_SCP_Global.Controls.Add(lbl_Global_lang);
            gb_SCP_Global.Controls.Add(cb_Global_region);
            gb_SCP_Global.Controls.Add(lbl_Global_region);
            gb_SCP_Global.Controls.Add(cb_Global_lang);
            gb_SCP_Global.Location = new Point(906, 9);
            gb_SCP_Global.Name = "gb_SCP_Global";
            gb_SCP_Global.Size = new Size(189, 123);
            gb_SCP_Global.TabIndex = 28;
            gb_SCP_Global.TabStop = false;
            gb_SCP_Global.Text = "Global Settings";
            // 
            // gb_SCP_Directory
            // 
            gb_SCP_Directory.Controls.Add(btn_DIR_AppDir);
            gb_SCP_Directory.Controls.Add(lbl_DIR_Cache);
            gb_SCP_Directory.Controls.Add(lbl_DIR_Root);
            gb_SCP_Directory.Controls.Add(btn_SCP_Cache);
            gb_SCP_Directory.Controls.Add(btn_DIR_Root);
            gb_SCP_Directory.Location = new Point(3, 181);
            gb_SCP_Directory.Name = "gb_SCP_Directory";
            gb_SCP_Directory.Size = new Size(474, 107);
            gb_SCP_Directory.TabIndex = 29;
            gb_SCP_Directory.TabStop = false;
            gb_SCP_Directory.Text = "Directory Settings";
            // 
            // btn_DIR_AppDir
            // 
            btn_DIR_AppDir.Location = new Point(354, 78);
            btn_DIR_AppDir.Name = "btn_DIR_AppDir";
            btn_DIR_AppDir.Size = new Size(114, 23);
            btn_DIR_AppDir.TabIndex = 4;
            btn_DIR_AppDir.Text = "AppSettings Dir";
            btn_DIR_AppDir.UseVisualStyleBackColor = true;
            btn_DIR_AppDir.Click += btn_DIR_AppDir_Click;
            // 
            // lbl_DIR_Cache
            // 
            lbl_DIR_Cache.AutoSize = true;
            lbl_DIR_Cache.Location = new Point(93, 57);
            lbl_DIR_Cache.Name = "lbl_DIR_Cache";
            lbl_DIR_Cache.Size = new Size(121, 15);
            lbl_DIR_Cache.TabIndex = 3;
            lbl_DIR_Cache.Text = "Please Select A Folder";
            // 
            // lbl_DIR_Root
            // 
            lbl_DIR_Root.AutoSize = true;
            lbl_DIR_Root.Location = new Point(93, 32);
            lbl_DIR_Root.Name = "lbl_DIR_Root";
            lbl_DIR_Root.Size = new Size(121, 15);
            lbl_DIR_Root.TabIndex = 2;
            lbl_DIR_Root.Text = "Please Select A Folder";
            // 
            // btn_SCP_Cache
            // 
            btn_SCP_Cache.Location = new Point(12, 53);
            btn_SCP_Cache.Name = "btn_SCP_Cache";
            btn_SCP_Cache.Size = new Size(75, 23);
            btn_SCP_Cache.TabIndex = 1;
            btn_SCP_Cache.Text = "Cache Dir";
            btn_SCP_Cache.UseVisualStyleBackColor = true;
            btn_SCP_Cache.Click += btn_DIR_SE_Click;
            // 
            // btn_DIR_Root
            // 
            btn_DIR_Root.Location = new Point(12, 28);
            btn_DIR_Root.Name = "btn_DIR_Root";
            btn_DIR_Root.Size = new Size(75, 23);
            btn_DIR_Root.TabIndex = 0;
            btn_DIR_Root.Text = "ROMs Dir";
            btn_DIR_Root.UseVisualStyleBackColor = true;
            btn_DIR_Root.Click += btn_DIR_Root_Click;
            // 
            // btn_SSL_Save
            // 
            btn_SSL_Save.Location = new Point(196, 129);
            btn_SSL_Save.Name = "btn_SSL_Save";
            btn_SSL_Save.Size = new Size(58, 29);
            btn_SSL_Save.TabIndex = 6;
            btn_SSL_Save.Text = "Save";
            btn_SSL_Save.UseVisualStyleBackColor = true;
            btn_SSL_Save.Click += btn_SSL_Save_Click;
            // 
            // SettingsControlPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gb_SCP_Directory);
            Controls.Add(gb_SCP_Global);
            Controls.Add(gb_SCP_GameList);
            Controls.Add(gb_SCP_Login);
            Controls.Add(gb_SCP_Scrape);
            Controls.Add(gb_SCP_Download);
            Controls.Add(btn_SCP_Close);
            MinimumSize = new Size(1100, 500);
            Name = "SettingsControlPanel";
            Size = new Size(1100, 500);
            gb_SCP_Download.ResumeLayout(false);
            gb_SCP_Download.PerformLayout();
            gb_SCP_Scrape.ResumeLayout(false);
            gb_SCP_Scrape.PerformLayout();
            gb_SCP_Login.ResumeLayout(false);
            gb_SCP_Login.PerformLayout();
            gb_SCP_GameList.ResumeLayout(false);
            gb_SCP_GameList.PerformLayout();
            gb_SCP_Global.ResumeLayout(false);
            gb_SCP_Global.PerformLayout();
            gb_SCP_Directory.ResumeLayout(false);
            gb_SCP_Directory.PerformLayout();
            ResumeLayout(false);
        }

        //for SettingsControlPanel
        private Button btn_SCP_Close;
        //for Download
        private CheckBox chk_DLS_sstitle;
        private CheckBox chk_DLS_ss;
        private CheckBox chk_DLS_fanart;
        private CheckBox chk_DLS_video;
        private CheckBox chk_DLS_video_normalized;
        private CheckBox chk_DLS_themehs;
        private CheckBox chk_DLS_screenmarquee;
        private CheckBox chk_DLS_screenmarqueesmall;
        private CheckBox chk_DLS_manuel;
        private CheckBox chk_DLS_steamgrid;
        private CheckBox chk_DLS_wheel;
        private CheckBox chk_DLS_wheel_carbon;
        private CheckBox chk_DLS_wheel_steel;
        private CheckBox chk_DLS_box_2d;
        private CheckBox chk_DLS_box_2d_side;
        private CheckBox chk_DLS_box_2d_back;
        private CheckBox chk_DLS_box_texture;
        private CheckBox chk_DLS_box_3d;
        private CheckBox chk_DLS_mixrbv1;
        private CheckBox chk_DLS_mixrbv2;
        private GroupBox gb_SCP_Download;
        //for scrpae
        private CheckBox chk_SS_Rename;
        private CheckBox chk_SS_Prompt;
        private CheckBox chk_SS_CRC32;
        private CheckBox chk_SS_MD5;
        private CheckBox chk_SS_SHA1;
        private CheckBox chk_SS_30Day;
        private GroupBox gb_SCP_Scrape;
        //for Login
        private TextBox tb_SSL_Username;
        private TextBox tb_SSL_Password;
        private Label lbl_SSL_Username;
        private Label lbl_SSL_Password;
        private Button btn_SSL_LoginTest;
        private GroupBox gb_SCP_Login;
        //for gameList
        private ComboBox cb_GLS_MainImage;
        private ComboBox cb_GLS_Thumb;
        private ComboBox cb_GLS_Marquee;
        private ComboBox cb_GLS_Video;
        private CheckBox chk_GLS_Manual;
        private Label lbl_GLS_MainImage;
        private Label lbl_GLS_Thumb;
        private Label lbl_GLS_Marquee;
        private Label lbl_GLS_Video;
        private GroupBox gb_SCP_GameList;
        //for globals
        private ComboBox cb_Global_region;
        private ComboBox cb_Global_lang;
        private Label lbl_Global_region;
        private Label lbl_Global_lang;
        private GroupBox gb_SCP_Global;
        private CheckBox chk_DLS_wheel_hd;
        private Label lbl_SSL_TestResult;
        private Button btn_DLS_All;
        private Button btn_DLS_Clear;
        private GroupBox gb_SCP_Directory;
        private Label lbl_DIR_Cache;
        private Label lbl_DIR_Root;
        private Button btn_SCP_Cache;
        private Button btn_DIR_Root;
        private Button btn_DIR_AppDir;
        private Button btn_SSL_Save;
    }
    #endregion
}













