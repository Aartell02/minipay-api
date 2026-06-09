using MediatR;
using MiniPay.Application.Interfaces;

namespace MiniPay.Application.Behaviours
{
    public class CachingBehaviour<TRequest, TResponse>(ICacheService cacheService) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is ICacheableQuery cacheableQuery)
            {

                if (cacheableQuery.BypassCache) return await next();

                var cachedResponse = await cacheService.GetAsync<TResponse>(cacheableQuery.CacheKey, cancellationToken);
                if (cachedResponse != null)
                {
                    return cachedResponse;
                }

                TResponse response = await next();
                await cacheService.SetAsync(cacheableQuery.CacheKey, response, cancellationToken);
                return response;
            }
            return await next();
        }
    }
}
