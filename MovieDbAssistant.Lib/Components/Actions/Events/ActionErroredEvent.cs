using System.Windows.Input;

using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions.Events;

/// <summary>
/// action errored event
/// </summary>
/// <param name="Context">action context</param>
/// <param name="Exception">exception (optional)</param>
/// <param name="Message">message (optional)</param>
/// <param name="StackTrace">stack trace (optional)</param>
public sealed record ActionErroredEvent(
    ActionContext Context,
    Exception? Exception = null,
    string? Message = null,
    string? StackTrace = null) : ISignal
{
    /// <summary>
    /// Gets the error from the exception or the message
    /// <para>returns empty string if no error defined</para>
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string GetError() => Exception?.Message ?? Message ?? string.Empty;

    /// <summary>
    ///  Gets the stack trace from the exception or the stored stack
    /// <para>returns empty string if no stack defined</para>
    /// </summary>
    public string GetTrace() => Exception ?.StackTrace ?? StackTrace ?? string.Empty;

    /// <inheritdoc/>
    public override string ToString() => GetError();
}
