using System.Diagnostics;

namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// The movie model
/// </summary>
[DebuggerDisplay("{Title} | {MinPicAlt}")]
public sealed partial class MovieModel
{
    /// <summary>
    /// key (base64,lettersAndDigits) from setup
    /// <para>initialized by <code>SetupModel</code></para>
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// template output filename
    /// <para>initialized by <code>SetupModel</code>and others</para>
    /// </summary>
    public string? Filename { get; set; }

    /// <summary>
    /// index in a list for any purpose
    /// <para>in a catalog context is setted to index in the owner movies list</para>
    /// </summary>
    public int? ListIndex { get; set; }
}
