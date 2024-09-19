namespace MovieDbAssistant.Lib.Components.InstanceCounter;

/// <summary>
/// The instance counter.
/// </summary>
public sealed class SharedCounter
{
    static readonly Dictionary<Type, int> _next = [];

    public int Value { get; private set; }

    public SharedCounter(object owner)
    {
        var t = owner.GetType();
        if (!_next.TryGetValue(t, out var next))
        {
            next = 1;
            _next.Add(t, next);
        }
        Value = next - 1;
        _next[t] = next;
    }
}
