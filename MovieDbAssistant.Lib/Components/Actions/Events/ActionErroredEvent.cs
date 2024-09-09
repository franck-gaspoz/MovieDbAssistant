using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions.Events;

/// <summary>
/// action errored event
/// </summary>
/// <param name="Exception">exception (optional)</param>
/// <param name="Message">message (optional)</param>
public sealed record ActionErroredEvent(
    Exception? Exception = null,
    string? Message = null) : ISignal
{
    /// <summary>
    /// Gets the error from the exception or the message
    /// <para>returns empty string if no error defined</para>
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Error => Exception?.Message ?? Message ?? string.Empty;
}
