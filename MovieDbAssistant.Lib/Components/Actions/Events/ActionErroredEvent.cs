using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions.Events;

/// <summary>
/// action errored event
/// </summary>
/// <param name="Exception">exception (optional)</param>
/// <param name="Message">message (optional)</param>
public sealed record ActionErroredEvent(
    Exception? Exception = null,
    string? Message = null,
    string? StackTrace = null) : ISignal
{
    /// <summary>
    /// Gets the error from the exception or the message
    /// <para>returns empty string if no error defined</para>
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Error => Exception?.Message ?? Message ?? string.Empty;

    /// <summary>
    ///  Gets the stack trace from the exception or the stored stack
    /// <para>returns empty string if no stack defined</para>
    /// </summary>
    public string Trace => Exception ?.StackTrace ?? StackTrace ?? string.Empty;

    /// <inheritdoc/>
    public override string ToString() => Error;
}
