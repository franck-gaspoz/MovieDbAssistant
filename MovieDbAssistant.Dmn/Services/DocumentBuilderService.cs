using MediatR;

using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Dmn.Components.DataProviders;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.Components;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.Dmn.Services;

/// <summary>
/// The document builder service.
/// </summary>
[Transient]
public sealed class DocumentBuilderService
{
    readonly BackgroundWorkerWrapper _backgroundWorkerWrapper = new();
    readonly IMediator _mediator;
    readonly DataProviderFactory _dataProviderFactory;
    DocumentBuilderContext? _context;

    public DocumentBuilderService(
        IMediator mediator,
        DataProviderFactory _dataProviderFactory)
    {
        _mediator = mediator;
        this._dataProviderFactory = _dataProviderFactory;
    }

    /// <summary>
    /// build a document
    /// </summary>
    /// <param name="context">The context.</param>
    public void Build(DocumentBuilderContext context)
    {
        _context = context;
        _backgroundWorkerWrapper.RunAction(
            (o, e) => BuildInternal());
    }

    void BuildInternal()
    {
        
        _mediator.Send(new BuildEndedEvent(this, Item_Id_Build_Json));
    }
}
