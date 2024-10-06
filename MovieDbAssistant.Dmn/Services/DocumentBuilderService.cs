using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Dmn.Components.DataProviders;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;
using MovieDbAssistant.Lib.Components.Logger;
using Microsoft.Extensions.Configuration;

using static MovieDbAssistant.Dmn.Components.Settings;
using static MovieDbAssistant.Dmn.Globals;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Sys;

namespace MovieDbAssistant.Dmn.Services;

/// <summary>
/// The document builder service.
/// </summary>
[Transient]
public sealed class DocumentBuilderService : IIdentifiable
{
    readonly BackgroundWorkerWrapper _backgroundWorkerWrapper;
    readonly ISignalR _signal;
    readonly DataProviderFactory _dataProviderFactory;
    readonly DocumentBuilderFactory _documentBuilderFactory;
    readonly IConfiguration _config;
    readonly ILogger<DocumentBuilderService> _logger;
    DocumentBuilderContext? _context;

    /// <summary>
    /// Gets the instance id.
    /// </summary>
    /// <value>A <see cref="SharedCounter"/></value>
    public SharedCounter InstanceId { get; }

    public DocumentBuilderService(
        IConfiguration configuration,
        ILogger<DocumentBuilderService> logger,
        ISignalR signal,
        DataProviderFactory dataProviderFactory,
        DocumentBuilderFactory documentBuilderFactory)
    {
        InstanceId = new(this);
        _config = configuration;
        _logger = logger;
        _signal = signal;
        _dataProviderFactory = dataProviderFactory;
        _documentBuilderFactory = documentBuilderFactory;
        _backgroundWorkerWrapper = new(logger, signal, this);
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

    void BuildInternal(ActionContext actionContext, DocumentBuilderContext context)
    {
        try
        {
            var dataProvider = context.DataProvider = 
                _dataProviderFactory.CreateDataProvider(
                    context.DataProviderType );

            var builder = context.Builder = 
                _documentBuilderFactory.CreateDocumentBuilder(
                    context.BuilderType );

            _logger.LogInformation(this, _config[ProcFile]! 
                + Path.GetFileName(context.Source) );

            var movies = dataProvider.Get(context.Source)
                ?? throw new InvalidOperationException(
                    _config[DataProvider_Failed]!
                    +context.Source?.ToString());

            builder.Build(context, movies);

            _signal.Send(this, new ActionEndedEvent(actionContext));
        }
        catch (Exception ex)
        {
            _signal.Send(this, new ActionErroredEvent(actionContext, ex));
        }
    }
}
