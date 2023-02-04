// <copyright file="IDropdownListService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace EducateMe.Services;

public interface IDropdownListService
{
    Task<List<SelectListItem>> SeedCategories();

    Task<List<SelectListItem>> SeedInterests();

    Task<(List<SelectListItem>, List<SelectListItem>)> SeedCities();
}
