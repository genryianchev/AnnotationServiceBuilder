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
    /// Service registration based on custom attributes found in the specified assembly.
    /// </summary>
    public class ServiceCollectionExtensions
    {
        private static readonly ConcurrentDictionary<Assembly, Type[]> _assemblyTypeCache = new ConcurrentDictionary<Assembly, Type[]>();

        private readonly Type[] _types;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCollectionExtensions"/> class.
        /// </summary>
        /// <param name="assembly">The assembly to scan for services.</param>
        public ServiceCollectionExtensions(Assembly assembly)
        {
            _types = GetTypesFromAssembly(assembly);
        }

        /// <summary>
        /// Retrieves all types from the specified assembly, using a cache to avoid repeated reflection calls.
        /// </summary>
        /// <param name="assembly">The assembly to scan for types.</param>
        /// <returns>An array of types in the assembly.</returns>
        private static Type[] GetTypesFromAssembly(Assembly assembly)
        {
            return _assemblyTypeCache.GetOrAdd(assembly, asm => asm.GetTypes());
        }

        /// <summary>
        /// Registers Refit clients based on the <see cref="RefitClientAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the clients are added.</param>
        /// <param name="defaultBaseUrl">The default base URL to use if none is specified in the attribute.</param>
        public void AddRefitClients(IServiceCollection services, string defaultBaseUrl)
        {
            foreach (var type in _types)
            {
                if (type.IsInterface && type.GetCustomAttribute<RefitClientAttribute>() != null)
                {
                    var attributes = type.GetCustomAttributes<RefitClientAttribute>();
                    foreach (var attribute in attributes)
                    {
                        var baseUrl = new Uri(attribute.BaseUrl ?? defaultBaseUrl);
                        var refitClientBuilder = services.AddRefitClient(type)
                                                         .ConfigureHttpClient(client => client.BaseAddress = baseUrl);

                        Console.WriteLine($"Registered Refit Client: {type.FullName} with BaseUrl: {baseUrl}");
                    }
                }
            }
        }

        /// <summary>
        /// Registers Refit clients based on the <see cref="RefitClientAttribute"/> found in the specified assembly with a custom HTTP message handler.
        /// </summary>
        /// <param name="services">The service collection to which the clients are added.</param>
        /// <param name="defaultBaseUrl">The default base URL to use if none is specified in the attribute.</param>
        /// <param name="customHandler">A custom HTTP message handler to be added to the client pipeline.</param>
        public void AddRefitClients(IServiceCollection services, string defaultBaseUrl, DelegatingHandler customHandler)
        {
            foreach (var type in _types)
            {
                if (type.IsInterface && type.GetCustomAttribute<RefitClientAttribute>() != null)
                {
                    var attributes = type.GetCustomAttributes<RefitClientAttribute>();
                    foreach (var attribute in attributes)
                    {
                        var baseUrl = new Uri(attribute.BaseUrl ?? defaultBaseUrl);
                        var refitClientBuilder = services.AddRefitClient(type)
                                                         .ConfigureHttpClient(client => client.BaseAddress = baseUrl);

                        if (customHandler != null)
                        {
                            refitClientBuilder.AddHttpMessageHandler(() => customHandler);
                        }

                        Console.WriteLine($"Registered Refit Client: {type.FullName} with BaseUrl: {baseUrl}");
                    }
                }
            }
        }

        /// <summary>
        /// Registers singleton services based on the <see cref="SingletonServiceAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the singleton services are added.</param>
        public void AddSingletonServices(IServiceCollection services)
        {
            foreach (var type in _types)
            {
                if (type.GetCustomAttribute<SingletonServiceAttribute>() != null && type.IsClass && !type.IsAbstract)
                {
                    services.AddSingleton(type);
                    Console.WriteLine($"Registered Singleton Service: {type.FullName}");
                }
            }
        }

        /// <summary>
        /// Registers transient services based on the <see cref="TransientServiceAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the transient services are added.</param>
        public void AddTransientServices(IServiceCollection services)
        {
            foreach (var type in _types)
            {
                if (type.GetCustomAttribute<TransientServiceAttribute>() != null && type.IsClass && !type.IsAbstract)
                {
                    services.AddTransient(type);
                    Console.WriteLine($"Registered Transient Service: {type.FullName}");
                }
            }
        }

        /// <summary>
        /// Registers scoped services based on the <see cref="ScopedServiceAttribute"/> and <see cref="ScopedGenericServiceAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the scoped services are added.</param>
        public void AddScopedServices(IServiceCollection services)
        {
            foreach (var type in _types)
            {
                if (type.GetCustomAttribute<ScopedServiceAttribute>() != null && type.IsClass && !type.IsAbstract)
                {
                    var attribute = type.GetCustomAttribute<ScopedServiceAttribute>();
                    var serviceType = attribute.ServiceType ?? type;
                    services.AddScoped(serviceType, type);
                    Console.WriteLine($"Registered Scoped Service: {type.FullName}");
                }
            }

            foreach (var type in _types)
            {
                if (type.GetCustomAttribute<ScopedGenericServiceAttribute>() != null && type.IsClass && !type.IsAbstract)
                {
                    var attribute = type.GetCustomAttribute<ScopedGenericServiceAttribute>();
                    services.AddScoped(attribute.ServiceType, type);
                    Console.WriteLine($"Registered Scoped Generic Service: {type.FullName}");
                }
            }
        }

        /// <summary>
        /// Registers all services based on custom attributes found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the services are added.</param>
        /// <param name="defaultBaseUrl">The default base URL to use for Refit clients if none is specified in the attribute.</param>
        public void AddAllServices(IServiceCollection services, string defaultBaseUrl)
        {
            AddRefitClients(services, defaultBaseUrl);
            AddSingletonServices(services);
            AddTransientServices(services);
            AddScopedServices(services);
        }

        /// <summary>
        /// Registers all services based on custom attributes found in the specified assembly with a custom HTTP message handler.
        /// </summary>
        /// <param name="services">The service collection to which the services are added.</param>
        /// <param name="defaultBaseUrl">The default base URL to use for Refit clients if none is specified in the attribute.</param>
        /// <param name="customHandler">A custom HTTP message handler to be added to Refit clients.</param>
        public void AddAllServices(IServiceCollection services, string defaultBaseUrl, DelegatingHandler customHandler)
        {
            AddRefitClients(services, defaultBaseUrl, customHandler);
            AddSingletonServices(services);
            AddTransientServices(services);
            AddScopedServices(services);
        }
    }
}
