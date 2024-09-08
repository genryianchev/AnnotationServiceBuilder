using Microsoft.Extensions.DependencyInjection;
using System;

namespace AnnotationServiceBuilder.Annotations.Patterns.CreationalDesignPatterns.Factory
{
    /// <summary>
    /// Attribute used to mark a class as a factory that follows the Factory Pattern.
    /// The class should be registered in the dependency injection container with a specific lifetime.
    /// </summary>
    [Obsolete("This class will be removed in future versions and moved to another library (AnnotationServiceBuilder.Patterns). Please update your code accordingly.", false)]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class FactoryPatternAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of the factory interface that the class implements.
        /// The interface must have a method that returns an object.
        /// </summary>
        public Type ExpectedFactoryType { get; }

        /// <summary>
        /// Gets the lifetime of the service when registered in the dependency injection container.
        /// Defaults to <see cref="ServiceLifetime.Singleton"/> if not specified.
        /// </summary>
        public ServiceLifetime Lifetime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryPatternAttribute"/> class.
        /// </summary>
        /// <param name="expectedFactoryType">The type of the factory interface that the class implements.</param>
        /// <param name="lifetime">The lifetime of the service when registered in the DI container. Defaults to <see cref="ServiceLifetime.Singleton"/>.</param>
        public FactoryPatternAttribute(Type expectedFactoryType, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            ExpectedFactoryType = expectedFactoryType;
            Lifetime = lifetime;
        }
    }
}
