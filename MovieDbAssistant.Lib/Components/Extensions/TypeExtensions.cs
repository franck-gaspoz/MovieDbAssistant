using System.Diagnostics.Metrics;

using MovieDbAssistant.Lib.Components.Actions;

namespace MovieDbAssistant.Lib.Components.Extensions;

/// <summary>
/// type extensions
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Has interface.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="itf">The itf.</param>
    /// <returns>A <see cref="bool"/></returns>
    public static bool HasInterface(this Type type, Type itf)
    {
        var itfs = type.GetInterfaces();
        if (itfs.Contains(itf)) return true;
        foreach (var item in itfs)
            if (item.HasInterface(itf)) return true;
        return false;
    }

    /// <summary>
    /// Get the interfaces having the specified interface type
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="itf">The itf.</param>
    /// <returns>A list of types.</returns>
    public static List<Type> GetInterfaces(this Type type, Type itf)
        => GetInterfacesOfInterfaceType(type, itf, []);

    static List<Type> GetInterfacesOfInterfaceType(Type type, Type ofType, List<Type> list)
    {
        var itfs = type.GetInterfaces();
        foreach (var item in itfs)
        {
            if (item.HasInterface(ofType)) list.Add(item);
        }
        return list;
    }

    /// <summary>
    /// checks an object is a feature. if true, provides it in feature
    /// </summary>
    /// <param name="o">object to be checked</param>
    /// <param name="feature">feature or null</param>
    /// <param name="ignoreErrors">if true simply ignore errors, dot not trace them (default false)</param>
    /// <returns>true if feature, false otherwise</returns>
    public static bool CheckIsFeature(this object o, out IActionFeature? feature, bool ignoreErrors = true)
    {
        if (o is not IActionFeature _feature)
        {
            if (!ignoreErrors)
                Console.Error.WriteLine($"error: sender type mismatch. expected {nameof(IActionFeature)} but got {o.GetType().Name} ");

            feature = null;
            return false;
        }
        feature = _feature;
        return true;
    }

    /// <summary>
    /// get a key for an object type and a counter
    /// </summary>
    /// <param name="o">object</param>
    /// <param name="key">key value</param>
    /// <returns>string key</returns>
    public static string GetKey(this object o,ref int key)
        => o.GetType().Name + "-" + key++ + "";
}
