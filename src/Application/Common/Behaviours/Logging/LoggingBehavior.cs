using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours.Logging
{
    public class LoggingBehavior<TRequest, TResponse> :
           IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger logger;

        public LoggingBehavior(ILogger logger)
        {
            this.logger = logger;
        }

        public Task<TResponse> Handle(TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return next();
            }
            catch (Exception e)
            {
                logger.LogError(e, "");
                throw;
            }
        }
    }
}
