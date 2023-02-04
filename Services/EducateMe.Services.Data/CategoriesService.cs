﻿// <copyright file="CategoriesService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EducateMe.Data.Common.Repositories;
using EducateMe.Data.Models.Common;
using EducateMe.Services.Data.Interfaces;
using EducateMe.Web.ViewModels.Administration.Categories;
using Microsoft.EntityFrameworkCore;

namespace EducateMe.Services.Data;

public class CategoriesService : ICategoriesService
{
    private readonly IRepository<Category> categoriesRepository;

    public CategoriesService(IRepository<Category> categoriesRepository)
    {
        this.categoriesRepository = categoriesRepository;
    }

    public async Task<List<Category>> GetCategories()
    {
        return await this.categoriesRepository.AllAsNoTracking().Select(
            x => new Category
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
    }

    public async Task<List<CategoryTableViewModel>> GetCategoriesTableData()
    {
        return await this.categoriesRepository.AllAsNoTracking().Select(
            x => new CategoryTableViewModel()
            {
                Name = x.Name,
                StudentsCount = x.Students.Count,
                EventsCount = x.Events.Count,
            }).ToListAsync();
    }

    public async Task<Category> CreateCategory(string name)
    {
        var category = new Category() { Name = name };

        await this.categoriesRepository.AddAsync(category);

        await this.categoriesRepository.SaveChangesAsync();

        return category;
    }

    public async Task<bool> ExistsWithName(string name)
    {
        return await this.categoriesRepository.AllAsNoTracking().AnyAsync(x => x.Name == name);
    }
}
