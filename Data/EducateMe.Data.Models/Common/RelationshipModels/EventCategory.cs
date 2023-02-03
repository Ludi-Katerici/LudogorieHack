// <copyright file="EventCategory.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

namespace EducateMe.Data.Models.Common.RelationshipModels;

public class EventCategory
{
    public int EventId { get; set; }

    public Event Event { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; }
}
