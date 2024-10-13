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

    /// <summary>
    /// clone a list of T
    /// </summary>
    /// <typeparam name="T"/>
    /// <param name="t">The T.</param>
    /// <returns>the clone list</returns>
    public static List<T>? Clone<T>(this List<T>? t)
        => t != null ? new List<T>(t) : null;

    /// <summary>
    /// add range of elements to a list and returns the distinct elements
    /// </summary>
    /// <typeparam name="T">elements type</typeparam>
    /// <param name="t">collection</param>
    /// <param name="add">added items</param>
    /// <returns>distinct collection with new elements</returns>
    public static List<T>? AddRangeDistinct<T>(
        this List<T>? t, IEnumerable<T> add)
    {
        if (t == null) return null;
        t!.AddRange(add);
        return t!.Distinct().ToList();
    }
}
