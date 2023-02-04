// <copyright file="IUsersService.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Threading.Tasks;

namespace EducateMe.Services.Data.Interfaces;

public interface IUsersService
{
    Task SetUsersOrganizationId(string userId, int organizationId);

    Task SetUsersStudentId(string userId, int studentId);
}
