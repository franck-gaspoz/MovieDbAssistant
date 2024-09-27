namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

#pragma warning disable CD1606 // The property must have a documentation header.

/// <summary>
/// The actor model.
/// </summary>
public sealed class ActorModel
{
    public string? Actor { get; set; }

    public List<string> PicUrl { get; set; } = [];

    public List<string> Characters { get; set; } = []; 
}