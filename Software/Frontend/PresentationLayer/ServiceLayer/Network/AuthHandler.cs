using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ServiceLayer.Services;

namespace ServiceLayer.Network
{
    public class AuthHandler : DelegatingHandler
    {
        private readonly TokenManager _tokenManager;

        public AuthHandler(TokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _tokenManager.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
