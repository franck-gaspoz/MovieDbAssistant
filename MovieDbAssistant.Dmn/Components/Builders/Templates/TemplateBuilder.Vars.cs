﻿using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using MovieDbAssistant.Dmn.Components.Builders.Html;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Extensions;

using Newtonsoft.Json.Linq;

using JObject = Newtonsoft.Json.Linq.JObject;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

#pragma warning disable CA1822 // Marquer les membres comme étant static

/// <summary>
/// The template builder.
/// </summary>
public partial class TemplateBuilder
{
    public const string Var_Prefix = "{{";

    public const string Var_Postfix = "}}";

    public const string Include_Part_Condition_Separator = ":";

    public const string Include_Part_Condition_Value_Prefix = "=";

    public const string Include_Part_Condition_Value_Postfix = "=";

    public const string Include_Part_Condition_Default = "default";

    static string KeyToVar(string key) => key.ToFirstLower();

    (string tpl,Dictionary<string,object?>? nprops) SetVars(string tpl)
    {
        (tpl,var nprops) = SetVars(tpl, GetTemplateProps(false, null, null));
        return (tpl,nprops);
    }

    (string tpl, Dictionary<string, object?> tplProps, Dictionary<string, object?>? nprops) 
        SetVars(string tpl, HtmlDocumentBuilderContext htmlContext)
    {
        var props = GetTemplateProps(false, null, htmlContext);
        (tpl,var nprops) = SetVars(tpl, props);
        return (tpl, props, nprops);
    }

    (string tpl, Dictionary<string, object?> tplProps, Dictionary<string, object?>? nprops) 
        SetVars(
            string tpl,
            HtmlDocumentBuilderContext htmlContext,
            MovieModel data)
    {
        var props = GetTemplateProps(true, data, htmlContext);
        (tpl,var nprops) = SetVars(tpl, props);
        return (tpl, props, nprops);
    }

    (string tpl,Dictionary<string,object?>? nprops) SetVars(
        string tpl,
        Dictionary<string, object?> vars,
        string? prefix = null, 
        bool skipExpandDefaultDecls = false)
    {
        Dictionary<string, object?>? nprops = null;

        foreach (var kvp in vars)
        {
            var val = kvp.Value;
            var varnp = KeyToVar(kvp.Key);
            var vtype = val?.GetType();

            if (val != null && val is JsonElement jval)
            {
                // dynamic json
                var jprops = new Dictionary<string, object?>();
                foreach (var jprop in jval.EnumerateObject())
                {
                    var propval = jprop.Value;

                    if (propval.ValueKind != JsonValueKind.Undefined)
                    {
                        if (propval.ValueKind == JsonValueKind.Object)
                            jprops.Add(
                                jprop.Name,
                                propval);
                        else
                            jprops.Add(
                                jprop.Name,
                                propval.ToString());
                    }
                }
                (tpl, _) = SetVars(
                    tpl,
                    jprops,
                    prefix != null ?
                        prefix + '.' + varnp
                        : varnp
                        );
            }

            if (val != null
                && vtype!.Namespace!
                    .StartsWith(GetType()
                        .Namespace!
                        .Split('.')[0])
                    && !vtype!.IsArray)
            {
                // model not null
                (tpl, _) = SetVars(
                    tpl,
                    val.GetProperties(x =>
                        x.GetCustomAttribute<IgnoreDataMemberAttribute>(true) == null),
                    prefix != null ?
                        prefix + '.' + varnp
                        : varnp
                        );
            }
            else
            {
                // not a model or null model
                var k = KeyToVar(kvp.Key);
                if (prefix != null)
                    k = prefix + '.' + k;

                tpl = SetVar(
                    tpl,
                    KeyToVar(k),
                    VarToString(TransformValue(k, val)));
            }
        }

        // expand not yet extended default decl
        if (!skipExpandDefaultDecls)
            (tpl,nprops) = ExpandIgnoredDefaultDecls(tpl);

        return (tpl,nprops);
    }

