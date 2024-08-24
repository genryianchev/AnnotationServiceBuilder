using System;

namespace AnnationServiceBilder.Data.Transient_Services
{
    /// <summary>
    /// Specifies that a class should be registered with a transient lifetime in the dependency injection container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TransientServiceAttribute : Attribute
    {
    }
}
