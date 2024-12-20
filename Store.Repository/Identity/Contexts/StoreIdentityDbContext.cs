using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Store.Repository.Identity.Configurations.SeedConfigurations;

namespace Store.Repository.Identity.Contexts
{
    public class StoreIdentityDbContext :IdentityDbContext<AppUser,Role,string>
    {
        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options):base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RoleConfiguration());

            base.OnModelCreating(builder);
        }

    }
}
