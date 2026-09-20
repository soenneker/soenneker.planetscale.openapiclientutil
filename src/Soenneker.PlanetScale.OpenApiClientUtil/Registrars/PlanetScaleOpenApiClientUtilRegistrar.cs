using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.PlanetScale.HttpClients.Registrars;
using Soenneker.PlanetScale.OpenApiClientUtil.Abstract;

namespace Soenneker.PlanetScale.OpenApiClientUtil.Registrars;

/// <summary>
/// Registers the OpenAPI client utility for dependency injection.
/// </summary>
public static class PlanetScaleOpenApiClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="PlanetScaleOpenApiClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddPlanetScaleOpenApiClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddPlanetScaleOpenApiHttpClientAsSingleton()
                .TryAddSingleton<IPlanetScaleOpenApiClientUtil, PlanetScaleOpenApiClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="PlanetScaleOpenApiClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddPlanetScaleOpenApiClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddPlanetScaleOpenApiHttpClientAsSingleton()
                .TryAddScoped<IPlanetScaleOpenApiClientUtil, PlanetScaleOpenApiClientUtil>();

        return services;
    }
}
