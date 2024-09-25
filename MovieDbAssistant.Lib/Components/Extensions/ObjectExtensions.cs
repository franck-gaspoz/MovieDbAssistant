using MovieDbAssistant.Lib.ComponentModels;

namespace MovieDbAssistant.Lib.Components.Extensions;

/// <summary>
/// extensions of object
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// get an id from an IIdentifiable or else the type name
    /// </summary>
    /// <param name="o">object</param>
    /// <returns>text id</returns>
    public static string GetId(this object o)
    {
        if (o is IIdentifiable identifiable)
            return identifiable.Id();
        else
            return o.GetType().Name;
    }

    /// <summary>
    /// get a key for an object type and a counter
    /// </summary>
    /// <param name="o">object</param>
    /// <param name="key">key value</param>
    /// <returns>string key</returns>
    public static string GetKey(this object o, ref int key)
        => o.GetType().Name + "-" + key++ + "";
}
