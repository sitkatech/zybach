using System.Linq;

namespace Zybach.EFModels.Entities
{
    public partial class FileResourceMimeType
    {
        public static FileResourceMimeType GetFileResourceMimeTypeByContentTypeName(ZybachDbContext dbContext, string contentTypeName)
        {
            return FileResourceMimeType.All.Single(x => x.FileResourceMimeTypeContentTypeName == contentTypeName);
        }
    }
}