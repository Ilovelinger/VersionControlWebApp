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
            string desc1 = "Admin can create ,view,delete posts,and view comments";

            string role2 = "Customer";
            string desc2 = "Cutomer can view posts and add comments";

            string password = "Password123!";

            if (await roleManager.FindByNameAsync(role1) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role1,desc1));
            }
            if (await roleManager.FindByNameAsync(role2) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role2,desc2));
            }

            //Create user Memeber1
            if (await userManager.FindByNameAsync("Member1@Email.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "Member1@Email.com",
                    Email = "Member1@Email.com",
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role1);
                }
            }

            //Create user Customer1
            if (await userManager.FindByNameAsync("Customer1@Email.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "Customer1@Email.com",
                    Email = "Customer1@Email.com",
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }

            //Create user Customer2
            if (await userManager.FindByNameAsync("Customer2@Email.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "Customer2@Email.com",
                    Email = "Customer2@Email.com",
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }

            //Create user Customer3
            if (await userManager.FindByNameAsync("Customer3@Email.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "Customer3@Email.com",
                    Email = "Customer3@Email.com",
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }

            //Create user Customer4
            if (await userManager.FindByNameAsync("Customer4@Email.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "Customer4@Email.com",
                    Email = "Customer4@Email.com",
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }

            //Create user Customer5
            if (await userManager.FindByNameAsync("Customer5@Email.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "Customer5@Email.com",
                    Email = "Customer5@Email.com",
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }
        }
    }
}