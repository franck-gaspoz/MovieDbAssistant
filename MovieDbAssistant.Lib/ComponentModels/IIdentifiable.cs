using MovieDbAssistant.Lib.Components.InstanceCounter;

namespace MovieDbAssistant.Lib.ComponentModels;

/// <summary>
/// an identifiable
/// </summary>
public interface IIdentifiable
{
    /// <summary>
    /// gets the id
    /// </summary>
    public SharedCounter InstanceId { get; }

    /// <summary>
    /// préfix
    /// </summary>
    public string GetNamePrefix();
}
