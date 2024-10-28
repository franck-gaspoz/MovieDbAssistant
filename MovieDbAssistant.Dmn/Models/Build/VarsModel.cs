namespace MovieDbAssistant.Dmn.Models.Build;

#pragma warning disable CA1822 // Marquer les membres comme étant static

/// <summary>
/// The variables model.
/// </summary>
public sealed class VarsModel
{
    /// <summary>
    /// Gets the system.
    /// </summary>
    /// <value>An <see cref="object"/></value>
    public SystemVarsModel System => new();
}
