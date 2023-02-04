// <copyright file="UsersService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Linq;
using System.Threading.Tasks;

using EducateMe.Data;
using EducateMe.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducateMe.Services.Data;

public class UsersService : IUsersService
{
    private readonly ApplicationDbContext applicationDbContext;

    public UsersService(ApplicationDbContext applicationDbContext)
    {
        this.applicationDbContext = applicationDbContext;
    }

    public async Task SetUsersOrganizationId(string userId, int organizationId)
    {
        var user = await this.applicationDbContext.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        user.OrganizationId = organizationId;
        await this.applicationDbContext.SaveChangesAsync();
    }

    public async Task SetUsersStudentId(string userId, int studentId)
    {
        var user = await this.applicationDbContext.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        user.StudentId = studentId;
        await this.applicationDbContext.SaveChangesAsync();
    }

    public async Task<int> GetUsersStudentId(string userId)
    {
        var studentId = await this.applicationDbContext.Users.Where(x => x.Id == userId).Select(x => x.StudentId).FirstAsync();

        return (int)studentId;
    }

    public async Task<int> GetUsersOrganizationId(string userId)
    {
        var organizationId = await this.applicationDbContext.Users.Where(x => x.Id == userId).Select(x => x.OrganizationId).FirstAsync();

        return (int)organizationId;
    }
}
