using System;
using System.Collections.Generic;
using System.Linq;

namespace AnnotationServiceBuilder.Annotations.Systems.Helpers
{
    public static class ReflectionHelpers
    {
        /// <summary>
        /// Determines whether the specified factory type has a method that returns a non-void type and has no parameters.
        /// </summary>
        /// <param name="factoryType">The factory type to inspect.</param>
        /// <returns>True if a method with a non-void return type and no parameters exists; otherwise, false.</returns>
        public static bool HasCreateMethodWithReturnType(Type factoryType)
        {
            var methods = factoryType.GetMethods();
            return methods.Any(m => m.ReturnType != typeof(void) && m.ReturnType.IsClass && m.GetParameters().Length == 0);
        }

        /// <summary>
        /// Determines whether the specified factory type has a method that returns a non-void type and has parameters.
        /// </summary>
        /// <param name="factoryType">The factory type to inspect.</param>
        /// <returns>True if a method with a non-void return type and parameters exists; otherwise, false.</returns>
        public static bool HasCreateMethodWithParameters(Type factoryType)
        {
            var methods = factoryType.GetMethods();
            return methods.Any(m => m.ReturnType != typeof(void) && m.ReturnType.IsClass && m.GetParameters().Any());
        }

        /// <summary>
        /// Checks if the specified type contains any methods that return an <see cref="object"/>.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>Returns <c>true</c> if the type contains methods that return an <see cref="object"/>; otherwise, <c>false</c>.</returns>
        public static bool HasMethodReturningObject(Type type)
        {
            return type.GetMethods()
                       .Any(method => method.ReturnType == typeof(object));
        }

        public static bool HasMethodReturningObjectGetMethods(Type type)
        {
            if (type == null)
                return false;

            // Define the method names to exclude
            var excludedMethodNames = new HashSet<string>
    {
        "Equals", "GetHashCode", "GetType", "ToString", "Finalize", "MemberwiseClone"
    };

            // Get methods and filter based on return type and exclusions
            return type.GetMethods()
                .Where(method => method.ReturnType != typeof(void))
                .Any(method => !excludedMethodNames.Contains(method.Name) &&
                               method.ReturnType.IsClass &&
                               method.ReturnType != typeof(object) &&
                               !method.ReturnType.IsPrimitive &&
                               !method.ReturnType.IsEnum);
        }

    }
}
