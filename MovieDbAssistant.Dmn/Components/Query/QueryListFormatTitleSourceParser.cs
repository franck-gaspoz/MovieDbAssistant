using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.InstanceCounter;

namespace MovieDbAssistant.Dmn.Components.Query;

/// <summary>
/// The query list format title parser.
/// </summary>
[Singleton]
public sealed class QueryListFormatTitleSourceParser : 
    IIdentifiable,
    IQueryListFormatParser
{
    /// <summary>
    /// Gets the instance id.
    /// </summary>
    /// <value>A <see cref="SharedCounter"/></value>
    public SharedCounter InstanceId { get; }

    public QueryListFormatTitleSourceParser() 
        => InstanceId = new(this);

    /// <inheritdoc/>
    public List<QueryModelSearchByTitle> Parse(string[] lines)
    {
        var queries = new List<QueryModelSearchByTitle>();

        void AddQueryModel(
            string title,
            string? source,
            string? download)
        {
            queries.Add(new QueryModelSearchByTitle(title)
            {
                Metadata = new(source, download)
            });
        }

        var i = 0;
        while (i < lines.Length)
        {
            var s = lines[i];
            if (!s.IsCommentLine() && !s.IsEmptyLine())
            {
                var title = lines[i];
                var source = i < lines.Length - 1 ? lines[i + 1] : null;
                var t = source?.Split(',');
                var play = t?[0];
                var download = t != null ? (t.Length > 1 ? t[1] : null) : null;
                AddQueryModel(title, source, download);
                i += 3;
            }
            else i++;
        }

        return queries;
    }
}
