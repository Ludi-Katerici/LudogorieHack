// <copyright file="EnsureOneElementAttribute.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace EducateMe.Web.Infrastructure.ValidationAttributes;

public class EnsureOneElementAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var list = value as IList;
        if (list != null)
        {
            return list.Count > 0;
        }

        return false;
    }
}
