namespace ScrapeEdit
{
    public static class M3USettings
    {
        public static string[] GameFiles = new string[0];
        public static bool moveFiles;
        public static int IndexOption;
        public static bool hideFiles;
        public static string fileNameM3U = "";
        public static bool copyMetaData = false;
    }

    public static class CreateM3U
    { 
        public static void CreateM3UFile(string filePath)
        {
            // Create a new M3U file
            string m3uPath = Path.GetFullPath(filePath);

            string m3uName = Path.GetFileNameWithoutExtension(M3USettings.fileNameM3U) + ".m3u";

            string m3uFullPath = Path.Combine(m3uPath, m3uName);

            // Create the directory if it doesn't exist
            if (!Directory.Exists(m3uPath))
            {
                Directory.CreateDirectory(m3uPath);
            }
            // Create the M3U file
            using (StreamWriter file = new StreamWriter(m3uFullPath))
            {
                // Write the header
                //file.WriteLine("#EXTM3U");
                // Write the game files
                foreach (string gameFile in M3USettings.GameFiles)
                {
                    file.WriteLine(gameFile);
                }
            }

            MessageBox.Show("M3U file created successfully.", "M3U File Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

}
