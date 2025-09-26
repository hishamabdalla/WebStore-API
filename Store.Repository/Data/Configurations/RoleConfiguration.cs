using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Id = "d4e8fae0-dcf2-4a8c-bb69-882df1c72f60",
                    Name = "Customer",
                    NormalizedName = "CUSTOMER",
                    Description = "The Customer role for the user"
                }, new Role
                {
                    Id = "7a5f64e1-3c9d-4c37-9b8c-2b2aaf7fcb20",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "The Admin role for the user"
                });

        }
    }
}
