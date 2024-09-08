using System;

namespace AnnotationServiceBuilder.Annotations.Patterns.CreationalDesignPatterns.Factory
{
    /// <summary>
    /// Defines a factory interface for creating instances of type <typeparamref name="T"/>.
    /// This interface will be removed in future versions and moved to another library (AnnotationServiceBuilder.Patterns).
    /// Please update your code accordingly.
    /// </summary>
    [Obsolete("This interface will be removed in future versions and moved to another library (AnnotationServiceBuilder.Patterns). Please update your code accordingly.", false)]
    public interface IFactory<T>
    {
        /// <summary>
        /// Creates an instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>A new instance of type <typeparamref name="T"/>.</returns>
        T Create();
    }
}
