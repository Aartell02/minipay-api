using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MiniPay.Application.Behaviours;

namespace MiniPay.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(DependencyInjection).Assembly);

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CacheInvalidationBehaviour<,>));

            return services;
        }
    }
}
