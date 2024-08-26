using System;

namespace AnnotationServiceBuilder.Annotations.Singleton
{
    /// <summary>
    /// Specifies that a class should be registered with a singleton lifetime in the dependency injection container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class SingletonServiceAttribute : Attribute
    {
    }
}

