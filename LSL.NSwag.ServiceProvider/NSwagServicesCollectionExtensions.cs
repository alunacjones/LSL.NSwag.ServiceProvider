using System;
using System.Linq;
using System.Reflection;
using LSL.NSwag.CommonTypes.Client;
using Microsoft.Extensions.DependencyInjection;

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
        /// <returns></returns>
        public static IHttpClientBuilder AddHttpClientForNSwagClients(this IServiceCollection source, Assembly apiAssembly)
        {
            var genericMethod = typeof(HttpClientBuilderExtensions).GetMethods()
                .Single(m => m.Name == "AddTypedClient" && m.GetGenericArguments().Length == 2);

            var httpClientBuilder = source.AddHttpClient(apiAssembly.GetName().Name);

            var eligibleTypes = apiAssembly
                .GetTypes()
                .Where(t => !t.IsAbstract && typeof(INSwagClient).IsAssignableFrom(t))
                .Select(t => new { Type = t, EligibleInterface = t.GetInterfaces().First(i => typeof(INSwagClient).IsAssignableFrom(i)) })
                .Where(t => t.EligibleInterface != null);

            foreach (var type in eligibleTypes)
            {
                var @delegate = genericMethod
                    .MakeGenericMethod(
                        new[] { type.EligibleInterface, type.Type }
                    )
                    .CreateDelegate(typeof(Func<IHttpClientBuilder, IHttpClientBuilder>));

                ((Func<IHttpClientBuilder, IHttpClientBuilder>)@delegate)(httpClientBuilder);
            }

            return httpClientBuilder;
        }
    }

}