using AnnationServiceBilder.Annotations.Refit;
using AnnationServiceBilder.Annotations.Scoped;
using AnnationServiceBilder.Annotations.Singleton;
using AnnationServiceBilder.Data.Transient_Services;
using Refit;
using System.Reflection;

namespace AnnationServiceBilder.Annotations
{
    public static class ServiceCollectionExtensions
    {
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

        public static void AddAnnotatedSingletonServices(this IServiceCollection services, Assembly assembly)
        {
            var singletonTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<SingletonServiceAttribute>() != null && t.IsClass && !t.IsAbstract);

            foreach (var type in singletonTypes)
            {
                services.AddSingleton(type);
            }
        }

        public static void AddAnnotatedTransientServices(this IServiceCollection services, Assembly assembly)
        {
            var transientTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<TransientServiceAttribute>() != null && t.IsClass && !t.IsAbstract);

            foreach (var type in transientTypes)
            {
                services.AddTransient(type);
            }
        }

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
