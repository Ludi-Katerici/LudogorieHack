// <copyright file="EventInterest.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

namespace EducateMe.Data.Models.Common.RelationshipModels;

public class EventInterest
{
    public int EventId { get; set; }

    public Event Event { get; set; }

    public int InterestId { get; set; }

    public Interest Interest { get; set; }
}
