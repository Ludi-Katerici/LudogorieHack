// <copyright file="Event.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System;
using System.Collections.Generic;

using EducateMe.Data.Common.Models;
using EducateMe.Data.Models.Common;
using EducateMe.Data.Models.Common.RelationshipModels;

namespace EducateMe.Data.Models;

public class Event : BaseDeletableModel<int>
{
    public string Name { get; set; }

    public int Clicks { get; set; }

    public string Description { get; set; }

    public int MinAge { get; set; }

    public int MaxAge { get; set; }

    public string Requirements { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string ApplicationUrl { get; set; }

    public int CityId { get; set; }

    public City City { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int OrganizationId { get; set; }

    public string ImageUrl { get; set; }

    public Organization Organization { get; set; }

    public ICollection<EventCategory> Categories { get; set; } = new HashSet<EventCategory>();

    public ICollection<EventInterest> Interests { get; set; } = new HashSet<EventInterest>();

    public ICollection<EventStudent> Students { get; set; } = new HashSet<EventStudent>();
}
