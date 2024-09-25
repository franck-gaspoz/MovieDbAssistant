using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;

namespace MovieDbAssistant.Lib.Components.Logger;

/// <summary>
/// app logger provider
/// </summary>
[ProviderAlias("AppLogger")]
public sealed class AppLoggerProvider : ILoggerProvider
{
    readonly Func<AppLoggerConfiguration, AppLoggerConfiguration>? _configure = null;
    readonly ConcurrentDictionary<string, AppLogger> _loggers =
        new(StringComparer.OrdinalIgnoreCase);

    public AppLoggerProvider(Func<AppLoggerConfiguration, AppLoggerConfiguration>? configure = null)
        => _configure = configure;

    /// <inheritdoc/>
    public ILogger CreateLogger(string categoryName)
        => _loggers.GetOrAdd(categoryName, name =>
            new AppLogger(name, _configure));

    /// <summary>
    /// dispose resources
    /// </summary>
    public void Dispose()
        => _loggers.Clear();
}