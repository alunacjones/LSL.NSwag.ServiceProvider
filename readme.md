[![Build status](https://img.shields.io/appveyor/ci/alunacjones/lsl-nswag-serviceprovider.svg)](https://ci.appveyor.com/project/alunacjones/lsl-nswag-serviceprovider)
[![Coveralls branch](https://img.shields.io/coverallsCoverage/github/alunacjones/LSL.NSwag.ServiceProvider)](https://coveralls.io/github/alunacjones/LSL.NSwag.ServiceProvider)
[![NuGet](https://img.shields.io/nuget/v/LSL.NSwag.ServiceProvider.svg)](https://www.nuget.org/packages/LSL.NSwag.ServiceProvider/)

# LSL.NSwag.ServiceProvider

Provides a quick mechanism to register NSwag Clients that are based on the `INSwagClient` interface defined in the [LSL.NSwag.CommonTypes](https://www.nuget.org/packages/LSL.NSwag.CommonTypes) NuGet library.

## Quickstart

When registering services the following line can be used to register all NSwag generated clients:

```csharp
using LSL.NSwag.ServiceProvider;
...
serviceCollection
    .AddHttpClientForNSwagClientsFromAssembly(typeof(IClient1).Assembly)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://test.com"));
```

> **NOTE**: The extension methods return an `IHttpClientBuilder` so that further configuration can be done (like setting up a common base url in the preceding example)

The same can be achieved with the generic method too:

```csharp
using LSL.NSwag.ServiceProvider;
...
serviceCollection
    .AddHttpClientForNSwagClientsFromAssemblyOf<IClient1>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://test.com"));
```
