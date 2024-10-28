namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// The transform model.
/// </summary>
public sealed class TransformModel
{
    /// <summary>
    /// Gets or sets the target.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Target { get; set; }

    /// <summary>
    /// Gets or sets the operation.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Operation { get; set; }

    public TransformModel(string target, string operation)
    {
        Target = target;
        Operation = operation;
    }
}