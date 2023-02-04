// <copyright file="InterestsController.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Threading.Tasks;

using EducateMe.Common;
using EducateMe.Services.Data.Interfaces;
using EducateMe.Web.Controllers;
using EducateMe.Web.ViewModels.Administration.Interests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducateMe.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
public class InterestsController : BaseController
{
    private readonly IInterestsService interestsService;

    public InterestsController(IInterestsService interestsService)
    {
        this.interestsService = interestsService;
    }

    public async Task<IActionResult> Index()
    {
        var interests = await this.interestsService.GetInterestsTableData();
        return this.View(
            new InputInterestViewModel()
            {
                Interests = interests,
            });
    }

    [HttpPost]
    public async Task<IActionResult> Index(InputInterestViewModel inputInterestViewModel)
    {
        if (!this.ModelState.IsValid)
        {
            var interests = await this.interestsService.GetInterestsTableData();
            return this.View(
                new InputInterestViewModel()
                {
                    Interests = interests,
                });
        }

        if (await this.interestsService.ExistsWithName(inputInterestViewModel.Name))
        {
            this.ModelState.AddModelError("Name", "Вече има интерес с това име");
        }
        else
        {
            await this.interestsService.CreateInterest(inputInterestViewModel.Name);
        }

        var interestsTableData = await this.interestsService.GetInterestsTableData();
        return this.View(
            new InputInterestViewModel()
            {
                Interests = interestsTableData,
            });
    }
}
