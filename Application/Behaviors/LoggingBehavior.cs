using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace Application.Behaviors
{
    internal class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) 
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            using (_logger.BeginScope(new Dictionary<string, object?>
            {
                ["RequestName"] = requestName,
                ["RequestType"] = typeof(TRequest).FullName
            }))
            {
                _logger.LogInformation("Handling request with content {@Request}", request);

                var stopwatch = Stopwatch.StartNew();
                var response = await next(cancellationToken);
                stopwatch.Stop();

                _logger.LogInformation("Handled request in {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);

                return response;
            }
        }
    }
}
