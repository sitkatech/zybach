using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DroolTool.EFModels.Entities
{
    public partial class FileResourceMimeType
    {
        public static FileResourceMimeType GetFileResourceMimeTypeByContentTypeName(DroolToolDbContext dbContext, string contentTypeName)
        {
            return dbContext.FileResourceMimeType.Single(x => x.FileResourceMimeTypeContentTypeName == contentTypeName);
        }
    }
}