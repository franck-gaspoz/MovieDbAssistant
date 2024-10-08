namespace MovieDbAssistant.Lib.Components.Extensions;

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
    /// action if an object is not null or null
    /// </summary>
    /// <param name="check">value that triggers the acion if false</param>
    /// <param name="onTrue">action executed if flag is false</param>
    /// <returns></returns>
    public static R IfNullElse<T,R>(
        this T? check, 
        Func<R> onTrue,
        Func<T,R> onFalse)
        where T : class
    {
        var t = check;
        var c = t == null;

        return c?
            onTrue()        
            : onFalse(t!);
    }
}
