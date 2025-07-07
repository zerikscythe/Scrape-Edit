namespace ScrapeEdit
{
    partial class ConsoleGrid
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
            pb_Console_MainImg = new PictureBox();
            pb_Console_IllustrationImg = new PictureBox();
            pb_Console_Controller = new PictureBox();
            lbl_Console_Developer = new Label();
            tb_Console_Description = new TextBox();
            lbl_Console_ReleaseDate = new Label();
            lbl_Console_EoLDate = new Label();
            lbl_Console_RomCount = new Label();
            ((System.ComponentModel.ISupportInitialize)pb_Console_MainImg).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_Console_IllustrationImg).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_Console_Controller).BeginInit();
            SuspendLayout();
            // 
            // pb_Console_MainImg
            // 
            pb_Console_MainImg.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pb_Console_MainImg.Location = new Point(3, 3);
            pb_Console_MainImg.Name = "pb_Console_MainImg";
            pb_Console_MainImg.Size = new Size(729, 130);
            pb_Console_MainImg.SizeMode = PictureBoxSizeMode.Zoom;
            pb_Console_MainImg.TabIndex = 0;
            pb_Console_MainImg.TabStop = false;
            // 
            // pb_Console_IllustrationImg
            // 
            pb_Console_IllustrationImg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pb_Console_IllustrationImg.Location = new Point(461, 139);
            pb_Console_IllustrationImg.Name = "pb_Console_IllustrationImg";
            pb_Console_IllustrationImg.Size = new Size(271, 210);
            pb_Console_IllustrationImg.SizeMode = PictureBoxSizeMode.Zoom;
            pb_Console_IllustrationImg.TabIndex = 1;
            pb_Console_IllustrationImg.TabStop = false;
            // 
            // pb_Console_Controller
            // 
            pb_Console_Controller.Location = new Point(3, 139);
            pb_Console_Controller.Name = "pb_Console_Controller";
            pb_Console_Controller.Size = new Size(271, 210);
            pb_Console_Controller.SizeMode = PictureBoxSizeMode.Zoom;
            pb_Console_Controller.TabIndex = 2;
            pb_Console_Controller.TabStop = false;
            // 
            // lbl_Console_Developer
            // 
            lbl_Console_Developer.AutoSize = true;
            lbl_Console_Developer.Location = new Point(280, 139);
            lbl_Console_Developer.Name = "lbl_Console_Developer";
            lbl_Console_Developer.Size = new Size(66, 15);
            lbl_Console_Developer.TabIndex = 3;
            lbl_Console_Developer.Text = "Developer: ";
            // 
            // tb_Console_Description
            // 
            tb_Console_Description.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tb_Console_Description.Location = new Point(3, 355);
            tb_Console_Description.Multiline = true;
            tb_Console_Description.Name = "tb_Console_Description";
            tb_Console_Description.ReadOnly = true;
            tb_Console_Description.Size = new Size(729, 322);
            tb_Console_Description.TabIndex = 4;
            // 
            // lbl_Console_ReleaseDate
            // 
            lbl_Console_ReleaseDate.AutoSize = true;
            lbl_Console_ReleaseDate.Location = new Point(280, 156);
            lbl_Console_ReleaseDate.Name = "lbl_Console_ReleaseDate";
            lbl_Console_ReleaseDate.Size = new Size(59, 15);
            lbl_Console_ReleaseDate.TabIndex = 5;
            lbl_Console_ReleaseDate.Text = "Released: ";
            // 
            // lbl_Console_EoLDate
            // 
            lbl_Console_EoLDate.AutoSize = true;
            lbl_Console_EoLDate.Location = new Point(280, 173);
            lbl_Console_EoLDate.Name = "lbl_Console_EoLDate";
            lbl_Console_EoLDate.Size = new Size(71, 15);
            lbl_Console_EoLDate.TabIndex = 6;
            lbl_Console_EoLDate.Text = "End Of Life: ";
            // 
            // lbl_Console_RomCount
            // 
            lbl_Console_RomCount.AutoSize = true;
            lbl_Console_RomCount.Location = new Point(285, 334);
            lbl_Console_RomCount.Name = "lbl_Console_RomCount";
            lbl_Console_RomCount.Size = new Size(74, 15);
            lbl_Console_RomCount.TabIndex = 7;
            lbl_Console_RomCount.Text = "Rom Count: ";
            // 
            // ConsoleGrid
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lbl_Console_RomCount);
            Controls.Add(lbl_Console_EoLDate);
            Controls.Add(lbl_Console_ReleaseDate);
            Controls.Add(tb_Console_Description);
            Controls.Add(lbl_Console_Developer);
            Controls.Add(pb_Console_Controller);
            Controls.Add(pb_Console_IllustrationImg);
            Controls.Add(pb_Console_MainImg);
            Name = "ConsoleGrid";
            Size = new Size(735, 680);
            ((System.ComponentModel.ISupportInitialize)pb_Console_MainImg).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_Console_IllustrationImg).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_Console_Controller).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pb_Console_MainImg;
        private PictureBox pb_Console_IllustrationImg;
        private PictureBox pb_Console_Controller;
        private Label lbl_Console_Developer;
        private TextBox tb_Console_Description;
        private Label lbl_Console_ReleaseDate;
        private Label lbl_Console_EoLDate;
        private Label lbl_Console_RomCount;
    }
}
