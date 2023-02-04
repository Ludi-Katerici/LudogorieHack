// <copyright file="CitiesController.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Linq;
using System.Threading.Tasks;

using EducateMe.Services.Data.Interfaces;
using EducateMe.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducateMe.Web.Areas.Administration.Controllers;

[AllowAnonymous]
public class CitiesController : BaseController
{
    private readonly ICitiesService citiesService;

    public CitiesController(ICitiesService citiesService)
    {
        this.citiesService = citiesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProvinces()
    {
        var provinces = await this.citiesService.GetProvinces();

        return this.Ok(new { provinces });
    }

    [HttpPost("[controller]/{province}")]
    public async Task<IActionResult> GetCities(string province)
    {
        var provinces = await this.citiesService.GetCities(province);

        var result = provinces.Select(x => new { x.Id, x.Name, x.Municipality, x.PostalCode, }).ToList();

        return this.Ok(result);
    }
}
