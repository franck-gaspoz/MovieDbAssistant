using Microsoft.Extensions.Logging;

using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Errors;

/// <summary>
/// stack of errors
/// </summary>
[Scoped]
public sealed class StackErrors : Stack<StackError>
{
    /// <summary>
    /// merge a stack in this stack
    /// </summary>
    /// <param name="stack">The stack.</param>
    /// <returns>this object</returns>
    public StackErrors Merge(StackErrors stack)
    {
        Copy(stack);
        return this;
    }

    /// <summary>
    /// copy values from another stack (append)
    /// </summary>
    /// <param name="stack">stack</param>
    void Copy(StackErrors stack)
    {
        var items = stack.ToList();
        items.Reverse();
        foreach (var item in items)
            Push(item);
    }

    /// <summary>
    /// copy values from another stack
    /// </summary>
    /// <param name="stack">stack</param>
    /// <returns>this object</returns>
    public StackErrors Setup(StackErrors stack)
    {
        Clear();
        Copy(stack);
        return this;
    }
}
