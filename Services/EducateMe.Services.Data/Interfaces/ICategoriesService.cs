// <copyright file="ICategoriesService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

using EducateMe.Data.Models.Common;

namespace EducateMe.Services.Data.Interfaces;

public interface ICategoriesService
{
    Task<List<Category>> GetCategories();
}
