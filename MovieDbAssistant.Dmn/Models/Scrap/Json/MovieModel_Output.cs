using System.Diagnostics;

namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// The movie model
/// </summary>
[DebuggerDisplay("{Title} | {MinPicAlt}")]
public sealed partial class MovieModel
{
    /// <summary>
    /// key (base64) from setup
    /// <para>initialized by <code>SetupModel</code></para>
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// template output filename
    /// <para>initialized by <code>SetupModel</code>and others</para>
    /// </summary>
    public string? Filename { get; set; }
}
