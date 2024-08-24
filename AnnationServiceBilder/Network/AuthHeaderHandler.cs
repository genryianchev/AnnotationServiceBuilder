using AnnationServiceBilder.Helpers;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AnnationServiceBilder.Network
{
    class AuthHeaderHandler : DelegatingHandler
    {
        private readonly SessionHelper SessionHelper;

        public AuthHeaderHandler(SessionHelper SessionHelper)
        {
            this.SessionHelper = SessionHelper;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                SessionHelper.LoggedIn = false;
            }
            return response;
        }
    }
}
