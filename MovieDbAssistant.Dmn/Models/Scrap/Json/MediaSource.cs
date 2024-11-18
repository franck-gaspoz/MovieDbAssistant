namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// a source of a media
/// </summary>
public sealed partial class MediaSource
{
    /// <summary>
    /// source path
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// media provider identifier (from MediaProviders.Urls)
    /// </summary>
    public string? MediaProviderId { get; set; }

    /// <summary>
    /// media physical type id (hdd,ssd,...) if any
    /// </summary>
    public string? MediaPhysicalTypeId { get; set; }

    /// <summary>
    /// require VPN
    /// </summary>
    public bool RequireVPN { get; set; }

    /// <summary>
    /// indicates if the provider insert adds in media (free with adds)s
    /// </summary>
    public bool WithAds { get; set; }

    /// <summary>
    /// geo restricted (2 digits country codes list)
    /// </summary>
    public List<string> GeoRestricted { get; set; } = [];

    /// <summary>
    /// ui embeded player name/ref if any
    /// </summary>
    public string? Embed { get; set; }

    public MediaSource(string? path = null) => Path = path;
}
