using Domain.Caching;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours.Caching
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICacheService _cacheService;

        public CachingBehavior(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {

            //https://github.com/SorenZ/Alamut.MediatR.Caching/blob/master/src/Alamut.MediatR.Caching/CachingBehavior.cs
            //https://anderly.com/2019/12/12/cross-cutting-concerns-with-mediatr-pipeline-behaviors/

            if (request is ICacheable cacheable)
            {
                if (cacheable.Key == null)
                { throw new ArgumentNullException(nameof(cacheable.Key)); }

                var response = await _cacheService.Get(cacheable.Key);

                if (!string.IsNullOrEmpty(response))
                {
                    return await _cacheService.Get<TResponse>(cacheable.Key);
                }
                else
                {
                    var value = await next();
                    await _cacheService.Set(cacheable.Key, value, cacheable.AbsoluteExpirationRelativeToNow);
                    return value;
                }

            }

            return await next();
        }
    }
}
