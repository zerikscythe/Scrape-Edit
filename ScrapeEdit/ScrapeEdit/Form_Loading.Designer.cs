namespace ScrapeEdit
{
    partial class Form_Loading
    {

        private Label lblStatus;
        private ProgressBar progressBar;
        private Label lblFileProgress;
        private ProgressBar progressBarFiles;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>


        private void InitializeComponent()
        {
            lblStatus = new Label();
            progressBar = new ProgressBar();
            lblFileProgress = new Label();
            progressBarFiles = new ProgressBar();
            SuspendLayout();
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(12, 9);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(70, 15);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Initializing...";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(12, 30);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(360, 23);
            progressBar.TabIndex = 1;
            // 
            // lblFileProgress
            // 
            lblFileProgress.AutoSize = true;
            lblFileProgress.Location = new Point(12, 60);
            lblFileProgress.Name = "lblFileProgress";
            lblFileProgress.Size = new Size(87, 15);
            lblFileProgress.TabIndex = 2;
            lblFileProgress.Text = "Game Files: 0/0";
            // 
            // progressBarFiles
            // 
            progressBarFiles.Location = new Point(12, 80);
            progressBarFiles.Name = "progressBarFiles";
            progressBarFiles.Size = new Size(360, 23);
            progressBarFiles.TabIndex = 3;
            // 
            // Form_Loading
            // 
            ClientSize = new Size(384, 115);
            ControlBox = false;
            Controls.Add(lblStatus);
            Controls.Add(progressBar);
            Controls.Add(lblFileProgress);
            Controls.Add(progressBarFiles);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "Form_Loading";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Loading Files...";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}