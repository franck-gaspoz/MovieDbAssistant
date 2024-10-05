using System.Diagnostics;

using Microsoft.Extensions.Logging;

using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Logger;

namespace MovieDbAssistant.Lib.Components.Sys;

/// <summary>
/// The process wrapper.
/// </summary>
[Transient]
public sealed class ProcessWrapper : IIdentifiable
{
    readonly ILogger<ProcessWrapper> _logger;

    /// <summary>
    /// process start info
    /// </summary>
    public ProcessStartInfo? Psi { get; private set; }

    /// <summary>
    /// process
    /// </summary>
    public Process? Process { get; private set; }

    /// <summary>
    /// Gets the instance id.
    /// </summary>
    /// <value>A <see cref="SharedCounter"/></value>
    public SharedCounter InstanceId { get; }

    public ProcessWrapper(
        ILogger<ProcessWrapper> logger)
    {
        InstanceId = new(this);
        _logger = logger;
    }

    /// <summary>
    /// starts and eventually waits a process
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <param name="args">The args.</param>
    public void Start(
        string filename,
        IEnumerable<string> args)
    {
        Psi = new ProcessStartInfo(
            filename,
            args)
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = false,
            CreateNoWindow = false,
            WindowStyle = ProcessWindowStyle.Normal
        };

        using var process = Process.Start(Psi!);
        Process = process;
        if (process != null)
        {
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.WaitForExit();
            _logger.LogInformation(this, _remain);
        }
    }

    void Process_ErrorDataReceived(
        object sender,
        DataReceivedEventArgs e)
    {
        var s = e.Data;
        if (s == null) return;
        AppendLog(s, ref _remain_error);
    }

    void Process_OutputDataReceived(
        object sender,
        DataReceivedEventArgs e)
    {
        var s = e.Data;
        if (s == null) return;
        AppendLog(s, ref _remain);
    }

    string _remain = "";
    string _remain_error = "";

    void AppendLog(string s, ref string remain)
    {
        var t = s.Split('\n');
        remain += t[0];
        if (t.Length > 1)
        {
            _logger.LogInformation(this, _remain);
            remain = t[1];
        }
    }
}
