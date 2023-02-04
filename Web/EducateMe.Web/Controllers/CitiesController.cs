// <copyright file="CitiesController.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EducateMe.Services.Data.Interfaces;
using EducateMe.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducateMe.Web.Controllers;

[AllowAnonymous]
public class CitiesController : BaseController
{
    private readonly ICitiesService citiesService;

    public CitiesController(ICitiesService citiesService)
    {
        this.citiesService = citiesService;
    }

    [HttpGet]
    [Route("[controller]/GetProvinces")]
    public async Task<List<ProvinceViewModel>> GetProvinces()
    {
        var result = await this.citiesService.GetProvinces();

        var provinces = result.Select(x => new ProvinceViewModel { Name = x }).ToList();

        return provinces;
    }

    [HttpGet]
    [Route("[controller]/{province}")]
    public async Task<IActionResult> GetCities(string province)
    {
        var provinces = await this.citiesService.GetCities(province);

        var result = provinces.Select(x => new CityViewModel { Id = x.Id, Name = x.Name, Municipality = x.Municipality, PostalCode = x.PostalCode, }).ToList();

        return this.Ok(result);
    }
}
