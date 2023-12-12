using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Zybach.EFModels.Entities;

namespace Zybach.API.Services
{
    public static class HttpUtilities
    {
        public static async Task<FileResource> MakeFileResourceFromFormFile(IFormFile file, ZybachDbContext dbContext, HttpContext httpContext)
        {
            byte[] inputFileContents;
            await using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                inputFileContents = ms.ToArray();
            }

            var userDto = UserContext.GetUserFromHttpContext(dbContext, httpContext);

            var fileResourceMimeType = FileResourceMimeTypes.GetFileResourceMimeTypeByContentTypeName(dbContext,
                file.ContentType);

            var clientFilename = file.FileName;
            var extension = clientFilename.Split('.').Last();
            var fileResourceGuid = Guid.NewGuid();

            return new FileResource
            {
                CreateDate = DateTime.Now,
                CreateUserID = userDto.UserID,
                FileResourceData = inputFileContents,
                FileResourceGUID = fileResourceGuid,
                FileResourceMimeTypeID = fileResourceMimeType.FileResourceMimeTypeID,
                OriginalBaseFilename = clientFilename,
                OriginalFileExtension = extension,
            };
        }

        public static async Task<byte[]> GetData(this HttpRequest httpRequest)
        {
            byte[] bytes;


            using (var ms = new MemoryStream(2048))
            {
                await httpRequest.Body.CopyToAsync(ms);
                bytes = ms.ToArray();
            }

            return bytes;
        }

    }
}