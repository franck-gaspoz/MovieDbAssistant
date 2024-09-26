using System.Reflection;

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
        String methodName = "On" + name;

        var mi = target.GetType().GetMethod(
              methodName,
              BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new ArgumentException("Cannot find event thrower named " + methodName);

        mi.Invoke(target, [args]);
    }
}
