namespace ScrapeEdit
{
    public partial class Form_M3U : Form
    {
        public Form_M3U()
        {
            InitializeComponent();
            // Wire up event handlers.
            chk_M3U_CreateSubDir.CheckedChanged += chk_M3U_CreateSubDir_CheckedChanged;
            chk_M3U_HideFiles.CheckedChanged += Chk_M3U_HideFiles_CheckedChanged;
            chk_M3U_CopyMetaData.CheckedChanged += Chk_M3U_CopyMetaData_CheckedChanged;

            // Initialize the list and file name.
            LoadGameFiles();
            UpdateFileNameFromSelection();
        }

        // Event handlers that update settings.
        private void Chk_M3U_CopyMetaData_CheckedChanged(object sender, EventArgs e)
        {
            M3USettings.copyMetaData = chk_M3U_CopyMetaData.Checked;
        }

        private void Chk_M3U_HideFiles_CheckedChanged(object sender, EventArgs e)
        {
            M3USettings.hideFiles = chk_M3U_HideFiles.Checked;
        }

        private void chk_M3U_CreateSubDir_CheckedChanged(object sender, EventArgs e)
        {
            M3USettings.moveFiles = chk_M3U_CreateSubDir.Checked;
        }

        // Load the game files into the listbox.
        void LoadGameFiles()
        {
            lb_M3U_gamefiles.Items.Clear();
            lb_M3U_gamefiles.Items.AddRange(M3USettings.GameFiles);
        }

        // Called by both move handlers to update file name text box and settings.
        void UpdateFileNameFromSelection()
        {
            // Ensure there is at least one file to avoid errors.
            if (lb_M3U_gamefiles.Items.Count > 0)
            {
                M3USettings.GameFiles = lb_M3U_gamefiles.Items.Cast<string>().ToArray();
                tb_M3U_FileName.Text = Path.GetFileNameWithoutExtension(M3USettings.GameFiles[0]);
            }
            else
            {
                tb_M3U_FileName.Text = string.Empty;
            }
        }

        // Button handler for creating M3U.
        private void btn_M3U_Create_Click(object sender, EventArgs e)
        {
            M3USettings.fileNameM3U = tb_M3U_FileName.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        // Generic method to move the selected list item.
        private void MoveListBoxItem(ListBox listBox, int direction)
        {
            // direction: +1 for down, -1 for up.
            int count = listBox.Items.Count;
            if (count < 2 || listBox.SelectedIndex < 0)
                return;

            int currentIndex = listBox.SelectedIndex;
            int targetIndex = currentIndex + direction;

            // If moving outside bounds, perform a rotation.
            if (targetIndex < 0)
                targetIndex = count - 1;
            else if (targetIndex >= count)
                targetIndex = 0;

            // Swap or rotate.
            object temp = listBox.Items[targetIndex];
            listBox.Items[targetIndex] = listBox.Items[currentIndex];
            listBox.Items[currentIndex] = temp;
            listBox.SelectedIndex = targetIndex;
        }

        // Handler for moving an entry down.
        private void btn_M3U_EntryDOWN_Click(object sender, EventArgs e)
        {
            // Move the selected item down (direction +1).
            MoveListBoxItem(lb_M3U_gamefiles, +1);
            UpdateFileNameFromSelection();
        }

        // Handler for moving an entry up.
        private void btn_M3U_EntryUP_Click(object sender, EventArgs e)
        {
            // Move the selected item up (direction -1).
            MoveListBoxItem(lb_M3U_gamefiles, -1);
            UpdateFileNameFromSelection();
        }

        private void btn_M3U_Delete_Click(object sender, EventArgs e)
        {
            //get selected item index(s)
            int[] selectedIndices = lb_M3U_gamefiles.SelectedIndices.Cast<int>().ToArray();
            //remove selected items from the listbox
            foreach (int index in selectedIndices.OrderByDescending(i => i))
            {
                lb_M3U_gamefiles.Items.RemoveAt(index);
            }
        }

        private void btn_M3U_Add_Click(object sender, EventArgs e)
        {
            //add items to listbox from tb_M3U_FileName
            string newItem = tb_M3U_ManulEntry.Text.Trim(); // Get the trimmed text from the manual entry textbox
            if (!string.IsNullOrEmpty(newItem) && !lb_M3U_gamefiles.Items.Contains(newItem))
            {
                lb_M3U_gamefiles.Items.Add(newItem);
                tb_M3U_FileName.Clear(); // Clear the text box after adding
                UpdateFileNameFromSelection(); // Update the file name based on the new selection
            }
            else
            {
                MessageBox.Show("Please enter a valid file name that is not already in the list.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
