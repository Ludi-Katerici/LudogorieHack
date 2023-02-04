// <copyright file="AdminSeeder.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using EducateMe.Common;
using EducateMe.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EducateMe.Data.Seeding;

public class AdminSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

        var exists = await userManager.FindByEmailAsync("admin@admin.com") != null;

        if (!exists)
        {
            var user = new ApplicationUser()
            {
                Email = "admin@admin.com",
                UserName = "admin@admin.com",
            };

            var result = await userManager.CreateAsync(user, "admin1234");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
