using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication8.Models;

namespace WebApplication8.Data
{
    /// <summary>
    /// This class is using for creating roles and seed database with several users.
    /// </summary>
    public class DummyData
    {
        public static async Task Initialise(ApplicationDbContext context,
                            UserManager<ApplicationUser> userManager,
                            RoleManager<ApplicationRole> roleManager)
        {
            context.Database.EnsureCreated();

            string role1 = "Admin";
            string desc1 = "Admin can do everything";

            string role2 = "Player";
            string desc2 = "Null";

            string role3 = "CommonUser";
            string desc3 = "Null";

            string password = "Password123!";

            if (await roleManager.FindByNameAsync(role1) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role1,desc1));
            }
            if (await roleManager.FindByNameAsync(role2) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role2,desc2));
            }
            if (await roleManager.FindByNameAsync(role3) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role3, desc3));
            }

            //Create user Memeber1
            if (await userManager.FindByNameAsync("Admin@Email.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "Admin@Email.com",
                    Email = "Admin@Email.com",
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role1);
                }
            }

            //Create user Customer1
            if (await userManager.FindByNameAsync("Player1@Email.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "Player1@Email.com",
                    Email = "Player1@Email.com",
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }

            //Create user Customer2
            if (await userManager.FindByNameAsync("User1@Email.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "User1@Email.com",
                    Email = "User1@Email.com",
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role3);
                }
            }
        }
    }
}