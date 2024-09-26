using MovieDbAssistant.Lib.Components.Actions.Events;

namespace MovieDbAssistant.Lib.Components.Errors;

/// <summary>
/// stackable error
/// </summary>
public sealed record StackError
(string Error, string? StackTrace, ActionErroredEvent Event);

