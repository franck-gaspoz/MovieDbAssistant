namespace MovieDbAssistant.Lib.Components.Signal;

/// <summary>
/// signal method handler
/// </summary>
/// <typeparam name="T">signal type</typeparam>
public interface ISignalMethodHandler<T> where T : ISignal
{
    /// <summary>
    /// handle the signal of type T
    /// </summary>   
    /// <param name="sender">sender</param>
    /// <param name="signal">signal</param>
    void Handle(object sender, T signal);
}
