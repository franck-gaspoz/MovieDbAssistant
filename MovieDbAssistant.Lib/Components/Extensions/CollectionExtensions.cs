namespace MovieDbAssistant.Lib.Components.Extensions;

/// <summary>
/// exctensions of collections
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Try the add.
    /// </summary>
    /// <typeparam name="TKey">type of key</typeparam>
    /// <typeparam name="TValue">type of value</typeparam>
    /// <param name="dic">The dic.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public static void TryAdd<TKey, TValue>(
        this Dictionary<TKey, List<TValue>> dic,
        TKey key,
        TValue value)
        where TKey : notnull
    {
        if (!dic.TryGetValue(key, out var list))
            dic.Add(key, list = []);
        if (!list.Contains(value)) list.Add(value);
    }
}
