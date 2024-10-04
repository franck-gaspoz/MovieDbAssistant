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
    public QueryBuilder(ILogger<QueryBuilder> _logger) {
        InstanceId = new(this);
        this._logger = _logger;
    }

    const string Prefix_Comment = "//";

    readonly List<QueryModel> _queries = [];
    readonly ILogger<QueryBuilder> _logger;
    string[]? _lines;

    /// <summary>
    /// Gets the instance id.
    /// </summary>
    /// <value>A <see cref="SharedCounter"/></value>
    public SharedCounter InstanceId { get; }

    /// <summary>
    /// build from a query source file content
    /// </summary>
    /// <param name="content">query source file content</param>
    /// <returns>this object</returns>
    public List<QueryModel> Build(string content)
    {
        _queries.Clear();
        _lines = content
            .Split('\n');
        Trim();
        IQueryListFormatParser formatParser =
            IsFormatTitleList() ? new QueryListFormatTitleParser()
            : IsFormatTitleSourceList() ? new QueryListFormatTitleSourceParser()
                : new QueryListFormatTitleParser();

        _logger.LogInformation(this, "query parser: "+formatParser.GetType().Name);

        var res = formatParser.Parse(content);

        return res;
    }

    bool IsFormatTitleList()
        => !_lines!.Any(x => IsEmptyLine(x));

    bool IsFormatTitleSourceList()
    {
        var res = true;
        var i = 0;
        bool Exists(int i) => _lines!.Length > i;

        while (i < _lines!.Length)
        {
            var s = _lines[i];
            if (!IsCommentLine(s))
            {
                res &= !IsEmptyLine(s)
                    && Exists(i + 1) && !IsEmptyLine(_lines[i + 1])
                    && (!Exists(i + 2) || IsEmptyLine(_lines[i + 2]));
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

        while (IsEmptyLine(_lines![i]))
        {
            remove.Add(i);
            i++;
        }
        var j = _lines.Length - 1;
        while (IsEmptyLine(_lines[j]))
        {
            remove.Add(j);
            j--;
        }
        _lines = _lines[i .. (j + 1)];
    }

    static bool IsEmptyLine(string s) => string.IsNullOrWhiteSpace(s);

    static bool IsCommentLine(string s) => s.StartsWith(Prefix_Comment);
}
