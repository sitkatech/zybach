using System.Linq;

namespace Zybach.EFModels.Entities
{
    public partial class FileResourceMimeType
    {
        public static FileResourceMimeType GetFileResourceMimeTypeByContentTypeName(ZybachDbContext dbContext, string contentTypeName)
        {
            return dbContext.FileResourceMimeTypes.Single(x => x.FileResourceMimeTypeContentTypeName == contentTypeName);
        }
    }
}