using System.Diagnostics;

using Microsoft.Extensions.Logging;

namespace MovieDbAssistant.Lib.Components.Logger;

#pragma warning disable IDE1006 // Styles d'affectation de noms

/// <summary>
/// The app logger.
/// </summary>
/// <param name="Name">The name.</param>
/// <param name="configure">The configure.</param>
/// <param name="logFilePath">The log file path. (default logs/log.txt)</param>

public class AppLogger : ILogger
{
    static int _itemCount = 0;
    const string LogFilePath = "logs/log.txt";

    readonly string _name;
    readonly Func<AppLoggerConfiguration, AppLoggerConfiguration>? _configure;
    static string _logFilePath;

    static object _logLock = new();

    public AppLogger(
        string name,
        Func<AppLoggerConfiguration, AppLoggerConfiguration>? configure, 
        AppLoggerConfiguration? appLoggerConfiguration = null,
        string? logFilePath = LogFilePath)
    {
        _name = name;
        _configure = configure;
        if (appLoggerConfiguration!=null)
            _appLoggerConfiguration = appLoggerConfiguration;
        logFilePath ??= LogFilePath;
        _logFilePath = logFilePath;
    }

    AppLoggerConfiguration __appLoggerConfiguration = new();
    AppLoggerConfiguration _appLoggerConfiguration
    {
        get => _configure != null
            ? _configure(__appLoggerConfiguration)
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
        var config = _appLoggerConfiguration;
        if (!config.IsEnabled) return;

        void Dbg(string caller, string? msg)
        {
            msg = msg?.Replace('\n', ' ');
            var txt = $"{_itemCount++,6} | {AppLoggerConfiguration.GetLogLevel(logLevel),5} | {Environment.CurrentManagedThreadId,5} | {caller,30} | {msg}";
            // debug ouput
            Debug.WriteLine(txt);
            // file output
            lock (_logLock)
            {
                using var sw = AppendLogFile(txt,GetLogFilePath());
            }
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

    static StreamWriter AppendLogFile(string txt,string path)
    {
        var sw = File.AppendText(path);
        sw.WriteLine(txt);
        sw.Flush();
        sw.Close();
        return sw;
    }

    /// <summary>
    /// Get log file path.
    /// </summary>
    /// <returns>A <see cref="string"/></returns>
    public static string GetLogFilePath(string? logFilePath = null) 
        => Path.Combine(
            Directory.GetCurrentDirectory(), 
            (logFilePath ?? _logFilePath) ?? LogFilePath);

    /// <summary>
    /// Clear log file.
    /// </summary>
    /// <param name="logFilePath">The log file path.</param>
    public static void ClearLogFile(string? logFilePath = null)
        => File.WriteAllText(
            GetLogFilePath(logFilePath),
            string.Empty);

    /// <summary>
    /// Appends a line to the log file
    /// </summary>
    /// <param name="newLine">The new line.</param>
    /// <param name="logFilePath">The log file path.</param>
    public static void AppendLine(string newLine,string? logFilePath = null)
    {
        lock (_logLock)
        {
            AppendLogFile(newLine, GetLogFilePath(logFilePath));
        }
    }
}