using Microsoft.AspNetCore.Identity;

namespace AMS.Service
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = { "Admin", "Accountant", "Viewer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var user = await userManager.FindByEmailAsync("admin@account.com");

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = "admin@account.com",
                    Email = "admin@account.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, "Admin");
            }

            var accountantEmail = "accountant@example.com";
            if (await userManager.FindByEmailAsync(accountantEmail) == null) 
            {
                var Auser = new IdentityUser { UserName = accountantEmail, Email = accountantEmail };
                await userManager.CreateAsync(Auser, "Accountant@123");
                await userManager.AddToRoleAsync(Auser, "Accountant");
            }


            var normalUserEmail = "user@example.com";
            if (await userManager.FindByEmailAsync(normalUserEmail) == null)
            {
                var Nuser = new IdentityUser { UserName = normalUserEmail, Email = normalUserEmail };
                await userManager.CreateAsync(Nuser, "User@123");
                await userManager.AddToRoleAsync(Nuser, "Viewer");
            }
        }

    }
}
