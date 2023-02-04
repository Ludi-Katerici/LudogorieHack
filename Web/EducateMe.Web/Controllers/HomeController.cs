using System.Diagnostics;

using EducateMe.Common;
using EducateMe.Web.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace EducateMe.Web.Controllers;

public class HomeController : BaseController
{
    public IActionResult Index()
    {
        if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
        {
            return this.RedirectToAction("Index", "Categories", new { area = "Administration" });
        }

        return this.View();
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
