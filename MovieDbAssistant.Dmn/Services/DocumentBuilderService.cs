using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Dmn.Components.DataProviders;
using MovieDbAssistant.Lib.Components;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Dmn.Services;

/// <summary>
/// The document builder service.
/// </summary>
[Scoped]
public sealed class DocumentBuilderService
{
    readonly BackgroundWorkerWrapper _backgroundWorkerWrapper;
    readonly ISignalR _signal;
    readonly DataProviderFactory _dataProviderFactory;
    DocumentBuilderContext? _context;

    public DocumentBuilderService(
        ISignalR signal,
        DataProviderFactory _dataProviderFactory)
    {
        _signal = signal;
        this._dataProviderFactory = _dataProviderFactory;
        _backgroundWorkerWrapper = new(signal, this);
    }

    /// <summary>
    /// build a document
    /// </summary>
    /// <param name="context">The context.</param>
    public void Build(ActionContext actionContext, DocumentBuilderContext context)
    {
        _context = context;
        _backgroundWorkerWrapper
            .RunAction(
                this,
                actionContext,
                (ctx, o, e) => BuildInternal(actionContext, context));
    }

    void BuildInternal(ActionContext actionContext, DocumentBuilderContext _)
    {
        //throw new NotImplementedException();    //crash test
        try
        {
            _signal.Send(this, new ActionEndedEvent(actionContext));
        }
        catch (Exception ex)
        {
            _signal.Send(this, new ActionErroredEvent(actionContext, ex));
        }
    }
}
