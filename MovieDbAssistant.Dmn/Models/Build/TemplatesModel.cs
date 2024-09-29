namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// template properties model
/// </summary>
public sealed class TemplatesModel
{
    /// <summary>
    /// tpl list filename
    /// </summary>
    public string List { get; set; } = string.Empty;

    /// <summary>
    /// tpl list item filename
    /// </summary>
    public string Item { get; set; } = string.Empty;

    /// <summary>
    /// tpl details file name
    /// </summary>
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// template list
    /// </summary>
    public string? TplList { get; set; }

    /// <summary>
    /// template list item
    /// </summary>
    public string? TplItem { get; set; }

    /// <summary>
    /// template details
    /// </summary>
    public string? TplDetails { get; set; }

    public TemplatesModel(string list, string item, string details)
    {
        List = list;
        Item = item;
        Details = details;
    }

    /// <summary>
    /// load templates
    /// </summary>
    public void Load(string templatePath)
    {
        TplList = File.ReadAllText(
            Path.Combine(templatePath,List));

        TplItem = File.ReadAllText(
            Path.Combine(templatePath, Item));

        TplDetails = File.ReadAllText(
            Path.Combine(templatePath, Details));
    }
}
