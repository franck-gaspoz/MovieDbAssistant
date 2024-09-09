namespace MovieDbAssistant.Lib.Components.InstanceCounter;

/// <summary>
/// The instance counter.
/// </summary>
public sealed class SharedCounter
{
    static int _next = 0;

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>An <see cref="int"/></value>
    public int Value { get; private set; } = 0;

    public SharedCounter()
        => Value = _next++;
}
