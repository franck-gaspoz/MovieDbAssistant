namespace MovieDbAssistant.Dmn.Components.Builders.Html;

/// <summary>
/// The html document builder context.
/// </summary>
/// <param name="Index">The index.</param>
/// <param name="Total">The total.</param>
/// <param name="HomeLink">The home link.</param>
/// <param name="PreviousLink">The previous link.</param>
/// <param name="NextLink">The next link.</param>
public sealed class HtmlDocumentBuilderContext
{
    /// <summary>
    /// Gets the index.
    /// </summary>
    /// <value>An <see cref="int"/></value>
    public int Index { get; }

    /// <summary>
    /// Gets the total.
    /// </summary>
    /// <value>An <see cref="int"/></value>
    public int Total { get; }

    /// <summary>
    /// Gets the home link.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string HomeLink { get; }

    /// <summary>
    /// Gets the previous link.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string? PreviousLink { get; }

    /// <summary>
    /// Gets the next link.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string? NextLink { get; }

    public HtmlDocumentBuilderContext(
        int index,
        int total,
        string homeLink,
        string? previousLink,
        string? nextLink)
    {
        Index = index;
        Total = total;
        HomeLink = homeLink;
        PreviousLink = previousLink;
        NextLink = nextLink;
    }
}
