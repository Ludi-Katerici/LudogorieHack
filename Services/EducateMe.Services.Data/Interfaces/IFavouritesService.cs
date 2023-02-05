// <copyright file="IFavouritesService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

using EducateMe.Data.Models.Common;
using EducateMe.Web.ViewModels.Home;

namespace EducateMe.Services.Data.Interfaces;

public interface IFavouritesService
{
    Task<int> RemoveEventFromFavourites(int eventId, int studentId);

    Task<Favourite> AddEventToStudentFavourites(int eventId, int studentId);

    Task<List<EventCardViewModel>> GetFavouriteEventsForUser(int studentId);
}