    (string text, Dictionary<string, object?> nprops) 
        ExpandIgnoredDefaultDecls(string text)
    {
        var nprops = new Dictionary<string, object?>();
        var nextPos = 0;
        while (nextPos != Index_NoNext)
        {
            (text, var name, var value, nextPos) =
                ExpandIgnoredDefaultDecl(text, nextPos);
            if (name!=null)
                nprops.AddOrReplace(name, value);
        }
        if (nprops.Any())
            (text, _) = SetVars(text,nprops,skipExpandDefaultDecls:true);
        return (text,nprops);
    }

    (string text, string? name, string? value, int nextPos) ExpandIgnoredDefaultDecl(
        string text, 
        int startPos,
        string? expectedName = null)
    {
        var def = (text, (string?)null, (string?)null, Index_NoNext);
        var left = Include_Part_Condition_Separator
            + Include_Part_Condition_Default
            + Include_Part_Condition_Value_Prefix;  // :default=

        var x2 = text.IndexOf(left, startPos);
        if (x2 < 0) return def;
        var x1 = text.SearchLeft(Var_Prefix, x2 - 1);
        if (x1 < 0) return def;
        var x1b = x1 + Var_Prefix.Length;

        var right = Include_Part_Condition_Value_Postfix
            + Var_Postfix;
        var y = text.SearchRight(right, x1 + 1);
        if (y < 0) return def;

        var a = x2 + left.Length;
        var b = y - 1;
        y += right.Length;
        var value = text[a..(b + 1)];
        var name = text[x1b..x2];

        if (expectedName !=null
            && name != expectedName) return def;

        text = text[..x1] + text[y..];

        // consume any other default decl for the same var
        var nextPos = y;
        while (nextPos != Index_NoNext)
            (text, _, _, nextPos) = ExpandIgnoredDefaultDecl(
                text, nextPos, name);

        return (text,name,value,y);
    }

    #region transforms

    /// <summary>
    /// apply a transform to a value depending on the config <code>tpl.Transforms</code>
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="value">value</param>
    /// <returns>transformed value or value</returns>
    /// <exception cref="InvalidOperationException">"value transformer method not found</exception>
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
            .GetMethod(transform.Operation)
                ?? throw new InvalidOperationException(
                    "value transformer method not found: " + transform.Operation);

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
                _tpl!.Options.HSep,
                t);
        }
        return o;
    }

    #endregion

    static string VarToString(object? value) => value?.ToString() ?? string.Empty;

    string SetVar(string text, string name, string? value)
    {
        var varn = Var(name);

        if (varn == value)
        {
            // Var(name) == value <=> included prop : no value
            // avoid (set to null) auto propagated variable in included prop
            value = null;
        }

        if (value == null)
        {
            var nextPos = 0;
            (text, var defaultValue, nextPos) = ExpandNextVarDefault(
                text,
                name,
                nextPos);

            if (nextPos > -1)
            {
                value = defaultValue;
            }
        }

        text = text.Replace(varn, value);

        return text;
    }

    (string content, string? value, int nextPos) ExpandNextVarDefault(
        string text,
        string varName,
        int startPos)
    {
        var nextPos = Index_NoNext;
        var left = Var_Prefix
            + varName
            + Include_Part_Condition_Separator
            + Include_Part_Condition_Default
            + Include_Part_Condition_Value_Prefix;
        var right = Include_Part_Condition_Value_Postfix
            + Var_Postfix;
        var def = (text, (string?)null, nextPos);

        var x = text.IndexOf(left, startPos);
        if (x < 0) return def;
        var a = x + left.Length;
        var y = text.IndexOf(right, x);
        if (y < 0) return def;
        var b = y - 1;
        y += right.Length;
        var decl = text[a..(b + 1)];

        text = text[0..x] + text[y..];
        nextPos = y;

        // consume any other default decl for the same var
        while (nextPos != Index_NoNext)
            (text, _, nextPos) = ExpandNextVarDefault(
                text, varName, nextPos);

        return (text, decl, y);
    }

    static string Var(string name) => Var_Prefix + name + Var_Postfix;
}
