namespace ScrapeEdit
{
    public partial class ScrapePanelControl : UserControl
    {
        private Label lblStatus;
        private ProgressBar progressBar;
        private Label lblFileName;

        /// <summary>
        /// The file name shown in the panel. Safe to set from any thread.
        /// </summary>
        public string FileName
        {
            get => lblFileName.Text;
            set
            {
                if (InvokeRequired)
                    Invoke(new Action(() => lblFileName.Text = value));
                else
                    lblFileName.Text = value;
            }
        }

        public ScrapePanelControl()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Width = 450;
            this.Height = 100;
            this.BorderStyle = BorderStyle.FixedSingle;

            lblFileName = new Label
            {
                Text = "Filename.ext",
                AutoSize = false,
                Width = 430,
                Height = 20,
                Location = new Point(10, 5),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.Black
            };
            this.Controls.Add(lblFileName);

            lblStatus = new Label
            {
                Text = "Initializing...",
                AutoSize = false,
                Width = 430,
                Height = 25,
                Location = new Point(10, 25),
                Font = new Font("Segoe UI", 8)
            };
            this.Controls.Add(lblStatus);

            progressBar = new ProgressBar
            {
                Width = 430,
                Height = 15,
                Location = new Point(10, 50),
                Minimum = 0,
                Maximum = 100
            };
            this.Controls.Add(progressBar);
        }

        public void SetStatus(string statusText)
        {
            if (InvokeRequired)
                Invoke(new Action(() => lblStatus.Text = statusText));
            else
                lblStatus.Text = statusText;
        }

        public void SetProgress(int percent)
        {
            int clamped = Math.Max(0, Math.Min(100, percent));
            if (InvokeRequired)
                Invoke(new Action(() => progressBar.Value = clamped));
            else
                progressBar.Value = clamped;
        }

        /// <summary>
        /// Change the color of the file-name label. Safe to call from any thread.
        /// </summary>
        public void SetFileNameColor(Color color)
        {
            if (InvokeRequired)
                Invoke(new Action(() => lblFileName.ForeColor = color));
            else
                lblFileName.ForeColor = color;
        }

        public event Action<ScrapePanelControl> ScrapeCompleted;
        public event Action<ScrapePanelControl, string> ScrapeFailed;

        public void RaiseScrapeCompleted()
        {
            ScrapeCompleted?.Invoke(this);
        }

        public void RaiseScrapeFailed(string reason)
        {
            ScrapeFailed?.Invoke(this, reason);
        }
    }
}
