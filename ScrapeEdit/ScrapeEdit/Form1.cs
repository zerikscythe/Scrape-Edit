using System.Text;
using ScapeEdit;
using System.Diagnostics;
using System.Globalization;
using System.ComponentModel;

namespace ScrapeEdit
{
    public partial class Form1 : Form
    {
        AppSettings appSettings;
        ScreenScraperApi ssa;
        Form_M3U m3u;
        Form_Loading form_Loading;
        Dictionary<string, string> ValidEXT;//  
        TreeView tv_ActiveRomDir = new TreeView();
        ImageList nodeImageList = new ImageList();

        ConsoleList consoleList;
        private SettingsControlPanel settingsPanel;

        private ToolStripMenuItem viewXMLNodeToolStripMenuItem;
        private ToolStripMenuItem scrapeNodeToolStripMenuItem;
        private ToolStripMenuItem renameNodeToolStripMenuItem;
        private ToolStripMenuItem deleteNodeToolStripMenuItem;
        private ToolStripMenuItem deleteMasterNodeToolStripMenuItem;
        private ToolStripMenuItem createM3UToolStripMenuItem;
        private ToolStripMenuItem rebuildMediaToolStripMenuItem;
        private ToolStripMenuItem extensionFixToolStripMenuItem;

        private bool suppressCheckboxEvent = false;

        private bool _scrapeInProgress;
        bool scrapeInProgress
        {
            get => _scrapeInProgress;
            set
            {
                _scrapeInProgress = value;
                settingsToolStripMenuItem1.Enabled = !value;
            }
        }
        private Queue<TreeNodeDetail> scrapeQueue;
        private List<TreeNodeDetail> scrapedNodes;
        private int activeScrapes;
        private readonly int maxConcurrency = 8;
        private Form_DownloadProgress progressForm;

