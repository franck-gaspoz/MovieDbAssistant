namespace MovieDbAssistant.Dmn.Models.Queries;

/// <summary>
/// The query model.
/// </summary>
public sealed record QueryModel(
    string Title,
    string? Year,
    string? Lang);
