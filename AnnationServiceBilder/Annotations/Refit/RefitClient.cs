namespace AnnationServiceBilder.Annotations.Refit
{

    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class RefitClientAttribute : Attribute
    {
        public string BaseUrl { get; }

        public RefitClientAttribute(string baseUrl = null)
        {
            BaseUrl = baseUrl;
        }
    }

}
