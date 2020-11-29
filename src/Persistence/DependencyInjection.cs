using Application.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            return services;
        }
    }
}