using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Components.Builder;
using MovieDbAssistant.Dmn.Components.Builders.Templates;
using MovieDbAssistant.Dmn.Components.DataProviders;
using MovieDbAssistant.Lib.Components.Logger;

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
    /// key / values builder options
    /// </summary>
    public Dictionary<string, object> BuilderOptions { get; set; } = [];

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
    /// resources path
    /// </summary>
    public string RscPath { get; set; }

    string? _source;
    /// <summary>
    /// Gets or sets the input file.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Source { 
        get => _source!;
        set
        {
            _source = value;
            var name = Path.GetFileNameWithoutExtension(_source);
            Name = name;
        }
    }

    string? _name;
    readonly ILogger _logger;

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Name { 
        get => _name!;
        set
        {
            OutputFolder = Path.Combine(OutputPath,value);
            _name = value;
        }
    }

    /// <summary>
    /// Gets or sets the output folder.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string? OutputFolder { get; set; }

    /// <summary>
    /// result target (folder,string,..)
    /// </summary>
    public string? Target { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentBuilderContext"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="source">The source.</param>
    /// <param name="outputPath">The output path.</param>
    /// <param name="rscPath">The rsc path.</param>
    /// <param name="dataProviderType">The data provider type.</param>
    /// <param name="builderType">The builder type.</param>
    /// <param name="builderOptions">The builder options.</param>
    public DocumentBuilderContext(
        ILogger logger,
        string source,
        string outputPath,
        string rscPath,
        Type dataProviderType,
        Type builderType,
        Dictionary<string,object>? builderOptions = null)
    {
        RscPath = rscPath;
        _logger = logger;
        OutputPath = outputPath;
        Source = source;
        BuilderType = builderType;
        DataProviderType = dataProviderType;
        if (builderOptions != null)
        {
            foreach (var kvp in builderOptions)
                BuilderOptions.TryAdd(kvp.Key, kvp.Value);
        }
    }

    /// <summary>
    /// Make output dir.
    /// </summary>
    /// <returns>A <see cref="DocumentBuilderContext"/></returns>
    public DocumentBuilderContext MakeOutputDir()
    {
        if (!Directory.Exists(OutputFolder))
        {
            Directory.CreateDirectory(OutputFolder!);
            _logger.LogDebug(this, "output folder created: " + OutputFolder);
        }
        return this;
    }

    /// <summary>
    /// Add output file.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="postFix">The post fix.</param>
    /// <param name="extension">The extension.</param>
    /// <param name="content">The content.</param>
    public void AddOutputFile(string key, string postFix, string extension, string content)
        => AddOutputFile(key+"-"+postFix,extension, content);

    /// <summary>
    /// Add output file.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="extension">The extension.</param>
    /// <param name="content">The content.</param>
    public void AddOutputFile(string name, string extension,string content)
    {
        var path = Path.Combine(
            OutputFolder!,
            name + "." + extension);
        File.WriteAllText(path, content);
    }
}
