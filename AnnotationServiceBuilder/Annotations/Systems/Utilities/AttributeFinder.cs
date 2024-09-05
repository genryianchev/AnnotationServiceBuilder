using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AnnotationServiceBuilder.Annotations.Systems.Utilities
{
    public static class AttributeFinder
    {
        /// <summary>
        /// Retrieves all attributes defined in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search for attributes.</param>
        /// <returns>A list of attribute types found in the assembly.</returns>
        public static List<Type> GetAttributesFromAssembly(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Attribute)) && !type.IsAbstract)
                .ToList();
        }

        /// <summary>
        /// Retrieves all attributes defined in the specified assembly and within the specified namespace.
        /// </summary>
        /// <param name="assembly">The assembly to search for attributes.</param>
        /// <param name="namespacePrefix">The namespace prefix to filter by (e.g., "AnnotationServiceBuilder").</param>
        /// <returns>A list of attribute types found in the specified namespace.</returns>
        public static HashSet<Type> GetAttributesFromAssembly(Assembly assembly, string namespacePrefix)
        {
            var types = assembly.GetTypes();

            var attributeTypes = new HashSet<Type>();

            foreach (var type in types)
            {
                if (type.IsSubclassOf(typeof(Attribute)) &&
                    !type.IsAbstract &&
                    type.Namespace != null &&
                    type.Namespace.StartsWith(namespacePrefix))
                {
                    attributeTypes.Add(type);
                }
            }

            return attributeTypes;
        }

    }
}
