using System.Diagnostics;
using System.Reflection;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.InstanceCounter;

namespace MovieDbAssistant.Lib.Components.Signal;

/// <summary>
/// The signaler.
/// </summary>
public sealed class SignalR : ISignalR
{
    /// <summary>
    /// instance id
    /// </summary>
    public SharedCounter InstanceId { get; }

    const string MethodName_Handle = "Handle";
    const string TraceLogPrefix = "''''' ";

    readonly Dictionary<Type, List<object>> _instanceMap = [];
    readonly Dictionary<Type, List<Type>> _typeMap = [];
    readonly Dictionary<object, List<object>> _subscribeMap = [];
    readonly IConfiguration _config;
    readonly IServiceProvider _serviceProvider;

    public SignalR(
        IConfiguration config,
        IServiceProvider serviceProvider)
    {
        InstanceId = new(this);
        _config = config;
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public SignalR Register<T>(object handler)
    {
        MapInstance(typeof(T), handler!);
        return this;
    }

    /// <inheritdoc/>
    public SignalR RegisterType<T>(Type handlerType)
    {
        MapType(typeof(T),handlerType);
        return this;
    }

    /// <inheritdoc/>
    public SignalR Subscribe(object caller,object listener, object publisher)
    {
#if DEBUG
        Debug.WriteLine(TraceLogPrefix + caller.GetId() + " ### subscribe: "+listener.GetId()+" --> "+publisher.GetId());
#endif
        MapSubscriber(listener, publisher);
        return this;
    }

    /// <inheritdoc/>
    public SignalR Unsubscribe(object caller,object listener, object publisher)
    {
#if DEBUG
        Debug.WriteLine(TraceLogPrefix + caller.GetId() + " ### UNSUBSCRIBE: " + listener.GetId() + " --> " + publisher.GetId());
#endif
        if (_subscribeMap.TryGetValue(publisher, out var list))
            list.Remove(listener);
        return this;
    }

    /// <inheritdoc/>
    public SignalR Unregister<T>(object handler)
    {
        if (_instanceMap.TryGetValue(typeof(T), out var list))
            list.Remove(handler);
        return this;
    }

    /// <inheritdoc/>
    public bool GetHandlerMethod(Type sigType, object handler, out MethodInfo? methodInfo)
    {
        methodInfo = handler.GetType()
            .GetMethod(
                MethodName_Handle,
                [typeof(object), sigType]);

        return methodInfo != null;
    }

    /// <inheritdoc/>
    public bool GetHandlerMethod<T>(object handler, out MethodInfo? methodInfo)
        => GetHandlerMethod(typeof(T), handler, out methodInfo);

    bool CanInvoke(Type sigType, object handler)
        => GetHandlerMethod(sigType, handler, out _);

    object? Invoke(Type sigType, object sender, object handler, ISignal signal)
    {
        GetHandlerMethod(sigType, handler, out var methodInfo);
        return methodInfo?.Invoke(handler, [sender, signal]);
    }

    // TODO: remove
    /// <inheritdoc/>
    public object? TryInvoke(Type sigType, object sender, object handler, ISignal signal)
    {
        GetHandlerMethod(sigType, handler, out var methodInfo);
#if DEBUG
        if (methodInfo != null)
            Debug.WriteLine(sender.GetId() + " --> internal signal: " + sigType.Name + " --> " + handler.GetId());
#endif
        return methodInfo?.Invoke(handler, [sender, signal]);
    }

    /// <inheritdoc/>
    public void Send(object sender, ISignal signal)
    {
        var sigType = signal.GetType();

#if TRACE
        Debug.WriteLine(">> signal: "
            + sender.GetId()
            + " --> "
            + signal.GetId());
#endif
        // subscribe map

        if (_subscribeMap.TryGetValue(sender, out var subscribers))
        {
            var localSubscribers = new List<object>(subscribers);
            foreach (var handler in localSubscribers)
            {
#if TRACE
                if (CanInvoke(sigType, handler))
                    Debug.WriteLine($"¤¤¤ ({signal.GetId()}) catched by subscriber: " + handler.GetId());
#endif
                Invoke(sigType, sender, handler, signal);
            }
        }

        // instance map

        if (_instanceMap.TryGetValue(sigType, out var handlersInstances))
        {
            var localHandlersInstances = new List<object>(handlersInstances);
            foreach (var handler in localHandlersInstances)
            {
#if TRACE
                if (CanInvoke(sigType, handler)) 
                    Debug.WriteLine($"¤¤¤ ({signal.GetId()}) catched by instance: " + handler.GetId());
#endif
                Invoke(sigType, sender, handler, signal);
            }
        }

        // type map

        if (_typeMap.TryGetValue(sigType, out var handlersTypes))
        {
            var localHandlerTypes = new List<Type>(handlersTypes);
            foreach (var handlerType in localHandlerTypes)
            {
                var methodInfo = handlerType.GetMethod(
                    MethodName_Handle,
                    [typeof(object), sigType])!;
                var target = _serviceProvider.GetService(handlerType);
                if (target != null)
                {
#if TRACE
                    Debug.WriteLine($"¤¤¤ ({signal.GetId()}) catched by type: " + target.GetId());
#endif
                    methodInfo.Invoke(target, [sender, signal]);
                }
            }
        }
    }

    /// <inheritdoc/>
    public void MapType(Type signal, Type handler)
    {
        if (!_typeMap.TryGetValue(signal, out var list))
            _typeMap.Add(signal, list = []);
        if (!list.Contains(handler)) list.Add(handler);
    }

    /// <inheritdoc/>
    public void MapInstance(Type signal, object handler)
    {
        if (!_instanceMap.TryGetValue(signal, out var list))
            _instanceMap.Add(signal, list = []);
        if (!list.Contains(handler)) list.Add(handler);
    }

    /// <inheritdoc/>
    public void MapSubscriber(object listener, object publisher)
    {
        if (!_subscribeMap.TryGetValue(publisher, out var list))
            _subscribeMap.Add(publisher, list = []);
        if (!list.Contains(listener)) list.Add(listener);
    }

    /// <inheritdoc/>
    public string GetNamePrefix() => string.Empty;
}
