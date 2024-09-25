namespace MovieDbAssistant.Lib.Components.Signal;

/// <summary>
/// signal handler
/// </summary>
/// <typeparam name="T">signal type</typeparam>
public interface ISignalHandler<T> :
    ISignalInternalMethodHandler<T>,
    ISignalHandlerBase
    where T : ISignal
{
}
