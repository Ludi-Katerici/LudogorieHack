// <copyright file="IStudentsService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

using EducateMe.Data.Models;

namespace EducateMe.Services.Data.Interfaces;

public interface IStudentsService
{
    Task<Student> CreateStudent(Student student, ICollection<int> interestsId, ICollection<int> categoriesId);
}
