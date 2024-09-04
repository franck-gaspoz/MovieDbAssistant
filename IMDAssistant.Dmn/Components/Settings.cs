using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

namespace IMDBAssistant.Dmn.Components;

/// <summary>
/// The settings.
/// </summary>
[Singleton]
public sealed class Settings
{
    public const string Path_Output = "Paths:Output";
    public const string Path_Input = "Paths:Input";

    public const string Label_Exit = "Texts:Exit";
    public const string Label_OpenCmdLine = "Texts:OpenCmdLine";
    public const string Label_OpenOutpFolder = "Texts:OpenOutpFolder";
    public const string Label_OpenInpFolder = "Texts:OpenInpFolder";
    public const string Label_Help = "Texts:Help";
    public const string Label_Settings = "Texts:Settings";
    public const string Label_BuildQueryFile = "Texts:BuildFromQueryFile";
    public const string Label_BuildJsonFile = "Texts:BuildFromJsonFile";
    public const string Label_BuildClipb = "Texts:BuildFromClipboard";
    public const string Label_BuildFromInputFolder = "Texts:BuildFromInputFolder";
    public const string ProcInpFold = "Texts:ProcInpFold";

    public const string IconFile = "IconFile";
    public const string AppTitle = "App:Title";

    public const string Path_Assets = "AssetsPath";

    public const string BalloonTip_Start = "BalloonTips:Start";
    public const string BalloonTip_End = "BalloonTips:End";
    public const string BalloonTip_Delay = "BalloonTips:Delay";

    public const string FolderExplorer_CommandLine = "FolderExplorer:CommandLine";

    public const string Shell_CommandLine = "Shell:CommandLine";
    public const string Shell_Args = "Shell:Args";

    public const string OpenBrowser_CommandLine = "OpenBrowser:CommandLine";
    public const string Url_HelpGitHub = "Urls:HelpGitHub";

    public const string SearchPattern_Json = "Build:SearchPatternJson";
    public const string SearchPattern_Txt = "Build:SearchPatternTxt";
    public const string PrefixFileDisabled = "Build:PrefixFileDisabled";
}
