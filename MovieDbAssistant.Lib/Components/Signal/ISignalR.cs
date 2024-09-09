using MovieDbAssistant.Lib.ComponentModels;

namespace MovieDbAssistant.Lib.Components.Signal;
public interface ISignalR : IIdentifiable
{
    /// <summary>
    /// add a mapping between a signal type and a handler type
    /// </summary>
    /// <param name="signal">The signal.</param>
    /// <param name="handler">The handler.</param>
    void Map(Type signal, Type handler);

    /// <summary>
    /// send a signal to listeners
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="signal">The signal.</param>
    void Send(object sender, ISignal signal);
}