using System;
using System.Net.Http;
using FluentAssertions;
using LSL.NSwag.CommonTypes.Client;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LSL.NSwag.ServiceProvider.Tests
{
    public class NSwagServicesCollectionExtensionsTests
    {
        [Test]
        public void GivenAnAssemblyToScan_ItShouldAddTheAppropriateClients()
        {
            // Arange
            var expectedBaseUri = new Uri("http://test.com");
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection
                .AddHttpClientForNSwagClientsFromAssembly(typeof(IClient1).Assembly)
                .ConfigureHttpClient(c => c.BaseAddress = expectedBaseUri);

            // Assert        
            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider.GetRequiredService<IClient1>()
                .HttpClient
                .BaseAddress
                .Should()
                .Be(expectedBaseUri);

            serviceProvider.GetRequiredService<IClient2>()
                .HttpClient
                .BaseAddress
                .Should()
                .Be(expectedBaseUri);                
        }

        [Test]
        public void GivenAnAssemblyToScanViaAType_ItShouldAddTheAppropriateClients()
        {
            // Arange
            var expectedBaseUri = new Uri("http://test.com");
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection
                .AddHttpClientForNSwagClientsFromAssemblyOf<IClient1>()
                .ConfigureHttpClient(c => c.BaseAddress = expectedBaseUri);

            // Assert        
            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider.GetRequiredService<IClient1>()
                .HttpClient
                .BaseAddress
                .Should()
                .Be(expectedBaseUri);

            serviceProvider.GetRequiredService<IClient2>()
                .HttpClient
                .BaseAddress
                .Should()
                .Be(expectedBaseUri);                
        }        

        private interface IClient1 : INSwagClient { HttpClient HttpClient { get; } }
        private interface IClient2 : INSwagClient { HttpClient HttpClient { get; } }

        private class Client1 : IClient1
        {
            public Client1(HttpClient httpClient)
            {
                HttpClient = httpClient;
            }

            public HttpClient HttpClient { get; }
        }

        private class Client2 : IClient2
        {
            public Client2(HttpClient httpClient)
            {
                HttpClient = httpClient;
            }

            public HttpClient HttpClient { get; }
        }
    }
}
