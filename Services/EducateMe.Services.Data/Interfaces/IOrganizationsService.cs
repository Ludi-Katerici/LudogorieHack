// <copyright file="IOrganizationsService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Threading.Tasks;

using EducateMe.Data.Models;

namespace EducateMe.Services.Data.Interfaces;

public interface IOrganizationsService
{
    Task<Organization> CreateOrganization(Organization organization);
}
