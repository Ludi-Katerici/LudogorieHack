// <copyright file="StudentCategory.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

namespace EducateMe.Data.Models.Common.RelationshipModels;

public class StudentCategory
{
    public int StudentId { get; set; }

    public Student Student { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; }
}
