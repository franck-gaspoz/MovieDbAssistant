using MovieDbAssistant.Dmn.Components.Builder;
using MovieDbAssistant.Dmn.Components.DataProviders;

namespace MovieDbAssistant.Dmn.Components.Builders;

/// <summary>
/// document builder context.
/// </summary>
public sealed class DocumentBuilderContext
{
    /// <summary>
    /// Gets or sets the builder.
    /// </summary>
    /// <value>An <see cref="IDocumentBuilder"/></value>
    public IDocumentBuilder? Builder { get; set; }

    /// <summary>
    /// Gets or sets the data provider.
    /// </summary>
    /// <value>An <see cref="IDataProvider"/></value>
    public IDataProvider? DataProvider { get; set; }

    /// <summary>
    /// Gets or sets the builder type.
    /// </summary>
    /// <value>A <see cref="Type"/></value>
    public Type BuilderType { get; set; }

    /// <summary>
    /// Gets or sets the data provider type.
    /// </summary>
    /// <value>A <see cref="Type"/></value>
    public Type DataProviderType { get; set; }

    /// <summary>
    /// Gets or sets the output path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string OutputPath { get; set; }

    /// <summary>
    /// Gets or sets the input file.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string InputFile { get; set; }

    public DocumentBuilderContext(
        string inputFile,
        string outputPath,
        Type builderType,
        Type dataProviderType)
    {
        InputFile = inputFile;
        OutputPath = outputPath;
        BuilderType = builderType;
        DataProviderType = dataProviderType;
    }
}
