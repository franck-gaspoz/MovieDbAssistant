using System.Text.Json;

using MovieDbAssistant.Dmn.Components.Json;

namespace MovieDbAssistant.Dmn;

/// <summary>
/// domaine globals
/// </summary>
public sealed class Globals
{
    /// <summary>
    /// json serializer properties
    /// </summary>
    public static readonly Lazy<JsonSerializerOptions> JsonSerializerProperties
        = new(() =>
        {
            var opts = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                PropertyNamingPolicy = new FirstLowerCaseNamingPolicy()
            };
            return opts;
        });

    /// <summary>
    /// json serializer properties
    /// </summary>
    public static readonly Lazy<JsonSerializerOptions> JsonDeserializerProperties
        = new(() =>
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            });

    /// <summary>
    /// item id build
    /// </summary>
    public const string Item_Id_Build = "Tag_Item_Build";
    public const string Item_Id_Build_Json = Item_Id_Build + "Item_Id_Build_Json";
    public const string Item_Id_Build_Query = Item_Id_Build + "Item_Id_Build_Query";
    public const string Item_Id_Build_Clipboard = Item_Id_Build + "Item_Id_Build_Clipboard";
    public const string Item_Id_Build_Input = Item_Id_Build + "Tag_Item_Build_Input";

    /// <summary>
    /// json file text
    /// </summary>
    public const string File_Extension_Json = ".json";

    // ----- consts for settings -----

    public const string Section_Scrap = "Scrap";
}