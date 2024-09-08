using AnnotationServiceBuilder.Annotations.Patterns.CreationalDesignPatterns.Factory;
using AnnotationServiceBuilder.Annotations.Systems.Helpers;
using AnnotationServiceBuilder.Annotations.Systems.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace AnnotationServiceBuilder.Annotations
{
    /// <summary>
    /// Provides methods to register factory services in the DI container following the Factory Pattern.
    /// This class will be removed in future versions and moved to another library (AnnotationServiceBuilder.Patterns).
    /// Please update your code accordingly.
    /// </summary>
    [Obsolete("This class will be removed in future versions and moved to another library (AnnotationServiceBuilder.Patterns). Please update your code accordingly.", false)]
    public static class AnnotationPatternRegistrar
    {
        /// <summary>
        /// Initializes the static class by caching the types from the specified assembly.
        /// This method should be called once to prepare the type cache for further operations.
        /// </summary>
        /// <param name="assembly">The assembly to scan for service types.</param>
        public static void Initialize(Assembly assembly)
        {
            TypeListCache.Initialize(assembly);
        }

        /// <summary>
        /// Registers all factory services marked with the <see cref="FactoryPatternAttribute"/> in the DI container.
        /// Uses the default service lifetime specified in the attribute or the provided default.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the factory services will be added.</param>
        public static void AddFactoryPatternServices(this IServiceCollection services)
        {
            RegisterFactoryServices(services);
        }

        /// <summary>
        /// Registers all factory services marked with the <see cref="FactoryPatternAttribute"/> in the DI container.
        /// This method is designed to be safe for environments with trimming and AOT (Ahead Of Time) optimizations.
        /// Uses the default service lifetime specified in the attribute or the provided default.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the factory services will be added.</param>
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(FactoryPatternAttribute))]
        public static void AddFactoryPatternServicesWithTrimmingSafety(this IServiceCollection services)
        {
            RegisterFactoryServices(services);
        }

        /// <summary>
        /// Registers factory services marked with the <see cref="FactoryPatternAttribute"/> in the DI container.
        /// Validates that each factory class matches the expected type and adds it to the container with the specified service lifetime.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the factory services will be added.</param>
        /// <param name="lifetime">The default <see cref="ServiceLifetime"/> to use if not specified in the attribute.</param>
        private static void RegisterFactoryServices(IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            var types = TypeListCache.GetTypes();

            // Retrieve types marked with the FactoryPatternAttribute
            var factoryTypes = types
                .Where(type => type.GetCustomAttribute<FactoryPatternAttribute>() != null)
                .ToList();

            foreach (var type in factoryTypes)
            {
                var attribute = type.GetCustomAttribute<FactoryPatternAttribute>();

                // Ensure the implementation class has a method that returns an object
                if (!ReflectionHelpers.HasMethodReturningObjectGetMethods(type))
                {
                    throw new InvalidOperationException(
                        $"The factory implementation class {type.Name} does not have a method that returns an object."
                    );
                }

                // Use the lifetime from the attribute or fall back to the method parameter
                var serviceLifetime = attribute.Lifetime != ServiceLifetime.Singleton
                    ? attribute.Lifetime
                    : lifetime;

                // Register the factory in the DI container based on the specified lifetime
                RegisterService(services, attribute.ExpectedFactoryType, type, serviceLifetime);

                Console.WriteLine($"Registered factory service: {type.Name} as {attribute.ExpectedFactoryType.Name} with lifetime {serviceLifetime}");
            }
        }

        /// <summary>
        /// Registers a service in the DI container based on the specified service lifetime.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
        /// <param name="factoryType">The expected factory type.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <param name="serviceLifetime">The service lifetime.</param>
        private static void RegisterService(IServiceCollection services, Type factoryType, Type implementationType, ServiceLifetime serviceLifetime)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(factoryType, implementationType);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped(factoryType, implementationType);
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient(factoryType, implementationType);
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported service lifetime: {serviceLifetime}");
            }
        }
    }
}
