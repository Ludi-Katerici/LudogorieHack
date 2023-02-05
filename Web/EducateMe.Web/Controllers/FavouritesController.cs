// <copyright file="FavouritesController.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Threading.Tasks;
using EducateMe.Common;
using EducateMe.Data.Models;
using EducateMe.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EducateMe.Web.Controllers;

public class FavouritesController : BaseController
{
    private readonly IFavouritesService favouritesService;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IUsersService usersService;

    public FavouritesController(IFavouritesService favouritesService, UserManager<ApplicationUser> userManager, IUsersService usersService)
    {
        this.favouritesService = favouritesService;
        this.userManager = userManager;
        this.usersService = usersService;
    }

    [Authorize(Roles = GlobalConstants.StudentRoleName)]
    public async Task<IActionResult> Index()
    {
        var userId = this.userManager.GetUserId(this.User);
        var studentId = await this.usersService.GetUsersStudentId(userId);

        var favourites = await this.favouritesService.GetFavouriteEventsForUser(studentId);

        return this.View(favourites);
    }

    [Authorize(Roles = GlobalConstants.StudentRoleName)]
    [Route("[controller]/AddFavourite/{id:int}")]
    public async Task<IActionResult> AddToFavourites(int id)
    {
        var userId = this.userManager.GetUserId(this.User);
        var studentId = await this.usersService.GetUsersStudentId(userId);

        var favourite = await this.favouritesService.AddEventToStudentFavourites(id, studentId);

        return this.RedirectToAction("Index");
    }

    [Authorize(Roles = GlobalConstants.StudentRoleName)]
    [Route("[controller]/RemoveFavourite/{id:int}")]
    public async Task<IActionResult> RemoveFromFavourites(int id)
    {
        var userId = this.userManager.GetUserId(this.User);
        var studentId = await this.usersService.GetUsersStudentId(userId);

        var favourite = await this.favouritesService.RemoveEventFromFavourites(id, studentId);

        return this.RedirectToAction("Index");
    }
}
