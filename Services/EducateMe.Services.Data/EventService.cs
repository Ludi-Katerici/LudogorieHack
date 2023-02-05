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

    public EventService(IDeletableEntityRepository<Event> eventRepository, IRepository<Interest> interestsRepository, IRepository<Category> categoriesRepository)
    {
        this.eventRepository = eventRepository;
        this.interestsRepository = interestsRepository;
        this.categoriesRepository = categoriesRepository;
    }

    public async Task<Event> CreateEvent(Event inputEvent, ICollection<int> interestsId, ICollection<int> categoriesId)
    {
        await this.eventRepository.AddAsync(inputEvent);

        await this.eventRepository.SaveChangesAsync();

        inputEvent.Categories = categoriesId.Select(
            categoryId => new EventCategory
            {
                CategoryId = categoryId,
                EventId = inputEvent.Id,
            }).ToList();

        inputEvent.Interests = interestsId.Select(
            interestId => new EventInterest
            {
                InterestId = interestId,
                EventId = inputEvent.Id,
            }).ToList();

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
            }).ToListAsync();
    }

    public async Task<EventDetailsViewModel> GetEventDetails(int id)
    {
        var x = await this.eventRepository
            .All()
            .Where(x => x.Id == id)
            .Include(x => x.Categories)
            .Include(x => x.Organization)
            .Include(x => x.Interests)
            .Include(x => x.City)
            .FirstOrDefaultAsync();

        var categoriesId = x.Categories.Select(x => x.CategoryId).ToList();
        var categoriesNames = new List<string>();
        var interestsNames = new List<string>();

        foreach (var categoryId in categoriesId)
        {
            var name = await this.categoriesRepository.AllAsNoTracking().Where(x => x.Id == categoryId).Select(x => x.Name)
                .FirstOrDefaultAsync();
            categoriesNames.Add(name);
        }

        var interestsId = x.Interests.Select(x => x.InterestId).ToList();
        foreach (var interestId in interestsId)
        {
            var name = await this.interestsRepository.AllAsNoTracking().Where(x => x.Id == interestId).Select(x => x.Name)
                .FirstOrDefaultAsync();
            interestsNames.Add(name);
        }

        var eventResult = new EventDetailsViewModel()
        {
            Id = x.Id,
            Description = x.Description,
            Interests = interestsNames,
            Categories = categoriesNames,
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
        };

        return eventResult;
    }
}
