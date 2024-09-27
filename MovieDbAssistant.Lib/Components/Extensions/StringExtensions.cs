namespace MovieDbAssistant.Lib.Components.Extensions;

/// <summary>
/// string extensions
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts to hex string.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A <see cref="string"/></returns>
    public static string ToHexString(this string value)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(value);
        var base64String = Convert.ToBase64String(bytes);
        return base64String;
    }
}
