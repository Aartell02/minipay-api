using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniPay.Application.Interfaces;
using MiniPay.Infrastructure.Cache;
using MiniPay.Infrastructure.Events;

namespace MiniPay.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EventStoreDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Database")));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });
            
            services.AddScoped<IEventStore, EventStore>();
            services.AddScoped<ICacheService, CacheService>();
            return services;
        }
    }
}
