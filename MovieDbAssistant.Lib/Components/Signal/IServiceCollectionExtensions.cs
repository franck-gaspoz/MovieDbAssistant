//#define DBG

#if DBG
using System.Diagnostics;
#endif
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Lib.Extensions;

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
        var mapActions = new List<Action<SignalR>>();
        Array.ForEach(fromTypes, x => BuildMap(x.Assembly, mapActions));
        Register(services, mapActions);
        return services;
    }

    /// <summary>
    /// add support for signal
    /// </summary>
    /// <param name="fromType">from assembly</param>
    /// <param name="fromAssembly">map actions</param>
    static void BuildMap(
        Assembly fromAssembly,
        List<Action<SignalR>> mapActions)
    {
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
                    s.MapType(itf.GetGenericArguments()[0], typeInfo.AsType()));
#if DBG
                Debug.WriteLine(itf.GetGenericArguments()[0].Name
                    + " => " + typeInfo.AsType().FullName);
#endif
            }
        }
    }

    static IServiceCollection Register(
        IServiceCollection services,
        List<Action<SignalR>> mapActions)
    {
        services.AddScoped<ISignalR>(serviceProvider =>
        {
            var s = new SignalR(
                serviceProvider.GetRequiredService<ILogger<SignalR>>(),
                serviceProvider.GetRequiredService<IConfiguration>(),
                serviceProvider);
            foreach (var m in mapActions)
                m(s);
            return s;
        });

        return services;
    }
}
