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

public static class Cities
{
    public static List<City> CityList { get; set; } = new();
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
        if (Cities.CityList.Count == 0)
        {
            Cities.CityList = this.citiesRepository.AllAsNoTracking().ToList();
        }

        return Cities.CityList
            .Select(x => x.Province)
            .Distinct()
            .ToList();
    }

    public async Task<List<City>> GetCities(string province)
    {
        if (Cities.CityList.Count == 0)
        {
            Cities.CityList = await this.citiesRepository.AllAsNoTracking().ToListAsync();
        }

        return Cities.CityList
            .Where(x => x.Province == province).Select(
                x => new City()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Municipality = x.Municipality,
                    PostalCode = x.PostalCode,
                }).ToList();
    }
}
