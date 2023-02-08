// <copyright file="CitiesService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EducateMe.Data.Common.Repositories;
using EducateMe.Data.Models.Common;
using EducateMe.Services.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EducateMe.Services.Data;

public class CitiesService : ICitiesService
{
    private readonly IRepository<City> citiesRepository;

    public static List<City> Cities { get; set; } = new();

    public static List<SelectListItem> Provinces { get; set; } = new(); 

    public CitiesService(IRepository<City> citiesRepository)
    {
        this.citiesRepository = citiesRepository;
    }

    public async Task<List<string>> GetProvinces()
    {
        await this.EnsureCityListIsPopulated();

        return Cities
            .Select(x => x.Province)
            .Distinct()
            .ToList();
    }

    public async Task<List<City>> GetCities(string province)
    {
        await this.EnsureCityListIsPopulated();

        var cities = new List<City>();

        for (var i = 0; i < Cities.Count; i++)
        {
            if (Cities[i].Province == province)
            {
                cities.Add(new City()
                {
                    Id = Cities[i].Id,
                    Name = Cities[i].Name,
                    Municipality = Cities[i].Municipality,
                    PostalCode = Cities[i].PostalCode,
                });
            }
        }

        return cities;
    }

    public async Task<List<SelectListItem>> ProvincesSelectList()
    {
        if (Provinces.Count == 0)
        {
            var provinces = await this.GetProvinces();

            Provinces = provinces.Select(x => new SelectListItem(x, x)).ToList();
        }

        return Provinces;
    }

    private async Task GetCitiesFromDatabase()
    {
        Cities = await this.citiesRepository.AllAsNoTracking().ToListAsync();
    }

    private async Task EnsureCityListIsPopulated()
    {
        if (Cities.Count == 0)
        {
            await this.GetCitiesFromDatabase();
        }
    }
}
