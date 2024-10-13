using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.InstanceCounter;

namespace MovieDbAssistant.Dmn.Components.Query;

/// <summary>
/// The query list format title parser.
/// </summary>
[Singleton]
public sealed class QueryListFormatTitleParser :
    IIdentifiable,
    IQueryListFormatParser
{
    /// <summary>
    /// Gets the instance id.
    /// </summary>
    /// <value>A <see cref="SharedCounter"/></value>
    public SharedCounter InstanceId { get; }

    public QueryListFormatTitleParser()
        => InstanceId = new(this);

    /// <inheritdoc/>
    public List<QueryModel> Parse(string[] lines)
    {
        var queries = new List<QueryModel>();

        void AddQueryModel(string title, int i)
        {
            var model = new QueryModel(title);
            model.Metadata ??= new();
            model.Metadata.QueryFileLine = i;
            queries.Add(model);
        }

        for (var i = 0; i < lines.Length; i++)
        {
            var s = lines[i];
            if (!s.IsCommentLine() && !s.IsEmptyLine())
                AddQueryModel(s.Trim(), i);
        }
        return queries;
    }
}
