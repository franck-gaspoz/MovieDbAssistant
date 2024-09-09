using Microsoft.Extensions.Configuration;

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

    readonly Dictionary<Type, List<Type>> _map = [];
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
    public void Send(object sender, ISignal signal)
    {
        var sigType = signal.GetType();
        if (_map.TryGetValue(sigType, out var handlers))
        {
            foreach (var handlerType in handlers)
            {
                var methodInfo = handlerType.GetMethod(
                    "Handle",
                    [typeof(object), sigType])!;

                var target = _serviceProvider.GetService(handlerType);
                if (target != null)
                    methodInfo.Invoke(target, [sender, signal]);
            }
        }
    }

    /// <inheritdoc/>
    public void Map(Type signal, Type handler)
    {
        if (!_map.TryGetValue(signal, out var list))
            _map.Add(signal, list = []);
        if (!list.Contains(handler)) list.Add(handler);
    }
}
