﻿using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

using Microsoft.Extensions.Configuration;

namespace IMDBAssistant.App.Services.Tray;

/// <summary>
/// The build service.
/// </summary>
[Singleton()]
public sealed class BuildService
{
    readonly Settings _settings;
    readonly IConfiguration _config;

    public BuildService(
         IConfiguration config,
         Settings settings)
         => ( _settings, _config) 
            = (settings, config);

    /// <summary>
    /// Build from file.
    /// </summary>
    public void BuildFromFile()
    {

    }

    /// <summary>
    /// Build from clipboard.
    /// </summary>
    public void BuildFromClipboard()
    {

    }
}