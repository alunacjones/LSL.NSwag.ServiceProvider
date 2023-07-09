using System;
using System.Linq;
using System.Reflection;
using LSL.NSwag.CommonTypes.Client;
using Microsoft.Extensions.DependencyInjection;
using LSL.HttpClients.ServiceProvider;

namespace LSL.NSwag.ServiceProvider
{
    /// <summary>
    /// NSwagServicesCollectionExtensions
    /// </summary>
    public static class NSwagServicesCollectionExtensions
    {
        /// <summary>
        /// Adds all NSwag client implementations from the given assembly
        /// </summary>
        /// <param name="source">The IServiceCollection to add the clients to</param>
        /// <param name="apiAssembly">The assembly to scan for INSwagClients</param>
        /// <returns>An IHttpClientBuilder to continue with configuration</returns>
        /// <exception cref="System.ArgumentException">Thrown if no clients could be found<exception>
        public static IHttpClientBuilder AddHttpClientForNSwagClientsFromAssembly(this IServiceCollection source, Assembly apiAssembly)
        {
            
            var httpClientBuilder = source.AddHttpClientForClientsFromAssembly(
                apiAssembly,
                t => !t.IsAbstract && typeof(INSwagClient).IsAssignableFrom(t),
                t => t.GetInterfaces().First(i => typeof(INSwagClient).IsAssignableFrom(i))
            );

            return httpClientBuilder;
        }

        /// <summary>
        /// Adds all NSwag client implementations from the containing assembly of the given generic type
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="TClient">A type from the assembly we wish to scan from (mus</typeparam>
        /// <returns>An IHttpClientBuilder to continue with configuration</returns>
        /// <exception cref="System.ArgumentException">Thrown if no clients could be found<exception>
        public static IHttpClientBuilder AddHttpClientForNSwagClientsFromAssemblyOf<TClient>(this IServiceCollection source) where TClient : INSwagClient
        {
            return source.AddHttpClientForNSwagClientsFromAssembly(typeof(TClient).Assembly);
        }
    }
}