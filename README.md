[![](https://img.shields.io/nuget/v/soenneker.planetscale.openapiclientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.planetscale.openapiclientutil/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.planetscale.openapiclientutil/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.planetscale.openapiclientutil/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.planetscale.openapiclientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.planetscale.openapiclientutil/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.PlanetScale.OpenApiClientUtil
### A thread-safe utility for obtaining PlanetScale's OpenApiClient singleton.

## Installation

```
dotnet add package Soenneker.PlanetScale.OpenApiClientUtil
```

## Usage

Register `services.AddPlanetScaleOpenApiClientUtilAsSingleton()` from
`Soenneker.PlanetScale.OpenApiClientUtil.Registrars`, then inject
`IPlanetScaleOpenApiClientUtil` from `Soenneker.PlanetScale.OpenApiClientUtil.Abstract`.

Configure `PlanetScale:ApiKey` with `<SERVICE_TOKEN_ID>:<SERVICE_TOKEN>` using your
secret store or the `PlanetScale__ApiKey` environment variable. The default API
endpoint is `https://api.planetscale.com/v1`. Override `PlanetScale:ClientBaseUrl`
when needed; both the HTTP client and generated request adapter use it.

```csharp
var client = await clientUtil.Get(cancellationToken);
var organizations = await client.Organizations.GetAsync(cancellationToken: cancellationToken);
```

For OAuth, set `PlanetScale:ApiKey` to the access token and
`PlanetScale:AuthHeaderValueTemplate` to `Bearer {token}`.

The client is cached for the utility's lifetime. Do not dispose it per request.
PlanetScale credentials and network access are not needed by the unit tests.

## Initial local build

Until the dependency packages are published, pack the sibling OpenApiClient and
HttpClients projects to a local NuGet feed, then restore this solution with
`dotnet restore -p:RestoreAdditionalProjectSources=C:\path\to\feed`.
The package references remain standard NuGet references for independent repositories.
