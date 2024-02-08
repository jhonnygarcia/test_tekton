using ITSystems.Framework.IComponents.Security;
using Microsoft.Extensions.Logging;

namespace Infraestructure.ServicesClients.DiscountsProduct
{
    public class DiscountHttpMessageHandler : DelegatingHandler
    {
        private ILogger<DiscountHttpMessageHandler> _logger;
        private readonly IIdentitySessionService _sessionService;
        public DiscountHttpMessageHandler(ILogger<DiscountHttpMessageHandler> logger,
            IIdentitySessionService sessionService)
        {
            _logger = logger;
            _sessionService = sessionService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = _sessionService.GetAccessTokenFromRequest();
            if (accessToken != null)
                request.Headers.Add("Authorization", $"bearer {accessToken}");

            var response = await base.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var contentString = await response.Content.ReadAsStringAsync();
                if (request.Method == HttpMethod.Get || request.Method == HttpMethod.Delete)
                {
                    _logger.LogWarning($"{request.Method}: {request.RequestUri}, response: {contentString}");
                }
                else
                {
                    var payload = request.Content.ReadAsStringAsync();
                    _logger.LogWarning($"{request.Method}: {request.RequestUri}, " +
                        $"payload: {payload}, response: {contentString}");
                }
            }
            return response;
        }
    }

}
