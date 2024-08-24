using AnnationServiceBilder.Annotations.Refit;
using AnnationServiceBilder.Annotations.Scoped;
using AnnationServiceBilder.Annotations.Singleton;
using AnnationServiceBilder.Data.Transient_Services;
using Refit;
using System.Reflection;

namespace AnnationServiceBilder.Annotations
{
    /// <summary>
    /// Provides extension methods for registering annotated services in the dependency injection container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Refit clients in the dependency injection container based on the <see cref="RefitClientAttribute"/> annotations.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the clients to.</param>
        /// <param name="assembly">The assembly to scan for interfaces marked with <see cref="RefitClientAttribute"/>.</param>
        /// <param name="defaultBaseUrl">The default base URL to use if the <see cref="RefitClientAttribute.BaseUrl"/> is not specified.</param>
        public static void AddRefitClientsFromAttributes(this IServiceCollection services, Assembly assembly, string defaultBaseUrl)
        {
            var refitClientTypes = assembly.GetTypes()
                .Where(t => t.IsInterface && t.GetCustomAttribute<RefitClientAttribute>() != null);

            foreach (var interfaceType in refitClientTypes)
            {
                var attribute = interfaceType.GetCustomAttribute<RefitClientAttribute>();
                var baseUrl = new Uri(attribute.BaseUrl ?? defaultBaseUrl);

                services.AddRefitClient(interfaceType)
                        .ConfigureHttpClient(c => c.BaseAddress = baseUrl);
            }
        }

        /// <summary>
        /// Registers services marked with <see cref="SingletonServiceAttribute"/> as singletons in the dependency injection container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="assembly">The assembly to scan for classes marked with <see cref="SingletonServiceAttribute"/>.</param>
        public static void AddAnnotatedSingletonServices(this IServiceCollection services, Assembly assembly)
        {
            var singletonTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<SingletonServiceAttribute>() != null && t.IsClass && !t.IsAbstract);

            foreach (var type in singletonTypes)
            {
                services.AddSingleton(type);
            }
        }

        /// <summary>
        /// Registers services marked with <see cref="TransientServiceAttribute"/> as transient in the dependency injection container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="assembly">The assembly to scan for classes marked with <see cref="TransientServiceAttribute"/>.</param>
        public static void AddAnnotatedTransientServices(this IServiceCollection services, Assembly assembly)
        {
            var transientTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<TransientServiceAttribute>() != null && t.IsClass && !t.IsAbstract);

            foreach (var type in transientTypes)
            {
                services.AddTransient(type);
            }
        }

        /// <summary>
        /// Registers services marked with <see cref="ScopedServiceAttribute"/> and <see cref="ScopedGenericServiceAttribute"/> as scoped in the dependency injection container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="assembly">The assembly to scan for classes marked with <see cref="ScopedServiceAttribute"/> and <see cref="ScopedGenericServiceAttribute"/>.</param>
        public static void AddAnnotatedScopedServices(this IServiceCollection services, Assembly assembly)
        {
            // Register non-generic scoped services
            var scopedTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<ScopedServiceAttribute>() != null && t.IsClass && !t.IsAbstract);

            foreach (var type in scopedTypes)
            {
                var attribute = type.GetCustomAttribute<ScopedServiceAttribute>();
                var serviceType = attribute.ServiceType ?? type;
                services.AddScoped(serviceType, type);
            }

            // Register generic scoped services
            var genericScopedTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<ScopedGenericServiceAttribute>() != null && t.IsClass && !t.IsAbstract);

            foreach (var type in genericScopedTypes)
            {
                var attribute = type.GetCustomAttribute<ScopedGenericServiceAttribute>();
                services.AddScoped(attribute.ServiceType, type);
            }
        }
    }
}
