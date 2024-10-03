namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// The transforms model.
/// </summary>
public sealed class TransformsModel
{
    /// <summary>
    /// items
    /// </summary>
    public List<TransformModel> Items { get; set; } = [];

#if NO
    /// <summary>
    /// Converts to transform model.
    /// </summary>
    /// <returns>A list of transformmodels.</returns>
    public List<TransformModel> ToTransformModel()
        => Items.Select(x => ToTransformModel(x))
            .ToList();

    static TransformModel ToTransformModel(string x)
    {
        var t = x.Split(':');
        return new TransformModel(t[0], t[1]);
    }
#endif
}