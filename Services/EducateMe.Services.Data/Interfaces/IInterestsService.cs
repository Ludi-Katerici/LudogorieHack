// <copyright file="IInterestsService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

using EducateMe.Data.Models.Common;
using EducateMe.Web.ViewModels.Administration.Interests;

namespace EducateMe.Services.Data.Interfaces;

public interface IInterestsService
{
    Task<List<Interest>> GetInterests();

    Task<List<InterestTableViewModel>> GetInterestsTableData();

    Task<Interest> CreateInterest(string name);

    Task<bool> ExistsWithName(string name);

    Task<int> DeleteInterest(int id);
}
