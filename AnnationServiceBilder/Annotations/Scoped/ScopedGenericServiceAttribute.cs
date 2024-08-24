namespace AnnationServiceBilder.Annotations.Scoped
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ScopedGenericServiceAttribute : Attribute
    {
        public Type ServiceType { get; }

        public ScopedGenericServiceAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }
    }
}
