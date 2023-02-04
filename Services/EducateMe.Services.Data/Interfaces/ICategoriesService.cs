// <copyright file="ICategoriesService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

using EducateMe.Data.Models.Common;
using EducateMe.Web.ViewModels.Administration.Categories;

namespace EducateMe.Services.Data.Interfaces;

public interface ICategoriesService
{
    Task<List<Category>> GetCategories();

    Task<List<CategoryTableViewModel>> GetCategoriesTableData();

    Task<Category> CreateCategory(string name);

    Task<bool> ExistsWithName(string name);

    Task<int> DeleteCategory(int id);
}
