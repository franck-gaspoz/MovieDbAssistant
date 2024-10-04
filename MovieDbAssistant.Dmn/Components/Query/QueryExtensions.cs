namespace MovieDbAssistant.Dmn.Components.Query;

/// <summary>
/// The query extensions.
/// </summary>
public static class QueryExtensions
{
    const string Prefix_Comment = "//";

    /// <summary>
    /// Checks if is empty line.
    /// </summary>
    /// <returns>A <see cref="bool"/></returns>
    public static bool IsEmptyLine(this string s) => string.IsNullOrWhiteSpace(s);

    /// <summary>
    /// Checks if is comment line.
    /// </summary>
    /// <returns>A <see cref="bool"/></returns>
    public static bool IsCommentLine(this string s) => s.StartsWith(Prefix_Comment);
}
