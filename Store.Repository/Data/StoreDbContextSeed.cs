using Microsoft.AspNetCore.Identity;
using Store.Core.Entities;
using Store.Core.Entities.Identity;
using Store.Core.Entities.Order;
using Store.Repository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Address = Store.Core.Entities.Identity.Address;

namespace Store.Repository.Data
{
    public static class StoreDbContextSeed
    {
        public async static Task SeedAsync(StoreDbContext _context, UserManager<AppUser> _userManager)
        {
            if (_context.Types.Count() == 0)
            {
                //1. Read Data From Json File
                var typesData = File.ReadAllText(@"..\Store.Repository\Data\DataSeed\types.json");

                //2. Convert Json String to List<T>
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                //3. Seed Data To DB
                if (types is not null && types.Count() > 0)
                {
                    await _context.Types.AddRangeAsync(types);
                    await _context.SaveChangesAsync();
                }

            }

            if (_context.Brands.Count() == 0)
            {
                //1. Read Data From Json File
                var brandsData = File.ReadAllText(@"..\Store.Repository\Data\DataSeed\brands.json");

                //2. Convert Json String to List<T>
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                //3. Seed Data To DB
                if (brands is not null && brands.Count() > 0)
                {
                    await _context.Brands.AddRangeAsync(brands);
                    await _context.SaveChangesAsync();
                }

            }

            if (_context.Products.Count() == 0)
            {
                //1. Read Data From Json File
                var productsData = File.ReadAllText(@"..\Store.Repository\Data\DataSeed\products.json");

                //2. Convert Json String to List<T>
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                //3. Seed Data To DB
                if (products is not null && products.Count() > 0)
                {
                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                }

            }

            if (_context.DeliveryMethods.Count() == 0)
            {
                //1. Read Data From Json File
                var deliveryData = File.ReadAllText(@"..\Store.Repository\Data\DataSeed\delivery.json");

                //2. Convert Json String to List<T>
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

                //3. Seed Data To DB
                if (deliveryMethods is not null && deliveryMethods.Count() > 0)
                {
                    await _context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                    await _context.SaveChangesAsync();
                }

            }

            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    Email = "Admin@gmail.com",
                    DisplayName = "Admin",
                    UserName = "Admin",
                    PhoneNumber = "0100000000",

                    Address = new Address()
                    {
                        FName = "Admin",
                        LName = "Admin",
                        City = "Mansoura",
                        Country = "Egypt",
                        Street = "123"
                    }
                   ,
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(user, "Admin@123");
                await _userManager.AddToRoleAsync(user, "Admin");

            }
        }
    }
}
