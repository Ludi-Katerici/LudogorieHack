// <copyright file="OrganizationController.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using EducateMe.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducateMe.Web.Controllers;

[Authorize(Roles = GlobalConstants.OrganizationRoleName)]
public class OrganizationsController : BaseController
{
    public IActionResult Index()
    {
        return this.View();
    }
}
