using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.Lib.Components.Errors;

/// <summary>
/// stack of errors
/// </summary>
[Scoped]
public sealed class StackErrors : Stack<StackError>
{
}
