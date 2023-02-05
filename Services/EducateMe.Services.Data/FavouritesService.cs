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
        var favouritesOfUser = await this.studentsRepository
            .AllAsNoTracking()
            .Where(x => x.Id == studentId)
            .Include(x => x.Favourites)
            .ThenInclude(x => x.Event)
            .Select(x => x.Favourites)
            .FirstOrDefaultAsync();

        var favouriteEvents = favouritesOfUser.Select(x => x.Event).ToList();

        return favouriteEvents.Select(
            x => new EventCardViewModel()
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                CreatedOn = x.CreatedOn,
                ImageUrl = x.ImageUrl,
                Clicks = x.Clicks,
            }).ToList();
    }
}