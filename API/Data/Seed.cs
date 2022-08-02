using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers( UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            //we return from this method if our Data context has any user data
            if(await userManager.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            //we want to Deserialize our json data into an Object of type AppUser
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            if(users == null) return;

            var roles = new List<AppRole> 
            {
                new AppRole{Name = "Member"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Moderator"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                // the userManager takes care of tracking and saving the chages to the databse
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member"); //we are assigning a role to the user directly in the db
            }

            // we are creating an admin user and the only difference for now is the username is admin
            var admin = new AppUser
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"}); //the admin has tow roles, so we used AddtoRoles method

        }
    }
}