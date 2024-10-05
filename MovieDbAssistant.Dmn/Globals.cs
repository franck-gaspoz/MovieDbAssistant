using System.Text.Json;

namespace MovieDbAssistant.Dmn;

/// <summary>
/// domaine globals
/// </summary>
public sealed class Globals
{
    public static readonly Lazy<JsonSerializerOptions> JsonSerializerProperties
        = new(() =>
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            });

    public const string Item_Id_Build = "Tag_Item_Build";
    public const string Item_Id_Build_Json = Item_Id_Build + "Item_Id_Build_Json";
    public const string Item_Id_Build_Query = Item_Id_Build + "Item_Id_Build_Query";
    public const string Item_Id_Build_Clipboard = Item_Id_Build + "Item_Id_Build_Clipboard";
    public const string Item_Id_Build_Input = Item_Id_Build + "Tag_Item_Build_Input";

    /// <summary>
    /// processing movie
    /// </summary>
    public const string ProcMovie = "Texts:ProcMovie";

    /// <summary>
    /// processing movie lidt
    /// </summary>
    public const string ProcMovieList = "Texts:ProcMovieList";

    /// <summary>
    /// build html input template filename
    /// </summary>
    public const string Build_Html_Template_Filename = "Build:Html:TemplateFilename";

    /// <summary>
    /// build html template id
    /// </summary>
    public const string Build_Html_Template_Id = "Build:Html:TemplateId";

    /// <summary>
    /// build html file extension
    /// </summary>
    public const string Build_HtmlFileExt = "Build:Html:Extension";

    /// <summary>
    /// build html output data filename
    /// </summary>
    public const string Build_Html_Filename_Data = "Build:Html:DataFilename";

    /// <summary>
    /// output path for pages
    /// </summary>
    public const string Path_OutputPages = "Paths:OutputPages";

    /// <summary>
    /// app title
    /// </summary>
    public const string App_Title = "App:Title";

    /// <summary>
    /// app lang
    /// </summary>
    public const string App_Lang = "App:Lang";

    /// <summary>
    /// app version date
    /// </summary>
    public const string App_VersionDate = "App:VersionDate";

    /// <summary>
    /// text data provider failed
    /// </summary>
    public const string DataProvider_Failed = "Texts:DataProviderFailed";

    /// <summary>
    /// scrap tool path
    /// </summary>
    public const string Scrap_Tool_Path = "Scrap:ToolPath";
}

