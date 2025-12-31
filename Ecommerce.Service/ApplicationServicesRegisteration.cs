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
            services.AddScoped<IServiceManager, ServiceManagerWithFactoryDelegate>();
            
            //services.AddScoped<IProductServices, ProductService>();
            //services.AddAutoMapper(X => X.AddProfile<ProductProfile>());

            //services.AddScoped<IBasketService, BasketService>();
            services.AddAutoMapper((X) => { }, typeof(ServiceAssemblyReference).Assembly);



            services.AddScoped<IProductServices, ProductService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<Func<IProductServices>>(serviceProvider => () => serviceProvider.GetRequiredService<IProductServices>());

            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<Func<IBasketService>>(serviceProvider => () => serviceProvider.GetRequiredService<IBasketService>());

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<Func<IAuthenticationService>>(serviceProvider => () => serviceProvider.GetRequiredService<IAuthenticationService>());

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<Func<IOrderService>>(serviceProvider => () => serviceProvider.GetRequiredService<IOrderService>());

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<Func<IPaymentService>>(serviceProvider => () => serviceProvider.GetRequiredService<IPaymentService>());

            return services;

        }
    }
}
