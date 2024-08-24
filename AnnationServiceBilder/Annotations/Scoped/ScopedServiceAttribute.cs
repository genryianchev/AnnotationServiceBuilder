namespace AnnationServiceBilder.Annotations.Scoped
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ScopedServiceAttribute : Attribute
    {
        public Type ServiceType { get; }

        public ScopedServiceAttribute(Type serviceType = null)
        {
            ServiceType = serviceType;
        }
    }
}
