﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {

        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
             if(!userManager.Users.Any() )
            {
                var user = new AppUser()
                {
                    DisplayName = "mohamed abuelmgd",
                    Email = "mohamedabuelmgd@gmail.com",
                    UserName = "mohamed.abuelmgd",
                    PhoneNumber = "01159304056"
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
