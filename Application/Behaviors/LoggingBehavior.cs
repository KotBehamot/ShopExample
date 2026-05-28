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
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            using (logger.BeginScope(new Dictionary<string, object?>
            {
                ["RequestName"] = requestName,
                ["RequestType"] = typeof(TRequest).FullName
            }))
            {
                logger.LogInformation("Handling request with content {@Request}", request);

                var stopwatch = Stopwatch.StartNew();
                try
                {
                    var response = await next(cancellationToken);
                    stopwatch.Stop();

                    logger.LogInformation("Handled request in {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);

                    return response;
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    logger.LogError(ex, "Request failed after {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }
}
