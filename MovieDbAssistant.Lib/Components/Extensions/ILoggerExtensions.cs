using Microsoft.Extensions.Logging;

namespace MovieDbAssistant.Lib.Components.Extensions;

/// <summary>
/// ILogger extensions
/// </summary>
public static class ILoggerExtensions
{
    /// <summary>
    /// log a debug message
    /// </summary>
    /// <param name="logger">logger</param>
    /// <param name="msg">message</param>
    public static ILogger Dbg(this ILogger logger, string msg)
    {
        logger.LogDebug(msg);
        return logger;
    }


}
