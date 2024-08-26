using AnnotationServiceBuilder.Annotations.Singleton;

namespace AnnotationServiceBuilder.Helpers
{
    [SingletonService]
    public class SessionHelper
    {
        public bool LoggedIn { get; set; } = true;
    }
}
