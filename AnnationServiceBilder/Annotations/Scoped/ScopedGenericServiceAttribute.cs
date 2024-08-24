namespace AnnationServiceBilder.Annotations.Scoped
{
    /// <summary>
    /// Specifies that a generic class should be registered with a scoped lifetime in the dependency injection container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ScopedGenericServiceAttribute : Attribute
    {
        /// <summary>
        /// Gets the service type that the generic class should be registered as.
        /// This type should be a closed generic type.
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopedGenericServiceAttribute"/> class.
        /// </summary>
        /// <param name="serviceType">
        /// The type of the service to register. This should be the closed generic type to be used
        /// for registration in the dependency injection container.
        /// </param>
        public ScopedGenericServiceAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }
    }
}
