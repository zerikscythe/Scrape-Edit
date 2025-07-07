using Force.Crc32;
using ScrapeEdit;
using System.Xml;

public static class MediaDownloader
{
    public static async Task DownloadMediaAsync(
        TreeNodeDetail treenode,
        string xmlData,
        List<string> mediaTypesToDownload,
        CancellationToken token
    )
    {
        // Now proceed with your existing download logic...
        try
        {
            var doc = new XmlDocument();
            xmlData = ObfuscateDevCredentials.DeObfuscate(xmlData);
            doc.LoadXml(xmlData);

            var mediaNodes = doc.SelectNodes("//jeu/medias/media")
                             ?? throw new ApplicationException("No media entries found in the XML.");

            var mediaGroups = new Dictionary<string, List<XmlNode>>();
            foreach (XmlNode node in mediaNodes)
            {
                var type = node.Attributes["type"]?.Value;
                if (type != null && mediaTypesToDownload.Contains(type))
                {
                    if (!mediaGroups.ContainsKey(type))
                        mediaGroups[type] = new List<XmlNode>();
                    mediaGroups[type].Add(node);
                }
            }

            var downloadTasks = new List<Task>();
            foreach (var kvp in mediaGroups)
            {
                token.ThrowIfCancellationRequested();
                string type = kvp.Key;
                var candidates = kvp.Value;

                XmlNode selected = candidates
                    .FirstOrDefault(n => n.Attributes["region"]?.Value == GlobalDefaults.DefaultRegionAbrv)
                    ?? candidates.FirstOrDefault(n => string.Equals(n.Attributes["region"]?.Value, "wor", StringComparison.OrdinalIgnoreCase))
                    ?? candidates.FirstOrDefault(n => string.IsNullOrEmpty(n.Attributes["region"]?.Value))
                    ?? candidates.First();

                string url = selected.InnerText;
                string format = selected.Attributes["format"]?.Value ?? "bin";

                downloadTasks.Add(
                    DownloadAndSaveMediaAsync(url, type, format, 
                    treenode, token)
                );
            }

            await Task.WhenAll(downloadTasks);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error downloading media for '{treenode.FileName}': {ex.Message}");
        }
    }

    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task DownloadAndSaveMediaAsync(
    string url,
    string type,
    string format,
    TreeNodeDetail node = null,
    CancellationToken ct = default)
    {
        try
        {
            string folderPath = "";

            if (!node.isConsole)
            {
                folderPath = string.IsNullOrEmpty(node.Tag_SubDirPath)
                ? Path.Combine(SessionSettings.SEDirectory_ROM, node.Tag_ConsoleName, type)
                : Path.Combine(SessionSettings.SEDirectory_ROM, node.Tag_ConsoleName, node.Tag_SubDirPath, type);
            }
            if (node.isConsole)
            {
                folderPath = Path.Combine(SessionSettings.SettingsFolder + "\\Cache\\Consoles\\"+node.Tag_ConsoleName);
            }

            Directory.CreateDirectory(folderPath);

            using var response = await _httpClient.GetAsync(url, ct);
            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            var mediaData = await response.Content.ReadAsByteArrayAsync(ct);

            string baseFileName = "";
            if(node.isConsole)
                baseFileName = $"{node.Tag_ConsoleName}-{type}.{format}";
            else
                baseFileName = $"{node.FileName}-{type}.{format}";

            string filePath = Path.Combine(folderPath, baseFileName);

            if (File.Exists(filePath))
            {
                string existingCrc = Hash.Compute_CRC32(filePath);
                string downloadedCrc = Crc32Algorithm.Compute(mediaData).ToString("X8");

                if (string.Equals(existingCrc, downloadedCrc, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
                else
                {
                    return; // ❌ Skip — don’t write a conflicting file or alt
                }
            }

            await File.WriteAllBytesAsync(filePath, mediaData, ct);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
        }
    }

}

