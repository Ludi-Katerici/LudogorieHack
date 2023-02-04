// <copyright file="InterestsSeeder.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using EducateMe.Data.Common.Repositories;
using EducateMe.Data.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EducateMe.Data.Seeding;

public class InterestsSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        var interestsRepository = serviceProvider.GetService<IRepository<Interest>>();

        var categories = await interestsRepository.AllAsNoTracking().ToListAsync();

        if (categories.Count == 0)
        {
            await interestsRepository.AddAsync(
                new Interest()
                {
                    Name = "Стаж",
                });

            await interestsRepository.AddAsync(
                new Interest()
                {
                    Name = "Обмяна на ученици",
                });

            await interestsRepository.AddAsync(
                new Interest()
                {
                    Name = "Благотворителност",
                });

            await interestsRepository.SaveChangesAsync();
        }
    }
}