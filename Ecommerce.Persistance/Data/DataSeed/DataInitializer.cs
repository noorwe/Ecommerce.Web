using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.IdentityModule;
using Ecommerce.Domain.Entities.ProductModule;
using Ecommerce.Domain.OrderModule;
using Ecommerce.Persistance.Data.DbContexts;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataInitializer(StoreDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }



        public async Task InitializeAsync()
        {
            try
            {
                var HasProducts = await _dbContext.Products.AnyAsync();
                var HasBrands = await _dbContext.ProductBrands.AnyAsync();
                var HasTypes = await _dbContext.ProductTypes.AnyAsync();
                var HasDeliveryMethods = await _dbContext.Set<DeliveryMethod>().AnyAsync();

                if (HasProducts && HasBrands && HasTypes && HasDeliveryMethods)
                    return;

                if (!HasBrands)
                {
                    await SeedDataFromJsonAsync<ProductBrand, int>(
                        "brands.json",
                        _dbContext.ProductBrands
                    );
                }

                if (!HasTypes)
                {
                    await SeedDataFromJsonAsync<ProductType, int>(
                        "types.json",
                        _dbContext.ProductTypes
                    );
                }

                if (!HasDeliveryMethods)
                {
                    await SeedDataFromJsonAsync<DeliveryMethod, int>(
                        "DeliveryMethodsSeed.json",
                        _dbContext.Set<DeliveryMethod>()
                    );
                }

                _dbContext.SaveChanges();

                if (!HasProducts)
                {
                    await SeedDataFromJsonAsync<Product, int>(
                        "products.json",
                        _dbContext.Products
                    );
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


        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    var roles = new List<IdentityRole>
                {
                    new IdentityRole("Admin"),
                    new IdentityRole("SuperAdmin")
                };
                    foreach (var role in roles)
                    {
                        await _roleManager.CreateAsync(role);
                    }
                }
                if (!_userManager.Users.Any())
                {
                    var admin = new ApplicationUser()
                    {
                        DisplayName = "Nour",
                        Email = "Nour@gmail.com",
                        PhoneNumber = "01012345678",
                        UserName = "Nour",

                    };
                    var superAdmin = new ApplicationUser()
                    {
                        DisplayName = "Ahmed",
                        Email = "Ahmed@gmail.com",
                        PhoneNumber = "01087654321",
                        UserName = "Ahmed",
                    };
                    await _userManager.CreateAsync(admin, "P@ssw0rd");
                    await _userManager.CreateAsync(superAdmin, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(admin, "Admin");
                    await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during identity data initialization: {ex.Message}");

            }
        }
    }    
}
