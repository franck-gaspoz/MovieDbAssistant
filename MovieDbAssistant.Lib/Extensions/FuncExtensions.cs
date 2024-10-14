namespace MovieDbAssistant.Lib.Extensions;

/// <summary>
/// functional extensions
/// </summary>
public static class FuncExtensions
{
    /// <summary>
    /// action if a bool is true
    /// </summary>
    /// <param name="check">flag that triggers the acion</param>
    /// <param name="onTrue">action executed if flag is true</param>
    /// <returns>the bool value</returns>
    public static bool Then(this bool check, Action onTrue)
    {
        if (check) onTrue();
        return check;
    }

    /// <summary>
    /// action if a bool is true
    /// </summary>
    /// <param name="check">triggers the acion if false</param>
    /// <param name="onTrue">action executed if flag is false</param>
    /// <returns>the bool value</returns>
    public static bool Else(this bool check, Action onTrue)
    {
        if (!check) onTrue();
        return check;
    }

    /// <summary>
    /// action if an object is not null
    /// </summary>
    /// <param name="check">triggers the acion if false</param>
    /// <param name="onTrue">action executed if flag is false</param>
    /// <returns></returns>
    public static bool IfNotNull<T>(this T? check, Action<T> onTrue)
        where T : class
    {
        var t = check;
        var c = t != null;
        if (c)
            onTrue(t!);
        return c;
    }

    /// <summary>
    /// get a value if parameter is not null
    /// </summary>
    /// <typeparam name="T">check type</typeparam>
    /// <typeparam name="R">result type</typeparam>
    /// <param name="check">checked value</param>
    /// <param name="value">func returning value</param>
    /// <returns>value or null</returns>
    public static R? NotNullThen<T, R>(
        this T? check,
        Func<T, R?> value)
        where R : struct
    {
        var r = check != null ? value(check) : null;
        return r;
    }

    /// <summary>
    /// return nulls if array is empty, otherwize 0
    /// </summary>
    /// <typeparam name="T">elements type</typeparam>
    /// <param name="t">array</param>
    /// <returns>null or 0</returns>
    public static object? NullIfEmpty<T>(this List<T>? t)
        => t == null ? 0 :
            t.Count == 0 ? null : 0;

    /// <summary>
    /// action if an object is not null or null
    /// </summary>
    /// <param name="check">value that triggers the acion if false</param>
    /// <param name="onTrue">action executed if flag is false</param>
    /// <returns></returns>
    public static R IfNullElse<T, R>(
        this T? check,
        Func<R> onTrue,
        Func<T, R> onFalse)
        where T : class
    {
        var t = check;
        var c = t == null;

        return c ?
            onTrue()
            : onFalse(t!);
    }
}
