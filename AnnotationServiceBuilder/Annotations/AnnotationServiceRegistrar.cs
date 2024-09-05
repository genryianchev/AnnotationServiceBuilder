using AnnotationServiceBuilder.Annotations.Refit;
using AnnotationServiceBuilder.Annotations.Scoped;
using AnnotationServiceBuilder.Annotations.Singleton;
using AnnotationServiceBuilder.Annotations.Systems.Utilities;
using AnnotationServiceBuilder.Annotations.Transient;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace AnnotationServiceBuilder.Annotations
{
    /// <summary>
    /// Service registration based on custom attributes found in the specified assembly.
    /// This class provides methods for registering services with and without Trimming Safety support.
    /// </summary>
    public static class AnnotationServiceRegistrar
    {
        /// <summary>
        /// Initializes the static class by caching the types from the provided assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan for services.</param>
        public static void Initialize(Assembly assembly)
        {
            TypeListCache.Initialize(assembly);
        }

        #region Service Registration Methods

        /// <summary>
        /// Registers Refit clients based on the <see cref="RefitClientAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the clients are added.</param>
        /// <param name="defaultBaseUrl">The default base URL to use if none is specified in the attribute.</param>
        public static void AddRefitClients(IServiceCollection services, string defaultBaseUrl)
        {
            RegisterRefitClients(services, defaultBaseUrl);
        }

        /// <summary>
        /// Registers singleton services based on the <see cref="SingletonServiceAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the singleton services are added.</param>
        public static void AddSingletonServices(IServiceCollection services)
        {
            RegisterSingletonServices(services);
        }

        /// <summary>
        /// Registers transient services based on the <see cref="TransientServiceAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the transient services are added.</param>
        public static void AddTransientServices(IServiceCollection services)
        {
            RegisterTransientServices(services);
        }

        /// <summary>
        /// Registers scoped services based on the <see cref="ScopedServiceAttribute"/> and <see cref="ScopedGenericServiceAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the scoped services are added.</param>
        public static void AddScopedServices(IServiceCollection services)
        {
            RegisterScopedServices(services);
        }

        /// <summary>
        /// Registers all services based on custom attributes found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the services are added.</param>
        /// <param name="defaultBaseUrl">The default base URL to use for Refit clients if none is specified in the attribute.</param>
        public static void AddAllServices(IServiceCollection services, string defaultBaseUrl)
        {
            AddRefitClients(services, defaultBaseUrl);
            AddSingletonServices(services);
            AddTransientServices(services);
            AddScopedServices(services);
        }

        #endregion

        #region Trimming Safety Methods

        /// <summary>
        /// Registers Refit clients with trimming and AOT safety by using <see cref="DynamicDependencyAttribute"/>.
        /// </summary>
        /// <param name="services">The service collection to which the clients are added.</param>
        /// <param name="defaultBaseUrl">The default base URL to use if none is specified in the attribute.</param>
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(RefitClientAttribute))]
        public static void AddRefitClientsWithTrimmingSafety(IServiceCollection services, string defaultBaseUrl)
        {
            RegisterRefitClients(services, defaultBaseUrl);
        }

        /// <summary>
        /// Registers singleton services with trimming and AOT safety by using <see cref="DynamicDependencyAttribute"/>.
        /// </summary>
        /// <param name="services">The service collection to which the singleton services are added.</param>
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(SingletonServiceAttribute))]
        public static void AddSingletonServicesWithTrimmingSafety(IServiceCollection services)
        {
            RegisterSingletonServices(services);
        }

        /// <summary>
        /// Registers transient services with trimming and AOT safety by using <see cref="DynamicDependencyAttribute"/>.
        /// </summary>
        /// <param name="services">The service collection to which the transient services are added.</param>
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TransientServiceAttribute))]
        public static void AddTransientServicesWithTrimmingSafety(IServiceCollection services)
        {
            RegisterTransientServices(services);
        }

        /// <summary>
        /// Registers scoped services with trimming and AOT safety by using <see cref="DynamicDependencyAttribute"/>.
        /// </summary>
        /// <param name="services">The service collection to which the scoped services are added.</param>
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ScopedServiceAttribute))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ScopedGenericServiceAttribute))]
        public static void AddScopedServicesWithTrimmingSafety(IServiceCollection services)
        {
            RegisterScopedServices(services);
        }

        /// <summary>
        /// Registers all services with trimming and AOT safety by using <see cref="DynamicDependencyAttribute"/>.
        /// </summary>
        /// <param name="services">The service collection to which the services are added.</param>
        /// <param name="defaultBaseUrl">The default base URL to use for Refit clients if none is specified in the attribute.</param>
        public static void AddAllServicesWithTrimmingSafety(IServiceCollection services, string defaultBaseUrl)
        {
            AddRefitClientsWithTrimmingSafety(services, defaultBaseUrl);
            AddSingletonServicesWithTrimmingSafety(services);
            AddTransientServicesWithTrimmingSafety(services);
            AddScopedServicesWithTrimmingSafety(services);
        }

        #endregion

        #region Private Registration Helpers

        /// <summary>
        /// Registers Refit clients based on the <see cref="RefitClientAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the clients are added.</param>
        /// <param name="defaultBaseUrl">The default base URL to use if none is specified in the attribute.</param>
        private static void RegisterRefitClients(IServiceCollection services, string defaultBaseUrl)
        {
            var types = TypeListCache.GetTypes();
            var refitClientTypes = types
                .Where(type => type.IsInterface && type.GetCustomAttribute<RefitClientAttribute>() != null)
                .ToList();

            foreach (var type in refitClientTypes)
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

        /// <summary>
        /// Registers singleton services based on the <see cref="SingletonServiceAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the singleton services are added.</param>
        private static void RegisterSingletonServices(IServiceCollection services)
        {
            var types = TypeListCache.GetTypes();
            var singletonTypes = types
                .Where(type => type.GetCustomAttribute<SingletonServiceAttribute>() != null && type.IsClass && !type.IsAbstract)
                .ToList();

            foreach (var type in singletonTypes)
            {
                services.AddSingleton(type);
                Console.WriteLine($"Registered Singleton Service: {type.FullName}");
            }
        }

        /// <summary>
        /// Registers transient services based on the <see cref="TransientServiceAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the transient services are added.</param>
        private static void RegisterTransientServices(IServiceCollection services)
        {
            var types = TypeListCache.GetTypes();
            var transientTypes = types
                .Where(type => type.GetCustomAttribute<TransientServiceAttribute>() != null && type.IsClass && !type.IsAbstract)
                .ToList();

            foreach (var type in transientTypes)
            {
                services.AddTransient(type);
                Console.WriteLine($"Registered Transient Service: {type.FullName}");
            }
        }

        /// <summary>
        /// Registers scoped services based on the <see cref="ScopedServiceAttribute"/> and <see cref="ScopedGenericServiceAttribute"/> found in the specified assembly.
        /// </summary>
        /// <param name="services">The service collection to which the scoped services are added.</param>
        private static void RegisterScopedServices(IServiceCollection services)
        {
            var types = TypeListCache.GetTypes();
            var scopedTypes = types
                .Where(type => type.GetCustomAttribute<ScopedServiceAttribute>() != null && type.IsClass && !type.IsAbstract)
                .ToList();

            foreach (var type in scopedTypes)
            {
                var attribute = type.GetCustomAttribute<ScopedServiceAttribute>();
                var serviceType = attribute.ServiceType ?? type;
                services.AddScoped(serviceType, type);
                Console.WriteLine($"Registered Scoped Service: {type.FullName}");
            }

            foreach (var type in types)
            {
                if (type.GetCustomAttribute<ScopedGenericServiceAttribute>() != null && type.IsClass && !type.IsAbstract)
                {
                    var attribute = type.GetCustomAttribute<ScopedGenericServiceAttribute>();
                    services.AddScoped(attribute.ServiceType, type);
                    Console.WriteLine($"Registered Scoped Generic Service: {type.FullName}");
                }
            }
        }

        #endregion

    }
}
