namespace MovieDbAssistant.Dmn.Models.Interface;

/// <summary>
/// The navigation model.
/// </summary>
public sealed class MovieListNavigationModel
{
    /// <summary>
    /// home link
    /// </summary>
    public string Home { get; set; }

    /// <summary>
    /// item index
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// next movie link
    /// </summary>
    public string? Next { get; set; }

    /// <summary>
    /// previous movie link
    /// </summary>
    public string? Previous { get; set; }

    /// <summary>
    /// total items
    /// </summary>
    public int Total { get; set; }

    public MovieListNavigationModel(
        string home,
        int index,
        string? next,
        string? previous,
        int total)
    {
        Home = home;
        Index = index;
        Next = next;
        Previous = previous;
        Total = total;
    }
}
