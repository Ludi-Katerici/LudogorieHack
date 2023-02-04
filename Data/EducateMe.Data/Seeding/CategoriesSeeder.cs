// <copyright file="CategoriesSeeder.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System;
using System.Threading.Tasks;

using EducateMe.Data.Common.Repositories;
using EducateMe.Data.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EducateMe.Data.Seeding;

public class CategoriesSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        var categoriesRepository = serviceProvider.GetService<IRepository<Category>>();

        var categories = await categoriesRepository.AllAsNoTracking().ToListAsync();

        if (categories.Count == 0)
        {
            await categoriesRepository.AddAsync(
                new Category()
                {
                    Name = "Музика",
                });

            await categoriesRepository.AddAsync(
                new Category()
                {
                    Name = "Спорт",
                });

            await categoriesRepository.AddAsync(
                new Category()
                {
                    Name = "Рисуване",
                });

            await categoriesRepository.SaveChangesAsync();
        }
    }
}
