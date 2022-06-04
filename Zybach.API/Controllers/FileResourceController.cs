using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zybach.API.Services;
using Zybach.EFModels.Entities;
using System;
using System.Linq;
using Microsoft.Extensions.Options;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class FileResourceController : SitkaController<FileResourceController>
    {
        public FileResourceController(ZybachDbContext dbContext, ILogger<FileResourceController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("FileResource/{fileResourceGuidAsString}")]
        public ActionResult DisplayResource(string fileResourceGuidAsString)
        {
            var isStringAGuid = Guid.TryParse(fileResourceGuidAsString, out var fileResourceGuid);
            if (isStringAGuid)
            {
                var fileResource = _dbContext.FileResources.SingleOrDefault(x => x.FileResourceGUID == fileResourceGuid);

                return DisplayResourceImpl(fileResourceGuidAsString, fileResource);
            }
            // Unhappy path - return an HTTP 404
            // ---------------------------------
            var message = $"File Resource {fileResourceGuidAsString} Not Found in database. It may have been deleted.";
            _logger.LogError(message);
            return NotFound(message);
        }

        private ActionResult DisplayResourceImpl(string fileResourcePrimaryKey, FileResource fileResource)
        {
            if (fileResource == null)
            {
                var message = $"File Resource {fileResourcePrimaryKey} Not Found in database. It may have been deleted.";
                _logger.LogError(message);
                return NotFound(message);
            }

            switch (fileResource.FileResourceMimeType.FileResourceMimeTypeName)
            {
                case "X-PNG":
                case "PNG":
                case "TIFF":
                case "BMP":
                case "GIF":
                case "JPEG":
                case "PJPEG":
                    return File(fileResource.FileResourceData, fileResource.FileResourceMimeType.FileResourceMimeTypeContentTypeName);
                case "Word (DOCX)":
                case "Word (DOC)":
                    return File(fileResource.FileResourceData, fileResource.FileResourceMimeType.FileResourceMimeTypeContentTypeName, fileResource.OriginalBaseFilename);
                default:
                    throw new NotSupportedException("Only image uploads are supported at this time.");
            }
        }

    }
}
