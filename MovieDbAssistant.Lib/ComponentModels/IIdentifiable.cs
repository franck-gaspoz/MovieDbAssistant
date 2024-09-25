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
    /// name préfix (default: blank)
    /// </summary>
    public virtual string GetNamePrefix() => string.Empty;

    /// <summary>
    /// name (default: type name)
    /// </summary>
    /// <returns>type name</returns>
    public virtual string GetName() => GetType().Name;
}
