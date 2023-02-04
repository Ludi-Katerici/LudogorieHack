// <copyright file="InterestsService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducateMe.Data.Common.Repositories;
using EducateMe.Data.Models.Common;
using EducateMe.Services.Data.Interfaces;
using EducateMe.Web.ViewModels.Administration.Interests;
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

    public async Task<List<InterestTableViewModel>> GetInterestsTableData()
    {
        return await this.interestsRepository.AllAsNoTracking().Select(
            x => new InterestTableViewModel()
            {
                Name = x.Name,
                StudentsCount = x.Students.Count,
                EventsCount = x.Events.Count,
                Id = x.Id,
            }).ToListAsync();
    }

    public async Task<Interest> CreateInterest(string name)
    {
        var interest = new Interest() { Name = name };

        await this.interestsRepository.AddAsync(interest);

        await this.interestsRepository.SaveChangesAsync();

        return interest;
    }

    public async Task<bool> ExistsWithName(string name)
    {
        return await this.interestsRepository.AllAsNoTracking().AnyAsync(x => x.Name == name);
    }

    public async Task<int> DeleteInterest(int id)
    {
        var interest = await this.interestsRepository.All().Where(x => x.Id == id).FirstOrDefaultAsync();

        this.interestsRepository.Delete(interest);

        return await this.interestsRepository.SaveChangesAsync();
    }
}
