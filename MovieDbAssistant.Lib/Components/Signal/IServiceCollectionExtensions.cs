//#define DBG

using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MovieDbAssistant.Lib.Components.Extensions;

namespace MovieDbAssistant.Lib.Components.Signal;

/// <summary>
/// service collection extensions
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// add support for signal
    /// </summary>
    /// <param name="services">services collection</param>
    /// <param name="fromTypes">from types assemblies</param>
    public static IServiceCollection AddSignalR(this IServiceCollection services, params Type[] fromTypes)
    {
        Array.ForEach(fromTypes, x => services.AddSignalR(x.Assembly));
        return services;
    }

    /// <summary>
    /// add support for signal
    /// </summary>
    /// <param name="services">services collection</param>
    /// <param name="fromType">from assembly</param>
    public static IServiceCollection AddSignalR(
        this IServiceCollection services,
        Assembly fromAssembly)
    {
        var mapActions = new List<Action<SignalR>>();

#if DBG
        Debug.WriteLine(fromAssembly.Location.ToString());
#endif

        foreach (var typeInfo in fromAssembly.DefinedTypes
            .Where(x => !x.IsAbstract))
        {
            var itfs = typeInfo.GetInterfaces(typeof(ISignalHandlerBase));
            foreach (var itf in itfs)
            {
                mapActions.Add(s =>
                    s.Map(itf.GetGenericArguments()[0], typeInfo.AsType()));
#if DBG
                Debug.WriteLine(itf.GetGenericArguments()[0].Name
                    + " => " + typeInfo.AsType().FullName);
#endif
            }
        }

        services.AddTransient<ISignalR>(serviceProvider =>
        {
            var s = new SignalR(
                serviceProvider.GetRequiredService<IConfiguration>(),
                serviceProvider);
            foreach (var m in mapActions)
                m(s);
            return s;
        });

        return services;
    }
}
