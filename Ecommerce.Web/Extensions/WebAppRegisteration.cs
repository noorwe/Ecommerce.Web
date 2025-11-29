using Ecommerce.Domain.Contracts;
using Ecommerce.Persistance.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Web.Extensions
{
    public static class WebAppRegisteration
    {
        public static async Task<WebApplication> MigrateDbAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            
            var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            var PendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (PendingMigrations.Any())
            {
                await dbContext.Database.MigrateAsync();
            }

            return app;

        }

        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
            
            await using var scope = app.Services.CreateAsyncScope();

            var dataInitializer = scope.ServiceProvider.GetRequiredService<IDataInitializer>();
            await dataInitializer.InitializeAsync();
            
            return app;
        }
    }
}
