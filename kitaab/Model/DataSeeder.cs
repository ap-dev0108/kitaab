using System.Net.Mail;
using Microsoft.AspNetCore.Identity;

namespace kitaab.Model;

public static class DataSeeder
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "Admin", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public static async Task SeedAdmin(UserManager<User> userManager)
    {
        const string mail = "admin@gmail.com";
        const string password = "demoAdmin@123";

        var existingAdmin = await userManager.FindByEmailAsync(mail);

        if (existingAdmin == null)
        {
            var admin = new User
            {
                UserName = "Aryan",
                Email = mail,
                PasswordHash = password,
                fullName = "Aryan",
                EmailConfirmed = true,
                isActive = true,
            };

            var result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}