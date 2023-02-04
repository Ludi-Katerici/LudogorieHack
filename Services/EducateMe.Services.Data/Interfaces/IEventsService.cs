// <copyright file="IEventsService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

using EducateMe.Data.Models;

namespace EducateMe.Services.Data.Interfaces;

public interface IEventsService
{
    Task<Event> CreateEvent(Event inputEvent, ICollection<int> interestsId, ICollection<int> categoriesId);
}
