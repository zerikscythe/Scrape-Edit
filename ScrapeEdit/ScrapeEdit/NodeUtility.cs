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

    }
}
