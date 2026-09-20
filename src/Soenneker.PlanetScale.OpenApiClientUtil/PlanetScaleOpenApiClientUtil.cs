using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.PlanetScale.HttpClients.Abstract;
using Soenneker.PlanetScale.OpenApiClientUtil.Abstract;
using Soenneker.PlanetScale.OpenApiClient;
using Soenneker.Kiota.GenericAuthenticationProvider;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.PlanetScale.OpenApiClientUtil;

public sealed class PlanetScaleOpenApiClientUtil : IPlanetScaleOpenApiClientUtil
{
    private readonly AsyncSingleton<PlanetScaleOpenApiClient> _client;

    public PlanetScaleOpenApiClientUtil(IPlanetScaleOpenApiHttpClient httpClientUtil, IConfiguration configuration)
    {
        _client = new AsyncSingleton<PlanetScaleOpenApiClient>(async token =>
        {
            HttpClient httpClient = await httpClientUtil.Get(token).NoSync();

            var apiKey = configuration.GetValueStrict<string>("PlanetScale:ApiKey");
            string authHeaderName = configuration["PlanetScale:AuthHeaderName"] ?? "Authorization";
            string authHeaderValueTemplate = configuration["PlanetScale:AuthHeaderValueTemplate"] ?? "{token}";
            string authHeaderValue = authHeaderValueTemplate.Replace("{token}", apiKey, StringComparison.Ordinal);

            var requestAdapter = new HttpClientRequestAdapter(new GenericAuthenticationProvider(headerName: authHeaderName, headerValue: authHeaderValue),
                httpClient: httpClient)
            {
                BaseUrl = httpClient.BaseAddress!.AbsoluteUri.TrimEnd('/')
            };

            return new PlanetScaleOpenApiClient(requestAdapter);
        });
    }

    public ValueTask<PlanetScaleOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }
}
