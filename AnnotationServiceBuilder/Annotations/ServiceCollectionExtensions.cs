using AnnotationServiceBuilder.Annotations.Refit;
using AnnotationServiceBuilder.Annotations.Scoped;
using AnnotationServiceBuilder.Annotations.Singleton;
using AnnotationServiceBuilder.Data.Transient_Services;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Reflection;

namespace AnnotationServiceBuilder.Annotations
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> to register services based on custom attributes.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Cache for types in the assembly to avoid repeated reflection calls.
        /// </summary>
        private static readonly ConcurrentDictionary<Assembly, Type[]> _assemblyTypeCache = new ConcurrentDictionary<Assembly, Type[]>();

        /// <summary>
        /// Registers Refit clients based on the <see cref="RefitClientAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the clients are added.</param>
        /// <param name="assembly">The assembly to scan for Refit clients.</param>
        /// <param name="defaultBaseUrl">The default base URL to use if none is specified in the attribute.</param>
        public static void AddRefitClientsFromAttributes(this IServiceCollection services, Assembly assembly, string defaultBaseUrl)
        {
            AddRefitClientsFromAttributes(services, assembly, defaultBaseUrl, null);
        }

        /// <summary>
        /// Registers Refit clients based on the <see cref="RefitClientAttribute"/> found in the specified assembly with an optional custom HTTP handler.
        /// </summary>
        /// <param name="services">The service collection to which the clients are added.</param>
        /// <param name="assembly">The assembly to scan for Refit clients.</param>
        /// <param name="defaultBaseUrl">The default base URL to use if none is specified in the attribute.</param>
        /// <param name="customHandler">An optional HTTP message handler to be added to the client pipeline.</param>
        public static void AddRefitClientsFromAttributes(this IServiceCollection services, Assembly assembly, string defaultBaseUrl, DelegatingHandler customHandler)
        {
            // Get all types in the assembly, with caching
            var types = _assemblyTypeCache.GetOrAdd(assembly, asm => asm.GetTypes());

            foreach (var type in types)
            {
                if (type.IsInterface && type.GetCustomAttribute<RefitClientAttribute>() != null)
                {
                    // Fetch all RefitClientAttribute attributes (if there are multiple)
                    var attributes = type.GetCustomAttributes<RefitClientAttribute>();

                    // Process each attribute
                    foreach (var attribute in attributes)
                    {
                        var baseUrl = new Uri(attribute.BaseUrl ?? defaultBaseUrl);

                        var refitClientBuilder = services.AddRefitClient(type)
                                                         .ConfigureHttpClient(client => client.BaseAddress = baseUrl);

                        // If a custom handler is provided, add it to the pipeline
                        if (customHandler != null)
                        {
                            refitClientBuilder.AddHttpMessageHandler(() => customHandler);
                        }

                        // Optional: Log the registration
                        Console.WriteLine($"Registered Refit Client: {type.FullName} with BaseUrl: {baseUrl}");
                    }
                }
            }
        }

        /// <summary>
        /// Registers singleton services based on the <see cref="SingletonServiceAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the singleton services are added.</param>
        /// <param name="assembly">The assembly to scan for singleton services.</param>
        public static void AddAnnotatedSingletonServices(this IServiceCollection services, Assembly assembly)
        {
            // Get all types in the assembly, with caching
            var types = _assemblyTypeCache.GetOrAdd(assembly, asm => asm.GetTypes());

            foreach (var type in types)
            {
                if (type.GetCustomAttribute<SingletonServiceAttribute>() != null && type.IsClass && !type.IsAbstract)
                {
                    services.AddSingleton(type);

                    // Optional: Log the registration
                    Console.WriteLine($"Registered Singleton Service: {type.FullName}");
                }
            }
        }

        /// <summary>
        /// Registers transient services based on the <see cref="TransientServiceAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the transient services are added.</param>
        /// <param name="assembly">The assembly to scan for transient services.</param>
        public static void AddAnnotatedTransientServices(this IServiceCollection services, Assembly assembly)
        {
            // Get all types in the assembly, with caching
            var types = _assemblyTypeCache.GetOrAdd(assembly, asm => asm.GetTypes());

            foreach (var type in types)
            {
                if (type.GetCustomAttribute<TransientServiceAttribute>() != null && type.IsClass && !type.IsAbstract)
                {
                    services.AddTransient(type);

                    // Optional: Log the registration
                    Console.WriteLine($"Registered Transient Service: {type.FullName}");
                }
            }
        }

        /// <summary>
        /// Registers scoped services based on the <see cref="ScopedServiceAttribute"/> and <see cref="ScopedGenericServiceAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the scoped services are added.</param>
        /// <param name="assembly">The assembly to scan for scoped services.</param>
        public static void AddAnnotatedScopedServices(this IServiceCollection services, Assembly assembly)
        {
            // Get all types in the assembly, with caching
            var types = _assemblyTypeCache.GetOrAdd(assembly, asm => asm.GetTypes());

            foreach (var type in types)
            {
                if (type.GetCustomAttribute<ScopedServiceAttribute>() != null && type.IsClass && !type.IsAbstract)
                {
                    var attribute = type.GetCustomAttribute<ScopedServiceAttribute>();
                    var serviceType = attribute.ServiceType ?? type;
                    services.AddScoped(serviceType, type);

                    // Optional: Log the registration
                    Console.WriteLine($"Registered Scoped Service: {type.FullName}");
                }
            }

            // Process generic scoped services separately
            foreach (var type in types)
            {
                if (type.GetCustomAttribute<ScopedGenericServiceAttribute>() != null && type.IsClass && !type.IsAbstract)
                {
                    var attribute = type.GetCustomAttribute<ScopedGenericServiceAttribute>();
                    services.AddScoped(attribute.ServiceType, type);

                    // Optional: Log the registration
                    Console.WriteLine($"Registered Scoped Generic Service: {type.FullName}");
                }
            }
        }
    }
}
