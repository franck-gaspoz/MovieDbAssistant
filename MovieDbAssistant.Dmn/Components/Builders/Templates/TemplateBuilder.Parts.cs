using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Configuration.Extensions;
using MovieDbAssistant.Lib.Components.Logger;
using MovieDbAssistant.Lib.Extensions;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// The template builder.
/// </summary>
public partial class TemplateBuilder
{
    public const string Include_Part_Prefix = "{{{";

    public const string Include_Part_Postfix = "}}}";

    public const char Include_Part_Prop_Escaper = '\\';

    public const string Include_Part_Prop_Prefix = "{{--";

    public const string Include_Part_Prop_Postfix = "--}}";

    public const string Include_Part_Block_Begin = "--";

    public const string Parts_File_Extensions = ".tpl.html";

    public const string Included_Prop_Condition_IfNotNullOrEmpty = "{{-if-not-null-or-empty}}";

    public const int Index_NoNext = -1;

    public const string Prop_Name_Tpl_Name = "tpl-name";

    string IncludeParts(string tpl, Dictionary<string, object?>? props = null)
    {
        tpl = ParseIncludes(tpl, props ?? []);
        return tpl;
    }

    string ParseIncludes(string tpl, Dictionary<string, object?> props)
    {
        var nextPos = 0;
        while (nextPos > Index_NoNext)
            (tpl, nextPos) = ParseNextInclude(tpl, props, nextPos);
        return tpl;
    }

    (string content, int nextPos) ParseNextInclude(
        string tpl,
        Dictionary<string, object?> props,
        int startPos = 0)
    {
        var x = tpl.IndexOf(Include_Part_Prefix, startPos);
        if (x < 0) return (tpl, Index_NoNext);
        var y = tpl.IndexOf(Include_Part_Postfix, x);
        if (y < 0) return (tpl, Index_NoNext);
        var nextY = y + Include_Part_Postfix.Length;
        var a = x + Include_Part_Postfix.Length;
        var b = y - 1;
        var name = tpl.Substring(a, b - a + 1);

        // include with props (..)
        (tpl, name, var tpIProps) = ParseIncludeProps(
            tpl,
            name,
            y + Include_Part_Postfix.Length);
        tpIProps.MergeInto(props);

        // include with props --
        (name, var tplProps) = ExtractProps(name);
        tplProps.MergeInto(props);

        var partFile = name + Parts_File_Extensions;
        var file = GetTemplateFile(partFile);
        if (file == (null,null)) 
            return (tpl, nextY);
        var partContent = File.ReadAllText(file.Path!);

        // set auto props
        props.AddOrReplace(
            Prop_Name_Tpl_Name,
            file.Folder!  
                + '/'
                + name);

        // parse part vars
        (partContent, var nprops) = SetVars(partContent, props);
        nprops.MergeInto(props);

        // recurse part
        partContent = ParseIncludes(partContent, props);

        var left = tpl[..x];
        var right = tpl[nextY..];
        tpl = left + partContent + right;
        return (tpl, nextY);
    }

    /// <summary>parse props included in tpl</summary>
    /// <param name="x">first index after the name with tags</param>
    (string tpl, string name, Dictionary<string, object?> props) ParseIncludeProps(
        string tpl, string name, int x)
    {
        var props = new Dictionary<string, object?>();
        var def = (tpl, name, props);

        if (!name.EndsWith(Include_Part_Block_Begin)) return def;

        var a = x + name.Length;
        name = name[..^(Include_Part_Block_Begin.Length)];
        var closeName = Include_Part_Block_Begin + name;
        var b = tpl.IndexOf(closeName, a);
        var y = b;
        if (y < 0)
        {
            _logger.LogError(this, "parse include props: missing name close: " + closeName);
            return def;
        }
        y += Include_Part_Postfix.Length + closeName.Length;

        var x0 = x;
        var block = tpl.Substring(x0, b - 1 - x0 + 1 - Include_Part_Prefix.Length);

        var startIndex = 0;
        while (startIndex != Index_NoNext)
            startIndex = ParseNextIncludeProp(block,props,startIndex);

        tpl = tpl[..x] + tpl[y..];

        return (tpl, name, props);
    }

    int ParseNextIncludeProp(string tpl, Dictionary<string,object?> props, int startIndex = 0)
    {
        var r = Index_NoNext;
        var x = tpl.IndexOf(Include_Part_Prop_Prefix, startIndex);
        if (x < 0) return r;

        var y = tpl.IndexOf(Include_Part_Prop_Postfix, x);
        if (y < 0)
        {
            _logger.LogError(this, "parse include prop end sub-block missing: " + tpl[x..]);
            return r;
        }
        var x2 = x + Include_Part_Prop_Prefix.Length;
        var propName = tpl.Substring(x2,y-x2);

        var z = tpl.IndexOf(Include_Part_Prop_Prefix, y);
        if (z < 0)
        {
            r = Index_NoNext;
            z = tpl.Length;
        }
        else
            r = z;
        var a = y + Include_Part_Prop_Postfix.Length;
        var b = z - 1;
        var propValue = tpl.Substring(a, b - a + 1);
        if (!string.IsNullOrEmpty(propValue))
            propValue = propValue.Trim();

        props.AddOrReplace(propName, propValue);

        return r;
    }

    (string name, Dictionary<string, object?> props) ExtractProps(string name)
    {
        var props = new Dictionary<string, object?>();
        var def = (name, props);

        if (!name.EndsWith(')')) return def;
        var x = name.IndexOf('(');
        if (x < 0)
        {
            _logger.LogError(this,
                "part include properties delcaration error: missing ( in props list: " + name);
            return def;
        }
        var decl = name[(x + 1)..^1].Trim();
        decl = EscapeCharacters(decl);

        name = name[..x];
        name = EscapeCharacters(name);

        var decls = decl.Split(',');
        foreach (var pdecl in decls)
        {            
            var t = pdecl.Split('=');
            if (t.Length != 2)
            {
                _logger.LogError(this,
                    "part include properties declaration error: attempted <key> = <value> but found: "
                    + decl
                    );
                return def;
            }
            props.AddOrReplace(
                UnescapeCharacters(t[0].Trim()), 
                UnescapeCharacters(t[1].Trim()));
        }

        return (name, props);
    }

    static string EscapeCharacters(string s)
    {
        var r = "";
        for (int i=0;i<s.Length;i++)
        {
            var c = s[i];
            if (c == Include_Part_Prop_Escaper
                && i+1 < s.Length)
            {
                var nc = s[i + 1];
                c = (char)(10000 + (int)nc);
                i++;
            }
            r += c;
        }
        return r;
    }

    static string UnescapeCharacters(string s)
    {
        var r = "";
        for (int i = 0; i < s.Length; i++)
        {
            var c = s[i];
            var n = (int)c;
            if ( n >= 10000)
                c = (char)(-10000 + n);
            r += c;
        }
        return r;
    }

    (string? Folder,string? Path) GetTemplateFile(string partFile)
    {
        // search in tpl
        var tplPartsPath = Path.Combine(
            Context.TplPath,
            _tpl!.Options.Paths.Parts
            );
        var file = Path.Combine(tplPartsPath, partFile);
        if (File.Exists(file)) 
            return (_tpl!.Options.Paths.Parts,file);

        var rscPartsPath = _dmnSettings.Value.EngineTpsPath();
        file = Path.Combine(rscPartsPath, partFile);
        if (File.Exists(file)) 
            return (_dmnSettings.Value.Paths.RscHtmlAssetsTpl
                , file);

        _logger.LogError(this, "template part not found: " + partFile);

        return (null,null);
    }

}
