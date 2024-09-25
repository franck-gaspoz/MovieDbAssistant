using System.Reflection;

using MovieDbAssistant.Lib.ComponentModels;

namespace MovieDbAssistant.Lib.Components.Signal;
public interface ISignalR : IIdentifiable
{
    /// <summary>
    /// add a mapping between a signal type and a handler type
    /// </summary>
    /// <param name="signal">The signal.</param>
    /// <param name="handler">The handler.</param>
    void MapType(Type signal, Type handler);

    /// <summary>
    /// add a mapping between a signal type and a handler object
    /// </summary>
    /// <param name="signal">The signal.</param>
    /// <param name="handler">The handler.</param>
    void MapInstance(Type signal, object handler);

    /// <summary>
    /// add a mapping between a signal publisher and a signal listener
    /// </summary>
    /// <param name="listener">listener</param>
    /// <param name="publisher">publisher</param>
    /// <param name="once">if true, map for the next handling only</param>
    void MapSubscriber(object listener, object publisher,bool once);

    /// <summary>
    /// Get handler method.
    /// </summary>
    /// <param name="sigType">The sig type.</param>
    /// <param name="handler">The handler.</param>
    /// <param name="methodInfo">method info or null</param>
    /// <returns>true if found, false otherwise</returns>
    public bool GetHandlerMethod(Type sigType, object handler, out MethodInfo? methodInfo);

    /// <summary>
    /// get handler method
    /// </summary>
    /// <typeparam name="T">signal type</typeparam>
    /// <param name="handler">handler</param>
    /// <param name="methodInfo">method info or null</param>
    /// <returns>true if found, false otherwise</returns>
    public bool GetHandlerMethod<T>(object handler, out MethodInfo? methodInfo);

    /// <summary>
    /// Try the invoke.
    /// </summary>
    /// <param name="sigType">The sig type.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="handler">The handler.</param>
    /// <param name="signal">The signal.</param>
    /// <returns>An <see cref="object? "/></returns>
    public object? TryInvoke(Type sigType, object sender, object handler, ISignal signal);

    /// <summary>
    /// register an instance for a type. avoid type map for this type
    /// </summary>
    /// <typeparam name="T">type</typeparam>
    /// <param name="handler">instance</param>
    /// <returns>this object</returns>
    public SignalR Register<T>(object handler);

    /// <summary>
    /// register a type for a signal type. extends type map for this type
    /// </summary>
    /// <typeparam name="T">type</typeparam>
    /// <param name="handler">instance</param>
    /// <returns>this object</returns>
    public SignalR RegisterType<T>(Type handler);

    /// <summary>
    /// subscribe to all signals of a publisher
    /// </summary>
    /// <param name="caller">caller</param>
    /// <param name="listener">listener</param>
    /// <param name="publisher">publisher</param>
    /// <returns>this object</returns>
    SignalR Subscribe(object caller, object listener, object publisher);

    /// <summary>
    /// subscribe to all signals of a publisher, unsubscribe after first handling
    /// </summary>
    /// <param name="caller">caller</param>
    /// <param name="listener">listener</param>
    /// <param name="publisher">publisher</param>
    /// <returns>this object</returns>
    SignalR SubscribeOnce(
        object caller,
        object listener,
        object publisher);

    /// <summary>
    /// unsubscribe a listener of a publisher
    /// </summary>
    /// <param name="caller">caller</param>
    /// <param name="listener">listener</param>
    /// <param name="publisher">publisher</param>
    /// <param name="details">details (default "")</param>
    /// <returns>this object</returns>
    SignalR Unsubscribe(
        object caller,
        object listener,
        object publisher,
        string? details = "");

    /// <summary>
    /// unregister an instance handler
    /// </summary>
    /// <typeparam name="T">type</typeparam>
    /// <param name="handler">instance</param>
    /// <returns>this object</returns>
    public SignalR Unregister<T>(object handler);

    /// <summary>
    /// send a signal to listeners
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="signal">The signal.</param>
    void Send(object sender, ISignal signal);
}