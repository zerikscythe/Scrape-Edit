namespace ScrapeEdit
{
    public partial class Form_YesNoApply : Form
    {
        private CheckBox chkApplyToAll;
        private Button btnYes;
        private Button btnNo;

        public bool ApplyToAll { get; private set; } // Store checkbox state

        public Form_YesNoApply(string message, string title)
        {
            Text = title;
            Width = 400;
            Height = 180;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            Label lblMessage = new Label
            {
                Text = message,
                AutoSize = true,
                Location = new System.Drawing.Point(20, 20)
            };
            Controls.Add(lblMessage);

            chkApplyToAll = new CheckBox
            {
                Text = "Apply this to all subsequent prompts",
                AutoSize = true,
                Location = new System.Drawing.Point(20, 50)
            };
            Controls.Add(chkApplyToAll);

            btnYes = new Button
            {
                Text = "Yes",
                DialogResult = DialogResult.Yes,
                Location = new System.Drawing.Point(60, 90),
                Width = 80
            };
            btnYes.Click += (sender, e) => { ApplyToAll = chkApplyToAll.Checked; Close(); };
            Controls.Add(btnYes);

            btnNo = new Button
            {
                Text = "No",
                DialogResult = DialogResult.No,
                Location = new System.Drawing.Point(160, 90),
                Width = 80
            };
            btnNo.Click += (sender, e) => { ApplyToAll = chkApplyToAll.Checked; Close(); };
            Controls.Add(btnNo);

            AcceptButton = btnYes;
            CancelButton = btnNo;
        }

        public static (DialogResult result, bool applyToAll) ShowYesNoApplyDialog(string message, string title)
        {
            Form_YesNoApply form = new Form_YesNoApply(message, title);
            DialogResult result = form.ShowDialog();
            bool applyToAll = form.ApplyToAll;
            form.Dispose();
            return (result, applyToAll);
        }
    }
}