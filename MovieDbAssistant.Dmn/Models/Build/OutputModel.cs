namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// The output model.
/// </summary>
public sealed class OutputModel
{
    /// <summary>
    /// file ext
    /// </summary>
    public string Ext { get; set; }

    /// <summary>
    /// page folder
    /// </summary>
    public string Pages { get; set; }

    public OutputModel(string ext, string pages)
    {
        Ext = ext;
        Pages = pages;
    }
}
