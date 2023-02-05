using System.Diagnostics;
using System.Threading.Tasks;
using EducateMe.Common;
using EducateMe.Services.Data.Interfaces;
using EducateMe.Web.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace EducateMe.Web.Controllers;

public class HomeController : BaseController
{
    private readonly IEventsService eventsService;

    public HomeController(IEventsService eventsService)
    {
        this.eventsService = eventsService;
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
