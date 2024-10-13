using System.Diagnostics;

using Microsoft.Extensions.Logging;

namespace MovieDbAssistant.Lib.Components.Logger;

#pragma warning disable IDE1006 // Styles d'affectation de noms

/// <summary>
/// The app logger.
/// </summary>
/// <param name="Name">The name.</param>
/// <param name="configure">The configure.</param>

public class AppLogger(
    string Name,
    Func<AppLoggerConfiguration, AppLoggerConfiguration>? configure = null) : ILogger
{
    static int _itemCount = 0;

    AppLoggerConfiguration __appLoggerConfiguration = new();
    AppLoggerConfiguration _appLoggerConfiguration
    {
        get => configure != null
            ? configure(__appLoggerConfiguration)
            : __appLoggerConfiguration;
        set => __appLoggerConfiguration = value;
    }

    /// <inheritdoc/>
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel) =>
        _appLoggerConfiguration.IsEnabled;

    /// <inheritdoc/>
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        _ = Name;
        var config = _appLoggerConfiguration;
        if (!config.IsEnabled) return;

        void Dbg(string caller, string? msg)
        {
            msg = msg?.Replace('\n', ' ');
            Debug.WriteLine($"{_itemCount++,6} | {AppLoggerConfiguration.GetLogLevel(logLevel),5} | {Environment.CurrentManagedThreadId,5} | {caller,30} | {msg}");
        }

        var msg = state?.ToString() ?? string.Empty;
        var (caller, message) = msg.ExtractCallerMessage();
        Dbg(caller, message);

        if (exception != null)
        {
            //Dbg(caller,exception.Message);
            if (config.DumpStackTraces)
                Debug.WriteLine(exception.StackTrace);
        }
    }
}