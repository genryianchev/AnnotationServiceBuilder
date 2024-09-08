using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AnnotationServiceBuilder.Annotations.Systems.Utilities
{
    public static class TypeListCache
    {
        private static readonly ConcurrentDictionary<Assembly, Type[]> _assemblyTypeListCache = new ConcurrentDictionary<Assembly, Type[]>();
        private static Type[] _types;

        /// <summary>
        /// Initializes the TypeListCache by caching the types from the provided assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan for types.</param>
        public static void Initialize(Assembly assembly)
        {
            _types = GetTypesWithAttributesFromAssembly(assembly);
        }

        /// <summary>
        /// Retrieves the cached types from the initialized assembly.
        /// </summary>
        /// <returns>An array of types in the cached assembly.</returns>
        public static Type[] GetTypes()
        {
            if (_types == null)
            {
                throw new InvalidOperationException("TypeListCache is not initialized. Call Initialize first.");
            }
            return _types;
        }

        /// <summary>
        /// Retrieves types marked with any of the attributes defined in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search for attributes and types.</param>
        /// <returns>An array of types that are marked with any of the attributes defined in the assembly.</returns>
        public static Type[] GetTypesWithAttributesFromAssembly(Assembly assembly)
        {
            var attributeTypes = AttributeFinder.attributeTypes;
            return GetTypesFromAssembly(assembly, attributeTypes);
        }

        /// <summary>
        /// Retrieves all types from the specified assembly, using a cache to avoid repeated reflection calls.
        /// Optionally filters the types using the provided list of attributes.
        /// </summary>
        /// <param name="assembly">The assembly to scan for types.</param>
        /// <param name="attributeTypes">Optional list of attribute types to filter by.</param>
        /// <returns>An array of types in the assembly.</returns>
        private static Type[] GetTypesFromAssembly(Assembly assembly, HashSet<Type> attributeTypes = null)
        {
            return _assemblyTypeListCache.GetOrAdd(assembly, asm =>
            {
                var types = asm.GetTypes();

                // If the attribute list is empty or null, return all types
                if (attributeTypes == null || attributeTypes.Count == 0)
                {
                    return types;
                }

                // If the attribute list is not empty, filter only the types that have the required attributes
                types = types.Where(t => attributeTypes.Any(attrType => t.IsDefined(attrType, false)))
                             .ToArray();

                return types;
            });
        }
    }
}
