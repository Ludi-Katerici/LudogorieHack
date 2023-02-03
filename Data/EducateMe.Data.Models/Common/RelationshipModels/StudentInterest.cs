// <copyright file="StudentInterest.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

namespace EducateMe.Data.Models.Common.RelationshipModels;

public class StudentInterest
{
    public int StudentId { get; set; }

    public Student Student { get; set; }

    public int InterestId { get; set; }

    public Interest Interest { get; set; }
}
