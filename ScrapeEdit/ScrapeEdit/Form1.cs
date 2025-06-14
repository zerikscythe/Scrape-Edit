using System.Text;
using ScapeEdit;
using System.Diagnostics;
using System.Globalization;

namespace ScrapeEdit
{
    public partial class Form1 : Form
    {
        AppSettings appSettings;
        ScreenScraperApi ssa;
        Form_M3U m3u;
        Form_Loading form_Loading;

        //private ContextMenuStrip contextMenuTreeView;
        private ToolStripMenuItem viewXMLNodeToolStripMenuItem;
        private ToolStripMenuItem scrapeNodeToolStripMenuItem;
        private ToolStripMenuItem renameNodeToolStripMenuItem;
        private ToolStripMenuItem deleteNodeToolStripMenuItem;
        private ToolStripMenuItem deleteMasterNodeToolStripMenuItem;
        private ToolStripMenuItem createM3UToolStripMenuItem;
        private ToolStripMenuItem rebuildMediaToolStripMenuItem;

        bool FoundUser = false;
        string romDir = @"";
        string seDir = @"";
        string seRomDir = @"";

        Dictionary<string, string> ValidEXT = new Dictionary<string, string>();
        TreeView tv_ActiveRomDir = new TreeView();

        void Update_SE_DIRs(string path)
        {
            seDir = path;
            seRomDir = Path.Combine(seDir, "roms\\");
        }
        private async void Form1_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(romDir))
            {
                MessageBox.Show("Please select a ROM directory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

                InitializeTreeViewLoading();

            //for testing
            testCodeToolStripMenuItem.Visible = false;
            devModeOffToolStripMenuItem.Visible = false;
            devModeOffToolStripMenuItem.Enabled = false;
                //this.devModeOffToolStripMenuItem_Click(sender, e); // Ensure dev mode is on after loading
        }
        private async void InitializeTreeViewLoading()
        {
            form_Loading = new Form_Loading();
            form_Loading.Show();

            var progress = new Progress<StatusInfo>(info =>
            {
                form_Loading.UpdateStatus(info.Message, info.Current, info.Total, info.Percent);
            });

            var fileProgress = new Progress<FileProgressInfo>(fp =>
            {
                form_Loading.UpdateFileProgress(fp.Current, fp.Total);
            });

            try
            {
                this.Hide();
                await Task.Run(() => PopulateTreeView(progress, fileProgress));
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show($"Missing key in ValidEXT dictionary: {ex.Message}", "Key Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unhandled error: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                form_Loading.Invoke(new Action(() => form_Loading.Close()));
                this.Show();
            }
        }

        private void SetupConsoleIDs()
        {
            var dir = Path.GetDirectoryName(ConsoleIDHandler.filePath);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (!File.Exists(ConsoleIDHandler.filePath))
            {
                //create copy of default console IDs
                ConsoleIDHandler.SaveConsoleIds(ConsoleIDHandler.consoleIds_backup);
                // load the default console IDs
                ConsoleIDHandler.consoleIds = ConsoleIDHandler.consoleIds_backup;
            }
            else
            {
                // load the console IDs from the file
                ConsoleIDHandler.consoleIds = ConsoleIDHandler.LoadConsoleIds();
            }

        }
        public Form1()
        {
            InitializeComponent();
            SetupConsoleIDs();
            SetupTreeView();
            FormStartup();
            this.Shown += Form1_Shown;
        }


        async void FormStartup()
        {
            //appSettings
            appSettings = new AppSettings();
            romDir = appSettings.Load()[0];
            Update_SE_DIRs(appSettings.Load()[1]);

            //CanScrape = appSettings.WorkingCreds;

            ssa = new ScreenScraperApi();
            ssa.LoadCredentials(appSettings.UserName, appSettings.Password);

            // Ensure contextMenuTreeView is initialized before assigning it

            //contextMenuTreeView = new ContextMenuStrip();
            scrapeNodeToolStripMenuItem = new ToolStripMenuItem("Scrape");
            rebuildMediaToolStripMenuItem = new ToolStripMenuItem("Rebuild Media");
            renameNodeToolStripMenuItem = new ToolStripMenuItem("Rename");
            createM3UToolStripMenuItem = new ToolStripMenuItem("Create M3U");
            deleteMasterNodeToolStripMenuItem = new ToolStripMenuItem("Delete Master Data");
            viewXMLNodeToolStripMenuItem = new ToolStripMenuItem("View Raw XML");
            deleteNodeToolStripMenuItem = new ToolStripMenuItem("Delete File(s)");

            // Add items to context menu
            contextMenuTreeView.Items.Add(scrapeNodeToolStripMenuItem);
            contextMenuTreeView.Items.Add(rebuildMediaToolStripMenuItem);
            contextMenuTreeView.Items.Add(createM3UToolStripMenuItem);
            contextMenuTreeView.Items.Add(viewXMLNodeToolStripMenuItem);
            contextMenuTreeView.Items.Add(renameNodeToolStripMenuItem);
            contextMenuTreeView.Items.Add(deleteNodeToolStripMenuItem);
            contextMenuTreeView.Items.Add(deleteMasterNodeToolStripMenuItem);


            // Ensure the TreeView is not null before assigning its context menu
            if (tv_ActiveRomDir != null)
                tv_ActiveRomDir.ContextMenuStrip = contextMenuTreeView;

            // Ensure event handlers exist before assigning them
            renameNodeToolStripMenuItem.Click += renameNodeToolStripMenuItem_Click;
            viewXMLNodeToolStripMenuItem.Click += viewXMLNodeToolStripMenuItem_Click;
            deleteMasterNodeToolStripMenuItem.Click += deleteMasterNodeToolStripMenuItem_Click;
            deleteNodeToolStripMenuItem.Click += deleteNodeToolStripMenuItem_Click;
            scrapeNodeToolStripMenuItem.Click += scrapeNodeToolStripMenuItem_Click;
            createM3UToolStripMenuItem.Click += CreateM3UToolStripMenuItem_Click;
            rebuildMediaToolStripMenuItem.Click += RebuildMediaToolStripMenuItem_Click;
            lbl_Main_Video.Click += LBL_Video_Click;
            lbl_Main_Man.Click += LBL_Manual_Click;
            chk_Main_EDIT.CheckedChanged += Chk_Main_EDIT_CheckedChanged;
            btn_Main_SaveChanges.Click += Btn_Main_SaveChanges_Click;
            btn_Main_ImgDOWN.Click += btn_Main_ImgDOWN_Click;
            btn_Main_ImgUP.Click += btn_Main_ImgUP_Click;

            //PB
            pb_ImgDisplay.MouseClick += Pb_ImgDisplay_Click;
            pb_ImgDisplay.SizeMode = PictureBoxSizeMode.Zoom;

            DisableEditControls();
            btn_Main_ImgDOWN.Enabled = false;
            btn_Main_ImgUP.Enabled = false;
            chk_Main_EDIT.Enabled = false;


        }
        private void viewXMLNodeToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            Form_XML_Editor xmlEditor = new Form_XML_Editor(active_TreeNode());
            xmlEditor.ShowDialog();

        }
        void Rebuild(bool local, TreeNodeDetail node)
        {
            if (local)
            {
                if (File.Exists(Path.Combine(node.Tag_ConsolePath, "images", node.FileName + "-image.png")))
                {
                    node.Game.Name = Path.GetFileNameWithoutExtension(node.Text);
                    node.Game.Path = node.Tag_RelativePath;
                    //node.Game = SetImageLinks(node);
                    GameListManager.PostProcess(node, seRomDir);
                    GameListManager.WriteGameListToFile(node);

                    PopulateDisplayArea();
                }
                else
                {
                    MessageBox.Show("No local image found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else   //search for SE data
            {
                string pathToXML = seRomDir + node.Tag_ConsoleName + node.Tag_ConsoleRomPath + ".xml";
                string xmlData = File.ReadAllText(pathToXML, Encoding.UTF8);
                node.Game = GameListManager.ConvertSSXmlToScrapedGame(xmlData, node);
                PostScrapeUpdates(node);
            }

            PopulateDisplayArea();
        }
        private void RebuildMediaToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            TreeNode[] selectedNodes = GetSelectedNodes();
            if (selectedNodes.Length == 0)
            {
                MessageBox.Show("No games selected.");
                return;
            }

            // Prompt user once
            DialogResult result = MessageBox.Show(
                "Use Local Y, Cache N, Cancel",
                "Use Local or Cache Media",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Cancel)
            {
                MessageBox.Show("Operation canceled.");
                return;
            }

            bool useLocal = result == DialogResult.Yes;

            // Rebuild each selected node with the chosen method
            foreach (TreeNode node in selectedNodes)
            {
                if (node is TreeNodeDetail detail && !detail.isConsole && !detail.isSubDir)
                {
                    Rebuild(useLocal, detail);
                }
            }
        }
        private void Pb_ImgDisplay_Click(object? sender, MouseEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            TreeNodeDetail node = active_TreeNode();

            // Check if the right mouse button was clicked
            if (e.Button == MouseButtons.Right && editingInProgress)
            {
                if (node.Game == null)
                    node.Game = new ScrapedGame() { Path = node.Tag_RelativePath };

                // Create and configure the OpenFileDialog
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp|All Files|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    // Open the dialog and check if the user selected a file
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Get the selected file's path
                        string selectedFilePath = openFileDialog.FileName;

                        // Define the destination directory (customize this as needed)
                        string destinationDirectory = Path.Combine(node.Tag_ConsolePath, "images");

                        // Create the directory if it doesn't exist
                        if (!Directory.Exists(destinationDirectory))
                        {
                            Directory.CreateDirectory(destinationDirectory);
                        }

                        // Define a new file name (customize the logic as needed)
                        // For example, rename to "NewImage" with the same file extension.
                        string imgType = ActiveImgIndex == 0 ? "-image" :
                                         ActiveImgIndex == 1 ? "-thumb" :
                                         ActiveImgIndex == 2 ? "-marquee"
                                         : "";

                        string newFileName = node.FileName + imgType + Path.GetExtension(selectedFilePath);
                        string staticImgDir = "./images";

                        if (ActiveImgIndex == 0)
                            node.Game.Image = Path.Combine(staticImgDir, newFileName).Replace("\\", "/");
                        else if (ActiveImgIndex == 1)
                            node.Game.Thumbnail = Path.Combine(staticImgDir, newFileName).Replace("\\", "/");
                        else if (ActiveImgIndex == 2)
                            node.Game.Marquee = Path.Combine(staticImgDir, newFileName).Replace("\\", "/");

                        string destinationFilePath = Path.Combine(destinationDirectory, newFileName);

                        try
                        {
                            // Copy the selected file to the destination directory
                            // Set overwrite to true to replace an existing file with the same name.
                            File.Copy(selectedFilePath, destinationFilePath, true);

                            // Optionally, load the copied image into the PictureBox
                            pb.Image = Image.FromFile(destinationFilePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error copying file: " + ex.Message);
                        }
                    }
                }
            }

        }
        void SaveScrapedGameUserData(TreeNodeDetail node)
        {
            if (node.Game == null)
            {
                node.Game = new ScrapedGame() { Path = node.Tag_RelativePath };
                // return;
            }
            //AddNewScrapedGameToActiveGameList();
            node.Game.Path = node.Tag_RelativePath;
            node.Game.Favorite = chk_Main_Favorite.Checked;
            node.Game.Hidden = chk_Main_Hidden.Checked;
            node.Game.Name = tb_Main_Title.Text;
            node.Game.Description = rtb_Main_Desc.Text;
            node.Game.ReleaseDate = tb_Main_ReleaseDate.Text;
            node.Game.Players = tb_Main_Players.Text;
            node.Game.Publisher = tb_Main_Publisher.Text;
            node.Game.Developer = tb_Main_Dev.Text;
            node.Game.Rating = Convert.ToDouble(tb_Main_Rating.Text == "" ? 0 : tb_Main_Rating.Text);
            node.Game.Genre = tb_Main_Genre.Text;
            node.Game.Region = tb_Main_Region.Text;


            GameListManager.UpdateGameListEntry(node);
            node.ForeColor = Color.Black;
            UpdateGameNodeLabel(node);

        }
        private void Btn_Main_SaveChanges_Click(object? sender, EventArgs e)
        {

            //edit the values of the scraped game 
            SaveScrapedGameUserData(active_TreeNode());

            //overwrite old entry
            GameListManager.WriteGameListToFile(active_TreeNode());

            //disable editing
            chk_Main_EDIT.Checked = false;
        }
        private void Chk_Main_EDIT_CheckedChanged(object? sender, EventArgs e)
        {
            if (chk_Main_EDIT.Checked)
                EnableEditControls();
            else
                DisableEditControls();

        }
        private void CreateM3UToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            toolStripMenuItem2_Click(sender, e);
        }
        void SetupTreeView()
        {
            // Ensure tv_ActiveRomDir is created before use
            if (tv_ActiveRomDir == null)
            {
                tv_ActiveRomDir = new TreeView();
            }
            tv_ActiveRomDir.CheckBoxes = true;
            tv_ActiveRomDir.Location = new System.Drawing.Point(10, 40);
            tv_ActiveRomDir.Width = 500;
            tv_ActiveRomDir.Height = 630;
            tv_ActiveRomDir.AfterExpand += Tv_ActiveRomDir_AfterExpand;
            tv_ActiveRomDir.AfterSelect += TV_GameNodeSelect;
            tv_ActiveRomDir.MouseUp += tv_ActiveRomDir_MouseUp;
            tv_ActiveRomDir.AfterCheck += TV_CHK_NodeSelection;

            tv_ActiveRomDir.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            Controls.Add(tv_ActiveRomDir);
        }
        ScrapedGame Active_ScrapedGame()
        {
            return active_TreeNode().Game; //activeScrapedGame;
        }
        bool editingInProgress = false;
        void EnableEditControls()
        {
            editingInProgress = true;

            tb_Main_Title.ReadOnly = false;
            btn_Main_SaveChanges.Enabled = true;
            chk_Main_Favorite.Enabled = true;
            chk_Main_Hidden.Enabled = true;
            rtb_Main_Desc.ReadOnly = false;
            tb_Main_ReleaseDate.ReadOnly = false;
            tb_Main_Players.ReadOnly = false;
            btn_Main_ImgDOWN.Enabled = true;
            btn_Main_ImgUP.Enabled = true;
            tb_Main_Publisher.ReadOnly = false;
            tb_Main_Dev.ReadOnly = false;
            tb_Main_Genre.ReadOnly = false;
            tb_Main_Rating.ReadOnly = false;
            tb_Main_Region.ReadOnly = false;
            btn_Main_ShowAllImages.Enabled = true;

            if (ActiveImgSet == null || ActiveImgSet.Length == 0)
            {
                ActiveImgSet = new string[3]
                {
                    "","",""
                };
            }
        }
        void DisableEditControls()
        {
            editingInProgress = false;

            tb_Main_Title.ReadOnly = true;
            btn_Main_SaveChanges.Enabled = false;
            chk_Main_Favorite.Enabled = false;
            chk_Main_Hidden.Enabled = false;
            rtb_Main_Desc.ReadOnly = true;
            tb_Main_Players.ReadOnly = true;
            tb_Main_ReleaseDate.ReadOnly = true;
            tb_Main_Dev.ReadOnly = true;
            tb_Main_Publisher.ReadOnly = true;
            tb_Main_Rating.ReadOnly = true;
            tb_Main_Genre.ReadOnly = true;
            tb_Main_Region.ReadOnly = true;
            btn_Main_ShowAllImages.Enabled = false;
        }
        private void btn_Main_ImgUP_Click(object sender, EventArgs e)
        {
            ActiveImgIndex--;

            if (ActiveImgIndex < 0)
                ActiveImgIndex = ActiveImgSet.Count() - 1;

            LoadSGImages();

        }
        private void btn_Main_ImgDOWN_Click(object sender, EventArgs e)
        {
            ActiveImgIndex++;

            if (ActiveImgIndex > ActiveImgSet.Count() - 1)
                ActiveImgIndex = 0;

            LoadSGImages();

        }
        Image LoadImage(string filePath)
        {
            byte[] imageBytes = File.ReadAllBytes(filePath);
            using (var ms = new MemoryStream(imageBytes))
            {
                return Image.FromStream(ms);
            }
        }

        string[] ActiveImgSet;
        int ActiveImgIndex = 0;
        void LoadSGImages()
        {
            string missing = "";

            // Check if the file exists
            if (File.Exists(ActiveImgSet[ActiveImgIndex]))
            {
                // Load the image
                pb_ImgDisplay.Image = LoadImage(ActiveImgSet[ActiveImgIndex]);
            }
            else
            {
                pb_ImgDisplay.Image = null;
                missing = " Link Broken or File Missing!";
            }

            lbl_Main_ImgName.Text = Path.GetFileNameWithoutExtension(ActiveImgSet[ActiveImgIndex]);
            lbl_Main_ImgError.Text = missing;
            btn_Main_ImgDOWN.Enabled = true;
            btn_Main_ImgUP.Enabled = true;
        }
        void TV_CHK_NodeSelection(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;

            // Expand node if it has children
            if (e.Node.Nodes.Count > 0)//&& e.Node.Text != "roms")
            {
                e.Node.Expand();
            }

            // Set all child nodes' checkboxes to the same value as the parent
            SetChildNodesChecked(e.Node, e.Node.Checked);
        }
        private void SetChildNodesChecked(TreeNode node, bool isChecked)
        {
            foreach (TreeNode child in node.Nodes)
            {
                child.Checked = isChecked; // Set checked state
                                           // SetChildNodesChecked(child, isChecked); // Recursively check all children
            }
        }
        void TV_GameNodeSelect(object sender, TreeViewEventArgs e)
        {
            if (editingInProgress)
            {
                MessageBox.Show("You must finish editing before selecting a new game.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // Get selected node
            TreeView treeView = sender as TreeView;
            TreeNode selectedNode = e.Node;

            if (selectedNode == null) return; // Ensure node is selected

            DeselectAllNodes();
            selectedNode.Checked = true;

            // Disable editing for root or console-level nodes
            if (selectedNode.Parent == null || selectedNode.Level == 1)
                chk_Main_EDIT.Enabled = false; // Disable checkbox
            else
                chk_Main_EDIT.Enabled = true; // Enable checkbox for game nodes

            DisableEditControls();

            PopulateDisplayArea();

        }
        private void PopulateDisplayArea()
        {
            //setActive_ScrapedGame();
            ScrapedGame sg = active_TreeNode().Game; //Active_ScrapedGame();

            if (sg != null)
            {
                tb_Main_Title.Text = sg.Name;
                tb_Main_Filename.Text = sg.Path;
                rtb_Main_Desc.Text = sg.Description;
                tb_Main_ReleaseDate.Text = sg.ParsedReleaseDate;
                tb_Main_Players.Text = sg.Players;
                chk_Main_Hidden.Checked = sg.Hidden;
                chk_Main_Favorite.Checked = sg.Favorite;
                tb_Main_Dev.Text = sg.Developer;
                tb_Main_Publisher.Text = sg.Publisher;
                tb_Main_Genre.Text = sg.Genre;
                tb_Main_Rating.Text = sg.Rating.ToString();
                tb_Main_Region.Text = sg.Region;
                lbl_Main_HASH.Text = "Hash: " + active_TreeNode().Tag_HashMatchFound.ToString();

                ActiveImgSet = new string[3]
                {
                    Path.Combine(active_TreeNode().Tag_ConsolePath, sg.Image == null  || sg.Image == "" ? "Missing Image File" : sg.NormalizePath(sg.Image)),
                    Path.Combine(active_TreeNode().Tag_ConsolePath, sg.Thumbnail == null || sg.Thumbnail == "" ? "Missing Thumbnail File" : sg.NormalizePath(sg.Thumbnail)),
                    Path.Combine(active_TreeNode().Tag_ConsolePath, sg.Marquee == null || sg.Marquee == "" ? "Missing Marquee File" : sg.NormalizePath(sg.Marquee))
                };
                ActiveImgIndex = 0;
                LoadSGImages();
            }
            else
            {
                tb_Main_Title.Text = active_TreeNode().Text; //a gameNode w/ no properties but a name should still exist
                pb_ImgDisplay.Image = null;
                rtb_Main_Desc.Text = "";
                tb_Main_ReleaseDate.Text = "";
                tb_Main_Players.Text = "";
                chk_Main_Favorite.Checked = false;
                chk_Main_Hidden.Checked = false;
                ActiveImgSet = new string[0];
                lbl_Main_ImgName.Text = "";
                lbl_Main_ImgError.Text = "";
                btn_Main_ImgDOWN.Enabled = false;
                btn_Main_ImgUP.Enabled = false;
                tb_Main_Publisher.Text = "";
                tb_Main_Dev.Text = "";
                tb_Main_Rating.Text = "";
                tb_Main_Genre.Text = "";
                tb_Main_Region.Text = "";
            }

        }
        private void tv_ActiveRomDir_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode clickedNode = tv_ActiveRomDir.GetNodeAt(e.X, e.Y);

                if (clickedNode != null)
                {
                    tv_ActiveRomDir.SelectedNode = clickedNode; // Select the node before showing the menu
                    contextMenuTreeView.Show(tv_ActiveRomDir, e.Location); // Show context menu at cursor position
                }
            }
        }
        private void scrapeNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (scrapeInProgress)
            {
                MessageBox.Show("Active Scrape in Progress! Please wait!");
                return;
            }
            if (!editingInProgress)
                ScrapeSingleOrMulti(GetSelectedNodes());
            else
                MessageBox.Show("Before Scraping ensure:\nAccount is loaded!\nNot Editing\nHave a game selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        private void renameNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNodeDetail node = active_TreeNode();

            if (node != null)
            {
                string oldPath = node.Tag_FullPath;

                // Check if the node represents either a file or a directory.
                if (oldPath == null || (!File.Exists(oldPath) && !Directory.Exists(oldPath)))
                {
                    MessageBox.Show("File/Folder not found or invalid node.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Get parent directory; if it's a directory, this will be its parent's path.
                string parentDirectory = Path.GetDirectoryName(oldPath);

                // For consistency, extract the extension. This works whether the node is a file
                // or a folder with a "game extension" (like .PS3).
                string extension = Path.GetExtension(oldPath);

                string newNameInput = Microsoft.VisualBasic.Interaction.InputBox(
                    "Enter new file name (without extension):",
                    "Rename File/Folder",
                    node.FileName);

                if (string.IsNullOrWhiteSpace(newNameInput))
                {
                    return; // User canceled
                }

                // Create new name by reattaching the original extension.
                string newPath = Path.Combine(parentDirectory, newNameInput + extension);

                // Check if any file or folder already exists at the new path.
                if (File.Exists(newPath) || Directory.Exists(newPath))
                {
                    MessageBox.Show("An item with this name already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    if (Directory.Exists(oldPath))
                    {
                        // Rename the directory without affecting its contents.
                        Directory.Move(oldPath, newPath);
                    }
                    else if (File.Exists(oldPath))
                    {
                        // Rename the file.
                        File.Move(oldPath, newPath);
                    }

                    // Update the node details.
                    node.Text = newNameInput + extension;
                    node.Tag_FullPath = newPath;
                    node.Game.Path = newNameInput + extension;

                    Invoke(new Action(() =>
                    {
                        SortTreeNodesAlphabetically(node.Parent as TreeNodeDetail);
                    }));

                    MessageBox.Show("Item successfully renamed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error renaming item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void deleteNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1. Gather all selected game nodes
            var selected = GetSelectedNodes()
                .OfType<TreeNodeDetail>()
                .Where(n => !n.isConsole && !n.isSubDir)
                .ToArray();
            if (selected.Length == 0) return;

            // 2. Confirm deletion
            var msg = $"Delete {selected.Length} ROM{(selected.Length > 1 ? "s" : "")}?";
            if (MessageBox.Show(msg, "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                != DialogResult.Yes) return;

            // 3. Group by console so we rewrite each gamelist only once
            var byConsole = selected.GroupBy(n => n.Tag_ConsoleName);

            foreach (var group in byConsole)
            {
                // For each console:
                //  a) delete files & update in-memory list
                foreach (var node in group)
                {
                    var path = node.Tag_FullPath;
                    if (File.Exists(path))
                        File.Delete(path);

                    if (GameListManager.gameLists.TryGetValue(group.Key, out var gl))
                        gl.ScrapedGames.RemoveAll(g => g.Path == node.Game.Path);

                    GameListManager.PurgeOldMediaFiles(node);
                }

                //  b) rewrite console's gamelist.xml
                //     find any one node in this group to get its console TreeNodeDetail
                var consoleNode = ReturnConsoleNodeFromChild(group.First());
                GameListManager.WriteGameListToFile(consoleNode);

                //  c) remove the nodes from the TreeView
                foreach (var node in group)
                    node.Remove();
            }
        }

        private void deleteMasterNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (active_TreeNode() != null)
            {
                string filePath = active_TreeNode().Tag_FullPath;

                if (filePath == null || !File.Exists(filePath))
                {
                    MessageBox.Show("File not found or invalid node.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                string xmlFilePath = Path.Combine(seRomDir, fileNameWithoutExtension + ".xml");

                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete this file and all related metadata?\n{filePath}",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Delete the ROM file
                        //File.Delete(filePath);

                        // Delete the XML file if it exists
                        if (File.Exists(xmlFilePath))
                        {
                            File.Delete(xmlFilePath);
                        }

                        // Delete all associated media files
                        DeleteMasterGameMetadata(active_TreeNode());

                        // Remove the node from TreeView
                        //tv_ActiveRomDir.SelectedNode.Remove();

                        MessageBox.Show("File and metadata successfully deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting file or metadata: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void DeleteMasterGameMetadata(TreeNodeDetail node)
        {
            if (!Directory.Exists(seRomDir))
            {
                Console.WriteLine($"[DeleteGameMetadata] Directory does not exist: {seRomDir}");
                return;
            }

            string systemFolder = Path.Combine(seRomDir, node.Tag_ConsoleName, node.Tag_ConsolePath);// Path.GetFileName(Path.GetDirectoryName(fileNameWithoutExtension)));

            if (!Directory.Exists(systemFolder))
            {
                //Console.WriteLine($"[DeleteGameMetadata] System folder does not exist: {systemFolder}");
                return;
            }

            // Get all subfolders inside the system folder (e.g., "roms/snes/")
            foreach (string folder in Directory.GetDirectories(systemFolder))
            {
                try
                {
                    // Get all files inside this subfolder
                    string[] mediaFiles = Directory.GetFiles(folder);

                    foreach (string mediaFile in mediaFiles)
                    {
                        // Extract the filename
                        string mediaFileName = Path.GetFileName(mediaFile);

                        // Check if the filename starts with the game name + "-" (to prevent false positives)
                        if (mediaFileName.StartsWith(node.FileName + "-", StringComparison.OrdinalIgnoreCase))
                        {
                            File.Delete(mediaFile);
                            //tool_StatusLabel.Text = $"[DeleteGameMetadata] Deleted: {mediaFile}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"[DeleteGameMetadata] Error deleting files in {folder}: {ex.Message}");
                }
            }
        }

        void OpenFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show($"{Path.GetFileName(filePath)}, not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Let the OS decide which app to use
            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true // This tells Windows to open it with the default program
            });


        }

        void LBL_Manual_Click(object sender, EventArgs e)
        {
            if (editingInProgress)
            {
                TreeNodeDetail node = active_TreeNode();

                //if (Active_ScrapedGame() == null)
                //    AddNewScrapedGameToActiveGameList();

                // Create and configure the OpenFileDialog
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    openFileDialog.Filter = "PDF Files|*.pdf;|All Files|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    // Open the dialog and check if the user selected a file
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Get the selected file's path
                        string selectedFilePath = openFileDialog.FileName;

                        // Define the destination directory (customize this as needed)
                        string destinationDirectory = Path.Combine(node.Tag_ConsolePath, "manuals");

                        // Create the directory if it doesn't exist
                        if (!Directory.Exists(destinationDirectory))
                        {
                            Directory.CreateDirectory(destinationDirectory);
                        }

                        string newFileName = node.FileName + "-manual" + Path.GetExtension(selectedFilePath);
                        string staticImgDir = "./manuals";

                        node.Game.Manual = Path.Combine(staticImgDir, newFileName).Replace("\\", "/");

                        string destinationFilePath = Path.Combine(destinationDirectory, newFileName);

                        try
                        {
                            // Copy the selected file to the destination directory
                            // Set overwrite to true to replace an existing file with the same name.
                            File.Copy(selectedFilePath, destinationFilePath, true);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error copying file: " + ex.Message);
                        }
                    }
                }


            }
            else if (Active_ScrapedGame() != null)
            {

                if (Active_ScrapedGame().Manual != null)
                    OpenFile(Path.Combine(active_TreeNode().Tag_ConsolePath, active_TreeNode().Game.Manual));
                else
                    MessageBox.Show("No Manual Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void LBL_Video_Click(object sender, EventArgs e)
        {
            if (editingInProgress)
            {

                //if (Active_ScrapedGame() == null)
                //    AddNewScrapedGameToActiveGameList();

                TreeNodeDetail node = active_TreeNode();

                // Create and configure the OpenFileDialog
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                    openFileDialog.Filter = "MP4 Files|*.mp4;|All Files|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    // Open the dialog and check if the user selected a file
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Get the selected file's path
                        string selectedFilePath = openFileDialog.FileName;

                        // Define the destination directory (customize this as needed)
                        string destinationDirectory = Path.Combine(node.Tag_ConsolePath, "videos");

                        // Create the directory if it doesn't exist
                        if (!Directory.Exists(destinationDirectory))
                        {
                            Directory.CreateDirectory(destinationDirectory);
                        }

                        string newFileName = node.FileName + "-video" + Path.GetExtension(selectedFilePath);
                        string staticImgDir = "./videos";

                        node.Game.Video = Path.Combine(staticImgDir, newFileName).Replace("\\", "/");

                        string destinationFilePath = Path.Combine(destinationDirectory, newFileName);

                        try
                        {
                            // Copy the selected file to the destination directory
                            // Set overwrite to true to replace an existing file with the same name.
                            File.Copy(selectedFilePath, destinationFilePath, true);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error copying file: " + ex.Message);
                        }
                    }
                }


            }

            else if (active_TreeNode().Game != null)
            {

                if (active_TreeNode().Game.Video != null)
                    OpenFile(Path.Combine(active_TreeNode().Tag_ConsolePath, active_TreeNode().Game.Video));
                else
                    MessageBox.Show("No Video Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //void OpenScrapeSettings()
        //{
        //    sss.ShowDialog();
        //}
        //void OpenGlobalSettings()
        //{
        //    global.ShowDialog();
        //}
        //void OpenDownloadSettings()
        //{
        //    dls.ShowDialog();
        //}
        //public void OpenGameListSettings()
        //{
        //    gls.ProcessValues();
        //    gls.ShowDialog();
        //}
        TreeNodeDetail active_TreeNode()
        {
            return tv_ActiveRomDir.SelectedNode as TreeNodeDetail;
        }
        void SetValidEXT(string basePath)
        {
            ValidEXT = new Dictionary<string, string>();

            foreach (var systemDir in Directory.GetDirectories(basePath))
            {
                string systemName = new DirectoryInfo(systemDir).Name.ToLower(); // Get console folder name
                string infoFilePath = Path.Combine(systemDir, "_info.txt");

                if (File.Exists(infoFilePath))
                {
                    string[] lines = File.ReadAllLines(infoFilePath);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("ROM files extensions accepted:"))
                        {
                            string extPart = line.Split('"')[1].Trim(); // Extracts the text inside quotes
                            extPart = extPart.Replace(" ", ",");

                            if (!string.IsNullOrEmpty(extPart))
                            {
                                ValidEXT[systemName] = extPart;
                            }
                        }
                    }
                }
                else if (!ValidEXT.ContainsKey(systemName))
                {
                    // If no _info.txt and system not in ValidEXT, do not load files
                    ValidEXT[systemName] = "";
                }
            }
        }

        private async void setRootDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                romDir = dialog.SelectedPath;
                if (string.IsNullOrEmpty(romDir))
                    return;

                //GameListManager.gameLists = new Dictionary<string, GameList>();
                appSettings.SetRomDir(romDir);

                // hide main, show loading
                this.Hide();
                using var loadingForm = new Form_Loading();
                loadingForm.Show();

                var progress = new Progress<StatusInfo>(info =>
                    loadingForm.UpdateStatus(info.Message, info.Current, info.Total, info.Percent)
                );
                var fileProgress = new Progress<FileProgressInfo>(info =>
                    loadingForm.UpdateFileProgress(info.Current, info.Total)
                );

                // run the long work on background thread
                await Task.Run(() => PopulateTreeView(progress, fileProgress));

                // close loading, show main
                loadingForm.Close();
                this.Show();

            }
        }

        public void PopulateTreeView(IProgress<StatusInfo> progress, IProgress<FileProgressInfo> fileProgress)
        {
            Invoke(() =>
            {
                tv_ActiveRomDir.BeginUpdate();
                tv_ActiveRomDir.Nodes.Clear();
            });

            DirectoryInfo rootDir = new DirectoryInfo(romDir);
            TreeNodeDetail rootNode = new TreeNodeDetail(rootDir.Name) { Tag = rootDir };
            Invoke(() => tv_ActiveRomDir.Nodes.Add(rootNode));

            SetValidEXT(rootDir.FullName);
            var consoleDirs = rootDir.GetDirectories();
            int totalConsoles = consoleDirs.Length;
            int consoleIndex = 0;

            foreach (var consoleDir in consoleDirs.OrderBy(d => d.Name))
            {
                consoleIndex++;
                string consoleKey = consoleDir.Name.ToLowerInvariant();
                if (!ValidEXT.ContainsKey(consoleKey)) continue;

                progress?.Report(new StatusInfo($"Processing: {consoleDir.Name}", consoleIndex, totalConsoles, consoleIndex * 100 / totalConsoles));

                TreeNodeDetail consoleNode = new TreeNodeDetail(consoleDir.Name)
                {
                    Tag_ConsolePath = consoleDir.FullName,
                    Tag_FullPath = consoleDir.FullName,
                    isConsole = true
                };

                Invoke(() => rootNode.Nodes.Add(consoleNode));

                HashSet<string> validExts = ValidEXT[consoleKey]
                    .Split(',').Select(e => e.Trim().ToLowerInvariant()).ToHashSet();

                string gamelistPath = Path.Combine(consoleDir.FullName, "gamelist.xml");
                GameList gameList = File.Exists(gamelistPath)
                    ? new GameList().LoadGameListXML(gamelistPath)
                    : new GameList();

                GameListManager.gameLists[consoleKey] = gameList;

                HashSet<string> loadedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                int gameIndex = 0;
                int gameTotal = gameList.ScrapedGames.Count;

                foreach (var scraped in gameList.ScrapedGames)
                {
                    if (string.IsNullOrWhiteSpace(scraped.Path)) continue;

                    string relativePath = scraped.Path.Trim().Replace("./", "").Replace("/", "\\");
                    string fullPath = Path.GetFullPath(Path.Combine(consoleDir.FullName, relativePath));
                    if (!File.Exists(fullPath) && !Directory.Exists(fullPath)) continue;

                    // Add normalized relative path for deduplication
                    string normalizedRelPath = Path.GetRelativePath(consoleDir.FullName, fullPath).Replace("\\", "/");
                    if (!loadedPaths.Add(normalizedRelPath)) continue;

                    string[] pathParts = normalizedRelPath.Split('/');
                    TreeNodeDetail currentParent = consoleNode;

                    for (int i = 0; i < pathParts.Length - 1; i++)
                    {
                        string folderName = pathParts[i];

                        var existing = currentParent.Nodes
                            .Cast<TreeNodeDetail>()
                            .FirstOrDefault(n => n.Text.Equals(folderName, StringComparison.OrdinalIgnoreCase) && n.isSubDir);

                        if (existing == null)
                        {
                            TreeNodeDetail subdirNode = new TreeNodeDetail(folderName)
                            {
                                isSubDir = true,
                                Tag_FullPath = Path.Combine(consoleDir.FullName, Path.Combine(pathParts.Take(i + 1).ToArray())),
                                Tag_ConsolePath = consoleDir.FullName
                            };

                            Invoke(() => currentParent.Nodes.Add(subdirNode));
                            currentParent = subdirNode;
                        }
                        else
                        {
                            currentParent = existing;
                        }
                    }

                    string fileName = pathParts[^1];
                    TreeNodeDetail gameNode = new TreeNodeDetail(fileName)
                    {
                        Tag = fullPath,
                        Tag_FullPath = fullPath,
                        Game = scraped,
                        Tag_ConsolePath = consoleDir.FullName
                    };

                    UpdateGameNodeLabel(gameNode);

                    SetNodeGrayIfIncomplete(gameNode);
                    Invoke(() => currentParent.Nodes.Add(gameNode));

                    gameIndex++;
                    fileProgress?.Report(new FileProgressInfo(gameIndex, gameTotal));
                }

                AddMissingRomNodes(consoleDir, consoleNode, validExts, loadedPaths, gameList, fileProgress, ref gameIndex, ref gameTotal);

                Invoke(new Action(() =>
                {
                    SortTreeNodesAlphabetically(consoleNode);
                }));

                GameListManager.WriteGameListToFile(consoleNode);
            }

            Invoke(() =>
            {
                TrimTreeViewNodes(rootNode);
                rootNode.Expand();
                tv_ActiveRomDir.EndUpdate();
            });
        }
        void TrimTreeViewNodes(TreeNodeDetail parentNode)
        {
            foreach (TreeNodeDetail node in parentNode.Nodes.Cast<TreeNodeDetail>().ToList())
            {
                if (node.isConsole && node.Nodes.Count == 0)
                {
                    parentNode.Nodes.Remove(node);
                }
            }
        }
        private void UpdateGameNodeLabel(TreeNodeDetail node)
        {
            if (node.Game == null) return;

            string baseName = node.Game.Name ?? Path.GetFileNameWithoutExtension(node.Tag_FullPath);
            if (node.Game.Favorite)
                baseName += " 💖";
            if (node.Game.Hidden)
                baseName += " 🚫";

            node.Text = baseName;

            if (node.Game.Name == node.Text)
                node.ForeColor = Color.Black;
        }
        private void AddMissingRomNodes(DirectoryInfo folder, TreeNodeDetail parentNode,
            HashSet<string> validExts, HashSet<string> loadedPaths,
            GameList gameList,
            IProgress<FileProgressInfo> fileProgress,
            ref int gameIndex, ref int gameTotal)
        {
            foreach (var entry in folder.EnumerateFileSystemInfos().OrderBy(e => e.Name))
            {
                string ext = Path.GetExtension(entry.Name).ToLowerInvariant();

                TreeNodeDetail consoleNode = ReturnConsoleNodeFromChild(parentNode);
                string consolePath = consoleNode?.Tag_ConsolePath;

                if (string.IsNullOrWhiteSpace(consolePath)) continue;

                string relPath = Path.GetRelativePath(consolePath, entry.FullName)
                                     .Replace("\\", "/");

                if (loadedPaths.Contains(relPath)) continue;

                if ((entry is FileInfo file && validExts.Contains(ext)) ||
                    (entry is DirectoryInfo dir && validExts.Contains(Path.GetExtension(dir.Name).ToLowerInvariant())))
                {
                    string fullPath = entry.FullName;

                    ScrapedGame sg = new ScrapedGame
                    {
                        Name = Path.GetFileNameWithoutExtension(entry.Name),
                        Path = $"./{relPath}"
                    };

                    TreeNodeDetail gameNode = new TreeNodeDetail(entry.Name)
                    {
                        Tag = fullPath,
                        Tag_FullPath = fullPath,
                        Game = sg,
                        Tag_ConsolePath = consolePath
                    };

                    SetNodeGrayIfIncomplete(gameNode);
                    Invoke(() => parentNode.Nodes.Add(gameNode));
                    gameList.AddGame(sg);

                    gameIndex++;
                    gameTotal++;
                    fileProgress?.Report(new FileProgressInfo(gameIndex, gameTotal));
                }
                else if (entry is DirectoryInfo subDir && FolderContainsValidFiles(subDir, validExts))
                {
                    var existing = parentNode.Nodes
                        .Cast<TreeNodeDetail>()
                        .FirstOrDefault(n => n.Text.Equals(subDir.Name, StringComparison.OrdinalIgnoreCase) && n.isSubDir);

                    TreeNodeDetail subDirNode;

                    if (existing == null)
                    {
                        subDirNode = new TreeNodeDetail(subDir.Name)
                        {
                            Tag_FullPath = subDir.FullName,
                            Tag_ConsolePath = consolePath,
                            isSubDir = true
                        };

                        Invoke(() => parentNode.Nodes.Add(subDirNode));
                    }
                    else
                    {
                        subDirNode = existing;
                    }

                    AddMissingRomNodes(subDir, subDirNode, validExts, loadedPaths, gameList, fileProgress, ref gameIndex, ref gameTotal);

                }
            }
        }

        private void SetNodeGrayIfIncomplete(TreeNodeDetail node)
        {
            if (node.Game == null || node.isSubDir || node.isConsole) return;

            bool missingAssets = string.IsNullOrWhiteSpace(node.Game.Image) ||
                                 !File.Exists(Path.Combine(node.Tag_ConsolePath, node.Game.Image?.TrimStart('.', '/', '\\')));

            bool missingDescription = string.IsNullOrWhiteSpace(node.Game.Description);
            bool hasExtension = !string.IsNullOrWhiteSpace(Path.GetExtension(node.Tag_FullPath));

            bool badDescription = missingDescription && !hasExtension;

            if (missingAssets || badDescription)
            {
                node.ForeColor = Color.Gray;
            }
        }
        private bool FolderContainsValidFiles(DirectoryInfo folder, HashSet<string> validExtensions)
        {
            return folder.EnumerateFiles("*", SearchOption.AllDirectories)
                .Any(f => validExtensions.Contains(f.Extension.ToLower()));
        }
        TreeNodeDetail ReturnConsoleNodeFromChild(TreeNodeDetail parentNode)
        {
            TreeNodeDetail grandparent = parentNode.Parent as TreeNodeDetail;

            if (parentNode.isConsole)
            {
                return parentNode;
            }
            else
            {
                return ReturnConsoleNodeFromChild(grandparent);
            }

        }
        private void Tv_ActiveRomDir_AfterExpand(object sender, TreeViewEventArgs e)
        {
            // Collapse all sibling nodes when a node is expanded
            if (e.Node.Parent != null)
            {
                foreach (TreeNode sibling in e.Node.Parent.Nodes)
                {
                    if (sibling != e.Node && sibling.IsExpanded)
                    {
                        sibling.Collapse();
                    }
                }
            }

            DeselectAllNodes();
        }

        private void singleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            scrapeNodeToolStripMenuItem_Click(sender, e);

        }

        void GetAllCheckedNodes(TreeNodeCollection nodes, List<TreeNode> checkedNodes)
        {

            foreach (TreeNode node in nodes)
            {
                TreeNodeDetail convertedNode = node as TreeNodeDetail;

                // If the node is checked, add it to the list
                if (convertedNode.Checked && !convertedNode.isConsole && !convertedNode.isSubDir)
                {
                    checkedNodes.Add(node);
                }
                // Also recurse into children
                GetAllCheckedNodes(node.Nodes, checkedNodes);
            }
        }

        bool scrapeInProgress = false;

        private async void ScrapeSingleOrMulti(TreeNode[] nodes)
        {
            TreeNodeDetail[] detailNodes = nodes.Cast<TreeNodeDetail>().ToArray();
            if (!GameFileProcessor.SetValuesFirst(seRomDir, ssa, ValidEXT)) return;

            Form_DownloadProgress progressForm = new Form_DownloadProgress();
            progressForm.SetTotal(detailNodes.Length);
            progressForm.Show();

            scrapeInProgress = true;

            //reload consoleIDs file incase any changes were made
            ConsoleIDHandler.consoleIds = ConsoleIDHandler.LoadConsoleIds();

            await RunScrapes(detailNodes, progressForm); // ✅ This starts and completes the full scrape process.

            //GameListManager.WriteGameListToFile(scrapedNodes[0]);

            scrapeInProgress = false;
            //MessageBox.Show("Scraping Complete!");
        }


        private Queue<TreeNodeDetail> scrapeQueue;
        private List<TreeNodeDetail> scrapedNodes;
        private int activeScrapes;
        private readonly int maxConcurrency = 4;
        private Form_DownloadProgress progressForm;

        public async Task RunScrapes(TreeNodeDetail[] nodes, Form_DownloadProgress progressForm)
        {
            this.progressForm = progressForm;
            scrapeQueue = new Queue<TreeNodeDetail>(nodes);
            scrapedNodes = new List<TreeNodeDetail>();
            activeScrapes = 0;

            // Tell the form how many total games we're about to do:
            progressForm.SetTotal(nodes.Length);

            // Start up to maxConcurrency scrapes:
            for (int i = 0; i < maxConcurrency && scrapeQueue.Count > 0; i++)
                _ = StartNextScrapeAsync();

            // Wait until either:
            //  - everything finishes, or
            //  - user hits Cancel
            while (activeScrapes > 0 && !progressForm.Token.IsCancellationRequested)
                await Task.Delay(100);

            // if the user cancelled, we bail out now
            if (progressForm.Token.IsCancellationRequested)
                return;

            if (scrapedNodes.Any())
            {
                var affectedConsoles = scrapedNodes
                    .Select(n => n.Tag_ConsoleName)
                    .Distinct(StringComparer.OrdinalIgnoreCase);

                Invoke(new Action(() =>
                {
                    SortTreeNodesAlphabetically(ReturnConsoleNodeFromChild(scrapedNodes[0]));
                }));


                foreach (var consoleKey in affectedConsoles)
                {
                    if (!GameListManager.gameLists.TryGetValue(consoleKey, out var gameList))
                        continue;

                    // Remove entries pointing to missing files
                    gameList.ScrapedGames.RemoveAll(g =>
                    {
                        string relPath = g.Path.Replace("./", "").Replace("/", "\\");
                        string fullPath = Path.Combine(romDir, consoleKey, relPath);
                        return !File.Exists(fullPath);
                    });

                    // Write the cleaned gamelist
                    string consolePath = Path.Combine(romDir, consoleKey);
                    TreeNodeDetail dummy = new TreeNodeDetail()
                    {
                        Tag_ConsolePath = consolePath
                    };


                    GameListManager.WriteGameListToFile(dummy);
                }
            }

        }
        private async Task StartNextScrapeAsync()
        {
            if (scrapeQueue.Count == 0 || activeScrapes >= maxConcurrency)
                return;

            TreeNodeDetail node = scrapeQueue.Dequeue();
            var panel = new ScrapePanelControl { FileName = node.FileName + node.FileExtension };
            progressForm.AddScrapePanel(panel);
            activeScrapes++;

            // queue‐management only
            panel.ScrapeCompleted += async panelControl =>
            {
                scrapedNodes.Add(node);
                activeScrapes--;
                await StartNextScrapeAsync();
            };

            panel.ScrapeFailed += async (panelControl, reason) =>
            {
                progressForm.HandleScrapeFailed(panelControl, reason);
                activeScrapes--;
                await StartNextScrapeAsync();
            };

            // fire off the scrape, driving the panel UI...
            var resultNode = await GameFileProcessor.ProcessSingleGameFileAsync(
                node,
                (pct, status) =>
                {
                    panel.SetProgress(pct);
                    panel.SetStatus(status);
                },
                // ← new “onRenamed” callback, runs as soon as GetXmlDataForNode does its rename
                newFullPath =>
                {
                    Invoke(new Action(() =>
                    {
                        panel.FileName = Path.GetFileName(newFullPath);
                        panel.SetFileNameColor(Color.Purple);
                    }));
                },
                progressForm.Token
            );

            // ...and finally tell the panel whether we succeeded or not:
            if (resultNode == null)
            {
                panel.RaiseScrapeFailed("Scrape failed");
            }
            else
            {
                // you no longer need to re-check UseHash here,
                // since the onRenamed callback already did the filename+color swap.
                PostScrapeUpdates(node);
                panel.RaiseScrapeCompleted();
            }
        }

        private void SortTreeNodesAlphabetically(TreeNodeDetail consoleNode)
        {
            var sorted = consoleNode.Nodes.Cast<TreeNode>()
                .OrderBy(n => n.Text, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            consoleNode.Nodes.Clear();
            consoleNode.Nodes.AddRange(sorted);
        }

        void PostScrapeUpdates(TreeNodeDetail node)
        {

            GameListManager.PostProcess(node, seRomDir);
            UpdateGameNodeLabel(node);

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) //M3U
        {
            //string checks = active_TreeNodeName();

            TreeNode[] checkedNodes = GetSelectedNodes();
            TreeNodeDetail node = (TreeNodeDetail)checkedNodes[0];

            if (node.isConsole || node.isSubDir)
                node = (TreeNodeDetail)checkedNodes[1];

            //string subPath = "";
            List<string> gamePaths = new List<string>();


            foreach (TreeNodeDetail n in checkedNodes)
            {
                if (n.isSubDir || n.isConsole)
                    continue;
                else
                    gamePaths.Add(n.Tag_ConsoleRomPath.TrimStart('\\'));
            }




            //get all nodes that are checked
            //send them to the m3u form for editing then send back for processing
            if (checkedNodes.Length > 1)
            {
                M3USettings.GameFiles = gamePaths.ToArray();  //GetSelectedNodeNames();
                m3u = new Form_M3U();
                DialogResult dr = m3u.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    CreateM3U.CreateM3UFile(Path.Combine(romDir, node.Tag_ConsoleName, node.Tag_ConsolePath));
                    string fullPathNew = Path.Combine(romDir, node.Tag_ConsoleName, M3USettings.fileNameM3U) + ".m3u";

                    //if(node.Tag_SubDirPath != null)
                    //    fullPathNew = Path.Combine(romDir, node.Tag_ConsoleName, node.Tag_SubDirPath, M3USettings.fileNameM3U)+".m3u";

                    TreeNodeDetail m3uNode = new TreeNodeDetail(M3USettings.fileNameM3U)
                    {
                        Tag_FullPath = fullPathNew,

                        Game = new ScrapedGame() { Name = M3USettings.fileNameM3U },
                        Text = M3USettings.fileNameM3U + ".m3u",
                        Tag_ConsolePath = node.Tag_ConsolePath,
                    };

                    if (M3USettings.copyMetaData)
                    {
                        m3uNode.Game = node.Game.Clone(m3uNode.Tag_RelativePath);
                        // m3uNode.Game.Path = m3uNode.Tag_RelativePath;

                    }

                    if (M3USettings.hideFiles)
                    {
                        foreach (TreeNode tn in checkedNodes)
                        {
                            var myNode = tn as TreeNodeDetail;

                            myNode.Game.Hidden = true;
                        }
                    }


                    TreeNode consoleNode = ReturnConsoleNodeFromChild(node);
                    consoleNode.Nodes.Add(m3uNode);

                    GameListManager.UpdateGameListEntry(m3uNode);
                    GameListManager.WriteGameListToFile(m3uNode);
                }
                DeselectAllNodes();

            }
            else
                MessageBox.Show("Need to select multiple files to create an M3U file");

        }
        public void DeselectAllNodes()
        {
            foreach (TreeNode node in tv_ActiveRomDir.Nodes)
            {
                // If the node is checked, return it
                if (node.Checked)
                {
                    node.Checked = false;
                }

                // Also recurse into children
                foreach (TreeNode child in GetCheckedNodes(node.Nodes))
                {
                    child.Checked = false;
                }
            }
        }
        public static IEnumerable<TreeNode> GetCheckedNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                TreeNodeDetail customNode = node as TreeNodeDetail;

                // If the node is checked, return it
                if (customNode.Checked)
                {
                    yield return customNode;
                }

                // Also recurse into children
                foreach (TreeNode child in GetCheckedNodes(node.Nodes))
                {
                    TreeNodeDetail myChild = child as TreeNodeDetail;
                    yield return myChild;
                }
            }
        }
        TreeNode[] GetSelectedNodes()
        {
            List<TreeNode> checkedNodes = new List<TreeNode>();
            GetAllCheckedNodes(tv_ActiveRomDir.Nodes, checkedNodes);
            return checkedNodes.ToArray();
        }

        private void testCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ssa.TestCredentials();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Application.Exit();
        }

        private Image LoadThumbnailImage(string imgPath, int maxWidth, int maxHeight)
        {
            // Load the file into a byte array so the file is closed immediately.
            byte[] imageBytes = File.ReadAllBytes(imgPath);
            using (var ms = new MemoryStream(imageBytes))
            {
                using (Image original = Image.FromStream(ms))
                {
                    // Calculate the scale factor to fit the image within the maximum dimensions.
                    float scale = Math.Min((float)maxWidth / original.Width, (float)maxHeight / original.Height);
                    int thumbWidth = (int)(original.Width * scale);
                    int thumbHeight = (int)(original.Height * scale);

                    // Create a new bitmap with the calculated dimensions.
                    Bitmap thumb = new Bitmap(thumbWidth, thumbHeight);
                    using (Graphics g = Graphics.FromImage(thumb))
                    {
                        // Set high-quality interpolation.
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.DrawImage(original, new Rectangle(0, 0, thumbWidth, thumbHeight));
                    }
                    return thumb;
                }
            }
        }

        // Helper method to extract the image type label from a file name.
        private string GetImageTypeLabel(string imgPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(imgPath);
            // Split the filename on dashes.
            var parts = fileName.Split('-');
            if (parts.Length >= 2)
            {
                // If there are at least three parts, join the last two tokens.
                string typeLabel = parts.Length >= 3
                    ? $"{parts[parts.Length - 2]} {parts[parts.Length - 1]}"
                    : parts[parts.Length - 1];
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(typeLabel.ToLower());
            }
            return "Unknown";
        }
        string clickedImgPath = ""; // store selected image path globally

        private void btn_Main_ShowAllImages_Click(object sender, EventArgs e)
        {
            TreeNodeDetail node = active_TreeNode();
            if (!Path.Exists(seRomDir)) return;

            Form formImages = new Form { Text = "All Images", Size = new Size(850, 600), StartPosition = FormStartPosition.CenterScreen, AutoScroll = true };
            List<string> imgPaths = new();
            string searchPattern = $"*{node.FileName}*.png";
            string[] files = Directory.Exists(seRomDir) ? Directory.GetFiles(seRomDir, searchPattern, SearchOption.AllDirectories) : Array.Empty<string>();
            if (files.Length == 0)
                files = Directory.GetFiles(node.Tag_ConsolePath, searchPattern, SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                MessageBox.Show("No images found.");
                return;
            }

            imgPaths.AddRange(files.Where(f => f.Contains(node.FileName)));

            int x = 10, y = 10;
            foreach (string imgPath in imgPaths)
            {
                PictureBox pictureBox = new PictureBox
                {
                    Size = new Size(250, 250),
                    Location = new Point(x, y),
                    SizeMode = PictureBoxSizeMode.CenterImage,
                    Image = LoadThumbnailImage(imgPath, 250, 250),
                    Tag = imgPath
                };

                pictureBox.MouseUp += PictureBox_MouseUp; // assign context menu handler
                formImages.Controls.Add(pictureBox);

                formImages.Controls.Add(new Label
                {
                    Text = GetImageTypeLabel(imgPath),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(250, 20),
                    Location = new Point(x, y + 250)
                });

                x += 260;
                if (x > formImages.ClientSize.Width - 250) { x = 10; y += 270; }
            }

            formImages.ShowDialog();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void PictureBox_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && editingInProgress)
            {
                PictureBox pic = sender as PictureBox;
                clickedImgPath = pic.Tag.ToString();

                var contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Main", null, (s, ea) => SetGameImage("Main"));
                contextMenu.Items.Add("Thumbnail", null, (s, ea) => SetGameImage("Thumbnail"));
                contextMenu.Items.Add("Marquee", null, (s, ea) => SetGameImage("Marquee"));
                contextMenu.Show(Cursor.Position);
            }
        }

        private void SetGameImage(string type)
        {
            if (string.IsNullOrEmpty(clickedImgPath)) return;

            TreeNodeDetail node = active_TreeNode();
            if (node.Game == null)
                node.Game = new ScrapedGame() { Path = node.Tag_RelativePath };

            string destDir = Path.Combine(node.Tag_ConsolePath, "images");
            Directory.CreateDirectory(destDir);

            string suffix = type == "Main" ? "-image.png" :
                            type == "Thumbnail" ? "-thumb.png" :
                            "-marquee.png";

            string destFileName = node.FileName + suffix;
            string destPath = Path.Combine(destDir, destFileName);

            try
            {
                File.Copy(clickedImgPath, destPath, true); // overwrite = true

                string relPath = Path.Combine("./images", destFileName).Replace("\\", "/");

                if (type == "Main")
                {
                    node.Game.Image = relPath;
                    if (node.Game.Description != "")
                    {
                        node.ForeColor = Color.Black;
                        node.BackColor = Color.White;
                    }
                }
                else if (type == "Thumbnail")
                    node.Game.Thumbnail = relPath;
                else
                    node.Game.Marquee = relPath;

                //MessageBox.Show($"{type} image set successfully.");
                PopulateDisplayArea();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to assign image: {ex.Message}");
            }
        }

        //private void setSEDirToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    using (FolderBrowserDialog dialog = new FolderBrowserDialog())
        //    {
        //        if (dialog.ShowDialog() == DialogResult.OK)
        //        {
        //            seRomDir = dialog.SelectedPath + "\\roms\\";
        //            Directory.CreateDirectory(seDir);

        //            if (!string.IsNullOrEmpty(seDir))
        //            {
        //                appSettings.SetSEDir(seDir);
        //            }
        //        }
        //    }
        //}

        private void devModeOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devModeOffToolStripMenuItem.Text.Contains("Off"))
            {
                devModeOffToolStripMenuItem.Text = "Dev Mode On";
                devModeOffToolStripMenuItem.BackColor = Color.Red;
                ScrapeSettings.UseDummyHash = true;
            }
            else
            {
                devModeOffToolStripMenuItem.Text = "Dev Mode Off";
                devModeOffToolStripMenuItem.BackColor = Color.LightGray;
                ScrapeSettings.UseDummyHash = false;
            }


        }

        private SettingsControlPanel settingsPanel;

        void OpenSettingsControlPanel()
        {
            // Avoid adding multiple instances
            if (settingsPanel != null) return;

            settingsPanel = new SettingsControlPanel(appSettings, ssa);
            settingsPanel.Dock = DockStyle.Fill;

            // Optionally wire up a close event (next section shows how)
            settingsPanel.OnCloseRequested += SettingsPanel_OnCloseRequested;

            settingsPanel.OnCloseRequestedWithInfo += async msg =>
            {
                if (msg[0])
                {
                    romDir = appSettings.RomDirectory;
                    InitializeTreeViewLoading();
                }
                if (msg[1]) 
                {
                   Update_SE_DIRs( appSettings.SEDirectory);
                }
            };


            this.Controls.Add(settingsPanel);
            settingsPanel.BringToFront();

            // Disable conflicting UI
            gameToolStripMenuItem.Enabled = false;
            settingsToolStripMenuItem1.Enabled = false;
        }


        private void SettingsPanel_OnCloseRequested()
        {
            if (settingsPanel != null)
            {
                this.Controls.Remove(settingsPanel);
                settingsPanel.Dispose();
                settingsPanel = null;

                // Re-enable the rest of the UI
                gameToolStripMenuItem.Enabled = true;
                settingsToolStripMenuItem1.Enabled = true;
            }
        }



        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenSettingsControlPanel();

        }
    }
}

