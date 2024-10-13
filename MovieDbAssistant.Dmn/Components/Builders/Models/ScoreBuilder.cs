using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.Dmn.Components.Builders.Models;

/// <summary>
/// The score builder.
/// </summary>
[Transient]
public class ScoreBuilder
{
    /// <summary>
    /// Gets or sets the note.
    /// </summary>
    /// <value>A <see cref="double"/></value>
    public double Note { get; protected set; } = 0;

    /// <summary>
    /// Gets or sets the tot weight.
    /// </summary>
    /// <value>A <see cref="double"/></value>
    public double TotWeight { get; protected set; } = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScoreBuilder"/> class.
    /// </summary>
    public ScoreBuilder() { }

    /// <summary>
    /// Add the note.
    /// </summary>
    /// <param name="note">The note.</param>
    /// <param name="weight">The weight.</param>
    public void AddNote(double note, double weight)
    {
        Note += note * weight;
        TotWeight += weight;
    }

    /// <summary>
    /// build the average
    /// </summary>
    /// <returns>the average. -1 if can't be built</returns>
    public double Average()
    {
        Note = TotWeight != 0 ? Note /= TotWeight : -1;
        return Note;
    }
}
