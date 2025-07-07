namespace ScrapeEdit
{
    partial class GameGrid
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
            lbl_Main_Man = new Label();
            tb_Main_Filename = new TextBox();
            btn_Main_ShowAllImages = new Button();
            tb_Main_Region = new TextBox();
            lbl_Main_Region = new Label();
            tb_Main_Rating = new TextBox();
            tb_Main_Genre = new TextBox();
            lbl_Main_Rating = new Label();
            lbl_Main_Genre = new Label();
            tb_Main_Publisher = new TextBox();
            tb_Main_Dev = new TextBox();
            lbl_Main_Publisher = new Label();
            lbl_Main_Dev = new Label();
            lbl_Main_ImgError = new Label();
            tb_Main_Players = new TextBox();
            tb_Main_ReleaseDate = new TextBox();
            btn_Main_ImgDOWN = new Button();
            btn_Main_ImgUP = new Button();
            lbl_Main_ImgName = new Label();
            tb_Main_Title = new TextBox();
            chk_Main_EDIT = new CheckBox();
            chk_Main_Hidden = new CheckBox();
            chk_Main_Favorite = new CheckBox();
            btn_Main_SaveChanges = new Button();
            lbl_Main_HASH = new Label();
            lbl_Main_Players = new Label();
            lbl_Main_Release = new Label();
            lbl_Main_Video = new Label();
            rtb_Main_Desc = new RichTextBox();
            pb_ImgDisplay = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pb_ImgDisplay).BeginInit();
            SuspendLayout();
            // 
            // lbl_Main_Man
            // 
            lbl_Main_Man.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Main_Man.Image = Properties.Resources.gameManual;
            lbl_Main_Man.ImageAlign = ContentAlignment.BottomCenter;
            lbl_Main_Man.Location = new Point(374, 251);
            lbl_Main_Man.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Man.MinimumSize = new Size(48, 48);
            lbl_Main_Man.Name = "lbl_Main_Man";
            lbl_Main_Man.Size = new Size(48, 48);
            lbl_Main_Man.TabIndex = 66;
            lbl_Main_Man.TextAlign = ContentAlignment.TopCenter;
            // 
            // tb_Main_Filename
            // 
            tb_Main_Filename.BorderStyle = BorderStyle.None;
            tb_Main_Filename.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Italic);
            tb_Main_Filename.Location = new Point(3, 31);
            tb_Main_Filename.Name = "tb_Main_Filename";
            tb_Main_Filename.ReadOnly = true;
            tb_Main_Filename.Size = new Size(726, 17);
            tb_Main_Filename.TabIndex = 65;
            // 
            // btn_Main_ShowAllImages
            // 
            btn_Main_ShowAllImages.Location = new Point(587, 302);
            btn_Main_ShowAllImages.Name = "btn_Main_ShowAllImages";
            btn_Main_ShowAllImages.Size = new Size(143, 29);
            btn_Main_ShowAllImages.TabIndex = 64;
            btn_Main_ShowAllImages.Text = "Show all Images";
            btn_Main_ShowAllImages.UseVisualStyleBackColor = true;
            // 
            // tb_Main_Region
            // 
            tb_Main_Region.BorderStyle = BorderStyle.None;
            tb_Main_Region.Location = new Point(668, 52);
            tb_Main_Region.Name = "tb_Main_Region";
            tb_Main_Region.Size = new Size(61, 16);
            tb_Main_Region.TabIndex = 63;
            // 
            // lbl_Main_Region
            // 
            lbl_Main_Region.AutoSize = true;
            lbl_Main_Region.Location = new Point(611, 52);
            lbl_Main_Region.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Region.Name = "lbl_Main_Region";
            lbl_Main_Region.Size = new Size(47, 15);
            lbl_Main_Region.TabIndex = 62;
            lbl_Main_Region.Text = "Region:";
            // 
            // tb_Main_Rating
            // 
            tb_Main_Rating.BorderStyle = BorderStyle.None;
            tb_Main_Rating.Location = new Point(668, 78);
            tb_Main_Rating.Name = "tb_Main_Rating";
            tb_Main_Rating.Size = new Size(62, 16);
            tb_Main_Rating.TabIndex = 61;
            // 
            // tb_Main_Genre
            // 
            tb_Main_Genre.BorderStyle = BorderStyle.None;
            tb_Main_Genre.Location = new Point(430, 77);
            tb_Main_Genre.Name = "tb_Main_Genre";
            tb_Main_Genre.Size = new Size(175, 16);
            tb_Main_Genre.TabIndex = 60;
            // 
            // lbl_Main_Rating
            // 
            lbl_Main_Rating.AutoSize = true;
            lbl_Main_Rating.Location = new Point(611, 78);
            lbl_Main_Rating.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Rating.Name = "lbl_Main_Rating";
            lbl_Main_Rating.Size = new Size(41, 15);
            lbl_Main_Rating.TabIndex = 59;
            lbl_Main_Rating.Text = "Rating";
            // 
            // lbl_Main_Genre
            // 
            lbl_Main_Genre.AutoSize = true;
            lbl_Main_Genre.Location = new Point(327, 78);
            lbl_Main_Genre.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Genre.Name = "lbl_Main_Genre";
            lbl_Main_Genre.Size = new Size(38, 15);
            lbl_Main_Genre.TabIndex = 58;
            lbl_Main_Genre.Text = "Genre";
            // 
            // tb_Main_Publisher
            // 
            tb_Main_Publisher.BorderStyle = BorderStyle.None;
            tb_Main_Publisher.Location = new Point(430, 127);
            tb_Main_Publisher.Name = "tb_Main_Publisher";
            tb_Main_Publisher.Size = new Size(175, 16);
            tb_Main_Publisher.TabIndex = 57;
            // 
            // tb_Main_Dev
            // 
            tb_Main_Dev.BorderStyle = BorderStyle.None;
            tb_Main_Dev.Location = new Point(430, 102);
            tb_Main_Dev.Name = "tb_Main_Dev";
            tb_Main_Dev.Size = new Size(175, 16);
            tb_Main_Dev.TabIndex = 56;
            // 
            // lbl_Main_Publisher
            // 
            lbl_Main_Publisher.AutoSize = true;
            lbl_Main_Publisher.Location = new Point(327, 127);
            lbl_Main_Publisher.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Publisher.Name = "lbl_Main_Publisher";
            lbl_Main_Publisher.Size = new Size(56, 15);
            lbl_Main_Publisher.TabIndex = 55;
            lbl_Main_Publisher.Text = "Publisher";
            // 
            // lbl_Main_Dev
            // 
            lbl_Main_Dev.AutoSize = true;
            lbl_Main_Dev.Location = new Point(327, 102);
            lbl_Main_Dev.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Dev.Name = "lbl_Main_Dev";
            lbl_Main_Dev.Size = new Size(60, 15);
            lbl_Main_Dev.TabIndex = 54;
            lbl_Main_Dev.Text = "Developer";
            // 
            // lbl_Main_ImgError
            // 
            lbl_Main_ImgError.AutoSize = true;
            lbl_Main_ImgError.Location = new Point(3, 355);
            lbl_Main_ImgError.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_ImgError.Name = "lbl_Main_ImgError";
            lbl_Main_ImgError.Size = new Size(0, 15);
            lbl_Main_ImgError.TabIndex = 53;
            // 
            // tb_Main_Players
            // 
            tb_Main_Players.BorderStyle = BorderStyle.None;
            tb_Main_Players.Location = new Point(668, 102);
            tb_Main_Players.Name = "tb_Main_Players";
            tb_Main_Players.Size = new Size(61, 16);
            tb_Main_Players.TabIndex = 52;
            // 
            // tb_Main_ReleaseDate
            // 
            tb_Main_ReleaseDate.BorderStyle = BorderStyle.None;
            tb_Main_ReleaseDate.Location = new Point(430, 52);
            tb_Main_ReleaseDate.Name = "tb_Main_ReleaseDate";
            tb_Main_ReleaseDate.Size = new Size(175, 16);
            tb_Main_ReleaseDate.TabIndex = 51;
            // 
            // btn_Main_ImgDOWN
            // 
            btn_Main_ImgDOWN.Location = new Point(381, 302);
            btn_Main_ImgDOWN.Name = "btn_Main_ImgDOWN";
            btn_Main_ImgDOWN.Size = new Size(41, 29);
            btn_Main_ImgDOWN.TabIndex = 50;
            btn_Main_ImgDOWN.Text = ">";
            btn_Main_ImgDOWN.UseVisualStyleBackColor = true;
            // 
            // btn_Main_ImgUP
            // 
            btn_Main_ImgUP.Location = new Point(325, 302);
            btn_Main_ImgUP.Name = "btn_Main_ImgUP";
            btn_Main_ImgUP.Size = new Size(41, 29);
            btn_Main_ImgUP.TabIndex = 49;
            btn_Main_ImgUP.Text = "<";
            btn_Main_ImgUP.UseVisualStyleBackColor = true;
            // 
            // lbl_Main_ImgName
            // 
            lbl_Main_ImgName.AutoSize = true;
            lbl_Main_ImgName.Location = new Point(3, 335);
            lbl_Main_ImgName.Name = "lbl_Main_ImgName";
            lbl_Main_ImgName.Size = new Size(0, 15);
            lbl_Main_ImgName.TabIndex = 48;
            // 
            // tb_Main_Title
            // 
            tb_Main_Title.BorderStyle = BorderStyle.None;
            tb_Main_Title.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tb_Main_Title.Location = new Point(3, 3);
            tb_Main_Title.Name = "tb_Main_Title";
            tb_Main_Title.ReadOnly = true;
            tb_Main_Title.Size = new Size(726, 22);
            tb_Main_Title.TabIndex = 47;
            // 
            // chk_Main_EDIT
            // 
            chk_Main_EDIT.AutoSize = true;
            chk_Main_EDIT.Location = new Point(641, 277);
            chk_Main_EDIT.Name = "chk_Main_EDIT";
            chk_Main_EDIT.Size = new Size(84, 19);
            chk_Main_EDIT.TabIndex = 46;
            chk_Main_EDIT.Text = "Enable Edit";
            chk_Main_EDIT.UseVisualStyleBackColor = true;
            // 
            // chk_Main_Hidden
            // 
            chk_Main_Hidden.AutoSize = true;
            chk_Main_Hidden.Location = new Point(649, 151);
            chk_Main_Hidden.Name = "chk_Main_Hidden";
            chk_Main_Hidden.RightToLeft = RightToLeft.Yes;
            chk_Main_Hidden.Size = new Size(65, 19);
            chk_Main_Hidden.TabIndex = 45;
            chk_Main_Hidden.Text = "Hidden";
            chk_Main_Hidden.UseVisualStyleBackColor = true;
            // 
            // chk_Main_Favorite
            // 
            chk_Main_Favorite.AutoSize = true;
            chk_Main_Favorite.Location = new Point(646, 126);
            chk_Main_Favorite.Name = "chk_Main_Favorite";
            chk_Main_Favorite.RightToLeft = RightToLeft.Yes;
            chk_Main_Favorite.Size = new Size(68, 19);
            chk_Main_Favorite.TabIndex = 44;
            chk_Main_Favorite.Text = "Favorite";
            chk_Main_Favorite.UseVisualStyleBackColor = true;
            // 
            // btn_Main_SaveChanges
            // 
            btn_Main_SaveChanges.Enabled = false;
            btn_Main_SaveChanges.Location = new Point(587, 337);
            btn_Main_SaveChanges.Name = "btn_Main_SaveChanges";
            btn_Main_SaveChanges.Size = new Size(142, 34);
            btn_Main_SaveChanges.TabIndex = 43;
            btn_Main_SaveChanges.Text = "Save Changes";
            btn_Main_SaveChanges.UseVisualStyleBackColor = true;
            // 
            // lbl_Main_HASH
            // 
            lbl_Main_HASH.AutoSize = true;
            lbl_Main_HASH.Location = new Point(327, 152);
            lbl_Main_HASH.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_HASH.Name = "lbl_Main_HASH";
            lbl_Main_HASH.Size = new Size(74, 15);
            lbl_Main_HASH.TabIndex = 42;
            lbl_Main_HASH.Text = "Hash Match:";
            // 
            // lbl_Main_Players
            // 
            lbl_Main_Players.AutoSize = true;
            lbl_Main_Players.Location = new Point(611, 102);
            lbl_Main_Players.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Players.Name = "lbl_Main_Players";
            lbl_Main_Players.Size = new Size(47, 15);
            lbl_Main_Players.TabIndex = 41;
            lbl_Main_Players.Text = "Players:";
            // 
            // lbl_Main_Release
            // 
            lbl_Main_Release.AutoSize = true;
            lbl_Main_Release.Location = new Point(327, 52);
            lbl_Main_Release.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Release.Name = "lbl_Main_Release";
            lbl_Main_Release.Size = new Size(76, 15);
            lbl_Main_Release.TabIndex = 40;
            lbl_Main_Release.Text = "Release Date:";
            // 
            // lbl_Main_Video
            // 
            lbl_Main_Video.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Main_Video.Image = Properties.Resources.fuzzTV;
            lbl_Main_Video.ImageAlign = ContentAlignment.BottomCenter;
            lbl_Main_Video.Location = new Point(325, 251);
            lbl_Main_Video.Margin = new Padding(4, 0, 4, 0);
            lbl_Main_Video.MinimumSize = new Size(48, 48);
            lbl_Main_Video.Name = "lbl_Main_Video";
            lbl_Main_Video.Size = new Size(48, 48);
            lbl_Main_Video.TabIndex = 39;
            lbl_Main_Video.TextAlign = ContentAlignment.TopCenter;
            // 
            // rtb_Main_Desc
            // 
            rtb_Main_Desc.Location = new Point(0, 379);
            rtb_Main_Desc.Margin = new Padding(4);
            rtb_Main_Desc.Name = "rtb_Main_Desc";
            rtb_Main_Desc.Size = new Size(729, 288);
            rtb_Main_Desc.TabIndex = 38;
            rtb_Main_Desc.Text = "";
            // 
            // pb_ImgDisplay
            // 
            pb_ImgDisplay.Location = new Point(3, 51);
            pb_ImgDisplay.Margin = new Padding(4);
            pb_ImgDisplay.Name = "pb_ImgDisplay";
            pb_ImgDisplay.Size = new Size(316, 280);
            pb_ImgDisplay.TabIndex = 37;
            pb_ImgDisplay.TabStop = false;
            // 
            // GameGrid
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
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
            Name = "GameGrid";
            Size = new Size(735, 680);
            ((System.ComponentModel.ISupportInitialize)pb_ImgDisplay).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbl_Main_Man;
        private TextBox tb_Main_Filename;
        private Button btn_Main_ShowAllImages;
        private TextBox tb_Main_Region;
        private Label lbl_Main_Region;
        private TextBox tb_Main_Rating;
        private TextBox tb_Main_Genre;
        private Label lbl_Main_Rating;
        private Label lbl_Main_Genre;
        private TextBox tb_Main_Publisher;
        private TextBox tb_Main_Dev;
        private Label lbl_Main_Publisher;
        private Label lbl_Main_Dev;
        private Label lbl_Main_ImgError;
        private TextBox tb_Main_Players;
        private TextBox tb_Main_ReleaseDate;
        private Button btn_Main_ImgDOWN;
        private Button btn_Main_ImgUP;
        private Label lbl_Main_ImgName;
        private TextBox tb_Main_Title;
        private CheckBox chk_Main_EDIT;
        private CheckBox chk_Main_Hidden;
        private CheckBox chk_Main_Favorite;
        private Button btn_Main_SaveChanges;
        private Label lbl_Main_HASH;
        private Label lbl_Main_Players;
        private Label lbl_Main_Release;
        private Label lbl_Main_Video;
        private RichTextBox rtb_Main_Desc;
        private PictureBox pb_ImgDisplay;
    }
}
