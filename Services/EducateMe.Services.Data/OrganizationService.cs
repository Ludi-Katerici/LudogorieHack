// <copyright file="OrganizationService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Threading.Tasks;

using EducateMe.Data.Common.Repositories;
using EducateMe.Data.Models;
using EducateMe.Services.Data.Interfaces;

namespace EducateMe.Services.Data;

public class OrganizationService : IOrganizationsService
{
    private readonly IDeletableEntityRepository<Organization> organizationRepository;

    public OrganizationService(IDeletableEntityRepository<Organization> organizationRepository)
    {
        this.organizationRepository = organizationRepository;
    }

    public async Task<Organization> CreateOrganization(Organization organization)
    {
        await this.organizationRepository.AddAsync(organization);

        await this.organizationRepository.SaveChangesAsync();

        return organization;
    }
}
