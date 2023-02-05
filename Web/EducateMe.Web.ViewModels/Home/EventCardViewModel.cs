// <copyright file="EventCardViewModel.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System;

namespace EducateMe.Web.ViewModels.Home;

public class EventCardViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public DateTime CreatedOn { get; set; }
}
