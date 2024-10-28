using System.Reflection;

namespace MovieDbAssistant.Lib.Extensions;

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
            if (item.HasInterface(ofType)) list.Add(item);
        return list;
    }

    /// <summary>
    /// get public properties of object
    /// </summary>
    /// <param name="data">data</param>
    /// <param name="filter">any filter predicate or null</param>
    /// <returns>dictionary of names -&gt; value</returns>
    public static Dictionary<string, object?> GetProperties(
        this object data,
        Func<PropertyInfo, bool>? filter = null
        )
    {
        var r = new Dictionary<string, object?>();
        var props = data.GetType().GetProperties()
            .Where(x => filter == null || filter(x));

        foreach (var p in props)
            r.Add(p.Name, p.GetValue(data));

        return r;
    }

    /// <summary>
    /// gets a property value
    /// </summary>
    /// <param name="o">object</param>
    /// <param name="name">property name</param>
    /// <returns>property value</returns>
    public static object? GetPropertyValue(this object? o, string name)
    {
        if (o == null) return null;
        var p = o.GetType().GetProperty(name);
        return p?.GetValue(o);
    }

    /// <summary>
    /// Programatically fire an event handler of an object
    /// </summary>
    /// <param name="target">target</param>
    /// <param name="name">event name</param>
    /// <param name="args">event args</param>
    public static void FireEvent(
        this object target,
        string name,
        EventArgs args)
    {
        /*
         * By convention event handlers are internally called by a protected
         * method called OnEventName
         * e.g.
         *     public event TextChanged
         * is triggered by
         *     protected void OnTextChanged
         * 
         * If the object didn't create an OnXxxx protected method,
         * then you're screwed. But your alternative was over override
         * the method and call it - so you'd be screwed the other way too.
         */

        //Event thrower method name //e.g. OnTextChanged
        var methodName = "On" + name;

        var mi = target.GetType().GetMethod(
              methodName,
              BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new ArgumentException("Cannot find event thrower named " + methodName);

        mi.Invoke(target, [args]);
    }
}
