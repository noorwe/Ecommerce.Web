
using Ecommerce.Domain.Contracts;
using Ecommerce.Persistance.Data.DataSeed;
using Ecommerce.Persistance.Data.DbContexts;
using Ecommerce.Persistance.Repositories;
using Ecommerce.Service;
using Ecommerce.Service.MappingProfiles;
using Ecommerce.ServiceAbstractions;
using Ecommerce.Web.Extensions;
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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IDataInitializer, DataInitializer>();

            builder.Services.AddAutoMapper(X => X.AddProfile<ProductProfile>());

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductServices, ProductService>();


            var app = builder.Build();

            await app.MigrateDbAsync();
            await app.SeedDbAsync();



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
