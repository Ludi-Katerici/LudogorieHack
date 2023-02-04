// <copyright file="ICitiesService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

using EducateMe.Data.Models.Common;

namespace EducateMe.Services.Data.Interfaces;

public interface ICitiesService
{
    Task<List<string>> GetProvinces();

    Task<List<City>> GetCities(string province);
}
