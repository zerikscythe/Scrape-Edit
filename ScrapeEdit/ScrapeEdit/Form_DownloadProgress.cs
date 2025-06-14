namespace ScrapeEdit
{
    public partial class Form_DownloadProgress : Form
    {
        private FlowLayoutPanel panelContainer;
        private Label lbl_OverallStatus;
        private ProgressBar pb_Overall;
        private Label lbl_Summary;
        private Button btn_Cancel;

        private int totalGames;
        private int completedGames;
        private int failedGames;
        private readonly List<string> failedFiles = new();

        // Expose token so caller can observe cancellation
        private readonly CancellationTokenSource _cts = new();
        public CancellationToken Token => _cts.Token;

        public Form_DownloadProgress()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            Text = "Scraping Progress";
            Width = 475;
            Height = 650;
            MinimumSize = new Size(475, 650);
            StartPosition = FormStartPosition.CenterParent;

            // Summary label (behind panels)
            lbl_Summary = new Label
            {
                Text = string.Empty,
                AutoSize = true,
                Top = 10,
                Left = 10,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            Controls.Add(lbl_Summary);

            // Container for per-file panels
            panelContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Width = 440,
                Height = 450,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            Controls.Add(panelContainer);

            // Overall status label
            lbl_OverallStatus = new Label
            {
                Text = "Initializing...",
                AutoSize = true,
                Top = 470,
                Left = 10,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            Controls.Add(lbl_OverallStatus);

            // Overall progress bar
            pb_Overall = new ProgressBar
            {
                Top = 490,
                Left = 10,
                Width = 440,
                Height = 20,
                Minimum = 0,
                Maximum = 100,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            Controls.Add(pb_Overall);

            // Cancel button
            btn_Cancel = new Button
            {
                Text = "Cancel",
                Top = 520,
                Left = 10,
                Width = 80,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            btn_Cancel.Click += (s, e) =>
            {
                btn_Cancel.Enabled = false;
                _cts.Cancel();
            };
            Controls.Add(btn_Cancel);
        }

        public void SetTotal(int total)
        {
            totalGames = total;
            UpdateOverallProgress();
        }

        public void AddScrapePanel(ScrapePanelControl panel)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddScrapePanel(panel)));
                return;
            }
            panel.ScrapeCompleted += HandleScrapeCompleted;
            panel.ScrapeFailed += HandleScrapeFailed;
            panelContainer.Controls.Add(panel);
        }

        public void HandleScrapeCompleted(ScrapePanelControl panel)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleScrapeCompleted(panel)));
                return;
            }
            panelContainer.Controls.Remove(panel);
            completedGames++;
            UpdateOverallProgress();
        }

        public void HandleScrapeFailed(ScrapePanelControl panel, string fileName)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleScrapeFailed(panel, fileName)));
                return;
            }
            panelContainer.Controls.Remove(panel);
            failedGames++;
            failedFiles.Add(fileName);
            UpdateOverallProgress();
        }

        private void UpdateOverallProgress()
        {
            int done = completedGames + failedGames;
            lbl_OverallStatus.Text = $"Completed {done} of {totalGames}";
            pb_Overall.Value = Math.Min(100, (int)(done / (double)totalGames * 100));

            if (done == totalGames || Token.IsCancellationRequested)
                ShowFinalSummary();
        }

        private void ShowFinalSummary()
        {
            panelContainer.Hide();
            lbl_Summary.Text = $"Done: {totalGames} total, {completedGames} success, {failedGames} failed";
            if (failedFiles.Any())
            {
                lbl_Summary.Text += "\nFailed Files:\n" + string.Join("\n", failedFiles);
            }
        }
    }
}
