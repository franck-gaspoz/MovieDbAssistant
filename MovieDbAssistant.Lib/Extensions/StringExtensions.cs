using System.Text;

namespace MovieDbAssistant.Lib.Extensions;

/// <summary>
/// string extensions
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts to slash left right.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>A <see cref="string"/></returns>
    public static string? ToSlashLeftRight(this string? text)
        => text?.Replace('\\', '/');

    /// <summary>
    /// Converts to hex string.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A <see cref="string"/></returns>
    public static string ToHexString(this string? value)
    {
        if (value == null) return string.Empty;
        var bytes = Encoding.UTF8.GetBytes(value);
        var base64String = Convert.ToBase64String(bytes);
        return base64String;
    }

    /// <summary>
    /// Converts to hex having only letters and digits
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A <see cref="string"/></returns>
    public static string ToHexLettersAndDigitsString(this string? value)
    {
        if (value == null) return string.Empty;
        var bytes = Encoding.UTF8.GetBytes(value);
        var base64String = Convert.ToBase64String(bytes);
        StringBuilder sb = new();
        foreach (var c in base64String)
            if (char.IsAsciiLetterOrDigit(c))
                sb.Append(c);
        return sb.ToString();
    }

    /// <summary>
    /// normalize a path -&gt; set to absolute
    /// </summary>
    /// <param name="path">path</param>
    /// <returns>path</returns>
    public static string NormalizePath(this string path)
    {
        if (!Path.IsPathFullyQualified(path))
            path = Path.Combine(
                Directory.GetCurrentDirectory(),
                path);
        return path;
    }

    /// <summary>
    /// first letter in lower case
    /// </summary>
    /// <param name="s">string</param>
    /// <returns>first letter in lower case</returns>
    public static string ToFirstLower(this string s)
        => s[0].ToString().ToLower() + s[1..];

    /// <summary>
    /// first letter in upper case
    /// </summary>
    /// <param name="s">string</param>
    /// <returns>first letter in upper case</returns>
    public static string ToFirstUpper(this string s)
        => s[0].ToString().ToUpper() + s[1..];

    /// <summary>
    /// double quote a string
    /// </summary>
    /// <param name="s">string</param>
    /// <returns>double quoted string</returns>
    public static string DblQuote(this string s)
        => '"' + s + '"';

    /// <summary>
    /// Levenshtein distance
    /// <para>is bijective</para>
    /// </summary>
    /// <param name="s">source</param>
    /// <param name="t">traget</param>
    /// <returns>distance</returns>
    public static int LevenshteinDistance(this string s, string t)
    {
        var n = s.Length;
        var m = t.Length;
        var d = new int[n + 1, m + 1];

        if (n == 0) return m;
        if (m == 0) return n;

        for (var i = 0; i <= n; i++) d[i, 0] = i;
        for (var j = 0; j <= m; j++) d[0, j] = j;

        for (var i = 1; i <= n; i++)
            for (var j = 1; j <= m; j++)
            {
                var cost = t[j - 1] == s[i - 1] ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost
                );
            }

        return d[n, m];
    }

    /// <summary>
    /// Search a string pattern on the left.
    /// <para>returns <code>-1</code> if not found</para>
    /// </summary>
    /// <param name="s">The string to search in</param>
    /// <param name="pat">The pattern to search.</param>
    /// <returns>An <see cref="int"/></returns>
    public static int SearchLeft(this string s,string pat,int fromIndex)
    {
        var i = fromIndex;
        if (i < 0) return -1;
        var l = pat.Length;
        if (s[i..(i + l)] == pat) return i;
        return s.SearchLeft( pat, fromIndex-1 );
    }

    /// <summary>
    /// Search a string pattern on the left.
    /// <para>returns <code>-1</code> if not found</para>
    /// </summary>
    /// <param name="s">The string to search in</param>
    /// <param name="pat">The pattern to search.</param>
    /// <returns>An <see cref="int"/></returns>
    public static int SearchRight(this string s, string pat, int fromIndex)
    {
        var i = fromIndex;
        if (i < 0) return -1;
        var l = pat.Length;
        if (s[i..(i + l)] == pat) return i;
        return s.SearchRight(pat, fromIndex + 1);
    }
}
