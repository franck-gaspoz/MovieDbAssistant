﻿namespace MovieDbAssistant.Dmn.Models.Build;

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
    /// tpl details file name
    /// </summary>
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// template list
    /// </summary>
    public string? TplList { get; set; }

    /// <summary>
    /// template details
    /// </summary>
    public string? TplDetails { get; set; }

    public TemplatesModel(string list, string details)
    {
        List = list;
        Details = details;
    }

    /// <summary>
    /// load templates
    /// </summary>
    public void Load(string templatePath)
    {
        TplList = File.ReadAllText(
            Path.Combine(templatePath, List));

        TplDetails = File.ReadAllText(
            Path.Combine(templatePath, Details));
    }
}
