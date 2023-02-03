// <copyright file="AzureStorage.cs" company="PlaceholderCompany">Copyright (c) PlaceholderCompany. All rights reserved.</copyright>

#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using EducateMe.Web.AzureServices;
using EducateMe.Web.AzureServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EducateMe.Services;

public class AzureStorage : IAzureStorage
{
    private readonly string storageConnectionString;
    private readonly string storageContainerName;

    public AzureStorage(IConfiguration configuration)
    {
        this.storageConnectionString = configuration.GetValue<string>("BlobConnectionString")!;
        this.storageContainerName = configuration.GetValue<string>("BlobContainerName")!;
    }

    public static BlobSasBuilder BlobSasBuilderOptions(BlobContainerClient client)
    {
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = client.Name,
            Resource = "c",
            ExpiresOn = DateTimeOffset.UtcNow.AddYears(1),
        };

        sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);

        return sasBuilder;
    }

    public async Task<List<BlobDto>> ListAsync()
    {
        // Get a reference to a container named in appsettings.json
        var container = new BlobContainerClient(this.storageConnectionString, this.storageContainerName);

        // Create a new list object for
        var files = new List<BlobDto>();

        await foreach (var file in container.GetBlobsAsync())
        {
            // Add each file retrieved from the storage container to the files list by creating a BlobDto object
            var uri = container.Uri.ToString();
            var name = file.Name;
            var fullUri = $"{uri}/{name}";

            files.Add(
                new BlobDto
            {
                Uri = fullUri,
                Name = name,
                ContentType = file.Properties.ContentType,
            });
        }

        // Return all files to the requesting method
        return files;
    }

    public async Task<BlobResponseDto> UploadAsync(IFormFile blob)
    {
        // Create new upload response object that we can return to the requesting method
        var response = new BlobResponseDto();

        // Get a reference to a container named in appsettings.json and then create it
        var container = new BlobContainerClient(this.storageConnectionString, this.storageContainerName);

        // await container.CreateAsync();
        try
        {
            var fileExtension = blob.FileName.Split(".").Last();
            var blobName = Guid.NewGuid() + "." + fileExtension;

            // Get a reference to the blob just uploaded from the API in a container from configuration settings
            var client = container.GetBlobClient(blobName);

            // Open a stream for the file we want to upload
            await using (var data = File.Create(blobName))
            {
                await blob.CopyToAsync(data);
                data.Position = 0;

                // Upload the file async
                await client.UploadAsync(data);
            }

            if (client.CanGenerateSasUri)
            {
                response.Blob.Uri = client.GenerateSasUri(BlobSasBuilderOptions(container)).AbsoluteUri;
            }
            else
            {
                response.Status = $"File {blob.FileName} couldn't generate sas uri";
                response.Error = true;
                response.Blob.Name = client.Name;
            }

            // Everything is OK and file got uploaded
            response.Status = $"File {blob.FileName} Uploaded Successfully";
            response.Error = false;
            response.Blob.Name = client.Name;
            File.Delete(blobName);
        }

        // If we get an unexpected error, we catch it here and return the error message
        catch (RequestFailedException ex)
        {
            // Log error to console and create a new response we can return to the requesting method
            Console.WriteLine(
                $"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
            response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
            response.Error = true;
            return response;
        }

        // Return the BlobUploadResponse object
        return response;
    }

    public async Task<BlobDto?> DownloadAsync(string blobFilename)
    {
        // Get a reference to a container named in appsettings.json
        var client = new BlobContainerClient(this.storageConnectionString, this.storageContainerName);

        try
        {
            // Get a reference to the blob uploaded earlier from the API in the container from configuration settings
            var file = client.GetBlobClient(blobFilename);

            // Check if the file exists in the container
            if (await file.ExistsAsync())
            {
                if (client.CanGenerateSasUri)
                {
                    var absoluteUri = file.GenerateSasUri(BlobSasBuilderOptions(client)).AbsoluteUri;

                    return new BlobDto { Name = file.Name, Uri = absoluteUri };
                }
            }
        }
        catch (RequestFailedException ex)
            when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
        {
            // Log error to console
            Console.WriteLine($"File {blobFilename} was not found");
        }

        // File does not exist, return null and handle that in requesting method
        return null;
    }

    public async Task<BlobResponseDto> DeleteAsync(string blobFilename)
    {
        var client = new BlobContainerClient(this.storageConnectionString, this.storageContainerName);

        var file = client.GetBlobClient(blobFilename);

        try
        {
            // Delete the file
            await file.DeleteAsync();
        }
        catch (RequestFailedException ex)
            when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
        {
            // File did not exist, log to console and return new response to requesting method
            Console.WriteLine($"File {blobFilename} was not found");
            return new BlobResponseDto { Error = true, Status = $"File with name {blobFilename} not found." };
        }

        // Return a new BlobResponseDto to the requesting method
        return new BlobResponseDto { Error = false, Status = $"File: {blobFilename} has been successfully deleted." };
    }
}
