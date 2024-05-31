namespace SportData.Data.Seeders.SportDataDb;

using System;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using SportData.Common.Constants;
using SportData.Data.Models.Entities.SportData;
using SportData.Data.Seeders.Interfaces;

public class RolesSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetService<RoleManager<Role>>();

        await SeedRoleAsync(roleManager, Roles.SUPERADMIN);
        await SeedRoleAsync(roleManager, Roles.ADMIN);
        await SeedRoleAsync(roleManager, Roles.EDITOR);
        await SeedRoleAsync(roleManager, Roles.USER);
    }

    private async Task SeedRoleAsync(RoleManager<Role> roleManager, string roleName)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            var result = await roleManager.CreateAsync(new Role { Name = roleName, CreatedOn = DateTime.UtcNow });
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }
        }
    }
}