        private async void Form1_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SessionSettings.RomDirectory))
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
                consoleList = new ConsoleList();
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
            appSettings.Load();

            ssa = new ScreenScraperApi();

            // Ensure contextMenuTreeView is initialized before assigning it

            contextMenuTreeView.Opening += ContextMenuTreeView_Opening;


            //contextMenuTreeView = new ContextMenuStrip();
            scrapeNodeToolStripMenuItem = new ToolStripMenuItem("Scrape");
            rebuildMediaToolStripMenuItem = new ToolStripMenuItem("Rebuild Media");
            renameNodeToolStripMenuItem = new ToolStripMenuItem("Rename");
            createM3UToolStripMenuItem = new ToolStripMenuItem("Create M3U");
            deleteMasterNodeToolStripMenuItem = new ToolStripMenuItem("Delete Master Data");
            viewXMLNodeToolStripMenuItem = new ToolStripMenuItem("View Raw XML");
            deleteNodeToolStripMenuItem = new ToolStripMenuItem("Delete File(s)");

            extensionFixToolStripMenuItem = new ToolStripMenuItem("Fix Extensions");


            // Add items to context menu
            contextMenuTreeView.Items.Add(scrapeNodeToolStripMenuItem);
            contextMenuTreeView.Items.Add(rebuildMediaToolStripMenuItem);
            contextMenuTreeView.Items.Add(createM3UToolStripMenuItem);
            contextMenuTreeView.Items.Add(viewXMLNodeToolStripMenuItem);
            contextMenuTreeView.Items.Add(renameNodeToolStripMenuItem);
            contextMenuTreeView.Items.Add(deleteNodeToolStripMenuItem);
            contextMenuTreeView.Items.Add(deleteMasterNodeToolStripMenuItem);
            contextMenuTreeView.Items.Add(extensionFixToolStripMenuItem);


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

            extensionFixToolStripMenuItem.Click += ExtensionFixToolStripMenuItem_Click;

        }

        private void ExtensionFixToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            TreeNodeDetail node = active_TreeNode();

            if (node == null || !node.isConsole || node.Console == null)
            {
                MessageBox.Show("Please select a valid console node with metadata.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Try to get known extensions from ScrapedConsole
            string[] ext = node.Console.Extensions ?? [];

            // 2. Format into space-separated string like: .iso .chd .cue
            string prefill = string.Join(" ", ext.Where(e => !string.IsNullOrWhiteSpace(e)).Select(e =>
            {
                var trimmed = e.Trim();
                return trimmed.StartsWith(".") ? trimmed : "." + trimmed;
            }));

            // 3. Prompt user
            string result = Microsoft.VisualBasic.Interaction.InputBox(
                $"Enter the file extensions this console supports (space-separated):",
                "Fix Missing _info.txt",
                prefill
            );

            if (string.IsNullOrWhiteSpace(result))
            {
                MessageBox.Show("No extensions entered. Operation canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 4. Write _info.txt to parent directory
            try
            {
                string systemPath = node.Tag_ConsolePath;
                if (!Directory.Exists(systemPath))
                {
                    MessageBox.Show("System folder not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string infoFilePath = Path.Combine(systemPath, "_info.txt");
                string content = $"ROM files extensions accepted: \"{result.Trim()}\"";

                File.WriteAllText(infoFilePath, content);

                MessageBox.Show("_info.txt created successfully.\nPlease reload the ROM directory.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing _info.txt: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ContextMenuTreeView_Opening(object sender, CancelEventArgs e)
        {
            var selectedNode = tv_ActiveRomDir.SelectedNode;

            if (selectedNode?.Tag?.ToString() == "Error")
            {
                // Only allow the extension fix item
                foreach (ToolStripItem item in contextMenuTreeView.Items)
                {
                    if (item is ToolStripMenuItem menuItem)
                    {
                        menuItem.Visible = menuItem == extensionFixToolStripMenuItem;
                    }
                }
            }
            else
            {
                // Show everything as normal
                foreach (ToolStripItem item in contextMenuTreeView.Items)
                {
                    item.Visible = true;
                }
            }
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
                    GameListManager.PostProcess(node);
                    GameListManager.WriteGameListToFile(node);

                    //PopulateDisplayArea();
                }
                else
                {
                    MessageBox.Show("No local image found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else   //search for SE data
            {
                string pathToXML = SessionSettings.SEDirectory_ROM + "\\" + node.Tag_ConsoleName + node.Tag_ConsoleRomPath + ".xml";
                try
                {
                    if (!File.Exists(pathToXML))
                    {
                        MessageBox.Show("No XML data found for this game!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error accessing XML file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string xmlData = File.ReadAllText(pathToXML, Encoding.UTF8);
                node.Game = GameListManager.ConvertSSXmlToScrapedGame(xmlData, node);
                PostScrapeUpdates(node);
            }

            //PopulateDisplayArea();
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
            tv_ActiveRomDir.Location = new Point(10, 40);
            tv_ActiveRomDir.Width = 500;
            tv_ActiveRomDir.Height = 630;
            tv_ActiveRomDir.AfterExpand += Tv_ActiveRomDir_AfterExpand;
            tv_ActiveRomDir.AfterSelect += TV_GameNodeSelect;
            tv_ActiveRomDir.MouseUp += tv_ActiveRomDir_MouseUp;
            tv_ActiveRomDir.BeforeCheck += tv_CHK_BeforeCheck;
            tv_ActiveRomDir.AfterCheck += TV_CHK_NodeSelection;


            tv_ActiveRomDir.ImageList = nodeImageList;
            nodeImageList.ColorDepth = ColorDepth.Depth32Bit;
            nodeImageList.ImageSize = new Size(16, 16); // or 24, 24 if needed
            nodeImageList.Images.Add("blank", new Bitmap(16, 16));


            tv_ActiveRomDir.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            Controls.Add(tv_ActiveRomDir);
        }

        private void tv_CHK_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            TreeNodeDetail node = e.Node as TreeNodeDetail;

            // Prevent unchecking the currently active node
            if (node == active_TreeNode() && node.Checked && !node.isConsole)
            {
                e.Cancel = true;
                return;
            }
        }

        private void TV_CHK_NodeSelection(object sender, TreeViewEventArgs e)
        {
            TreeNodeDetail node = e.Node as TreeNodeDetail;
            if (node == null)
                return;

            suppressCheckboxEvent = true;

            try
            {
                if (node.isConsole)
                {
                    if (node.Checked)
                    {
                        node.Expand();
                        SetChildNodesChecked(node, true);
                    }
                    else
                    {
                        SetChildNodesChecked(node, false);
                    }
                }

            }
            finally
            {
                suppressCheckboxEvent = false;
            }
        }

        private void SetChildNodesChecked(TreeNode node, bool isChecked)
        {
            foreach (TreeNode child in node.Nodes)
            {
                child.Checked = isChecked;
                SetChildNodesChecked(child, isChecked);
            }
        }

        private void TV_GameNodeSelect(object sender, TreeViewEventArgs e)
        {
            TreeNodeDetail node = e.Node as TreeNodeDetail;

            Control oldGrid = this.Controls.OfType<GameGrid>().FirstOrDefault();

            if (oldGrid == null)
                oldGrid = this.Controls.OfType<ConsoleGrid>().FirstOrDefault();

            if (oldGrid != null)
                this.Controls.Remove(oldGrid);



            if (SessionSettings.editingInProgress)
            {
                MessageBox.Show("You must finish editing before selecting a new game.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (node == null)
                return;

            DeselectAllNodes();

            if (!node.isConsole)
            {
                node.Checked = true;
                GameGrid gameGrid = new GameGrid(node);
                gameGrid.Location = new Point(520, 40);
                this.Controls.Add(gameGrid);

            }
            else if(!node.isSubDir)
            {
                //node.Checked = true;
                ConsoleGrid consoleGrid = new ConsoleGrid(node);
                consoleGrid.Location = new Point(520, 40);
                this.Controls.Add(consoleGrid);
            }
            //chk_Main_EDIT.Enabled = !node.isConsole;



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
            if (!SessionSettings.editingInProgress)
            { 
                if(active_TreeNode().isConsole)//select all children nodes 1st
                {
                    active_TreeNode().Expand();
                    SetChildNodesChecked(active_TreeNode(), true);
                }

                ScrapeSingleOrMulti(GetSelectedNodes()); 
            
            }
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
                string xmlFilePath = Path.Combine(SessionSettings.SEDirectory_ROM, fileNameWithoutExtension + ".xml");

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
            if (!Directory.Exists(SessionSettings.RomDirectory))
            {
                Console.WriteLine($"[DeleteGameMetadata] Directory does not exist: {SessionSettings.RomDirectory}");
                return;
            }

            string systemFolder = Path.Combine(SessionSettings.SEDirectory_ROM, node.Tag_ConsoleName, node.Tag_ConsolePath);// Path.GetFileName(Path.GetDirectoryName(fileNameWithoutExtension)));

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

        TreeNodeDetail active_TreeNode()
        {
            return tv_ActiveRomDir.SelectedNode as TreeNodeDetail;
        }
        void SetValidEXT(string basePath)
        {
            ValidEXT = new Dictionary<string, string>();

            foreach (var systemDir in Directory.GetDirectories(basePath))
            {
                string systemName = new DirectoryInfo(systemDir).Name.ToLower();
                string infoFilePath = Path.Combine(systemDir, "_info.txt");

                if (File.Exists(infoFilePath))
                {
                    string[] lines = File.ReadAllLines(infoFilePath);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("ROM files extensions accepted:"))
                        {
                            string extPart = line.Split('"')[1].Trim(); // Extract text inside quotes

                            // Split, trim, and remove empty extensions
                            var cleaned = extPart
                                .Replace(" ", ",")
                                .Split(',')
                                .Select(e => e.Trim().ToLowerInvariant())
                                .Where(e => !string.IsNullOrWhiteSpace(e));

                            if (cleaned.Any())
                            {
                                ValidEXT[systemName] = string.Join(",", cleaned);
                            }
                        }
                    }
                }
                else if (!ValidEXT.ContainsKey(systemName))
                {
                    ValidEXT[systemName] = "";
                }
            }
        }

        public void PopulateTreeView(IProgress<StatusInfo> progress, IProgress<FileProgressInfo> fileProgress)
        {
            Invoke(() =>
            {
                tv_ActiveRomDir.BeginUpdate();
                tv_ActiveRomDir.Nodes.Clear();
            });

            DirectoryInfo rootDir = new DirectoryInfo(SessionSettings.RomDirectory);
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

                progress?.Report(new StatusInfo($"Processing: {consoleDir.Name}", consoleIndex, totalConsoles, consoleIndex * 100 / totalConsoles));

                TreeNodeDetail consoleNode = new TreeNodeDetail(consoleDir.Name)
                {
                    Tag_ConsolePath = consoleDir.FullName,
                    Tag_FullPath = consoleDir.FullName,
                    isConsole = true
                };

                Invoke(() => rootNode.Nodes.Add(consoleNode));

                // ✅ MISSING INFO.TXT HANDLING HERE:
                if (!ValidEXT.ContainsKey(consoleKey) || string.IsNullOrWhiteSpace(ValidEXT[consoleKey]))
                {
                    TreeNodeDetail errorNode = new TreeNodeDetail("Missing _info.txt")
                    {
                        ForeColor = Color.Red,
                        isSubDir = false,
                        Tag_ConsolePath = consoleDir.FullName,
                        Tag = "Error"

                    };

                    Invoke(() =>
                    {
                        consoleNode.Nodes.Clear();
                        consoleNode.Nodes.Add(errorNode);
                    });

                    continue; // ⛔ Skip further processing for this console
                }

                HashSet<string> validExts = ValidEXT[consoleKey]
                    .Split(',')
                    .Select(e => e.Trim().ToLowerInvariant())
                    .Where(e => !string.IsNullOrWhiteSpace(e))
                    .ToHashSet();

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
                                Tag_ConsolePath = consoleDir.FullName,
                                ImageKey = "blank",
                                SelectedImageKey = "blank"
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
                        Tag_ConsolePath = consoleDir.FullName,
                        ImageKey = "blank",
                        SelectedImageKey = "blank"
                    };

                    NodeUtility.SetNodeText_Color(gameNode);
                    Invoke(() => currentParent.Nodes.Add(gameNode));

                    gameIndex++;
                    fileProgress?.Report(new FileProgressInfo(gameIndex, gameTotal));
                }

                AddMissingRomNodes(consoleDir, consoleNode, validExts, loadedPaths, gameList, fileProgress, ref gameIndex, ref gameTotal);

                Invoke(() => SortTreeNodesAlphabetically(consoleNode));
                GameListManager.WriteGameListToFile(consoleNode);
            }

            Invoke(() =>
            {
                TrimTreeViewNodes(rootNode);
                foreach (TreeNodeDetail tnd in rootNode.Nodes.Cast<TreeNodeDetail>())
                {
                    AttachConsoleMetadata(tnd);
                }

                rootNode.Expand();
                tv_ActiveRomDir.EndUpdate();
            });
        }

        private void AttachConsoleMetadata(TreeNodeDetail consoleNode)
        {
            if (consoleNode == null || !consoleNode.isConsole)
                return;

            int id = Convert.ToInt32(ConsoleIDHandler.GetConsoleID(consoleNode.Tag_ConsoleName));
            if (id < 0)
                return;

            var scrapedConsole = consoleList.GetConsoleById(id);
            if (scrapedConsole == null)
                return;

            scrapedConsole.TreeName = consoleNode.Tag_ConsoleName;
            consoleNode.Console = scrapedConsole;

            if (scrapedConsole.IconImg != null)
            {
                string iconKey = consoleNode.Tag_ConsoleName;

                // Add image to shared ImageList if not already present
                if (!nodeImageList.Images.ContainsKey(iconKey))
                {
                    try
                    {
                        nodeImageList.Images.Add(iconKey, scrapedConsole.IconImg);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to add icon for '{iconKey}': {ex.Message}");
                        return;
                    }
                }

                // Assign icon ONLY to the console node
                consoleNode.ImageKey = consoleNode.SelectedImageKey = iconKey;
            }
            else
            {
                // Remove icon assignment if it was set before (clean fallback)
                consoleNode.ImageKey = null;
                consoleNode.SelectedImageKey = null;
            }
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

                    if (entry.Name == "PC10" || entry.Name == "Crash 'n Burn (USA)")
                    { 
                        string test = "";
                    }
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
                        Tag_ConsolePath = consolePath,
                        ImageKey = "blank",
                        SelectedImageKey = "blank"
                    };

                    NodeUtility.SetNodeText_Color(gameNode);
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
            // Collapse all sibling nodes
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

            // Select and focus the expanded node
            tv_ActiveRomDir.SelectedNode = e.Node;
            e.Node.EnsureVisible(); // optional: scroll into view
            tv_ActiveRomDir.Focus();

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
        private async void ScrapeSingleOrMulti(TreeNode[] nodes)
        {
            TreeNodeDetail[] detailNodes = nodes.Cast<TreeNodeDetail>().ToArray();
            if (!GameFileProcessor.SetValuesFirst(ssa, ValidEXT)) return;

            Form_DownloadProgress progressForm = new Form_DownloadProgress();
            progressForm.SetTotal(detailNodes.Length);
            progressForm.Show();

            scrapeInProgress = true;

            //reload consoleIDs file incase any changes were made
            ConsoleIDHandler.consoleIds = ConsoleIDHandler.LoadConsoleIds();

            await RunScrapes(detailNodes, progressForm); // ✅ This starts and completes the full scrape process.

            scrapeInProgress = false;
        }
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
                        string fullPath = Path.Combine(SessionSettings.RomDirectory, consoleKey, relPath);
                        return !File.Exists(fullPath);
                    });

                    // Write the cleaned gamelist
                    string consolePath = Path.Combine(SessionSettings.RomDirectory, consoleKey);
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
            GameListManager.PostProcess(node);//, SessionSettings.RomDirectory);
            NodeUtility.SetNodeText_Color(node);
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
                    CreateM3U.CreateM3UFile(Path.Combine(SessionSettings.RomDirectory, node.Tag_ConsoleName, node.Tag_ConsolePath));
                    string fullPathNew = Path.Combine(SessionSettings.RomDirectory, node.Tag_ConsoleName, M3USettings.fileNameM3U) + ".m3u";

                    TreeNodeDetail m3uNode = new TreeNodeDetail(M3USettings.fileNameM3U)
                    {
                        Tag_FullPath = fullPathNew,

                        Game = new ScrapedGame()
                        {
                            Name = M3USettings.fileNameM3U,
                            Path = Path.GetFileName(fullPathNew)
                        },
                        Text = M3USettings.fileNameM3U + ".m3u",
                        Tag_ConsolePath = node.Tag_ConsolePath,

                    };

                    if (M3USettings.copyMetaData)
                    {
                        m3uNode.Game = node.Game.Clone(m3uNode.Tag_RelativePath);

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

                    NodeUtility.SetNodeText_Color(m3uNode);

                    SortTreeNodesAlphabetically(consoleNode as TreeNodeDetail);

                    GameListManager.UpdateGameListEntry(m3uNode);
                    GameListManager.WriteGameListToFile(m3uNode);

                    tv_ActiveRomDir.SelectedNode = m3uNode; // Select the new M3U node
                    tv_ActiveRomDir.Focus();

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
                    appSettings.Save();
                    InitializeTreeViewLoading();
                }
                if (msg[1])
                {
                    appSettings.Save();
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

        private async void dLConsoleDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string region = GlobalDefaults.DefaultRegionAbrv ?? "wor";

            foreach (ScrapedConsole sc in consoleList.ScrapedConsoles)
            {
                TreeNodeDetail tempNode = new TreeNodeDetail()
                {
                    isConsole = true,
                    Console = sc,
                    Tag_ConsolePath = Path.Combine(SessionSettings.SettingsFolder, "Cache","Consoles", sc.ShortName)
                };

                if (Directory.Exists(tempNode.Tag_ConsolePath)) //skips if the directory already exists
                    continue;

                foreach (var kvp in sc.Media.MediaByType)
                {
                    // Skip media types not in the desired list
                    if (!sc.DesiredConsoleMediaTypes.Contains(kvp.Key))
                        continue;

                    var preferred = sc.Media.GetPreferred(kvp.Key, region);
                    if (preferred == null)
                        continue;

                    tool_StatusLabel.Text = $"Downloading {preferred.Type} for {sc.Name}";



                    await MediaDownloader.DownloadAndSaveMediaAsync(
                        ObfuscateDevCredentials.DeObfuscate(preferred.Url),
                        preferred.Type,
                        preferred.Format,
                        tempNode
                    );
                }
            }
        }

    }
}

