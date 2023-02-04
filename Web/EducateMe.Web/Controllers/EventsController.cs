// <copyright file="OrganizationController.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Threading.Tasks;
using EducateMe.Common;
using EducateMe.Services;
using EducateMe.Web.ViewModels.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducateMe.Web.Controllers;

[Authorize(Roles = GlobalConstants.OrganizationRoleName)]
public class EventsController : BaseController
{
    private readonly IDropdownListService dropdownListService;

    public EventsController(IDropdownListService dropdownListService)
    {
        this.dropdownListService = dropdownListService;
    }

    public async Task<IActionResult> Index()
    {
        var inputModel = new InputEventViewModel();

        var categories = await this.dropdownListService.SeedCategories();
        var interests = await this.dropdownListService.SeedInterests();

        var (provinces, cities) = await this.dropdownListService.SeedCities();

        inputModel.Categories = categories;
        inputModel.Interests = interests;

        inputModel.Provinces = provinces;
        inputModel.Cities = cities;

        return this.View(inputModel);
    }
}
