// <copyright file="BlobDto.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

#nullable enable
using System.IO;

namespace EducateMe.Web.AzureServices.Models;

public class BlobDto
{
    public string? Uri { get; set; }

    public string? Name { get; set; }

    public string? ContentType { get; set; }

    public Stream? Content { get; set; }
}
