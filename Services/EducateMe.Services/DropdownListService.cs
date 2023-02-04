// <copyright file="DropdownListService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EducateMe.Services.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EducateMe.Services;

public class DropdownListService : IDropdownListService
{
    private readonly ICitiesService citiesService;
    private readonly IInterestsService interestsService;
    private readonly ICategoriesService categoriesService;

    public DropdownListService(
        ICitiesService citiesService,
        IInterestsService interestsService,
        ICategoriesService categoriesService)
    {
        this.citiesService = citiesService;
        this.interestsService = interestsService;
        this.categoriesService = categoriesService;
    }

    public async Task<List<SelectListItem>> SeedCategories()
    {
        return (await this.categoriesService.GetCategories()).Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
    }

    public async Task<List<SelectListItem>> SeedInterests()
    {
        return (await this.interestsService.GetInterests()).Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
    }

    public async Task<(List<SelectListItem>, List<SelectListItem>)> SeedCities()
    {
        var provinces = await this.citiesService.GetProvinces();
        var provincesList = provinces.Select(x => new SelectListItem(x, x)).ToList();

        var cities = (await this.citiesService.GetCities(provincesList[0].Value)).OrderBy(x => x.PostalCode);
        var citiesList = cities.Select(x => new SelectListItem($"{x.Name}, {x.PostalCode}", x.Id.ToString())).ToList();

        return (provincesList, citiesList);
    }
}
