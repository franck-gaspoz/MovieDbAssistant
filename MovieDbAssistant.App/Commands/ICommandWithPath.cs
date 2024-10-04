namespace MovieDbAssistant.App.Commands;

/// <summary>
/// interface command with path.
/// </summary>
public interface ICommandWithPath
{
    /// <summary>
    /// Gets the path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    string Path { get;}
}
