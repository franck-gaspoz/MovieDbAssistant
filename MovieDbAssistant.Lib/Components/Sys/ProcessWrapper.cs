﻿using System.Diagnostics;
using System.Text;

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

    string _remain = "";
    string _remain_error = "";

    /// <summary>
    /// Gets the out stream.
    /// </summary>
    /// <value>A <see cref="StringBuilder"/></value>
    public StringBuilder OutStream { get; } = new();

    /// <summary>
    /// Gets the err stream.
    /// </summary>
    /// <value>A <see cref="StringBuilder"/></value>
    public StringBuilder ErrStream { get; } = new();

    /// <summary>
    /// Gets a value indicating whether has errors.
    /// </summary>
    /// <value>A <see cref="bool"/></value>
    public bool HasErrors => ErrStream.Length > 0;

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
    /// <param name="waitForExit">wait for exit (default true)</param>
    /// <param name="redirectStreams">redirect streams: catch outputs (default: false)</param>
    public void Start(
        string filename,
        IEnumerable<string> args,
        bool waitForExit = true,
        bool redirectStreams = false)
    {
        Psi = new ProcessStartInfo(
            filename,
            args)
        {
            UseShellExecute = !redirectStreams,
            RedirectStandardOutput = redirectStreams,
            RedirectStandardError = redirectStreams,
            RedirectStandardInput = redirectStreams,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            WorkingDirectory = Path.GetDirectoryName(filename)
        };

        using var process = Process.Start(Psi!);
        Process = process;
        if (process != null)
        {
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;

            if (waitForExit)
                process.WaitForExit();

            while (!process.HasExited)
            {
                Thread.Yield();
            }

            if (redirectStreams)
            {
                OutStream.Append(_remain);
                OutStream.Append(process.StandardOutput.ReadToEnd());
                ErrStream.Append(_remain_error);
                ErrStream.Append(process.StandardError.ReadToEnd());
            }

            var tx = OutStream.ToString();
            var terr = ErrStream.ToString();

            if (!string.IsNullOrWhiteSpace(tx))
                AppendLog(tx, false);
            if (!string.IsNullOrWhiteSpace(terr))
                AppendLog(terr, true);
        }
    }

    void Process_ErrorDataReceived(
        object sender,
        DataReceivedEventArgs e)
    {
        var s = e.Data;
        if (s == null) return;
        AppendLogDelta(s, ref _remain_error);
    }

    void Process_OutputDataReceived(
        object sender,
        DataReceivedEventArgs e)
    {
        var s = e.Data;
        if (s == null) return;
        AppendLogDelta(s, ref _remain);
    }

    void AppendLog(string txt, bool isError)
    {
        var t = txt.Split('\n');
        foreach (var s in t)
        {
            var ts = s.TrimEnd();
            if (!string.IsNullOrWhiteSpace(ts))
            {
                if (!isError)
                    _logger.LogInformation(this, ts);
                else
                    _logger.LogError(this, ts);
            }
        }
    }

    void AppendLogDelta(string s, ref string remain)
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
