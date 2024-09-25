namespace MovieDbAssistant.Lib.Components.Signal;

/// <summary>
/// The type extensions.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// attach a subscriber to a publisher
    /// </summary>
    /// <typeparam name="T"/>
    /// <param name="publisher">The publisher.</param>
    /// <param name="subscriber">The subscriber.</param>
    /// <param name="caller">caller</param>
    /// <param name="signal">The signal.</param>
    /// <returns>A <typeparamref name="T"/></returns>
    public static T AddListener<T>(this T publisher, object subscriber, object caller, ISignalR signal)
    {
        signal.Subscribe(caller, subscriber, publisher!);
        return publisher;
    }

    /// <summary>
    /// remove a subscriber of a publisher
    /// </summary>
    /// <typeparam name="T"/>
    /// <param name="publisher">The publisher.</param>
    /// <param name="subscriber">The subscriber.</param>
    /// <param name="caller">caller</param>
    /// <param name="signal">The signal.</param>
    /// <returns>A <typeparamref name="T"/></returns>
    public static T RemoveListener<T>(this T publisher, object subscriber, object caller, ISignalR signal)
    {
        signal.Unsubscribe(caller, subscriber, publisher!);
        return publisher;
    }
}
