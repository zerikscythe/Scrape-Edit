using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapeEdit
{
    public static class GitHubUpdate
    {

        private const string GitHubTagsUrl = "https://api.github.com/repos/zerikscythe/Scrape-Edit/tags";

        public static async Task<string?> GetLatestTagAsync()
        {
            const string GitHubTagsUrl = "https://api.github.com/repos/zerikscythe/Scrape-Edit/tags";

            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("ScrapeEdit");

                var response = await client.GetAsync(GitHubTagsUrl);
                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                var doc = System.Text.Json.JsonDocument.Parse(json);

                List<(Version version, string raw)> validTags = new();

                foreach (var element in doc.RootElement.EnumerateArray())
                {
                    string rawTag = element.GetProperty("name").GetString() ?? "";
                    string cleaned = CleanVersionString(rawTag);

                    if (Version.TryParse(cleaned, out var version))
                    {
                        validTags.Add((version, rawTag));
                    }
                }

                if (validTags.Count == 0)
                    return null;

                // Return the raw name of the latest version
                return validTags.OrderByDescending(v => v.version).First().raw;
            }
            catch
            {
                return null;
            }
        }
        public static string CleanVersionString(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                return "";

            version = version.Trim();

            // Remove "v" or "V" prefix if present
            if (version.StartsWith("v", StringComparison.OrdinalIgnoreCase))
                version = version.Substring(1);

            // Remove any build metadata (after '+') or prerelease suffix (after '-')
            int plus = version.IndexOf('+');
            if (plus > -1)
                version = version[..plus];

            int dash = version.IndexOf('-');
            if (dash > -1)
                version = version[..dash];

            return version.Trim();
        }
        public static bool IsUpdateAvailable(string? currentVersion, string? latestTag)
        {
            if (string.IsNullOrWhiteSpace(currentVersion) || string.IsNullOrWhiteSpace(latestTag))
                return false;

            try
            {
                string cur = CleanVersionString(currentVersion);
                string latest = CleanVersionString(latestTag);

                return Version.Parse(latest) > Version.Parse(cur);
            }
            catch
            {
                return false;
            }
        }

        

    }
}
