namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// template properties model
/// </summary>
public sealed class TemplatesModel
{
    public string List { get; set; } = string.Empty;

    public string Details { get; set; } = string.Empty;

    public TemplatesModel(string list, string details)
    {
        List = list;
        Details = details;
    }
}
