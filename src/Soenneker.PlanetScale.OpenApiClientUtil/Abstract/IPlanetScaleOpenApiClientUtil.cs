using Soenneker.PlanetScale.OpenApiClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.PlanetScale.OpenApiClientUtil.Abstract;

/// <summary>
/// Exposes a cached OpenAPI client instance.
/// </summary>
public interface IPlanetScaleOpenApiClientUtil: IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the cached generated client using the configured PlanetScale HTTP client and authentication.
    /// </summary>
    ValueTask<PlanetScaleOpenApiClient> Get(CancellationToken cancellationToken = default);
}
