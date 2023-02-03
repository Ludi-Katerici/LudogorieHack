// <copyright file="Interest.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;

using EducateMe.Data.Common.Models;
using EducateMe.Data.Models.Common.RelationshipModels;

namespace EducateMe.Data.Models.Common;

public class Interest : BaseModel<int>
{
    public string Name { get; set; }

    public ICollection<StudentInterest> Students { get; set; } = new HashSet<StudentInterest>();

    public ICollection<EventInterest> Events { get; set; } = new HashSet<EventInterest>();
}
