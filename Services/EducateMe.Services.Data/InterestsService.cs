// <copyright file="InterestsService.cs" company="AspNetCoreTemplate">
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

public class InterestsService : IInterestsService
{
    private readonly IRepository<Interest> interestsRepository;

    public InterestsService(IRepository<Interest> interestsRepository)
    {
        this.interestsRepository = interestsRepository;
    }

    public Task<List<Interest>> GetInterests()
    {
        return this.interestsRepository.AllAsNoTracking().Select(
            x => new Interest()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
    }
}
