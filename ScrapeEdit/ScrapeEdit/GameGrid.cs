using ScapeEdit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace ScrapeEdit
{
    public partial class GameGrid : UserControl
    {
        TreeNodeDetail node;

        string clickedImgPath = ""; // store selected image path globally
        string[] ActiveImgSet;
        int ActiveImgIndex = 0;

        public GameGrid(TreeNodeDetail node)
        {
            InitializeComponent();
            chk_Main_EDIT.CheckedChanged += Chk_Main_EDIT_CheckedChanged;
            lbl_Main_Video.Click += LBL_Video_Click;
            lbl_Main_Man.Click += LBL_Manual_Click;
            chk_Main_EDIT.CheckedChanged += Chk_Main_EDIT_CheckedChanged;
            btn_Main_SaveChanges.Click += Btn_Main_SaveChanges_Click;
            btn_Main_ImgDOWN.Click += btn_Main_ImgDOWN_Click;
            btn_Main_ImgUP.Click += btn_Main_ImgUP_Click;
            btn_Main_ShowAllImages.Click += btn_Main_ShowAllImages_Click;

            //PB
            pb_ImgDisplay.MouseClick += Pb_ImgDisplay_Click;
            pb_ImgDisplay.SizeMode = PictureBoxSizeMode.Zoom;

            DisableEditControls();
            btn_Main_ImgDOWN.Enabled = false;
            btn_Main_ImgUP.Enabled = false;
            chk_Main_EDIT.Enabled = false;


            this.node = node;
            chk_Main_EDIT.Enabled = !node.isConsole;

            DisableEditControls();
            PopulateDisplayArea();
        }

        private void Chk_Main_EDIT_CheckedChanged(object? sender, EventArgs e)
        {
            if (chk_Main_EDIT.Checked)
                EnableEditControls();
            else
                DisableEditControls();

        }

        private void PopulateDisplayArea()
        {
            ScrapedGame sg = node.Game; //Active_ScrapedGame();
            ScrapedConsole sc = node.Console;

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
                lbl_Main_HASH.Text = "Hash: " + node.Tag_HashMatchFound.ToString();

                ActiveImgSet = new string[3]
                {
                    Path.Combine(node.Tag_ConsolePath, sg.Image == null  || sg.Image == "" ? "Missing Image File" : sg.NormalizePath(sg.Image)),
                    Path.Combine(node.Tag_ConsolePath, sg.Thumbnail == null || sg.Thumbnail == "" ? "Missing Thumbnail File" : sg.NormalizePath(sg.Thumbnail)),
                    Path.Combine(node.Tag_ConsolePath, sg.Marquee == null || sg.Marquee == "" ? "Missing Marquee File" : sg.NormalizePath(sg.Marquee))
                };
                ActiveImgIndex = 0;
                LoadSGImages();
            }

        }


        void DisableEditControls()
        {
           SessionSettings.editingInProgress = false;

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
            //btn_Main_ShowAllImages.Enabled = false;
        }
        void EnableEditControls()
        {
            SessionSettings.editingInProgress = true;

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
            //btn_Main_ShowAllImages.Enabled = true;

            if (ActiveImgSet == null || ActiveImgSet.Length == 0)
            {
                ActiveImgSet = new string[3]
                {
                    "","",""
                };
            }
        }

        private void btn_Main_ShowAllImages_Click(object sender, EventArgs e)
        {

            if (node.isConsole || node.isSubDir)
                return;

            string fileFolder = Path.Combine(SessionSettings.SEDirectory_ROM, node.Tag_ConsoleName);
            if (!Path.Exists(fileFolder)) return;

            Form formImages = new Form { Text = "All Images", Size = new Size(850, 600), StartPosition = FormStartPosition.CenterScreen, AutoScroll = true };
            List<string> imgPaths = new();
            string searchPattern = $"*{node.FileName}*.png";
            string[] files = Directory.GetFiles(fileFolder, searchPattern, SearchOption.AllDirectories);// : Array.Empty<string>();
            
            // fallback to local directory if no images found in SE_ROM directory
            if (files.Length == 0)
                files = Directory.GetFiles( Path.Combine(SessionSettings.RomDirectory,node.Tag_ConsoleName), searchPattern, SearchOption.AllDirectories);

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
            if (e.Button == MouseButtons.Right && SessionSettings.editingInProgress)
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

        private void Btn_Main_SaveChanges_Click(object? sender, EventArgs e)
        {
            SaveScrapedGameUserData(node);
            NodeUtility.SetNodeText_Color(node);
            GameListManager.WriteGameListToFile(node);

            //disable editing
            chk_Main_EDIT.Checked = false;
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
        //Image LoadImage(string filePath)
        //{
        //    try
        //    {
        //        byte[] imageBytes = File.ReadAllBytes(filePath);
        //        using (var ms = new MemoryStream(imageBytes))
        //        {
        //            return Image.FromStream(ms);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return null;
        //    }
        //}

        void LoadSGImages()
        {
            string missing = "";

            // Check if the file exists
            if (File.Exists(ActiveImgSet[ActiveImgIndex]))
            {
                // Load the image
                pb_ImgDisplay.Image = NodeUtility.LoadImage(ActiveImgSet[ActiveImgIndex]);
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
            NodeUtility.SetNodeText_Color(node);

        }
        private void Pb_ImgDisplay_Click(object? sender, MouseEventArgs e)
        {
            PictureBox pb = sender as PictureBox;

            // Check if the right mouse button was clicked
            if (e.Button == MouseButtons.Right && SessionSettings.editingInProgress)
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

        void LBL_Manual_Click(object sender, EventArgs e)
        {
            if (SessionSettings.editingInProgress)
            {

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
            else if (node != null)
            {

                if (node.Game.Manual != null)
                    NodeUtility.OpenFile(Path.Combine(node.Tag_ConsolePath, node.Game.Manual));
                else
                    MessageBox.Show("No Manual Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void LBL_Video_Click(object sender, EventArgs e)
        {
            if (SessionSettings.editingInProgress)
            {

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

            else if (node.Game != null)
            {

                if (node.Game.Video != null)
                    NodeUtility.OpenFile(Path.Combine(node.Tag_ConsolePath, node.Game.Video));
                else
                    MessageBox.Show("No Video Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
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

        //void OpenFile(string filePath)
        //{
        //    if (!File.Exists(filePath))
        //    {
        //        MessageBox.Show($"{Path.GetFileName(filePath)}, not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    // Let the OS decide which app to use
        //    Process.Start(new ProcessStartInfo
        //    {
        //        FileName = filePath,
        //        UseShellExecute = true // This tells Windows to open it with the default program
        //    });
        //}


    }
}
