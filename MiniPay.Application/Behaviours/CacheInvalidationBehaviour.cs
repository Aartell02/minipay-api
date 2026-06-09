using MediatR;
using MiniPay.Application.Interfaces;


namespace MiniPay.Application.Behaviours
{
    public class CacheInvalidationBehaviour<TRequest, TResponse>(ICacheService cache) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            var response = await next();

            if (request is ICacheInvalidation invalidationCommand)
            {
                await cache.RemoveAsync(invalidationCommand.CacheKey, ct);
            }

            return response;
        }
    }
}

