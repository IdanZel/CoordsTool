using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace CoordsTool.Network;

public static class Updates
{
    private const string LatestReleaseUrl = "https://api.github.com/repos/IdanZel/CoordsTool/releases/latest";
    private const string VersionPropertyName = "name";
    private const string UrlPropertyName = "html_url";

    public static async Task<(bool IsAvailable, string ReleaseUrl)> CheckForUpdates(string? currentVersion)
    {
        using var client = new HttpClient
        {
            DefaultRequestHeaders =
            {
                UserAgent = { ProductInfoHeaderValue.Parse("CoordsTool") }
            }
        };

        var latestReleaseMetadata = await client.GetFromJsonAsync<JsonElement>(LatestReleaseUrl);

        var latestVersion = latestReleaseMetadata.GetProperty(VersionPropertyName).GetString();
        var releaseUrl = latestReleaseMetadata.GetProperty(UrlPropertyName).GetString()!;

        return string.Compare(currentVersion, latestVersion, StringComparison.InvariantCultureIgnoreCase) != 0
            ? (true, releaseUrl)
            : (false, string.Empty);
    }
}