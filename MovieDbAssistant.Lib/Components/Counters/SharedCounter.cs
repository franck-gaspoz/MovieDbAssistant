namespace MovieDbAssistant.Lib.Components.InstanceCounter;

/// <summary>
/// The instance counter.
/// </summary>
public sealed class SharedCounter
{
    static readonly Dictionary<Type, int> _next = [];

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>An <see cref="int"/></value>
    public int Value { get; private set; } = 0;

    public SharedCounter(object owner)
    {
        var t = owner.GetType();
        if (!_next.TryGetValue(t, out var next))
            _next.Add(t, 0);
        Value = next++;
        _next[t] = next;
    }
}
