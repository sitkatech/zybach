using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Zybach.EFModels.Entities;

namespace Zybach.API.Services;

public class BlobService
{
    private readonly ILogger<BlobService> _logger;
    private readonly IAzureStorage _azureStorage;

    public const string FileContainerName = "files";

    public BlobService(ILogger<BlobService> logger, IAzureStorage azureStorage)
    {
        _logger = logger;
        _azureStorage = azureStorage;
    }

    public string MakePrettyFileName(string filename)
    {
        var replacedFileName = filename
                              .Replace("(", "")
                              .Replace(")", "")
                              .Replace(" ", "")
                              .Replace(",", "")
                              .Replace(":", "")
                              .Replace(";", "")
                              .Replace("\"", "")
                              .Replace("&", "")
                              .Replace("#", "")
                              .Replace("'", "")
                              .Replace("/", "")
                              .Replace("\\", "")
                              .Replace(" ", "");

        return replacedFileName;
    }

    public async Task<Stream> GetFileStreamFromBlobStorage(string canonicalName)
    {
        try
        {
            var blobDto = await _azureStorage.DownloadAsync(FileContainerName, canonicalName);
            return blobDto.Content;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }

        return null;
    }

    public async Task SaveFileStreamToAzureBlobStorage(string canonicalName, Stream stream)
    {
        _logger.LogInformation($"Saving file stream {canonicalName} to {FileContainerName}");
        stream.Seek(0, SeekOrigin.Begin);
        await _azureStorage.UploadAsync(FileContainerName, canonicalName, stream);
    }

    public async Task<BlobResource> CreateBlobResource(ZybachDbContext dbContext, IFormFile file, int createUserID)
    {
        _logger.LogInformation($"Creating new File Resource from IFormFile {file.Name}");
        var canonicalName = Guid.NewGuid();
        await using var stream = file.OpenReadStream();
        stream.Seek(0, SeekOrigin.Begin);
        var uploadedFile = await _azureStorage.UploadAsync(FileContainerName, canonicalName.ToString(), stream);

        if (uploadedFile.Error)
        {
            throw new Exception($"There was an error uploading the FormFile \"{file.FileName}\" to blob storage with the canonical name \"{canonicalName}\". Error Details: {uploadedFile.Status}");
        }

        var fileNameSegments = file.FileName.Split(".");
        var newBlobResource = new BlobResource()
        {
            CreateDate = DateTime.UtcNow,
            CreateUserID = createUserID,
            BlobResourceCanonicalName = canonicalName.ToString(),
            BlobResourceGUID = canonicalName,
            OriginalFileExtension = fileNameSegments.Last(),
            OriginalBaseFilename = String.Join(".", fileNameSegments.Take(fileNameSegments.Length - 1)),
        };

        dbContext.BlobResources.Add(newBlobResource);
        await dbContext.SaveChangesAsync();
        await dbContext.Entry(newBlobResource).ReloadAsync();
        return newBlobResource;
    }

    public async Task<BlobResource> CreateBlobResource(ZybachDbContext dbContext, Stream stream, string fullFileName, int createUserID)
    {
        var canonicalName = Guid.NewGuid();
        stream.Seek(0, SeekOrigin.Begin);
        var uploadedFile = await _azureStorage.UploadAsync(FileContainerName, canonicalName.ToString(), stream);

        if (uploadedFile.Error)
        {
            throw new Exception($"There was an error uploading the FormFile \"{fullFileName}\" to blob storage with the canonical name \"{canonicalName}\". Error Details: {uploadedFile.Status}");
        }

        var fileNameSegments = fullFileName.Split(".");
        var newBlobResource = new BlobResource()
        {
            CreateDate = DateTime.UtcNow,
            CreateUserID = createUserID,
            BlobResourceCanonicalName = canonicalName.ToString(),
            BlobResourceGUID = canonicalName,
            OriginalFileExtension = fileNameSegments.Last(),
            OriginalBaseFilename = string.Join(".", fileNameSegments.Take(fileNameSegments.Length - 1)),
        };

        dbContext.BlobResources.Add(newBlobResource);
        await dbContext.SaveChangesAsync();
        await dbContext.Entry(newBlobResource).ReloadAsync();
        return newBlobResource;
    }

    //public async Task<FileStream> CreateZipFileFromBlobResources(List<BlobResource> blobResources)
    //{
    //    using var memoryStream = new MemoryStream();
    //    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
    //    {
    //        foreach (var fileResource in blobResources)
    //        {
    //            var fileInZip = archive.CreateEntry($"{fileResource.OriginalBaseFilename}.{fileResource.OriginalFileExtension}");
    //            var blobStream = await GetFileStreamFromBlobStorage(fileResource.BlobResourceCanonicalName);

    //            await using var s = fileInZip.Open();
    //            await blobStream.CopyToAsync(s);
    //        }
    //    }

    //    var disposableTempFile = DisposableTempFile.MakeDisposableTempFileEndingIn(".zip");
    //    var fileStream = new FileStream(disposableTempFile.FileInfo.FullName, FileMode.Create);
    //    memoryStream.Seek(0, SeekOrigin.Begin);
    //    await memoryStream.CopyToAsync(fileStream);
    //    fileStream.Seek(0, SeekOrigin.Begin);
    //    return fileStream;
    //}

    public async void DeleteFileStreamFromBlobStorage(string canonicalName)
    {
        try
        {
            await _azureStorage.DeleteAsync(FileContainerName, canonicalName);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }
}