// <copyright file="EventService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EducateMe.Data.Common.Repositories;
using EducateMe.Data.Models;
using EducateMe.Data.Models.Common.RelationshipModels;
using EducateMe.Services.Data.Interfaces;

namespace EducateMe.Services.Data;

public class EventService : IEventsService
{
    private readonly IDeletableEntityRepository<Event> eventRepository;

    public EventService(IDeletableEntityRepository<Event> eventRepository)
    {
        this.eventRepository = eventRepository;
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
}
