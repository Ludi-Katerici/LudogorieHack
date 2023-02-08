// <copyright file="CitiesService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EducateMe.Data.Common.Repositories;
using EducateMe.Data.Models.Common;
using EducateMe.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducateMe.Services.Data;

public static class CitiesList
{
    public static List<City> Cities { get; set; } = new();
}

public class CitiesService : ICitiesService
{
    private readonly IRepository<City> citiesRepository;

    public CitiesService(IRepository<City> citiesRepository)
    {
        this.citiesRepository = citiesRepository;
    }

    public async Task<List<string>> GetProvinces()
    {
        await this.EnsureCityListIsPopulated();

        return CitiesList.Cities
            .Select(x => x.Province)
            .Distinct()
            .ToList();
    }

    public async Task<List<City>> GetCities(string province)
    {
        await this.EnsureCityListIsPopulated();

        var cities = new List<City>();

        for (var i = 0; i < CitiesList.Cities.Count; i++)
        {
            if (CitiesList.Cities[i].Province == province)
            {
                cities.Add(new City()
                {
                    Id = CitiesList.Cities[i].Id,
                    Name = CitiesList.Cities[i].Name,
                    Municipality = CitiesList.Cities[i].Municipality,
                    PostalCode = CitiesList.Cities[i].PostalCode,
                });
            }
        }

        return cities;
    }

    private async Task GetCitiesFromDatabase()
    {
        CitiesList.Cities = await this.citiesRepository.AllAsNoTracking().ToListAsync();
    }

    private async Task EnsureCityListIsPopulated()
    {
        if (CitiesList.Cities.Count == 0)
        {
            await this.GetCitiesFromDatabase();
        }
    }
}
