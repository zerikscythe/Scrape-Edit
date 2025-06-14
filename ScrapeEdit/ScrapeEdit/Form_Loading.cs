namespace ScrapeEdit
{
    public partial class Form_Loading : Form
    {
        public Form_Loading()
        {
            InitializeComponent();
        }

        public void UpdateStatus(string folder, int current, int total, int percent)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateStatus(folder, current, total, percent)));
                return;
            }
            lblStatus.Text = $"Scanning {folder} ({current}/{total})";
            progressBar.Value = Math.Min(100, percent);
        }
        public void UpdateFileProgress(int current, int total)
        {
            //Thread.Sleep(10);
            //MessageBox.Show("Updating file progress...");
            if (InvokeRequired)
            {
                
                Invoke(new Action(() => UpdateFileProgress(current, total)));
                return;
            }
           
            lblFileProgress.Text = $"Game Files: {current}/{total}";
            progressBarFiles.Maximum = total > 0 ? total : 1;
            progressBarFiles.Value = Math.Min(current, progressBarFiles.Maximum);
        }

    }
    public class StatusInfo
    {
        public string Message;
        public int Current;
        public int Total;
        public int Percent;

        public StatusInfo(string message, int current, int total, int percent)
        {
            Message = message;
            Current = current;
            Total = total;
            Percent = percent;
        }
    }

    public class FileProgressInfo
    {
        public int Current;
        public int Total;

        public FileProgressInfo(int current, int total)
        {
            Current = current;
            Total = total;
        }
    }
}