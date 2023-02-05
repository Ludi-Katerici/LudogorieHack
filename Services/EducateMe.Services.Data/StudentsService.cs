// <copyright file="StudentsService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EducateMe.Data.Common.Repositories;
using EducateMe.Data.Models;
using EducateMe.Data.Models.Common.RelationshipModels;
using EducateMe.Services.Data.Interfaces;

namespace EducateMe.Services.Data;

public class StudentsService : IStudentsService
{
    private readonly IDeletableEntityRepository<Student> studentRepository;

    public StudentsService(IDeletableEntityRepository<Student> studentRepository)
    {
        this.studentRepository = studentRepository;
    }

    public async Task<Student> CreateStudent(
        Student student,
        ICollection<int> interestsId,
        ICollection<int> categoriesId)
    {
        await this.studentRepository.AddAsync(student);

        await this.studentRepository.SaveChangesAsync();

        student.Categories = categoriesId.Select(
            categoryId => new StudentCategory
            {
                CategoryId = categoryId,
                StudentId = student.Id,
            }).ToList();

        student.Interests = interestsId.Select(
            interestId => new StudentInterest
            {
                InterestId = interestId,
                StudentId = student.Id,
            }).ToList();

        await this.studentRepository.SaveChangesAsync();

        return student;
    }
}
