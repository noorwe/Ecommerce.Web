
using Ecommerce.Domain.Contracts;
using Ecommerce.Persistance;
using Ecommerce.Persistance.Data.DataSeed;
using Ecommerce.Persistance.Data.DbContexts;
using Ecommerce.Persistance.Repositories;
using Ecommerce.Service;
using Ecommerce.Service.MappingProfiles;
using Ecommerce.ServiceAbstractions;
using Ecommerce.Shared.ErrorModels;
using Ecommerce.Web.CustomMiddlewares;
using Ecommerce.Web.Extensions;
using Ecommerce.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ecommerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSwaggerServices();


            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddApplicationServices();

            builder.Services.AddWebAppServices();





            var app = builder.Build();

            await app.MigrateDbAsync();
            await app.SeedDbAsync();


            #region Configure the Http request pipeline with Custom Middleware

            app.UseCustomExceptionMiddleware();

            #endregion


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
