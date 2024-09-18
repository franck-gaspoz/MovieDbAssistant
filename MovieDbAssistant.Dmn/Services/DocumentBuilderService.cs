using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Dmn.Components.DataProviders;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.Components;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.Dmn.Services;

/// <summary>
/// The document builder service.
/// </summary>
[Scoped]
public sealed class DocumentBuilderService
{
    readonly BackgroundWorkerWrapper _backgroundWorkerWrapper = new();
    readonly ISignalR _signal;
    readonly DataProviderFactory _dataProviderFactory;
    DocumentBuilderContext? _context;

    public DocumentBuilderService(
        ISignalR signal,
        DataProviderFactory _dataProviderFactory)
    {
        _signal = signal;
        this._dataProviderFactory = _dataProviderFactory;
    }

    /// <summary>
    /// build a document
    /// </summary>
    /// <param name="context">The context.</param>
    public void Build(ActionContext actionContext,DocumentBuilderContext context)
    {
        _context = context;
        _backgroundWorkerWrapper.RunAction(
            (o, e) => BuildInternal(actionContext));
    }

    void BuildInternal(ActionContext context) {
        try
        {

        }
        catch (Exception ex)
        {
        }
        finally
        {

        }
    }
}
