namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// The template builder.
/// </summary>
public partial class TemplateBuilder
{
    public const string Include_Part_Prefix = "{{{";

    public const string Include_Part_Postfix = "}}}";

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
        var x = tpl.IndexOf(Include_Part_Prefix, startPos);
        if (x < 0) return (tpl, Index_NoNext);
        var y = tpl.IndexOf(Include_Part_Postfix,x);
        if (y < 0) return (tpl, Index_NoNext);
        var nextY = y + Include_Part_Postfix.Length;
        var a = x + Include_Part_Postfix.Length;
        var b = y - 1;
        var name = tpl.Substring(a,b-a+1);
        
        var partFile = name + Parts_File_Extensions;
        var file = GetTemplateFile(partFile);
        if (file==null) return (tpl, Index_NoNext);

        var partContent = File.ReadAllText(file);
        var left = tpl[..x];
        var right = tpl[nextY..];
        tpl = left + partContent + right;
        return (tpl, nextY);
    }

    string? GetTemplateFile(string partFile)
    {
        // search in tpl
        var tplPartsPath = Path.Combine(
            Context.TplPath,
            _tpl!.Options.Paths.Parts
            );
        var file = Path.Combine(tplPartsPath, partFile);
        if (file != null) return file;

        var rscPartsPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            _dmnSettings.Value.Paths.Resources,
            _dmnSettings.Value.Paths.RscHtml,
            _dmnSettings.Value.Paths.RscHtmlAssets,
            _dmnSettings.Value.Paths.RscHtmlAssetsTpl
            );
        file = Path.Combine(rscPartsPath, partFile);
        if (file!=null) return file;

        return null;
    }

}
