﻿using Microsoft.Extensions.Configuration;

using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;

namespace MovieDbAssistant.Dmn.Components;

/// <summary>
/// The settings.
/// </summary>
[Singleton]
public sealed class Settings
{
    #region consts

    /// <summary>
    /// The path output.
    /// </summary>
    public const string Path_Output = "Paths:Output";

    /// <summary>
    /// The path resources.
    /// </summary>
    public const string Path_Rsc = "Paths:Resources";

    /// <summary>
    /// The path resources html
    /// </summary>
    public const string Path_RscHtml = "Paths:RscHtml";

    /// <summary>
    /// The path resources html templates
    /// </summary>
    public const string Path_RscHtmlTemplates = "Paths:RscHtmlTemplates";

    /// <summary>
    /// The path resources html / assets
    /// </summary>
    public const string Path_RscHtmlAssets = "Paths:RscHtmlAssets";

    /// <summary>
    /// The path resources html / assets / movie-page-list-wallpapers
    /// </summary>
    public const string Path_RscHtmlAssetsMoviePageListWallpapers = "Paths:RscHtmlAssetsMoviePageListWallpapers";

    /// <summary>
    /// The path resources html / assets / fonts
    /// </summary>
    public const string Path_RscHtmlAssetsFonts = "Paths:RscHtmlAssetsFonts";

    /// <summary>
    /// The path resources html / assets
    /// </summary>
    public const string Path_AssetsListWallpaper = "Build:Html:Assets:ListWallpaper";

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
    /// processing file
    /// </summary>
    public const string ProcFile = "Texts:ProcFile";

    /// <summary>
    /// The proc clipbaord.
    /// </summary>
    public const string ProcClipboard = "Texts:ProcClipboard";

    /// <summary>
    /// app icon file.
    /// </summary>
    public const string Icon_App = "Assets:Icons:Tray";

    /// <summary>
    /// buzy step 1 icon file
    /// </summary>
    public const string Icon_Buzy_1 = "Assets:Icons:Buzy1";

    /// <summary>
    /// buzy step 2 icon file
    /// </summary>
    public const string Icon_Buzy_2 = "Assets:Icons:Buzy2";

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
    public const string Anim_Interval_Dot = "Anims:Interval:Dot";

    /// <summary>
    /// icon tray anim interval
    /// </summary>
    public const string Anim_Interval_TrayIcon = "Anims:Interval:WaitTrayIcon";

    /// <summary>
    /// wait icons animation
    /// </summary>
    public const string Anim_WaitIcons = "Anims:WaitIcons";

    /// <summary>
    /// input folder processed
    /// </summary>
    public const string InputFolderProcessed = "Texts:InputFolderProcessed";

    /// <summary>
    /// input folder processed with errors
    /// </summary>
    public const string InputFolderProcessedWithErrors = "Texts:InputFolderProcessedWithErrors";

    /// <summary>
    /// clipboard processed
    /// </summary>
    public const string ClipboardProcessed = "Texts:ClipboardProcessed";

    /// <summary>
    /// auto open settings on build
    /// </summary>
    public const string OpenOuputWindowOnBuild = "Options:OpenOuputWindowOnBuild";

    /// <summary>
    /// error message
    /// </summary>
    public const string Message_Error = "Texts:Error";

    /// <summary>
    /// error message
    /// </summary>
    public const string Message_Info = "Texts:Info";

    /// <summary>
    /// error message
    /// </summary>
    public const string Message_Warning = "Texts:Warning";

    /// <summary>
    /// unhandled error message
    /// </summary>
    public const string Message_Error_Unhandled = "Texts:ErrorUnhandled";

    /// <summary>
    /// builder busy error
    /// </summary>
    public const string Builder_Busy = "Texts:BuilderBusy";

    /// <summary>
    /// feature busy error
    /// </summary>
    public const string Feature_Busy = "Texts:FeatureBusy";

    /// <summary>
    /// input build ended without errors
    /// </summary>
    public const string Build_End_Input_Without_Errors = "Texts:BuildInputEndWithoutErrors";

    /// <summary>
    /// input build ended with errors
    /// </summary>
    public const string Build_End_Input_With_Errors = "Texts:BuildInputEndWithErrors";

    /// <summary>
    /// json build without errors
    /// </summary>
    public const string Build_End_Json_Without_Errors = "Texts:BuildJsonEndWithoutErrors";
    
    /// <summary>
    /// query built without errors
    /// </summary>
    public const string Build_End_Query_Without_Errors = "Texts:BuildQueryEndWithoutErrors";

    #endregion

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
    public string OutputPath => _config[Path_Output]!.NormalizePath();

    /// <summary>
    /// Gets the input path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string InputPath => _config[Path_Input]!.NormalizePath();

    /// <summary>
    /// full path of asset file from its name
    /// </summary>
    /// <param name="filename">asset filename</param>
    /// <returns>asset full path</returns>
    public string AssetPath(string filename) =>
        Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                _config[Path_Assets]!,
                filename));
}
