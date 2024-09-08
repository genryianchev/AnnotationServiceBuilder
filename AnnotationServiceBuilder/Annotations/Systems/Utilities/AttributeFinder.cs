using AnnotationServiceBuilder.Annotations.Patterns.CreationalDesignPatterns.Factory;
using AnnotationServiceBuilder.Annotations.Refit;
using AnnotationServiceBuilder.Annotations.Scoped;
using AnnotationServiceBuilder.Annotations.Singleton;
using AnnotationServiceBuilder.Annotations.Transient;
using System;
using System.Collections.Generic;

namespace AnnotationServiceBuilder.Annotations.Systems.Utilities
{
    public static class AttributeFinder
    {
        /// <summary>
        /// A predefined set of attribute types that are used for filtering types within assemblies.
        /// These attributes are commonly used to mark services or components for Dependency Injection (DI) 
        /// or other pattern-related functionalities.
        /// </summary>
        public static readonly HashSet<Type> attributeTypes = new HashSet<Type>
        {
            /// <summary>
            /// Attribute to mark generic services for scoped lifetime registration.
            /// </summary>
            typeof(ScopedGenericServiceAttribute),

            /// <summary>
            /// Attribute to mark services for singleton lifetime registration.
            /// </summary>
            typeof(SingletonServiceAttribute),

            /// <summary>
            /// Attribute to mark services for scoped lifetime registration.
            /// </summary>
            typeof(ScopedServiceAttribute),

            /// <summary>
            /// Attribute to define Refit clients for API communication.
            /// </summary>
            typeof(RefitClientAttribute),

            /// <summary>
            /// Attribute to mark services for transient lifetime registration.
            /// </summary>
            typeof(TransientServiceAttribute),

            /// <summary>
            /// Attribute used to implement the Factory design pattern in DI containers.
            /// </summary>
            typeof(FactoryPatternAttribute)
        };
    }
}
