using ScapeEdit;

namespace ScrapeEdit
{
    public class TreeNodeDetail : TreeNode
    {
        public string Tag_ConsoleName 
        {
            get
            {
                if (Tag_ConsolePath != null)
                    return Path.GetFileName(Tag_ConsolePath).Replace("\\", "");
                else
                    return null;
            }
        }
        public string Tag_ConsoleID
        {
            get
            {
                if (Tag_ConsoleName != null)
                    return ConsoleIDHandler.GetConsoleID(Tag_ConsoleName);
                else
                    return null;
            }
        }
        public string Tag_ConsolePath { get; set; } = null;

        public string Tag_SubDirPath
        {
            get
            {
                string _buffer = Tag_ConsoleRomPath.Replace(FileName + FileExtension, "");

                if (_buffer == "\\")
                    return null;
                else 
                {
                    _buffer = _buffer.Substring(1, _buffer.Length - 2);

                    return _buffer;
                } 
            }
        }

        public string Tag_FullPath { get; set; } = null;
        public string Tag_RelativePath
        {
            get {
                if(Tag_FullPath != null && Tag_ConsolePath != null)
                    return FormatToRelativePath(Tag_ConsoleRomPath);
                else
                    return null;
            } 
        }
        public string Tag_ConsoleRomPath //file
        {
            get
            {
                if (Tag_FullPath != null && Tag_ConsolePath != null)
                    return GetConsoleRomPath(Tag_FullPath, Tag_ConsolePath);
                else
                    return null;
            }
        }
        public string FileExtension
        {
            get
            {
                if (Tag_FullPath != null && !isConsole && !isSubDir)
                    return Path.GetExtension(Tag_FullPath);
                else
                    return null;
            }
        }

        public string Tag_Hash { get; set; } = null; // hash of the file
        public bool Tag_HashMatchFound { get; set; } = false; // if the hash was found in the database

        public string FileNameFull
        {
            get
            {
                if (Tag_FullPath != null && !isConsole && !isSubDir)
                    return Path.GetFileName(Tag_FullPath);
                else
                    return null;
            }
        }

        public string FileName
        {
            get
            {
                if (Tag_FullPath != null && !isConsole && !isSubDir)
                    return Path.GetFileNameWithoutExtension(Tag_FullPath);
                else
                    return null;
            }
        }
        public bool isConsole { get; set; } = false;
        public bool isSubDir { get; set; } = false;

        ScrapedGame? _game;
        public ScrapedGame Game 
        {
            get { return _game; }
            set 
            {
                if (this.isSubDir || this.isConsole)
                    return;
                else
                    _game = value ;
            } 
        }

        public TreeNodeDetail() : base()
        { }

        public TreeNodeDetail(string text) : base(text)
        {
            
        }

        static string FormatToRelativePath(string fullPath)
        {
            string result = fullPath;

            // Remove filename if present
            if (File.Exists(fullPath) || result.Contains("."))
            {
                result = Path.GetDirectoryName(result)?.Replace("/", "").Replace("\\", "");
                if(result == "")
                    result = "./"+ Path.GetFileName(fullPath);
                else
                    result = "./"+ result +"/" +Path.GetFileName(fullPath);
            }
            return result;
        }

        static string GetConsoleRomPath(string fullPath, string knownSubDir)
        {
            // Normalize path for consistency (in case of different slashes)
            fullPath = Path.GetFullPath(fullPath);//.Replace("\\", "/");

            // Find the index where the known subdir starts
            int index = fullPath.IndexOf(knownSubDir, StringComparison.OrdinalIgnoreCase);

            if (index == -1)
            {
                return ""; // Known subdir not found, return empty string
            }

            // Extract everything after the known subdirectory
            int startIndex = index + knownSubDir.Length; // +2 to skip the "/"
            string result = fullPath.Substring(startIndex);



            return result;
        }
    }
}
