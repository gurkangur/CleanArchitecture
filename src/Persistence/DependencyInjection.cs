using System;
using Application.Data.Interfaces;
using Domain.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Persistence.Caching;
using Persistence.Repositories;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<DbContextBase>(options =>
            {
                options.UseInMemoryDatabase("Test");
            });
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddMemoryCache();
            return services;
        }

        public static IServiceCollection AddMemoryCache(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IDistributedCache, MemoryDistributedCache>());

            RegisterCacheService(services);

            return services;
        }
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "connectionString";
            });

            RegisterCacheService(services);

            return services;
        }
        private static void RegisterCacheService(IServiceCollection services)
        {
            services.TryAdd(ServiceDescriptor.Transient<ICacheService, CacheService>());
        }
    }
}