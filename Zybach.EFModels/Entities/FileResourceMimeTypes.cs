using System.Linq;

namespace Zybach.EFModels.Entities
{
    public class FileResourceMimeTypes
    {
        public static FileResourceMimeType GetFileResourceMimeTypeByContentTypeName(ZybachDbContext dbContext, string contentTypeName)
        {
            return FileResourceMimeType.All.Single(x => x.FileResourceMimeTypeContentTypeName == contentTypeName);
        }
    }
}