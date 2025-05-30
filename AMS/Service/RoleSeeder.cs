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
        }

    }
}
