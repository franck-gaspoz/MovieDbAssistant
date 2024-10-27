using MovieDbAssistant.Dmn.Components.Builders.Templates.PageBuilders;

namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// The build model.
/// </summary>
public sealed class BuildModel
{
    /// <summary>
    /// Gets or sets the started at.
    /// </summary>
    /// <value>A <see cref="DateTime? "/></value>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// Gets or sets the finished at.
    /// </summary>
    /// <value>A <see cref="DateTime? "/></value>
    public DateTime? FinishedAt { get; set; }

    /// <summary>
    /// build duration
    /// </summary>
    public double? Duration => FinishedAt == null ? null
        : (FinishedAt!.Value - StartedAt).TotalMilliseconds;

    /// <summary>
    /// layout
    /// </summary>
    public Layouts Layout { get; set; }

    public BuildModel(Layouts layout)
    {
        Layout = layout;
        StartedAt = DateTime.UtcNow;
    }
}
