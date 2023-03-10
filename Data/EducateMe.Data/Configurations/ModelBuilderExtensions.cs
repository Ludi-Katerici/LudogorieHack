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
        builder.Entity<EventCategory>(
            entity =>
            {
                entity.HasKey(x => new { x.CategoryId, x.EventId });
                entity
                    .HasOne(x => x.Event)
                    .WithMany(x => x.Categories)
                    .HasForeignKey(x => x.EventId);
                entity
                    .HasOne(x => x.Category)
                    .WithMany(x => x.Events)
                    .HasForeignKey(x => x.CategoryId);
            });

        builder.Entity<EventInterest>(
            entity =>
            {
                entity.HasKey(x => new { x.InterestId, x.EventId });
                entity
                    .HasOne(x => x.Event)
                    .WithMany(x => x.Interests)
                    .HasForeignKey(x => x.EventId);
                entity
                    .HasOne(x => x.Interest)
                    .WithMany(x => x.Events)
                    .HasForeignKey(x => x.InterestId);
            });

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
                    .HasMany(x => x.Favourites)
                    .WithOne(x => x.Student)
                    .HasForeignKey(x => x.StudentId);

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
                    .HasMany(x => x.Favourites)
                    .WithOne(x => x.Event)
                    .HasForeignKey(x => x.EventId);

                _event
                    .HasOne(x => x.Organization)
                    .WithMany(x => x.Events)
                    .HasForeignKey(x => x.OrganizationId);

                /*
                _event
                    .HasMany(x => x.Categories)
                    .WithOne(x => x.Event)
                    .HasForeignKey(x => x.EventId);

                _event
                    .HasMany(x => x.Interests)
                    .WithOne(x => x.Event)
                    .HasForeignKey(x => x.EventId);*/

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
                    .HasIndex(x => x.Name)
                    .IsUnique();
            });
    }
}
