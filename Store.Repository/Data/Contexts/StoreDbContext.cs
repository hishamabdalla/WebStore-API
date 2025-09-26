using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Entities.Identity;
using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Data.Contexts
{
    public class StoreDbContext : IdentityDbContext<AppUser, Role, string>
    {

        public StoreDbContext(DbContextOptions<StoreDbContext> options):base(options) 
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> Brands { get; set; }
        public DbSet<ProductType> Types { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> Items { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
    }
}
