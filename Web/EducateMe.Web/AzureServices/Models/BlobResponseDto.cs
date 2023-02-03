// <copyright file="BlobResponseDto.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

#nullable enable
namespace EducateMe.Web.AzureServices.Models;

public class BlobResponseDto
{
    public BlobResponseDto()
    {
        this.Blob = new BlobDto();
    }

    public string? Status { get; set; }

    public bool Error { get; set; }

    public BlobDto Blob { get; set; }
}
