namespace ScrapeEdit
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            statusStrip1 = new StatusStrip();
            tool_StatusLabel = new ToolStripStatusLabel();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            resetToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            gameToolStripMenuItem = new ToolStripMenuItem();
            scrapeToolStripMenuItem = new ToolStripMenuItem();
            singleToolStripMenuItem = new ToolStripMenuItem();
            M3UtoolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem1 = new ToolStripMenuItem();
            testCodeToolStripMenuItem = new ToolStripMenuItem();
            devModeOffToolStripMenuItem = new ToolStripMenuItem();
            pb_ImgDisplay = new PictureBox();
            rtb_Main_Desc = new RichTextBox();
            lbl_Main_Video = new Label();
            lbl_Main_Release = new Label();
            lbl_Main_Players = new Label();
            lbl_Main_HASH = new Label();
            contextMenuTreeView = new ContextMenuStrip(components);
            btn_Main_SaveChanges = new Button();
            chk_Main_Favorite = new CheckBox();
            chk_Main_Hidden = new CheckBox();
            chk_Main_EDIT = new CheckBox();
            tb_Main_Title = new TextBox();
            lbl_Main_ImgName = new Label();
            btn_Main_ImgUP = new Button();
            btn_Main_ImgDOWN = new Button();
            tb_Main_ReleaseDate = new TextBox();
            tb_Main_Players = new TextBox();
            lbl_Main_ImgError = new Label();
            tb_Main_Publisher = new TextBox();
            tb_Main_Dev = new TextBox();
            lbl_Main_Publisher = new Label();
            lbl_Main_Dev = new Label();
            tb_Main_Rating = new TextBox();
            tb_Main_Genre = new TextBox();
            lbl_Main_Rating = new Label();
            lbl_Main_Genre = new Label();
            lbl_Main_Region = new Label();
            tb_Main_Region = new TextBox();
            btn_Main_ShowAllImages = new Button();
            tb_Main_Filename = new TextBox();
            lbl_Main_Man = new Label();
            tss_Main_Scrapes = new ToolStripStatusLabel();
            statusStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pb_ImgDisplay).BeginInit();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { tool_StatusLabel, tss_Main_Scrapes });
            statusStrip1.Location = new Point(0, 704);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 18, 0);
            statusStrip1.Size = new Size(1258, 22);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // tool_StatusLabel
            // 
            tool_StatusLabel.Name = "tool_StatusLabel";
            tool_StatusLabel.Size = new Size(132, 17);
            tool_StatusLabel.Text = "Do something already...";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, gameToolStripMenuItem, settingsToolStripMenuItem1, testCodeToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(8, 2, 0, 2);
            menuStrip1.Size = new Size(1258, 24);
            menuStrip1.TabIndex = 4;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { resetToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // resetToolStripMenuItem
            // 
            resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            resetToolStripMenuItem.Size = new Size(102, 22);
            resetToolStripMenuItem.Text = "Reset";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(102, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // gameToolStripMenuItem
            // 
            gameToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { scrapeToolStripMenuItem, M3UtoolStripMenuItem });
            gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            gameToolStripMenuItem.Size = new Size(50, 20);
            gameToolStripMenuItem.Text = "Game";
            // 
            // scrapeToolStripMenuItem
            // 
            scrapeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { singleToolStripMenuItem });
            scrapeToolStripMenuItem.Name = "scrapeToolStripMenuItem";
            scrapeToolStripMenuItem.Size = new Size(136, 22);
            scrapeToolStripMenuItem.Text = "Scrape";
            // 
            // singleToolStripMenuItem
            // 
            singleToolStripMenuItem.Name = "singleToolStripMenuItem";
            singleToolStripMenuItem.Size = new Size(118, 22);
            singleToolStripMenuItem.Text = "Selected";
            singleToolStripMenuItem.Click += singleToolStripMenuItem_Click;
            // 
            // M3UtoolStripMenuItem
            // 
            M3UtoolStripMenuItem.Name = "M3UtoolStripMenuItem";
            M3UtoolStripMenuItem.Size = new Size(136, 22);
            M3UtoolStripMenuItem.Text = "Create M3U";
            M3UtoolStripMenuItem.Click += toolStripMenuItem2_Click;
            // 
            // settingsToolStripMenuItem1
            // 
            settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            settingsToolStripMenuItem1.Size = new Size(61, 20);
            settingsToolStripMenuItem1.Text = "Settings";
            settingsToolStripMenuItem1.Click += settingsToolStripMenuItem1_Click;
            // 
            // testCodeToolStripMenuItem
            // 
            testCodeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { devModeOffToolStripMenuItem });
            testCodeToolStripMenuItem.Name = "testCodeToolStripMenuItem";
            testCodeToolStripMenuItem.Size = new Size(67, 20);
            testCodeToolStripMenuItem.Text = "TestCode";
            testCodeToolStripMenuItem.Click += testCodeToolStripMenuItem_Click;
            // 
            // devModeOffToolStripMenuItem
            // 
            devModeOffToolStripMenuItem.Name = "devModeOffToolStripMenuItem";
            devModeOffToolStripMenuItem.Size = new Size(147, 22);
            devModeOffToolStripMenuItem.Text = "DevMode-Off";
            devModeOffToolStripMenuItem.Click += devModeOffToolStripMenuItem_Click;
            // 
            // pb_ImgDisplay
            // 
            pb_ImgDisplay.Location = new Point(519, 84);
            pb_ImgDisplay.Margin = new Padding(4);
            pb_ImgDisplay.Name = "pb_ImgDisplay";
            pb_ImgDisplay.Size = new Size(316, 280);
            pb_ImgDisplay.TabIndex = 5;
            pb_ImgDisplay.TabStop = false;
            // 
            // rtb_Main_Desc
            // 
            rtb_Main_Desc.Location = new Point(516, 412);
            rtb_Main_Desc.Margin = new Padding(4);
            rtb_Main_Desc.Name = "rtb_Main_Desc";
            rtb_Main_Desc.Size = new Size(729, 288);
            rtb_Main_Desc.TabIndex = 6;
            rtb_Main_Desc.Text = "";
            // 
            // lbl_Main_Video
            // 
            lbl_Main_Video.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Main_Video.Image = Properties.Resources.fuzzTV;
            lbl_Main_Video.ImageAlign = ContentAlignment.BottomCenter;
            lbl_Main_Video.Location = new Point(841, 284);
            lbl_Main_Video.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Video.MinimumSize = new Size(48, 48);
            lbl_Main_Video.Name = "lbl_Main_Video";
            lbl_Main_Video.Size = new Size(48, 48);
            lbl_Main_Video.TabIndex = 7;
            lbl_Main_Video.TextAlign = ContentAlignment.TopCenter;
            // 
            // lbl_Main_Release
            // 
            lbl_Main_Release.AutoSize = true;
            lbl_Main_Release.Location = new Point(843, 85);
            lbl_Main_Release.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Release.Name = "lbl_Main_Release";
            lbl_Main_Release.Size = new Size(76, 15);
            lbl_Main_Release.TabIndex = 9;
            lbl_Main_Release.Text = "Release Date:";
            // 
            // lbl_Main_Players
            // 
            lbl_Main_Players.AutoSize = true;
            lbl_Main_Players.Location = new Point(1127, 135);
            lbl_Main_Players.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Players.Name = "lbl_Main_Players";
            lbl_Main_Players.Size = new Size(47, 15);
            lbl_Main_Players.TabIndex = 10;
            lbl_Main_Players.Text = "Players:";
            // 
            // lbl_Main_HASH
            // 
            lbl_Main_HASH.AutoSize = true;
            lbl_Main_HASH.Location = new Point(843, 185);
            lbl_Main_HASH.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_HASH.Name = "lbl_Main_HASH";
            lbl_Main_HASH.Size = new Size(74, 15);
            lbl_Main_HASH.TabIndex = 12;
            lbl_Main_HASH.Text = "Hash Match:";
            // 
            // contextMenuTreeView
            // 
            contextMenuTreeView.ImageScalingSize = new Size(20, 20);
            contextMenuTreeView.Name = "contextMenuTreeView";
            contextMenuTreeView.Size = new Size(61, 4);
            // 
            // btn_Main_SaveChanges
            // 
            btn_Main_SaveChanges.Enabled = false;
            btn_Main_SaveChanges.Location = new Point(1103, 370);
            btn_Main_SaveChanges.Name = "btn_Main_SaveChanges";
            btn_Main_SaveChanges.Size = new Size(142, 34);
            btn_Main_SaveChanges.TabIndex = 13;
            btn_Main_SaveChanges.Text = "Save Changes";
            btn_Main_SaveChanges.UseVisualStyleBackColor = true;
            // 
            // chk_Main_Favorite
            // 
            chk_Main_Favorite.AutoSize = true;
            chk_Main_Favorite.Location = new Point(1162, 159);
            chk_Main_Favorite.Name = "chk_Main_Favorite";
            chk_Main_Favorite.RightToLeft = RightToLeft.Yes;
            chk_Main_Favorite.Size = new Size(68, 19);
            chk_Main_Favorite.TabIndex = 14;
            chk_Main_Favorite.Text = "Favorite";
            chk_Main_Favorite.UseVisualStyleBackColor = true;
            // 
            // chk_Main_Hidden
            // 
            chk_Main_Hidden.AutoSize = true;
            chk_Main_Hidden.Location = new Point(1165, 184);
            chk_Main_Hidden.Name = "chk_Main_Hidden";
            chk_Main_Hidden.RightToLeft = RightToLeft.Yes;
            chk_Main_Hidden.Size = new Size(65, 19);
            chk_Main_Hidden.TabIndex = 15;
            chk_Main_Hidden.Text = "Hidden";
            chk_Main_Hidden.UseVisualStyleBackColor = true;
            // 
            // chk_Main_EDIT
            // 
            chk_Main_EDIT.AutoSize = true;
            chk_Main_EDIT.Location = new Point(1157, 310);
            chk_Main_EDIT.Name = "chk_Main_EDIT";
            chk_Main_EDIT.Size = new Size(84, 19);
            chk_Main_EDIT.TabIndex = 16;
            chk_Main_EDIT.Text = "Enable Edit";
            chk_Main_EDIT.UseVisualStyleBackColor = true;
            // 
            // tb_Main_Title
            // 
            tb_Main_Title.BorderStyle = BorderStyle.None;
            tb_Main_Title.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tb_Main_Title.Location = new Point(519, 36);
            tb_Main_Title.Name = "tb_Main_Title";
            tb_Main_Title.ReadOnly = true;
            tb_Main_Title.Size = new Size(726, 22);
            tb_Main_Title.TabIndex = 17;
            // 
            // lbl_Main_ImgName
            // 
            lbl_Main_ImgName.AutoSize = true;
            lbl_Main_ImgName.Location = new Point(519, 368);
            lbl_Main_ImgName.Name = "lbl_Main_ImgName";
            lbl_Main_ImgName.Size = new Size(0, 15);
            lbl_Main_ImgName.TabIndex = 18;
            // 
            // btn_Main_ImgUP
            // 
            btn_Main_ImgUP.Location = new Point(841, 335);
            btn_Main_ImgUP.Name = "btn_Main_ImgUP";
            btn_Main_ImgUP.Size = new Size(41, 29);
            btn_Main_ImgUP.TabIndex = 19;
            btn_Main_ImgUP.Text = "<";
            btn_Main_ImgUP.UseVisualStyleBackColor = true;
            // 
            // btn_Main_ImgDOWN
            // 
            btn_Main_ImgDOWN.Location = new Point(897, 335);
            btn_Main_ImgDOWN.Name = "btn_Main_ImgDOWN";
            btn_Main_ImgDOWN.Size = new Size(41, 29);
            btn_Main_ImgDOWN.TabIndex = 20;
            btn_Main_ImgDOWN.Text = ">";
            btn_Main_ImgDOWN.UseVisualStyleBackColor = true;
            // 
            // tb_Main_ReleaseDate
            // 
            tb_Main_ReleaseDate.BorderStyle = BorderStyle.None;
            tb_Main_ReleaseDate.Location = new Point(946, 85);
            tb_Main_ReleaseDate.Name = "tb_Main_ReleaseDate";
            tb_Main_ReleaseDate.Size = new Size(175, 16);
            tb_Main_ReleaseDate.TabIndex = 21;
            // 
            // tb_Main_Players
            // 
            tb_Main_Players.BorderStyle = BorderStyle.None;
            tb_Main_Players.Location = new Point(1184, 135);
            tb_Main_Players.Name = "tb_Main_Players";
            tb_Main_Players.Size = new Size(61, 16);
            tb_Main_Players.TabIndex = 22;
            // 
            // lbl_Main_ImgError
            // 
            lbl_Main_ImgError.AutoSize = true;
            lbl_Main_ImgError.Location = new Point(519, 388);
            lbl_Main_ImgError.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_ImgError.Name = "lbl_Main_ImgError";
            lbl_Main_ImgError.Size = new Size(0, 15);
            lbl_Main_ImgError.TabIndex = 23;
            // 
            // tb_Main_Publisher
            // 
            tb_Main_Publisher.BorderStyle = BorderStyle.None;
            tb_Main_Publisher.Location = new Point(946, 160);
            tb_Main_Publisher.Name = "tb_Main_Publisher";
            tb_Main_Publisher.Size = new Size(175, 16);
            tb_Main_Publisher.TabIndex = 27;
            // 
            // tb_Main_Dev
            // 
            tb_Main_Dev.BorderStyle = BorderStyle.None;
            tb_Main_Dev.Location = new Point(946, 135);
            tb_Main_Dev.Name = "tb_Main_Dev";
            tb_Main_Dev.Size = new Size(175, 16);
            tb_Main_Dev.TabIndex = 26;
            // 
            // lbl_Main_Publisher
            // 
            lbl_Main_Publisher.AutoSize = true;
            lbl_Main_Publisher.Location = new Point(843, 160);
            lbl_Main_Publisher.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Publisher.Name = "lbl_Main_Publisher";
            lbl_Main_Publisher.Size = new Size(56, 15);
            lbl_Main_Publisher.TabIndex = 25;
            lbl_Main_Publisher.Text = "Publisher";
            // 
            // lbl_Main_Dev
            // 
            lbl_Main_Dev.AutoSize = true;
            lbl_Main_Dev.Location = new Point(843, 135);
            lbl_Main_Dev.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Dev.Name = "lbl_Main_Dev";
            lbl_Main_Dev.Size = new Size(60, 15);
            lbl_Main_Dev.TabIndex = 24;
            lbl_Main_Dev.Text = "Developer";
            // 
            // tb_Main_Rating
            // 
            tb_Main_Rating.BorderStyle = BorderStyle.None;
            tb_Main_Rating.Location = new Point(1184, 111);
            tb_Main_Rating.Name = "tb_Main_Rating";
            tb_Main_Rating.Size = new Size(62, 16);
            tb_Main_Rating.TabIndex = 31;
            // 
            // tb_Main_Genre
            // 
            tb_Main_Genre.BorderStyle = BorderStyle.None;
            tb_Main_Genre.Location = new Point(946, 110);
            tb_Main_Genre.Name = "tb_Main_Genre";
            tb_Main_Genre.Size = new Size(175, 16);
            tb_Main_Genre.TabIndex = 30;
            // 
            // lbl_Main_Rating
            // 
            lbl_Main_Rating.AutoSize = true;
            lbl_Main_Rating.Location = new Point(1127, 111);
            lbl_Main_Rating.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Rating.Name = "lbl_Main_Rating";
            lbl_Main_Rating.Size = new Size(41, 15);
            lbl_Main_Rating.TabIndex = 29;
            lbl_Main_Rating.Text = "Rating";
            // 
            // lbl_Main_Genre
            // 
            lbl_Main_Genre.AutoSize = true;
            lbl_Main_Genre.Location = new Point(843, 111);
            lbl_Main_Genre.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Genre.Name = "lbl_Main_Genre";
            lbl_Main_Genre.Size = new Size(38, 15);
            lbl_Main_Genre.TabIndex = 28;
            lbl_Main_Genre.Text = "Genre";
            // 
            // lbl_Main_Region
            // 
            lbl_Main_Region.AutoSize = true;
            lbl_Main_Region.Location = new Point(1127, 85);
            lbl_Main_Region.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Region.Name = "lbl_Main_Region";
            lbl_Main_Region.Size = new Size(47, 15);
            lbl_Main_Region.TabIndex = 32;
            lbl_Main_Region.Text = "Region:";
            // 
            // tb_Main_Region
            // 
            tb_Main_Region.BorderStyle = BorderStyle.None;
            tb_Main_Region.Location = new Point(1184, 85);
            tb_Main_Region.Name = "tb_Main_Region";
            tb_Main_Region.Size = new Size(61, 16);
            tb_Main_Region.TabIndex = 33;
            // 
            // btn_Main_ShowAllImages
            // 
            btn_Main_ShowAllImages.Location = new Point(1103, 335);
            btn_Main_ShowAllImages.Name = "btn_Main_ShowAllImages";
            btn_Main_ShowAllImages.Size = new Size(143, 29);
            btn_Main_ShowAllImages.TabIndex = 34;
            btn_Main_ShowAllImages.Text = "Show all Images";
            btn_Main_ShowAllImages.UseVisualStyleBackColor = true;
            btn_Main_ShowAllImages.Click += btn_Main_ShowAllImages_Click;
            // 
            // tb_Main_Filename
            // 
            tb_Main_Filename.BorderStyle = BorderStyle.None;
            tb_Main_Filename.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Italic);
            tb_Main_Filename.Location = new Point(519, 64);
            tb_Main_Filename.Name = "tb_Main_Filename";
            tb_Main_Filename.ReadOnly = true;
            tb_Main_Filename.Size = new Size(726, 17);
            tb_Main_Filename.TabIndex = 35;
            // 
            // lbl_Main_Man
            // 
            lbl_Main_Man.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Main_Man.Image = Properties.Resources.gameManual;
            lbl_Main_Man.ImageAlign = ContentAlignment.BottomCenter;
            lbl_Main_Man.Location = new Point(890, 284);
            lbl_Main_Man.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Man.MinimumSize = new Size(48, 48);
            lbl_Main_Man.Name = "lbl_Main_Man";
            lbl_Main_Man.Size = new Size(48, 48);
            lbl_Main_Man.TabIndex = 36;
            lbl_Main_Man.TextAlign = ContentAlignment.TopCenter;
            // 
            // tss_Main_Scrapes
            // 
            tss_Main_Scrapes.Name = "tss_Main_Scrapes";
            tss_Main_Scrapes.Size = new Size(50, 17);
            tss_Main_Scrapes.Text = "Scrapes:";
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1258, 726);
            Controls.Add(lbl_Main_Man);
            Controls.Add(tb_Main_Filename);
            Controls.Add(btn_Main_ShowAllImages);
            Controls.Add(tb_Main_Region);
            Controls.Add(lbl_Main_Region);
            Controls.Add(tb_Main_Rating);
            Controls.Add(tb_Main_Genre);
            Controls.Add(lbl_Main_Rating);
            Controls.Add(lbl_Main_Genre);
            Controls.Add(tb_Main_Publisher);
            Controls.Add(tb_Main_Dev);
            Controls.Add(lbl_Main_Publisher);
            Controls.Add(lbl_Main_Dev);
            Controls.Add(lbl_Main_ImgError);
            Controls.Add(tb_Main_Players);
            Controls.Add(tb_Main_ReleaseDate);
            Controls.Add(btn_Main_ImgDOWN);
            Controls.Add(btn_Main_ImgUP);
            Controls.Add(lbl_Main_ImgName);
            Controls.Add(tb_Main_Title);
            Controls.Add(chk_Main_EDIT);
            Controls.Add(chk_Main_Hidden);
            Controls.Add(chk_Main_Favorite);
            Controls.Add(btn_Main_SaveChanges);
            Controls.Add(lbl_Main_HASH);
            Controls.Add(lbl_Main_Players);
            Controls.Add(lbl_Main_Release);
            Controls.Add(lbl_Main_Video);
            Controls.Add(rtb_Main_Desc);
            Controls.Add(pb_ImgDisplay);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(2);
            MinimumSize = new Size(1274, 765);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Scrape Edit";
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pb_ImgDisplay).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private StatusStrip statusStrip1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        //private ToolStripMenuItem setRootDirToolStripMenuItem;
        private ToolStripMenuItem gameToolStripMenuItem;
        private ToolStripMenuItem scrapeToolStripMenuItem;
        private ToolStripMenuItem singleToolStripMenuItem;
        private PictureBox pb_ImgDisplay;
        private RichTextBox rtb_Main_Desc;
        private Label lbl_Main_Video;
        private Label lbl_Main_Release;
        private Label lbl_Main_Players;
        private Label lbl_Main_HASH;
        private ToolStripStatusLabel tool_StatusLabel;
        private ContextMenuStrip contextMenuTreeView;
        private ToolStripMenuItem M3UtoolStripMenuItem;
        private ToolStripMenuItem testCodeToolStripMenuItem;
        private Button btn_Main_SaveChanges;
        private CheckBox chk_Main_Favorite;
        private CheckBox chk_Main_Hidden;
        private CheckBox chk_Main_EDIT;
        private TextBox tb_Main_Title;
        private Label lbl_Main_ImgName;
        private Button btn_Main_ImgUP;
        private Button btn_Main_ImgDOWN;
        private TextBox tb_Main_ReleaseDate;
        private TextBox tb_Main_Players;
        private Label lbl_Main_ImgError;
        private ToolStripMenuItem exitToolStripMenuItem;
        private TextBox tb_Main_Publisher;
        private TextBox tb_Main_Dev;
        private Label lbl_Main_Publisher;
        private Label lbl_Main_Dev;
        private TextBox tb_Main_Rating;
        private TextBox tb_Main_Genre;
        private Label lbl_Main_Rating;
        private Label lbl_Main_Genre;
        private Label lbl_Main_Region;
        private TextBox tb_Main_Region;
        private Button btn_Main_ShowAllImages;
        //private ToolStripMenuItem setSEDirToolStripMenuItem;
        private TextBox tb_Main_Filename;
        private ToolStripMenuItem devModeOffToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem1;
        private Label lbl_Main_Man;
        private ToolStripMenuItem resetToolStripMenuItem;
        private ToolStripStatusLabel tss_Main_Scrapes;
    }
}
