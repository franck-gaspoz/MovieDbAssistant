namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// The template model options.
/// </summary>
public sealed class TemplateModelOptions
{
    public TemplateModelPageOption PageList { get; set; }

    public TemplateModelOptions(TemplateModelPageOption pageList) 
        => PageList = pageList;
}
