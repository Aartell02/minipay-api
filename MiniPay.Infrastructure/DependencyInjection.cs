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

            // 1. Register the Distributed Cache Service
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Configuration"];
                options.InstanceName = configuration["Redis:InstanceName"];
            });
            
            services.AddScoped<IEventStore, EventStore>();
            services.AddScoped<ICacheService, CacheService>();
            return services;
        }
    }
}
