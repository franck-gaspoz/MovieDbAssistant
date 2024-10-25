using System.Text.Json;

using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.Logger;
using MovieDbAssistant.Lib.Extensions;

using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// The template builder.
/// </summary>
public partial class TemplateBuilder
{
    /// <summary>
    /// Copy tpl rsc.
    /// </summary>
    /// <returns>A <see cref="TemplateBuilder"/></returns>
    public TemplateBuilder CopyTplRsc()
    {
        foreach (var item in _tpl!.Files)
            CopyTemplateRsc(item);
        return this;
    }

    /// <summary>
    /// Copy the rsc.
    /// </summary>
    /// <returns>A <see cref="TemplateBuilder"/></returns>
    public TemplateBuilder CopyRsc()
    {
        foreach (var item in _tpl!.Resources)
            CopyRsc(item);
        return this;
    }

    void CopyRsc(string item)
    {
        var target = Path.Combine(
            Directory.GetCurrentDirectory(),
            Context.DocContext!.OutputFolder!);

        var t = item.Split(':');

        var src = Context.AssetsPath;
        src = Path.Combine(src, t[0][1..]);
        target = Path.Combine(
            target,
            t[1][1..]);

        if (!Directory.Exists(target))
            Directory.CreateDirectory(target);

        target = Path.Combine(target,
            Path.GetFileName(src));

        bool notFound = true;
        if (File.Exists(src))
        {
            notFound = false;
            if (src.IsNewestFile(target))
            {
                File.Copy(src, target, true);
                _logger.LogInformation(this, "file copied: " + src + " to: " + target);
                return;
            }
        }
        if (Directory.Exists(src))
        {
            notFound = false;
            src.CopyDirectory(target,logger:_logger);
            _logger.LogInformation(this, "folder copied: " + src + " to: " + target);
            return;
        }
        if (notFound)
            _logger.LogWarning(this, "resource not found: " + src );
    }

    void CopyTemplateRsc(string item,bool preserveNewest = true)
    {
        var src = Path.Combine(Context.TplPath, item[1..]);
        var target = Path.Combine(
            Directory.GetCurrentDirectory(), 
            Context.DocContext!.OutputFolder!);

        if (!item.StartsWith('/'))
        {
            target = Path.Combine(target, item[1..]);
            var tgtFolder = Path.GetDirectoryName(target)!;
            if (!Directory.Exists(tgtFolder))
                Directory.CreateDirectory(tgtFolder);

            if (File.Exists(src)
                && (!preserveNewest || src.IsNewestFile(target)))
            {
                File.Copy(src,target,true);

                _logger.LogInformation(this, "file copied: " + src + " to: " + target);
            }
            if (!File.Exists(src))
                _logger.LogWarning(this, "file not found: " + src);
        }
        else
        {
            if (Directory.Exists(src))
            {
                target = Path.Combine(
                        target,
                        Path.GetFileName(src));
                src.CopyDirectory(target,logger: _logger);

                _logger.LogInformation(this, "folder copied: " + src + " to: " + target);
            }
            else
                _logger.LogWarning(this, "folder not found: " + src);
        }
    }

    void ExportData(MoviesModel data)
    {
        var src = JsonSerializer.Serialize(
            data,
            JsonSerializerProperties.Value)!;

        src = $"const data = {src};";

        Context.DocContext!.AddOutputFile(
            _dmnSettings.Value.Build.Html.DataFilename,
            src);
    }
}
