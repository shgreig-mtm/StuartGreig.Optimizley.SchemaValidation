using Microsoft.Extensions.DependencyInjection;
using SchemaInspector.Services;

namespace SchemaInspector.Extensions;

/// <summary>
/// Extension methods for registering Schema Inspector services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the Schema Inspector component and services.
    /// Call this in your Startup.ConfigureServices method.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddSchemaInspector(this IServiceCollection services)
    {
        // Register the controller
        services.AddControllersWithViews()
            .AddApplicationPart(typeof(ServiceCollectionExtensions).Assembly);

        // Register the default schema service if not already registered
        services.TryAddSingleton<ISchemaService, DefaultSchemaService>();

        // Register the schema validator
        services.TryAddSingleton<ISchemaValidator, SchemaValidator>();

        return services;
    }

    /// <summary>
    /// Registers the Schema Inspector with a custom schema service implementation.
    /// </summary>
    /// <typeparam name="TSchemaService">Your custom ISchemaService implementation</typeparam>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddSchemaInspector<TSchemaService>(this IServiceCollection services)
        where TSchemaService : class, ISchemaService
    {
        // Register the controller
        services.AddControllersWithViews()
            .AddApplicationPart(typeof(ServiceCollectionExtensions).Assembly);

        // Register the custom schema service
        services.AddSingleton<ISchemaService, TSchemaService>();

        // Register the schema validator
        services.TryAddSingleton<ISchemaValidator, SchemaValidator>();

        return services;
    }

    private static void TryAddSingleton<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        if (!services.Any(x => x.ServiceType == typeof(TService)))
        {
            services.AddSingleton<TService, TImplementation>();
        }
    }
}
