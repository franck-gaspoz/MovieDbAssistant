using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.Lib.Components.DependencyInjection;

/// <summary>
/// The dependencies registerer.
/// </summary>
public static class Dependencies
{
    /// <summary>
    /// auto register depedencies
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="fromTypes">The from types assemblies</param>
    public static IServiceCollection AutoRegister(
        this IServiceCollection services,
        params Type[] fromTypes)
    {
        Array.ForEach(fromTypes, x => services.AutoRegister(x.Assembly));
        return services;
    }

    /// <summary>
    /// auto register depedencies
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="fromAssembly">The from assembly.</param>
    public static IServiceCollection AutoRegister(
        this IServiceCollection services,
        Assembly fromAssembly)
    {
#if DBG
        Debug.WriteLine(fromAssembly.Location.ToString());
#endif

        foreach (var typeInfo in fromAssembly.DefinedTypes)
        {
            var ca = typeInfo.GetCustomAttributes(true);
#if DBG
            var handled = false;
#endif
            if (ca.OfType<SingletonAttribute>().Any())
            {
                services.AddSingleton(typeInfo.AsType());
#if DBG
                handled = true;
#endif
            }
            if (ca.OfType<TransientAttribute>().Any())
            {
                services.AddTransient(typeInfo.AsType());
#if DBG
                handled = true;
#endif
            }
            if (ca.OfType<ScopedAttribute>().Any())
            {
                services.AddScoped(typeInfo.AsType());
#if DBG
                handled = true;
#endif
            }
#if DBG
            if (handled)
                Debug.WriteLine(typeInfo.AsType().FullName);
#endif
        }
        return services;
    }
}
