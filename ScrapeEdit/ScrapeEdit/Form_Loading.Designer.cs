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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Loading));
            lblStatus = new Label();
            progressBar = new ProgressBar();
            lblFileProgress = new Label();
            progressBarFiles = new ProgressBar();
            SuspendLayout();
            // 
            // lblStatus
            // 
            resources.ApplyResources(lblStatus, "lblStatus");
            lblStatus.Name = "lblStatus";
            // 
            // progressBar
            // 
            resources.ApplyResources(progressBar, "progressBar");
            progressBar.Name = "progressBar";
            // 
            // lblFileProgress
            // 
            resources.ApplyResources(lblFileProgress, "lblFileProgress");
            lblFileProgress.Name = "lblFileProgress";
            // 
            // progressBarFiles
            // 
            resources.ApplyResources(progressBarFiles, "progressBarFiles");
            progressBarFiles.Name = "progressBarFiles";
            // 
            // Form_Loading
            // 
            resources.ApplyResources(this, "$this");
            Controls.Add(lblStatus);
            Controls.Add(progressBar);
            Controls.Add(lblFileProgress);
            Controls.Add(progressBarFiles);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = Properties.Resources.xml_Wiz;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form_Loading";
            SizeGripStyle = SizeGripStyle.Hide;
            ResumeLayout(false);
            PerformLayout();
            Text = "   ...Loading Files...";
        }

        #endregion
    }
}