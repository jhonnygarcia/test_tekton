using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application._Common.Behaviours
{
    public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public RequestPerformanceBehaviour(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var name = typeof(TRequest).Name;
            var elapsedMilliseconds = _timer.ElapsedMilliseconds;
            if (elapsedMilliseconds > 5000)
            {
                _logger.LogWarning($"Long Running Request: {name} ({elapsedMilliseconds} milliseconds) {request}");
            }

            return response;
        }
    }
}
