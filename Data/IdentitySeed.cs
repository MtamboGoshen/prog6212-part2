// FILE: Data/IdentitySeed.cs
using ContractMonthlyClaim.Models;
using Microsoft.AspNetCore.Identity;

namespace ContractMonthlyClaim.Data
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Seed Roles
            await SeedRoleAsync(roleManager, "Manager");
            await SeedRoleAsync(roleManager, "Programme Coordinator"); // <-- NEW ROLE
            await SeedRoleAsync(roleManager, "Lecturer");

            // Seed Users
            await SeedUserAsync(userManager, "manager@test.com", "Password123", "Manager");
            await SeedUserAsync(userManager, "coordinator@test.com", "Password123", "Programme Coordinator"); // <-- NEW USER
            await SeedUserAsync(userManager, "lecturer@test.com", "Password123", "Lecturer");
        }

        // Helper methods (no changes needed below)
        private static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, string userName, string password, string role)
        {
            if (await userManager.FindByNameAsync(userName) == null)
            {
                var user = new ApplicationUser { UserName = userName, Email = userName, EmailConfirmed = true };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}