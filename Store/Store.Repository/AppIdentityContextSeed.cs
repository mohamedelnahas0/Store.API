using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.identityEntites;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class AppIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Ahmed Khaled",
                    Email = "ahmed@gmail.com",
                    UserName = "ahmedkhaled",
                    address = new Address
                    {

                        FirstName = "Ahmed",
                        LastName = "Khaled",
                        City = "Maadi",
                        State = "Cairo",
                        Street = "77",
                        ZipCode = "12345"
                    }
                    };

                await userManager.CreateAsync(user, "Passwoed123!");
            }
        }
    }
} 