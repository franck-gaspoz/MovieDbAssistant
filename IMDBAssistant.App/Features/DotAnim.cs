namespace IMDBAssistant.App.Features;

/// <summary>
/// The dot anim.
/// </summary>
public sealed class DotAnim
{
    const int MaxN = 3;
    const string SP = " ";
    private const char Dot = '.';
    int _n = 1;    
    readonly string _text;

    /// <summary>
    /// Initializes a new instance of the <see cref="DotAnim"/> class.
    /// </summary>
    /// <param name="text">The text.</param>
    public DotAnim(string text) => _text = text;

    /// <summary>
    /// provide next text having dots changed
    /// </summary>
    /// <returns>A <see cref="string"/>string ending with animated dots</returns>
    public string Next()
    {
        var dots = string.Empty.PadLeft(_n, Dot);
        _n++;
        if (_n > MaxN) _n = 1;
        return _text + SP + dots;
    }
}
