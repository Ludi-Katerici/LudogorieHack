// <copyright file="FavouritesService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EducateMe.Data.Common.Repositories;
using EducateMe.Data.Models;
using EducateMe.Data.Models.Common;
using EducateMe.Services.Data.Interfaces;
using EducateMe.Web.ViewModels.Home;
using Microsoft.EntityFrameworkCore;

namespace EducateMe.Services.Data;

public class FavouritesService : IFavouritesService
{
    private readonly IRepository<Favourite> favouritesRepository;
    private readonly IDeletableEntityRepository<Student> studentsRepository;

    public FavouritesService(IRepository<Favourite> favouritesRepository, IDeletableEntityRepository<Student> studentsRepository)
    {
        this.favouritesRepository = favouritesRepository;
        this.studentsRepository = studentsRepository;
    }

    public async Task<int> RemoveEventFromFavourites(int eventId, int studentId)
    {
        var favourite = await this.favouritesRepository.All().Where(x => x.EventId == eventId && x.StudentId == studentId).FirstOrDefaultAsync();

        this.favouritesRepository.Delete(favourite);

        return await this.favouritesRepository.SaveChangesAsync();
    }

    public async Task<Favourite> AddEventToStudentFavourites(int eventId, int studentId)
    {
        var favourite = new Favourite()
        {
            StudentId = studentId,
            EventId = eventId,
        };

        await this.favouritesRepository.AddAsync(favourite);
        await this.favouritesRepository.SaveChangesAsync();

        return favourite;
    }

    public async Task<List<EventCardViewModel>> GetFavouriteEventsForUser(int studentId)
    {
        var favouritesOfUser = await this.favouritesRepository
            .AllAsNoTracking()
            .Where(x => x.StudentId == studentId)
            .Select(
                x => new EventCardViewModel()
                {
                    Id = x.Event.Id,
                    Description = x.Event.Description,
                    Name = x.Event.Name,
                    CreatedOn = x.Event.CreatedOn,
                    ImageUrl = x.Event.ImageUrl,
                    Clicks = x.Event.Clicks,
                }).ToListAsync();

        return favouritesOfUser;
    }
}
