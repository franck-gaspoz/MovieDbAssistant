namespace MovieDbAssistant.App.Configuration;

/// <summary>
/// The texts settings.
/// </summary>
public sealed class TextsSettings
{

    /// <summary>
    /// Gets or sets the label exit.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string Exit { get; set; }

    /// <summary>
    /// Label settings.
    /// </summary>
    public required string Settings { get; set; }

    /// <summary>
    /// Label build query file.
    /// </summary>
    public required string BuildFromQueryFile { get; set; }

    /// <summary>
    /// Label build json file.
    /// </summary>
    public required string BuildFromJsonFile { get; set; }

    /// <summary>
    /// Label build clipb.
    /// </summary>
    public required string BuildFromClipboard { get; set; }

    /// <summary>
    /// Label build from input folder.
    /// </summary>
    public required string BuildFromInputFolder { get; set; }

    /// <summary>
    /// Label open outp folder.
    /// </summary>
    public required string OpenOutpFolder { get; set; }

    /// <summary>
    /// Label open inp folder.
    /// </summary>
    public required string OpenInpFolder { get; set; }

    /// <summary>
    /// Label help.
    /// </summary>
    public required string Help { get; set; }

    /// <summary>
    /// The proc inp fold.
    /// </summary>
    public required string ProcInpFold { get; set; }

    /// <summary>
    /// The proc clipbaord.
    /// </summary>
    public required string ProcClipboard { get; set; }

    /// <summary>
    /// input folder processed with errors
    /// </summary>
    public required string InputFolderProcessedWithErrors { get; set; }

    /// <summary>
    /// input folder processed
    /// </summary>
    public required string InputFolderProcessed { get; set; }

    /// <summary>
    /// clipboard processed
    /// </summary>
    public required string ClipboardProcessed { get; set; }

    /// <summary>
    /// json build without errors
    /// </summary>
    public required string BuildJsonEndWithoutErrors { get; set; }

    /// <summary>
    /// query built without errors
    /// </summary>
    public required string BuildQueryEndWithoutErrors { get; set; }

    /// <summary>
    /// error message
    /// </summary>
    public required string Error { get; set; }

    /// <summary>
    /// info message
    /// </summary>
    public required string Info { get; set; }

    /// <summary>
    /// error message
    /// </summary>
    public required string Warning { get; set; }

    /// <summary>
    /// unhandled error message
    /// </summary>
    public required string ErrorUnhandled { get; set; }

    /// <summary>
    /// feature busy error
    /// </summary>
    public required string FeatureBusy { get; set; }

    /// <summary>
    /// input build ended without errors
    /// </summary>
    public required string BuildInputEndWithoutErrors { get; set; }

    /// <summary>
    /// input build ended with errors
    /// </summary>
    public required string BuildInputEndWithErrors { get; set; }

}
