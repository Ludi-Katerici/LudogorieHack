// <copyright file="InterestTableViewModel.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

namespace EducateMe.Web.ViewModels.Administration.Interests;

public class InterestTableViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int EventsCount { get; set; }

    public int StudentsCount { get; set; }
}
