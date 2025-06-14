using Force.Crc32;
using ScrapeEdit;
using System.Xml;

public class MediaDownloader
{
    public static async Task DownloadMediaAsync(
        TreeNodeDetail treenode,
        string seDir,
        string xmlData,
        List<string> mediaTypesToDownload,
        CancellationToken token,
        string oldFilename
    )
    {
        // ⬇️ 1) Purge any leftover media from the *old* filename  
        if (!string.IsNullOrEmpty(oldFilename)
            && !oldFilename.Equals(treenode.FileName, StringComparison.OrdinalIgnoreCase))
        {
            GameListManager.PurgeOldMediaFiles(treenode, oldFilename);
        }

        // ⬇️ 2) Purge any existing media for the *new* filename  
        GameListManager.PurgeOldMediaFiles(treenode, treenode.FileName);

        // Now proceed with your existing download logic...
        try
        {
            var doc = new XmlDocument();
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
                    DownloadAndSaveMediaAsync(url, type, format, seDir, treenode, token)
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

    private static async Task DownloadAndSaveMediaAsync(
    string url,
    string type,
    string format,
    string baseFolderSE,
    TreeNodeDetail node,
    CancellationToken ct = default)
    {
        try
        {
            string folderPath = string.IsNullOrEmpty(node.Tag_SubDirPath)
                ? Path.Combine(baseFolderSE, node.Tag_ConsoleName, type)
                : Path.Combine(baseFolderSE, node.Tag_ConsoleName, node.Tag_SubDirPath, type);

            Directory.CreateDirectory(folderPath);

            using var response = await _httpClient.GetAsync(url, ct);
            if (!response.IsSuccessStatusCode)
            {
                //Console.WriteLine($"[MediaDownloader] Failed to GET {url}: {response.StatusCode}");
                return;
            }

            var mediaData = await response.Content.ReadAsByteArrayAsync(ct);

            string baseFileName = $"{node.FileName}-{type}.{format}";
            string filePath = Path.Combine(folderPath, baseFileName);

            if (File.Exists(filePath))
            {
                string existingCrc = Hash.Compute_CRC32(filePath);
                string downloadedCrc = Crc32Algorithm.Compute(mediaData).ToString("X8");

                if (string.Equals(existingCrc, downloadedCrc, StringComparison.OrdinalIgnoreCase))
                {
                    //Console.WriteLine($"[MediaDownloader] Skipped identical: {filePath}");
                    return;
                }
                else
                {
                    //Console.WriteLine($"[MediaDownloader] Conflict detected (but alt ignored): {filePath}");
                    return; // ❌ Skip — don’t write a conflicting file or alt
                }
            }

            await File.WriteAllBytesAsync(filePath, mediaData, ct);
            //Console.WriteLine($"[MediaDownloader] Saved: {filePath}");
        }
        catch (OperationCanceledException)
        {
            //Console.WriteLine($"[MediaDownloader] Download cancelled: {url}");
        }
        catch (Exception ex)
        {
            //Console.WriteLine($"[MediaDownloader] Error downloading {url}: {ex}");
        }
    }

}

