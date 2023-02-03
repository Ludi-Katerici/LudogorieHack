// <copyright file="EventStudent.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

namespace EducateMe.Data.Models.Common.RelationshipModels;

public class EventStudent
{
    public int StudentId { get; set; }

    public Student Student { get; set; }

    public int EventId { get; set; }

    public Event Event { get; set; }
}
