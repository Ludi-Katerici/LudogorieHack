// <copyright file="Student.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;

using EducateMe.Data.Common.Models;
using EducateMe.Data.Models.Common;
using EducateMe.Data.Models.Common.RelationshipModels;

namespace EducateMe.Data.Models;

public class Student : BaseDeletableModel<int>
{
    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string Status { get; set; }

    public int Age { get; set; }

    public ICollection<StudentInterest> Interests { get; set; } = new HashSet<StudentInterest>();

    public ICollection<StudentCategory> Categories { get; set; } = new HashSet<StudentCategory>();

    public ICollection<EventStudent> Events { get; set; } = new HashSet<EventStudent>();

    public int CityId { get; set; }

    public City City { get; set; }

    public string School { get; set; }

    public string ImageUrl { get; set; }
}
