using AnnationServiceBilder.Annotations.Singleton;

namespace AnnationServiceBilder.Helpers
{
    [SingletonService]
    public class SessionHelper
    {
        public bool LoggedIn { get; set; } = true;
    }
}
