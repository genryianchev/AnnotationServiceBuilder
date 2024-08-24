using System;
namespace AnnationServiceBilder.Annotations.Scoped
{
    /// <summary>
    /// Specifies that a class should be registered with a scoped lifetime in the dependency injection container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ScopedServiceAttribute : Attribute
    {
        /// <summary>
        /// Gets the service type that the class should be registered as.
        /// If not specified, the class itself will be registered as the service type.
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopedServiceAttribute"/> class.
        /// </summary>
        /// <param name="serviceType">
        /// The type of the service to register. If null, the class itself is used as the service type.
        /// </param>
        public ScopedServiceAttribute(Type serviceType = null)
        {
            ServiceType = serviceType;
        }
    }
}
