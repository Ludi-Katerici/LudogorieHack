// <copyright file="Organization.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;

using EducateMe.Data.Common.Models;
using EducateMe.Data.Models.Common;

namespace EducateMe.Data.Models;

public class Organization : BaseDeletableModel<int>
{
    public string Name { get; set; }

    public int CityId { get; set; }

    public City City { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public ICollection<Event> Events { get; set; } = new HashSet<Event>();
}
