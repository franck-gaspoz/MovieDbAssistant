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
}
