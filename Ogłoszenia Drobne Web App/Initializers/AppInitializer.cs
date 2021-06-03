using Microsoft.AspNetCore.Identity;
using Ogłoszenia_Drobne_Web_App.Data;
using Ogłoszenia_Drobne_Web_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogłoszenia_Drobne_Web_App.Initializers
{
    public static class AppInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!context.Database.EnsureCreated()) return;

            var role = new IdentityRole();
            role.Name = "Admin";

            await roleManager.CreateAsync(role);

            var user = new AppUser()
            {
                Email = "admin@admin.pl",
                UserName = "admin@admin.pl",
                Name = "Admin",
                Surname = "Admin",
                EmailConfirmed = true
            };

            string pass = "Admin1234%";

            IdentityResult checkUser = await userManager.CreateAsync(user, pass);

            if(checkUser.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }

            var secondUser = new AppUser()
            {
                Email = "jan@kowalski.pl",
                UserName = "jan@kowalski.pl",
                Name = "Jan",
                Surname = "Kowalski",
                EmailConfirmed = true
            };

            pass = "Haslo1234%";

            await userManager.CreateAsync(secondUser, pass);
        }
    }
}
