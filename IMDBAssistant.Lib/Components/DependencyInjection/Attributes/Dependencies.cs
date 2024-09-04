using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

/// <summary>
/// The dependencies registerer.
/// </summary>
public static class Dependencies
{
    /// <summary>
    /// auto register depedencies
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="fromType">The from type assembly.</param>
    public static IServiceCollection AutoRegister(
        this IServiceCollection services,
        Type fromType)
        => services.AutoRegister( fromType.Assembly );

    /// <summary>
    /// auto register depedencies
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="fromAssembly">The from assembly.</param>
    public static IServiceCollection AutoRegister(
        this IServiceCollection services,
        Assembly fromAssembly)
    {
        foreach (var typeInfo in fromAssembly.DefinedTypes)
        {
            var ca = typeInfo.GetCustomAttributes(true);
            if (ca.OfType<SingletonAttribute>().Any())
                services.AddSingleton(typeInfo.AsType());
            if (ca.OfType<TransientAttribute>().Any())
                services.AddTransient(typeInfo.AsType());
            if (ca.OfType<ScopedAttribute>().Any())
                services.AddScoped(typeInfo.AsType());
        }
        return services;
    }
}
