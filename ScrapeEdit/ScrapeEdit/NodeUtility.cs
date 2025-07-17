using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapeEdit
{
    public static class NodeUtility
    {
        public static TreeNodeDetail Active_TreeNode(TreeView tv)
        {
            return tv.SelectedNode as TreeNodeDetail;
        }
        public static void SetNodeText_Color(TreeNodeDetail node)
        {
            UpdateGameNodeLabel(node);
            SetNodeGrayIfIncomplete(node);
        }
        private static void UpdateGameNodeLabel(TreeNodeDetail node)
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
        private static void SetNodeGrayIfIncomplete(TreeNodeDetail node)
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
        public static Image LoadImage(string filePath)
        {
            try
            {
                byte[] imageBytes = File.ReadAllBytes(filePath);
                using (var ms = new MemoryStream(imageBytes))
                {
                    return Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        public static void OpenFile(string filePath)
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
        public static void OpenFileLocation(TreeNodeDetail node)
        {
            string targetPath;

            if (node.isConsole)
            {
                targetPath = node.Tag_FullPath;
            }
            else if (node.Parent == null)
            {
                targetPath = SessionSettings.RomDirectory;
            }
            else
            {
                targetPath = Path.GetDirectoryName(node.Tag_FullPath);
            }

            if (string.IsNullOrEmpty(targetPath) || !Directory.Exists(targetPath))
            {
                MessageBox.Show("Directory location not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = targetPath,
                    UseShellExecute = true // Open in File Explorer
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening file location: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void SetChildNodesChecked(TreeNode node, bool isChecked)
        {
            foreach (TreeNode child in node.Nodes)
            {
                child.Checked = isChecked;
                SetChildNodesChecked(child, isChecked);
            }
        }
        public static void TrimTreeViewNodes(TreeNodeDetail parentNode)
        {
            foreach (TreeNodeDetail node in parentNode.Nodes.Cast<TreeNodeDetail>().ToList())
            {
                if (node.isConsole && node.Nodes.Count == 0)
                {
                    parentNode.Nodes.Remove(node);
                }
            }
        }
        public static TreeNodeDetail ReturnConsoleNodeFromChild(TreeNodeDetail parentNode)
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
        public static void DeselectAllNodes(TreeView tv)
        {
            foreach (TreeNode node in tv.Nodes)
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
        public static IEnumerable<TreeNodeDetail> GetCheckedNodes(TreeNodeCollection nodes, Func<TreeNodeDetail, bool> filter = null)
        {
            foreach (TreeNode node in nodes)
            {
                if (node is TreeNodeDetail detail)
                {
                    if (detail.Checked && (filter == null || filter(detail)))
                        yield return detail;

                    foreach (var child in GetCheckedNodes(detail.Nodes, filter))
                        yield return child;
                }
            }
        }
        public static void SortTreeNodesAlphabetically(TreeNodeDetail consoleNode)
        {
            var sorted = consoleNode.Nodes.Cast<TreeNode>()
                .OrderBy(n => n.Text, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            consoleNode.Nodes.Clear();
            consoleNode.Nodes.AddRange(sorted);
        }
        public static void Rebuild(bool local, TreeNodeDetail node)
        {
            if (local)
            {
                if (File.Exists(Path.Combine(node.Tag_ConsolePath, "images", node.FileName + "-image.png")))
                {
                    node.Game.Name = Path.GetFileNameWithoutExtension(node.Text);
                    node.Game.Path = node.Tag_RelativePath;
                    GameListManager.PostProcess(node);
                    GameListManager.WriteGameListToFile(node);
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
                GameListManager.PostProcess(node);
                SetNodeText_Color(node);
            }
        }
    }
}
