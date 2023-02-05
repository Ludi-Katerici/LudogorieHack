// <copyright file="Favourite.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using EducateMe.Data.Common.Models;

namespace EducateMe.Data.Models.Common;

public class Favourite : BaseModel<int>
{
    public int EventId { get; set; }

    public Event Event { get; set; }

    public int StudentId { get; set; }

    public Student Student { get; set; }
}
