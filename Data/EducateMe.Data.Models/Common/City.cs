// <copyright file="City.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;

using EducateMe.Data.Common.Models;

namespace EducateMe.Data.Models.Common;

public class City : BaseModel<int>
{
    public int PostalCode { get; set; }

    public string Name { get; set; }

    public string Municipality { get; set; }

    public string Province { get; set; }

    public ICollection<Organization> Organizations { get; set; }

    public ICollection<Student> Students { get; set; }

    public ICollection<Event> Events { get; set; }
}
