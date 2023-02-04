// <copyright file="CategoriesController.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Threading.Tasks;
using EducateMe.Common;
using EducateMe.Data.Models.Common;
using EducateMe.Services.Data.Interfaces;
using EducateMe.Web.Controllers;
using EducateMe.Web.ViewModels.Administration.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EducateMe.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
public class CategoriesController : BaseController
{
    private readonly ICategoriesService categoriesService;

    public CategoriesController(ICategoriesService categoriesService)
    {
        this.categoriesService = categoriesService;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await this.categoriesService.GetCategoriesTableData();
        return this.View(
            new InputCategoryViewModel()
        {
            Categories = categories,
        });
    }

    [HttpPost]
    public async Task<IActionResult> Index(InputCategoryViewModel inputCategoryViewModel)
    {
        if (!this.ModelState.IsValid)
        {
            var categories = await this.categoriesService.GetCategoriesTableData();
            return this.View(
                new InputCategoryViewModel()
            {
                Categories = categories,
            });
        }

        if (await this.categoriesService.ExistsWithName(inputCategoryViewModel.Name))
        {
            this.ModelState.AddModelError("Name", "Вече има категория с това име");
        }
        else
        {
            await this.categoriesService.CreateCategory(inputCategoryViewModel.Name);
        }

        var categoriesTableData = await this.categoriesService.GetCategoriesTableData();
        return this.View(
            new InputCategoryViewModel()
            {
                Categories = categoriesTableData,
            });
    }
}
