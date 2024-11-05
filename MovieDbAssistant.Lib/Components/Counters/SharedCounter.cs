namespace MovieDbAssistant.Lib.Components.InstanceCounter;

/// <summary>
/// The instance counter.
/// </summary>
public sealed class SharedCounter
{
    static readonly object _lock = new();

    static readonly Dictionary<Type, int> _next = [];

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>An <see cref="int"/></value>
    public int Value { get; private set; }

    public SharedCounter(object owner)
    {
        var t = owner.GetType();
        lock (_lock)
        {
            if (!_next.TryGetValue(t, out var next))
            {
                next = 0;
                _next.Add(t, next);
            }
            Value = next;
            _next[t] = ++next;
        }
    }
}
