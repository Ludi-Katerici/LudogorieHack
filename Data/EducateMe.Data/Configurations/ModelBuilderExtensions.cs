// <copyright file="ModelBuilderExtensions.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using EducateMe.Data.Models;
using EducateMe.Data.Models.Common;
using EducateMe.Data.Models.Common.RelationshipModels;
using Microsoft.EntityFrameworkCore;

namespace EducateMe.Data.Configurations;

public static class ModelBuilderExtensions
{
    public static void ConfigureRelations(this ModelBuilder builder)
    {
        builder.Entity<EventCategory>().HasKey(x => new { x.CategoryId, x.EventId });

        builder.Entity<EventInterest>().HasKey(x => new { x.InterestId, x.EventId });

        builder.Entity<EventStudent>().HasKey(x => new { x.StudentId, x.EventId });

        builder.Entity<StudentCategory>().HasKey(x => new { x.CategoryId, x.StudentId });

        builder.Entity<StudentInterest>().HasKey(x => new { x.InterestId, x.StudentId });

        builder.Entity<ApplicationUser>(
            entity =>
            {
                entity
                    .HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(e => e.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(e => e.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasMany(e => e.Roles)
                    .WithOne()
                    .HasForeignKey(e => e.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                const string shouldHaveStudentId =
                    $"{nameof(ApplicationUser.OrganizationId)} IS NULL AND {nameof(ApplicationUser.StudentId)} IS NOT NULL";

                const string shouldHaveOrganizationId =
                    $"{nameof(ApplicationUser.OrganizationId)} IS NOT NULL AND {nameof(ApplicationUser.StudentId)} IS NULL";

                /*entity.HasCheckConstraint(
                    "chk_shouldHaveStudentOrOrganization",
                    $"({shouldHaveStudentId}) OR ({shouldHaveOrganizationId})");*/
            });

        builder.Entity<Student>(
            student =>
            {
                student
                    .HasOne(x => x.City)
                    .WithMany(x => x.Students)
                    .HasForeignKey(x => x.CityId);

                student
                    .HasMany(x => x.Interests)
                    .WithOne(x => x.Student)
                    .HasForeignKey(x => x.StudentId);

                student
                    .HasMany(x => x.Categories)
                    .WithOne(x => x.Student)
                    .HasForeignKey(x => x.StudentId);

                student
                    .HasMany(x => x.Events)
                    .WithOne(x => x.Student)
                    .HasForeignKey(x => x.StudentId);
            });

        builder.Entity<Organization>(
            organization =>
            {
                organization
                    .HasOne(x => x.City)
                    .WithMany(x => x.Organizations)
                    .HasForeignKey(x => x.CityId);
            });

        builder.Entity<Event>(
            _event =>
            {
                _event
                    .HasOne(x => x.City)
                    .WithMany(x => x.Events)
                    .HasForeignKey(x => x.CityId);

                _event
                    .HasOne(x => x.Organization)
                    .WithMany(x => x.Events)
                    .HasForeignKey(x => x.OrganizationId);

                _event
                    .HasMany(x => x.Categories)
                    .WithOne(x => x.Event)
                    .HasForeignKey(x => x.EventId);

                _event
                    .HasMany(x => x.Interests)
                    .WithOne(x => x.Event)
                    .HasForeignKey(x => x.EventId);

                _event
                    .HasMany(x => x.Students)
                    .WithOne(x => x.Event)
                    .HasForeignKey(x => x.EventId);
            });

        builder.Entity<Interest>(
            interest =>
            {
                interest
                    .HasMany(x => x.Students)
                    .WithOne(x => x.Interest)
                    .HasForeignKey(x => x.InterestId);

                interest
                    .HasMany(x => x.Events)
                    .WithOne(x => x.Interest)
                    .HasForeignKey(x => x.EventId);

                interest
                    .HasIndex(x => x.Name)
                    .IsUnique();
            });

        builder.Entity<Category>(
            category =>
            {
                category
                    .HasMany(x => x.Students)
                    .WithOne(x => x.Category)
                    .HasForeignKey(x => x.StudentId);

                category
                    .HasMany(x => x.Events)
                    .WithOne(x => x.Category)
                    .HasForeignKey(x => x.EventId);

                category
                    .HasIndex(x => x.Name)
                    .IsUnique();
            });
    }
}
