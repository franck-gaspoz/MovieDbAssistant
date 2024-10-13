using System.Collections;

using MovieDbAssistant.Dmn.Components.Builders.Html;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.Extensions;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// The template builder.
/// </summary>
public partial class TemplateBuilder
{
    public const string Var_Prefix = "{{";
    public const string Var_Postfix = "}}";

    static string KeyToVar(string key) => key.ToFirstLower();

    string SetVars(string tpl)
    {
        tpl = SetVars(tpl, GetTemplateProps(false, null, null));
        return tpl;
    }

    (string, Dictionary<string, object?>) SetVars(string tpl, HtmlDocumentBuilderContext htmlContext)
    {
        var props = GetTemplateProps(false, null, htmlContext);
        tpl = SetVars(tpl, props);
        return (tpl, props);
    }

    (string, Dictionary<string, object?>) SetVars(
        string tpl,
        HtmlDocumentBuilderContext htmlContext,
        MovieModel data)
    {
        var props = GetTemplateProps(true, data, htmlContext);
        tpl = SetVars(tpl, props);
        return (tpl, props);
    }

    string SetVars(
        string tpl,
        Dictionary<string, object?> vars,
        string? prefix = null)
    {
        foreach (var kvp in vars)
        {
            var val = kvp.Value;
            var varnp = KeyToVar(kvp.Key);
            var vtype = val?.GetType();

            if (val != null
                && vtype!.Namespace!
                    .StartsWith(GetType()
                        .Namespace!
                        .Split('.')[0])
                    && !vtype!.IsArray)
            {
                // model not null
                SetVars(
                    tpl,
                    val.GetProperties(),
                    prefix != null ?
                        prefix + '.' + varnp
                        : varnp
                        );
            }
            else
            {
                // not a model or null model
                var k = kvp.Key;
                if (prefix != null)
                    k = prefix + '.' + k;

                tpl = SetVar(
                    tpl,
                    KeyToVar(k),
                    VarToString(TransformValue(k, val)));
            }
        }
        return tpl;
    }

    object? TransformValue(string? key, object? value)
    {
        if (key == null) return null;
        if (value == null) return null;
        var transforms = _tpl!.Transforms;
        var transform = transforms
            .FirstOrDefault(x => x.Target == key)
            ?? transforms
                .FirstOrDefault(x => x.GetType().Name == key);
        if (transform == null) return value;
        var tm = GetType()
            .GetMethod(transform.Operation);
        if (tm == null)
            throw new InvalidOperationException("value transformer method not found: " + transform.Operation);
        value = tm!.Invoke(this, [value]);
        return value;
    }

    /// <summary>
    /// Transform actor simple.
    /// </summary>
    /// <param name="o">The object</param>
    /// <returns>An <see cref="object? "/></returns>
    public object? Transform_ActorSimple(object? o)
    {
        if (o == null) return null;
        if (o is ActorModel actor)
        {
            o = actor.Actor;
        }
        return o;
    }

    /// <summary>
    /// Transform the array.
    /// </summary>
    /// <param name="o">The object</param>
    /// <returns>An <see cref="object? "/></returns>
    public object? Transform_Array(object? o)
    {
        if (o == null) return null;
        if (o is IEnumerable list)
        {
            var t = list.Cast<object?>()
                .Select(x => TransformValue(
                    o?.GetType()?.GetGenericArguments()[0]?.Name,
                    x));
            o = string.Join(
                _tpl!.HSep,
                t);
        }
        return o;
    }

    static string VarToString(object? value) => value?.ToString() ?? string.Empty;

    string SetVar(string text, string name, string value)
    {
        text = text.Replace(Var(name), value);
        return text;
    }

    string Var(string name) => Var_Prefix + name + Var_Postfix;
}
