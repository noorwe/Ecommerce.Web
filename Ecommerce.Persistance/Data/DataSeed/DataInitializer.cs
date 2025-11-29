using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.ProductModule;
using Ecommerce.Persistance.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Persistance.Data.DataSeed
{
    public class DataInitializer : IDataInitializer
    {
        private readonly StoreDbContext _dbContext;

        public DataInitializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            try
            {
                var HasProducts = await _dbContext.Products.AnyAsync();
                var HasBrands = await _dbContext.ProductBrands.AnyAsync();
                var HasTypes = await _dbContext.ProductTypes.AnyAsync();
                if (HasProducts && HasBrands && HasTypes) return;


                if (!HasBrands)
                {
                    await SeedDataFromJsonAsync<ProductBrand, int>("brands.json", _dbContext.ProductBrands);
                }

                if (!HasTypes)
                {
                    await SeedDataFromJsonAsync<ProductType, int>("types.json", _dbContext.ProductTypes);
                }
                _dbContext.SaveChanges();

                if (!HasProducts)
                {
                    await SeedDataFromJsonAsync<Product, int>("products.json", _dbContext.Products);
                }
                _dbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during data initialization: {ex.Message}");

            }
        }


        private async Task SeedDataFromJsonAsync<T, TKey>(string fileName, DbSet<T> dbSet) where T : BaseEntity<TKey>
        {
            // D:\Asp.Net Core Web Api App\Ecommerce App\Ecommerce.Persistance\Data\DataSeed\JsonFiles\
            var filePath = @"..\Ecommerce.Persistance\Data\DataSeed\JsonFiles\" + fileName;

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Data seed file not found", filePath);

            try
            {
                using var DataStream = File.OpenRead(filePath);

                var Data = JsonSerializer.Deserialize<List<T>>(DataStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (Data != null && Data.Any())
                {
                    await dbSet.AddRangeAsync(Data);

                }


            }
            catch (Exception)
            {
                Console.WriteLine($"Error occured while seeding data from file: {fileName}");
            }
        }
    }
}
