using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.IdentityModule;
using Ecommerce.Persistance.Data.DataSeed;
using Ecommerce.Persistance.Data.DbContexts;
using Ecommerce.Persistance.Data.Identity;
using Ecommerce.Persistance.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistance
{
    public static class InfrastructureServicesRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();

            services.AddScoped<IDataInitializer, DataInitializer>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddScoped<ICacheRepository, CacheRepository>();


            services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnectionString"));
            });

            return services;
        }
    }
}
