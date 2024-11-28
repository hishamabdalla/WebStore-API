using Microsoft.AspNetCore.Identity;
using Store.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Identity
{
    public class StoreIdentityDbContextSeed
    {
        public async static Task SeedAppUserAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            { 
                var user = new AppUser()
                {
                    Email = "hishvmabdalla@gmail.com",
                    DisplayName = "Hisham Abdalla",
                    UserName = "Hishvm",
                    PhoneNumber = "0100000000",
                    Address = new Address()
                    {
                        FName = "Hisham",
                        LName = "Abdalla",
                        City = "Mansoura",
                        Country = "Egypt",
                        Street = "123"



                    }
                };

            await _userManager.CreateAsync(user, "Hisham@123");
             }
         }
    }
}
