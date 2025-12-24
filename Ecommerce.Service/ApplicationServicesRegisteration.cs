using Ecommerce.Service.MappingProfiles;
using Ecommerce.ServiceAbstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public static class ApplicationServicesRegisteration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductServices, ProductService>();
            services.AddAutoMapper(X => X.AddProfile<ProductProfile>());

            services.AddScoped<IBasketService, BasketService>();
            services.AddAutoMapper(X => X.AddProfile<BasketProfile>());

            return services;

        }
    }
}
