﻿using Microsoft.Extensions.Configuration;

using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.Dmn.Components;

/// <summary>
/// The settings.
/// </summary>
[Singleton]
public sealed class Settings
{
    /// <summary>
    /// The path output.
    /// </summary>
    public const string Path_Output = "Paths:Output";
    /// <summary>
    /// The path input.
    /// </summary>
    public const string Path_Input = "Paths:Input";

    /// <summary>
    /// Label exit.
    /// </summary>
    public const string Label_Exit = "Texts:Exit";
    /// <summary>
    /// Label open cmd line.
    /// </summary>
    public const string Label_OpenCmdLine = "Texts:OpenCmdLine";
    /// <summary>
    /// Label open outp folder.
    /// </summary>
    public const string Label_OpenOutpFolder = "Texts:OpenOutpFolder";
    /// <summary>
    /// Label open inp folder.
    /// </summary>
    public const string Label_OpenInpFolder = "Texts:OpenInpFolder";
    /// <summary>
    /// Label help.
    /// </summary>
    public const string Label_Help = "Texts:Help";
    /// <summary>
    /// Label settings.
    /// </summary>
    public const string Label_Settings = "Texts:Settings";
    /// <summary>
    /// Label build query file.
    /// </summary>
    public const string Label_BuildQueryFile = "Texts:BuildFromQueryFile";
    /// <summary>
    /// Label build json file.
    /// </summary>
    public const string Label_BuildJsonFile = "Texts:BuildFromJsonFile";
    /// <summary>
    /// Label build clipb.
    /// </summary>
    public const string Label_BuildClipb = "Texts:BuildFromClipboard";
    /// <summary>
    /// Label build from input folder.
    /// </summary>
    public const string Label_BuildFromInputFolder = "Texts:BuildFromInputFolder";
    /// <summary>
    /// The proc inp fold.
    /// </summary>
    public const string ProcInpFold = "Texts:ProcInpFold";

    /// <summary>
    /// The icon file.
    /// </summary>
    public const string IconFile = "IconFile";
    /// <summary>
    /// The app title.
    /// </summary>
    public const string AppTitle = "App:Title";

    /// <summary>
    /// The path assets.
    /// </summary>
    public const string Path_Assets = "AssetsPath";

    /// <summary>
    /// The balloon tip start.
    /// </summary>
    public const string BalloonTip_Start = "BalloonTips:Start";
    /// <summary>
    /// The balloon tip end.
    /// </summary>
    public const string BalloonTip_End = "BalloonTips:End";
    /// <summary>
    /// The balloon tip delay.
    /// </summary>
    public const string BalloonTip_Delay = "BalloonTips:Delay";

    /// <summary>
    /// The folder explorer command line.
    /// </summary>
    public const string FolderExplorer_CommandLine = "FolderExplorer:CommandLine";

    /// <summary>
    /// The shell command line.
    /// </summary>
    public const string Shell_CommandLine = "Shell:CommandLine";
    /// <summary>
    /// The shell args.
    /// </summary>
    public const string Shell_Args = "Shell:Args";

    /// <summary>
    /// Open browser command line.
    /// </summary>
    public const string OpenBrowser_CommandLine = "OpenBrowser:CommandLine";
    /// <summary>
    /// The url help git hub.
    /// </summary>
    public const string Url_HelpGitHub = "Urls:HelpGitHub";

    /// <summary>
    /// Search pattern json.
    /// </summary>
    public const string SearchPattern_Json = "Build:SearchPatternJson";
    /// <summary>
    /// Search pattern txt.
    /// </summary>
    public const string SearchPattern_Txt = "Build:SearchPatternTxt";
    /// <summary>
    /// The prefix file disabled.
    /// </summary>
    public const string PrefixFileDisabled = "Build:PrefixFileDisabled";
    /// <summary>
    /// dot anim interval
    /// </summary>
    public const string DotAnimInterval = "Anims:Interval:Dot";

    readonly IConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="Settings"/> class.
    /// </summary>
    /// <param name="config">The config.</param>
    public Settings(IConfiguration config)
        => _config = config;

    /// <summary>
    /// Gets the output path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string OutputPath => NormalizePath(_config[Path_Output]!);

    /// <summary>
    /// Gets the input path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string InputPath => NormalizePath(_config[Path_Input]!);

    static string NormalizePath(string path)
    {
        if (!Path.IsPathFullyQualified(path))
            path = Path.Combine(
                Directory.GetCurrentDirectory(),
                path);
        return path;
    }
}