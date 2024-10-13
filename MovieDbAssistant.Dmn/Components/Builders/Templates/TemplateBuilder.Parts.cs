namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// The template builder.
/// </summary>
public partial class TemplateBuilder
{
    public const string Include_Prefix = "{{{";

    public const string Include_Postfix = "}}}";

    public const string Parts_File_Extensions = ".tpl.html";
    
    const int Index_NoNext = -1;

    string IncludeParts(string tpl)
    {
        var nextPos = 0;
        while (nextPos > Index_NoNext)
            (tpl,nextPos) = ParseNextInclude(tpl,nextPos);
        return tpl;
    }

    (string content,int nextPos) ParseNextInclude(string tpl,int startPos=0)
    {
        var x = tpl.IndexOf(Include_Prefix, startPos);
        if (x < 0) return (tpl, Index_NoNext);
        var y = tpl.IndexOf(Include_Postfix,x);
        if (y < 0) return (tpl, Index_NoNext);
        var nextY = y + Include_Postfix.Length;
        var a = x + Include_Postfix.Length;
        var b = y - 1;
        var name = tpl.Substring(a,b-a+1);
        var partsPath = Path.Combine(
            Context.TplPath,
            _tpl!.Options.Paths.Parts            
            );
        var partFile = name + Parts_File_Extensions;
        var file = Path.Combine(partsPath, partFile);
        if (!File.Exists(file)) return (tpl, Index_NoNext);
        var partContent = File.ReadAllText(file);
        var left = tpl[..x];
        var right = tpl[nextY..];
        tpl = left + partContent + right;
        return (tpl, nextY);
    }
}
