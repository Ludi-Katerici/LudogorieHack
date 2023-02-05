using System.Diagnostics;
using System.Threading.Tasks;

using EducateMe.Common;
using EducateMe.Data.Models;
using EducateMe.Services.Data.Interfaces;
using EducateMe.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EducateMe.Web.Controllers;

public class HomeController : BaseController
{
    private readonly IEventsService eventsService;
    private readonly IUsersService usersService;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IFavouritesService favouritesService;

    public HomeController(IEventsService eventsService, IUsersService usersService, UserManager<ApplicationUser> userManager, IFavouritesService favouritesService)
    {
        this.eventsService = eventsService;
        this.usersService = usersService;
        this.userManager = userManager;
        this.favouritesService = favouritesService;
    }

    public async Task<IActionResult> Index()
    {
        if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
        {
            return this.RedirectToAction("Index", "Categories", new { area = "Administration" });
        }

        var events = await this.eventsService.GetEvents();

        return this.View(events);
    }

    [Route("[controller]/Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var eventViewModel = await this.eventsService.GetEventDetails(id);

        await this.eventsService.ClickIt(id);

        return this.View(eventViewModel);
    }

    public IActionResult Privacy()
    {
        return this.View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return this.View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }
}
