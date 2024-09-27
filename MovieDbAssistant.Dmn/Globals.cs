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

    public const string Build_Html_Template_Filename = "Build:Html:TemplateFilename";
}

