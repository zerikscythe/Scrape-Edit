using Force.Crc32;
using System.Security.Cryptography;
using System.Text;

namespace ScrapeEdit
{
    public static class Hash
    {
        public static string ReturnHashType(string hash)
        {
            string hashType = "unknown";

            // Determine hash type by length and content
            if (hash.Length == 8 && hash.All(Uri.IsHexDigit))
                hashType = "romcrc";
            else if (hash.Length == 32 && hash.All(Uri.IsHexDigit))
                hashType = "rommd5";
            else if (hash.Length == 40 && hash.All(Uri.IsHexDigit))
                hashType = "romsha1";

            return hashType;
        }
        public static void Compute_CRC32(TreeNodeDetail node)
        {
            if (ScrapeSettings.UseDummyHash)
            {
                node.Tag_Hash = ReadDummyHashValue(node.Tag_FullPath, 0); // CRC32 = column 0
                return;
            }
            if (!File.Exists(node.Tag_FullPath))
                throw new FileNotFoundException($"File not found: {node.Tag_FullPath}");

            byte[] fileBytes = File.ReadAllBytes(node.Tag_FullPath);
            uint crc = Crc32Algorithm.Compute(fileBytes);
            node.Tag_Hash = crc.ToString("X8");
        }
        public static string Compute_CRC32(string filePath)
        {
            if (ScrapeSettings.UseDummyHash)
                return ReadDummyHashValue(filePath, 0); // CRC32 = column 0

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            byte[] fileBytes = File.ReadAllBytes(filePath);
            uint crc = Crc32Algorithm.Compute(fileBytes);
            return crc.ToString("X8");
        }
        public static void Compute_MD5(TreeNodeDetail node)
        {
            if (ScrapeSettings.UseDummyHash)
            { 
                node.Tag_Hash = ReadDummyHashValue(node.Tag_FullPath, 1); // MD5 = column 1
                return;                                                            
            }

            using (MD5 md5 = MD5.Create())
            {
                if (Directory.Exists(node.Tag_FullPath))
                {
                    byte[] folderBytes = Encoding.UTF8.GetBytes(node.Tag_FullPath);
                    node.Tag_Hash = ToHex(md5.ComputeHash(folderBytes));
                }
                else if (File.Exists(node.Tag_FullPath))
                {
                    byte[] fileBytes = File.ReadAllBytes(node.Tag_FullPath);
                    node.Tag_Hash = ToHex(md5.ComputeHash(fileBytes));
                }
                else
                {
                    throw new FileNotFoundException($"File not found: {node.Tag_FullPath}");
                }
            }
        }
        public static void Compute_SHA1(TreeNodeDetail node)
        {
            if (ScrapeSettings.UseDummyHash)
            { 
                node.Tag_Hash = ReadDummyHashValue(node.Tag_FullPath, 2); // SHA1 = column 2
                return; 
            }

            if (!File.Exists(node.Tag_FullPath))
                throw new FileNotFoundException($"File not found: {node.Tag_FullPath}");

            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] fileBytes = File.ReadAllBytes(node.Tag_FullPath);
                node.Tag_Hash = ToHex(sha1.ComputeHash(fileBytes));
            }
        }
        private static string ReadDummyHashValue(string filePath, int columnIndex)
        {
            // Return a random MD5-like value if the input is a directory
            if (Directory.Exists(filePath))
                return Guid.NewGuid().ToString("N").ToUpperInvariant(); // 32 hex chars

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Dummy hash file not found: {filePath}");

            string line = File.ReadAllText(filePath);

            if (line.Contains('\n'))
                line = line.Split('\n').FirstOrDefault(l => !string.IsNullOrWhiteSpace(l)).Trim();

            var values = line.Split(',');

            if (values.Length <= columnIndex)
                throw new InvalidDataException($"Expected at least {columnIndex + 1} values in dummy file: {filePath}");

            return values[columnIndex].Trim();
        }
        private static string ToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
