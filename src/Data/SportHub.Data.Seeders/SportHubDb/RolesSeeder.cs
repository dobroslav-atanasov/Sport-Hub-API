﻿namespace SportHub.Data.Seeders.SportHubDb;

using System;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using SportHub.Common.Constants;
using SportHub.Data.Models.DbEntities.SportHub;
using SportHub.Data.Seeders.Interfaces;

public class RolesSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetService<RoleManager<Role>>();

        await this.SeedRoleAsync(roleManager, Roles.SUPERADMIN);
        await this.SeedRoleAsync(roleManager, Roles.ADMIN);
        await this.SeedRoleAsync(roleManager, Roles.EDITOR);
        await this.SeedRoleAsync(roleManager, Roles.USER);
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