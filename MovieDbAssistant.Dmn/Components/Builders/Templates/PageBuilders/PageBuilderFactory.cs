using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates.PageBuilders;

[Singleton]
public sealed class PageBuilderFactory
{
    readonly IServiceProvider _serviceProvider;

    public PageBuilderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// creates a page builder factory. delegates creates to the service provider
    /// </summary>
    /// <param name="name">The name.</param>
    /// <exception cref="InvalidOperationException">page builder not found</exception>
    /// <exception cref="InvalidOperationException">create page builder error</exception>
    /// <exception cref="InvalidOperationException">page builder has wrong type</exception>
    /// <returns>An <see cref="IPageBuilder"/></returns>
    public IPageBuilder Create(PageBuilders name)
    {
        var tl = GetType();
        var tn = tl.Namespace
            + "." + name;
        var t = tl.Assembly.GetType(tn)
            ?? throw new InvalidOperationException("page builder not found: " + tn);
        var r = Activator.CreateInstance(t)
            ?? throw new InvalidOperationException("create page builder error: " + t);
        if (r is IPageBuilder o)
            return o;
        else
            throw new InvalidOperationException($"page builder has wrong type: {r.GetType().Name}. expected {typeof(IPageBuilder)}");
    }
}
