using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.PlanetScale.HttpClients.Abstract;

namespace Soenneker.PlanetScale.OpenApiClientUtil.Tests;

public sealed class PlanetScaleOpenApiClientUtilTests
{
    [Test]
    public async Task Uses_configured_base_url_and_service_token_and_caches_client()
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["PlanetScale:ApiKey"] = "test-id:test-token"
        }).Build();
        using var handler = new RecordingHandler();
        using var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://planetscale.invalid/custom/v1/") };
        using var httpClientUtil = new StubHttpClient(httpClient);
        await using var utility = new PlanetScaleOpenApiClientUtil(httpClientUtil, configuration);

        var client = await utility.Get();
        await Assert.That(ReferenceEquals(client, await utility.Get())).IsTrue();
        var response = await client.Organizations.GetAsync();

        await Assert.That(handler.RequestUri).IsEqualTo("https://planetscale.invalid/custom/v1/organizations");
        await Assert.That(handler.Authorization).IsEqualTo("test-id:test-token");
        await Assert.That(response?.Data?.Count).IsEqualTo(0);
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        public string? RequestUri { get; private set; }
        public string? Authorization { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri?.AbsoluteUri;
            Authorization = request.Headers.GetValues("Authorization").Single();
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"data\":[]}", System.Text.Encoding.UTF8, "application/json")
            });
        }
    }

    private sealed class StubHttpClient(HttpClient client) : IPlanetScaleOpenApiHttpClient
    {
        public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default) => ValueTask.FromResult(client);
        public void Dispose() { }
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
