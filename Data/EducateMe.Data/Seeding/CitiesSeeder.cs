// <copyright file="CitiesSeeder.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EducateMe.Data.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace EducateMe.Data.Seeding;

public class CitiesSeeder : ISeeder
{
    private const string FileUrl =
        "https://raw.githubusercontent.com/yurukov/Bulgaria-geocoding/master/settlements_loc.csv";

    public static List<City> GetCitiesFromCsv()
    {
        var webClient = new WebClient();
        var file = webClient.DownloadString(FileUrl);
        var lines = file.Split(new[] { '\r', '\n' });

        var list = new List<City>();

        for (var i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(lines[i]))
            {
                var items = lines[i].Split(",");
                var name = items[1].Substring(1, items[1].Length - 2);
                var province = items[3].Substring(1, items[3].Length - 2);
                var municipality = items[4].Substring(1, items[4].Length - 2);
                var postalCode = items[5].Substring(1, items[5].Length - 2);

                // Unknown because of debelec, varna which doesn't have postal code
                var isValidPostalCode = int.TryParse(postalCode, out int postalCodeNumber);
                list.Add(
                    new City()
                    {
                        Municipality = municipality,
                        Name = name,
                        Province = province,
                        PostalCode = isValidPostalCode ? postalCodeNumber : 9999 + i,
                    });
            }
        }

        return list;
    }

    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        var citiesInDatabase = await dbContext.Cities.ToListAsync();

        if (citiesInDatabase.Count != 5267)
        {
            var cities = GetCitiesFromCsv();
            dbContext.Cities.AddRange(cities);
        }

        await dbContext.SaveChangesAsync();
    }
}
