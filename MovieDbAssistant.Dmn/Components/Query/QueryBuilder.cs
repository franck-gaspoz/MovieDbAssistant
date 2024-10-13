using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Logger;

namespace MovieDbAssistant.Dmn.Components.Query;

/// <summary>
/// The query builder.
/// </summary>
[Transient]
public sealed class QueryBuilder : IIdentifiable
{
    public QueryBuilder(
        ILogger<QueryBuilder> _logger,
        IServiceProvider _serviceProvider)
    {
        InstanceId = new(this);
        this._logger = _logger;
        this._serviceProvider = _serviceProvider;
    }

    readonly List<QueryModel> _queries = [];
    readonly ILogger<QueryBuilder> _logger;
    readonly IServiceProvider _serviceProvider;
    string[]? _lines;

    /// <summary>
    /// Gets the instance id.
    /// </summary>
    /// <value>A <see cref="SharedCounter"/></value>
    public SharedCounter InstanceId { get; }

    /// <summary>
    /// build from a query source file content
    /// </summary>
    /// <param name="queryFile">query file name</param>
    /// <param name="content">query source file content</param>
    /// <returns>queries models</returns>
    public List<QueryModel> Build(string queryFile, string content)
    {
        _queries.Clear();
        _lines = content
            .Split('\n');
        Trim();
        IQueryListFormatParser formatParser =
            IsFormatTitleSourceList() ? _serviceProvider
                .GetRequiredService<QueryListFormatTitleSourceParser>()
            : IsFormatTitleList() ? _serviceProvider
                .GetRequiredService<QueryListFormatTitleParser>()
                    : _serviceProvider.GetRequiredService<QueryListFormatTitleParser>();

        _logger.LogInformation(this, "query parser: " + formatParser.GetType().Name);

        var res = formatParser.Parse(_lines);
        AddMetadata(queryFile, res);

        return res;
    }

    static void AddMetadata(string queryFile, List<QueryModel>? queries)
    {
        if (queries == null) return;
        foreach (var query in queries)
        {
            query.Metadata ??= new QueryMetadata();
            var m = query.Metadata;
            m.QueryFile = queryFile;
        }
    }

    bool IsFormatTitleList()
        => !_lines!.Any(x => x.IsEmptyLine());

    bool IsFormatTitleSourceList()
    {
        var res = true;
        var i = 0;
        bool Exists(int i) => _lines!.Length > i;

        while (i < _lines!.Length)
        {
            var s = _lines[i];
            if (!s.IsCommentLine())
            {
                res &= !s.IsEmptyLine()
                    && Exists(i + 1) && !_lines[i + 1].IsEmptyLine()
                    && (!Exists(i + 2) || _lines[i + 2].IsEmptyLine());
                i += 3;
            }
            else
                i++;
        }
        return res;
    }

    void Trim()
    {
        var i = 0;
        var remove = new List<int>();

        while (_lines![i].IsEmptyLine())
        {
            remove.Add(i);
            i++;
        }
        var j = _lines.Length - 1;
        while (_lines[j].IsEmptyLine())
        {
            remove.Add(j);
            j--;
        }

        _lines = _lines[i..(j + 1)];
        for (i = 0; i < _lines.Length; i++)
            _lines[i] = _lines[i].Trim();
    }
}
