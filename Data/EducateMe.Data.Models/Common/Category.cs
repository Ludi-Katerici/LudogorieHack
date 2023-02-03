// <copyright file="Category.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;

using EducateMe.Data.Common.Models;
using EducateMe.Data.Models.Common.RelationshipModels;

namespace EducateMe.Data.Models.Common;

public class Category : BaseModel<int>
{
    public string Name { get; set; }

    public ICollection<StudentCategory> Students { get; set; } = new HashSet<StudentCategory>();

    public ICollection<EventCategory> Events { get; set; } = new HashSet<EventCategory>();
}
