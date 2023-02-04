// <copyright file="FutureDateAttribute.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;

namespace EducateMe.Web.Infrastructure.ValidationAttributes;

public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value) // Return a boolean value: true == IsValid, false != IsValid
    {
        var d = Convert.ToDateTime(value);
        return d >= DateTime.Now; // Dates Greater than or equal to today are valid (true)
    }
}
