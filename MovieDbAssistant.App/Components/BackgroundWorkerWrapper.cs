using System.ComponentModel;

namespace MovieDbAssistant.App.Components;

/// <summary>
/// The background worker wrapper.
/// </summary>
class BackgroundWorkerWrapper
{
    BackgroundWorker? _backgroundWorker;

    public bool End { get; protected set; } = false;

    /// <summary>
    /// Stop and destroy background worker.
    /// </summary>
    public void StopAndDestroyBackgroundWorker()
    {
        End = true;
        if (_backgroundWorker is null) return;
        _backgroundWorker.CancelAsync();
        _backgroundWorker.Dispose();
        _backgroundWorker = null;
    }
}
