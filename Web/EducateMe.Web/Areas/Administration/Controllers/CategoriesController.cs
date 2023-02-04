// <copyright file="CategoriesController.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using EducateMe.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EducateMe.Web.Areas.Administration.Controllers;

[Area("Administration")]
public class CategoriesController : BaseController
{
    public IActionResult Index()
    {
        return this.View();
    }
}
