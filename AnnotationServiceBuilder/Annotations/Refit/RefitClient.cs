using System;

namespace AnnotationServiceBuilder.Annotations.Refit
{
    /// <summary>
    /// Specifies that the interface is a Refit client.
    /// This attribute is used to mark interfaces that should be registered as Refit clients,
    /// enabling easy integration with HTTP APIs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class RefitClientAttribute : Attribute
    {
        /// <summary>
        /// Gets the base URL for the Refit client.
        /// This URL will be used as the base address for all requests made by the Refit client.
        /// </summary>
        public string BaseUrl { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefitClientAttribute"/> class.
        /// </summary>
        /// <param name="baseUrl">
        /// The base URL for the Refit client. This URL will be used as the base address for all requests.
        /// If not provided, the default base URL should be configured in the DI container.
        /// </param>
        public RefitClientAttribute(string baseUrl = null)
        {
            BaseUrl = baseUrl;
        }
    }
}
