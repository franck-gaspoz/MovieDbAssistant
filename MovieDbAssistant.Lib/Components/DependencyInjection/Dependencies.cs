﻿using System.Reflection;

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
        foreach (var typeInfo in fromAssembly.DefinedTypes)
        {
            var ca = typeInfo.GetCustomAttributes(true);
            if (ca.OfType<SingletonAttribute>().Any())
            {
                services.AddSingleton(typeInfo.AsType());
            }
            if (ca.OfType<TransientAttribute>().Any())
            {
                services.AddTransient(typeInfo.AsType());
            }
            if (ca.OfType<ScopedAttribute>().Any())
            {
                services.AddScoped(typeInfo.AsType());
            }
        }
        return services;
    }
}
