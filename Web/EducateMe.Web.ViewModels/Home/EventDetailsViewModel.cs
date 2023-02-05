// <copyright file="EventDetailsViewModel.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using EducateMe.Data.Models.Common;

namespace EducateMe.Web.ViewModels.Home;

public class EventDetailsViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int MinAge { get; set; }

    public int MaxAge { get; set; }

    public string Requirements { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string ApplicationUrl { get; set; }

    public string City { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string ImageUrl { get; set; }

    public string OrganizationName { get; set; }

    public string OrganizationDescription { get; set; }

    public string OrganizationImageUrl { get; set; }

    public ICollection<string> Interests { get; set; }

    public ICollection<string> Categories { get; set; }
}
