using MovieDbAssistant.Lib.Components.Extensions;

namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// The actor model.
/// </summary>
public sealed partial class ActorModel
{
    /// <summary>
    /// gets a clone
    /// </summary>
    /// <returns>An <see cref="ActorModel"/></returns>
    public ActorModel Clone()
        => new()
        {
            Actor = this.Actor,
            Characters =  this.Characters.Clone(),
            PicUrl = this.PicUrl.Clone()
        };
}