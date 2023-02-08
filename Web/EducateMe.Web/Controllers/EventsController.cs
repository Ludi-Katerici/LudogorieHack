// <copyright file="OrganizationController.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System;
using System.Threading.Tasks;

using EducateMe.Common;
using EducateMe.Data.Models;
using EducateMe.Services;
using EducateMe.Services.Data.Interfaces;
using EducateMe.Web.AzureServices;
using EducateMe.Web.ViewModels.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EducateMe.Web.Controllers;

[Authorize(Roles = GlobalConstants.OrganizationRoleName)]
public class EventsController : BaseController
{
    private readonly IDropdownListService dropdownListService;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IUsersService usersService;
    private readonly IEventsService eventsService;
    private readonly IAzureStorage azureStorage;

    public EventsController(
        IDropdownListService dropdownListService,
        UserManager<ApplicationUser> userManager,
        IUsersService usersService,
        IEventsService eventsService,
        IAzureStorage azureStorage)
    {
        this.dropdownListService = dropdownListService;
        this.userManager = userManager;
        this.usersService = usersService;
        this.eventsService = eventsService;
        this.azureStorage = azureStorage;
    }

    public async Task<IActionResult> Index()
    {
        var inputModel = new InputEventViewModel();

        return this.View(inputModel);
    }

    [HttpPost]
    public async Task<IActionResult> Index(InputEventViewModel inputEventViewModel)
    {
        var userId = this.userManager.GetUserId(this.User);
        inputEventViewModel.OrganizationId = await this.usersService.GetUsersOrganizationId(userId);

        if (inputEventViewModel.ExpirationDate <= DateTime.Today)
        {
            this.ModelState.AddModelError(
                "ExpirationDate",
                "Датата на кандидатстване трябва да бъде днеска или в бъдеще");
            return this.View(inputEventViewModel);
        }

        if (inputEventViewModel.StartDate >= inputEventViewModel.EndDate)
        {
            this.ModelState.AddModelError("StartDate", "Началната дата трябва да бъде преди крайната.");
            return this.View(inputEventViewModel);
        }

        if (inputEventViewModel.MinAge > inputEventViewModel.MaxAge)
        {
            this.ModelState.AddModelError("MinAge", "Минималните години трябва да са по-малки от максималните");
            return this.View(inputEventViewModel);
        }

        if (!this.ModelState.IsValid)
        {
            return this.RedirectToAction("Index");
        }

        var imageUrl = string.Empty;
        var imageResult = await this.azureStorage.UploadAsync(inputEventViewModel.Image);
        if (!imageResult.Error)
        {
            imageUrl = imageResult.Blob.Uri;
        }
        else
        {
            throw new ArgumentException("Error");
        }

        await this.eventsService.CreateEvent(
            new Event()
            {
                Name = inputEventViewModel.Name,
                MinAge = inputEventViewModel.MinAge,
                MaxAge = inputEventViewModel.MaxAge,
                OrganizationId = inputEventViewModel.OrganizationId,
                StartDate = inputEventViewModel.StartDate,
                EndDate = inputEventViewModel.EndDate,
                ExpirationDate = inputEventViewModel.ExpirationDate,
                CityId = inputEventViewModel.CityId,
                ApplicationUrl = inputEventViewModel.ApplicationUrl,
                Description = inputEventViewModel.Description,
                Requirements = inputEventViewModel.Requirements,
                ImageUrl = imageUrl,
            },
            inputEventViewModel.InterestsIds,
            inputEventViewModel.CategoriesIds);

        return this.RedirectToAction("Index", "Home");
    }

    [Route("[controller]/Delete/{id:int}")]
    [Authorize(Roles = $"{GlobalConstants.AdministratorRoleName},{GlobalConstants.OrganizationRoleName}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
        {
            await this.eventsService.DeleteEvent(id);
        }
        else
        {
            var userId = this.userManager.GetUserId(this.User);
            var organizationId = await this.usersService.GetUsersOrganizationId(userId);

            if (await this.eventsService.IsOrganizationOwnerOfEvent(organizationId, id))
            {
                await this.eventsService.DeleteEvent(id);
            }
        }

        return this.RedirectToAction("Index", "Home");
    }
}
