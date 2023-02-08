// <copyright file="EventService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EducateMe.Data.Common.Repositories;
using EducateMe.Data.Models;
using EducateMe.Data.Models.Common;
using EducateMe.Data.Models.Common.RelationshipModels;
using EducateMe.Services.Data.Interfaces;
using EducateMe.Web.ViewModels.Home;
using Microsoft.EntityFrameworkCore;

namespace EducateMe.Services.Data;

public class EventService : IEventsService
{
    private readonly IDeletableEntityRepository<Event> eventRepository;
    private readonly IRepository<Interest> interestsRepository;
    private readonly IRepository<Category> categoriesRepository;

    public EventService(
        IDeletableEntityRepository<Event> eventRepository,
        IRepository<Interest> interestsRepository,
        IRepository<Category> categoriesRepository)
    {
        this.eventRepository = eventRepository;
        this.interestsRepository = interestsRepository;
        this.categoriesRepository = categoriesRepository;
    }

    public async Task<Event> CreateEvent(Event inputEvent, ICollection<int> interestsId, ICollection<int> categoriesId)
    {
        inputEvent.Categories = categoriesId.Select(
            categoryId => new EventCategory
            {
                CategoryId = categoryId,
            }).ToList();

        inputEvent.Interests = interestsId.Select(
            interestId => new EventInterest
            {
                InterestId = interestId,
            }).ToList();
        await this.eventRepository.AddAsync(inputEvent);

        await this.eventRepository.SaveChangesAsync();

        return inputEvent;
    }

    public Task<List<EventCardViewModel>> GetEvents()
    {
        return this.eventRepository.AllAsNoTracking().Select(
            x => new EventCardViewModel()
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                CreatedOn = x.CreatedOn,
                ImageUrl = x.ImageUrl,
                Clicks = x.Clicks,
            }).ToListAsync();
    }

    public async Task<EventDetailsViewModel> GetEventDetails(int id)
    {
        var _event = await this.eventRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .Select(
                x => new EventDetailsViewModel()
                {
                    Id = x.Id,
                    Description = x.Description,
                    Interests = x.Interests.Select(x => x.Interest.Name).ToList(),
                    Categories = x.Categories.Select(x => x.Category.Name).ToList(),
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    City = $"{x.City.Name}, {x.City.Province} [{x.City.PostalCode}]",
                    Requirements = x.Requirements,
                    ApplicationUrl = x.ApplicationUrl,
                    EndDate = x.EndDate,
                    ExpirationDate = x.ExpirationDate,
                    MaxAge = x.MaxAge,
                    MinAge = x.MinAge,
                    OrganizationDescription = x.Organization.Description,
                    StartDate = x.StartDate,
                    OrganizationName = x.Organization.Name,
                    OrganizationImageUrl = x.Organization.ImageUrl,
                }).FirstOrDefaultAsync();

        return _event;
    }

    public async Task<int> DeleteEvent(int id)
    {
        var _event = await this.eventRepository.All().Where(x => x.Id == id).FirstOrDefaultAsync();
        this.eventRepository.Delete(_event);
        return await this.eventRepository.SaveChangesAsync();
    }

    public async Task ClickIt(int id)
    {
        var _event = await this.eventRepository.All().Where(x => x.Id == id).FirstOrDefaultAsync();

        var clicks = _event.Clicks;

        _event.Clicks = clicks + 1;

        await this.eventRepository.SaveChangesAsync();
    }

    public async Task<bool> IsOrganizationOwnerOfEvent(int organizationId, int eventId)
    {
        var _event = await this.eventRepository.All().Where(x => x.Id == eventId).FirstOrDefaultAsync();

        if (_event.OrganizationId == organizationId)
        {
            return true;
        }

        return false;
    }
}
