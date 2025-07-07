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
            tss_Main_Scrapes = new ToolStripStatusLabel();
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
            contextMenuTreeView = new ContextMenuStrip(components);
            dLConsoleDataToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { tool_StatusLabel, tss_Main_Scrapes });
            statusStrip1.Location = new Point(0, 704);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 18, 0);
            statusStrip1.Size = new Size(1384, 22);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // tool_StatusLabel
            // 
            tool_StatusLabel.Name = "tool_StatusLabel";
            tool_StatusLabel.Size = new Size(132, 17);
            tool_StatusLabel.Text = "Do something already...";
            // 
            // tss_Main_Scrapes
            // 
            tss_Main_Scrapes.Name = "tss_Main_Scrapes";
            tss_Main_Scrapes.Size = new Size(50, 17);
            tss_Main_Scrapes.Text = "Scrapes:";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, gameToolStripMenuItem, settingsToolStripMenuItem1, testCodeToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(8, 2, 0, 2);
            menuStrip1.Size = new Size(1384, 24);
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
            testCodeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { devModeOffToolStripMenuItem, dLConsoleDataToolStripMenuItem });
            testCodeToolStripMenuItem.Name = "testCodeToolStripMenuItem";
            testCodeToolStripMenuItem.Size = new Size(67, 20);
            testCodeToolStripMenuItem.Text = "TestCode";
            testCodeToolStripMenuItem.Click += testCodeToolStripMenuItem_Click;
            // 
            // devModeOffToolStripMenuItem
            // 
            devModeOffToolStripMenuItem.Name = "devModeOffToolStripMenuItem";
            devModeOffToolStripMenuItem.Size = new Size(180, 22);
            devModeOffToolStripMenuItem.Text = "DevMode-Off";
            devModeOffToolStripMenuItem.Click += devModeOffToolStripMenuItem_Click;
            // 
            // contextMenuTreeView
            // 
            contextMenuTreeView.ImageScalingSize = new Size(20, 20);
            contextMenuTreeView.Name = "contextMenuTreeView";
            contextMenuTreeView.Size = new Size(61, 4);
            // 
            // dLConsoleDataToolStripMenuItem
            // 
            dLConsoleDataToolStripMenuItem.Name = "dLConsoleDataToolStripMenuItem";
            dLConsoleDataToolStripMenuItem.Size = new Size(180, 22);
            dLConsoleDataToolStripMenuItem.Text = "DL Console Data";
            dLConsoleDataToolStripMenuItem.Click += dLConsoleDataToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1384, 726);
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
        private ToolStripStatusLabel tool_StatusLabel;
        private ContextMenuStrip contextMenuTreeView;
        private ToolStripMenuItem M3UtoolStripMenuItem;
        private ToolStripMenuItem testCodeToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem devModeOffToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem1;
        private ToolStripMenuItem resetToolStripMenuItem;
        private ToolStripStatusLabel tss_Main_Scrapes;
        private ToolStripMenuItem dLConsoleDataToolStripMenuItem;
    }
}
