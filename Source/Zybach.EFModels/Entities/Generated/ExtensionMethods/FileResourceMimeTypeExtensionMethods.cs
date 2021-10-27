//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[FileResourceMimeType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class FileResourceMimeTypeExtensionMethods
    {
        public static FileResourceMimeTypeDto AsDto(this FileResourceMimeType fileResourceMimeType)
        {
            var fileResourceMimeTypeDto = new FileResourceMimeTypeDto()
            {
                FileResourceMimeTypeID = fileResourceMimeType.FileResourceMimeTypeID,
                FileResourceMimeTypeName = fileResourceMimeType.FileResourceMimeTypeName,
                FileResourceMimeTypeDisplayName = fileResourceMimeType.FileResourceMimeTypeDisplayName,
                FileResourceMimeTypeContentTypeName = fileResourceMimeType.FileResourceMimeTypeContentTypeName,
                FileResourceMimeTypeIconSmallFilename = fileResourceMimeType.FileResourceMimeTypeIconSmallFilename,
                FileResourceMimeTypeIconNormalFilename = fileResourceMimeType.FileResourceMimeTypeIconNormalFilename
            };
            DoCustomMappings(fileResourceMimeType, fileResourceMimeTypeDto);
            return fileResourceMimeTypeDto;
        }

        static partial void DoCustomMappings(FileResourceMimeType fileResourceMimeType, FileResourceMimeTypeDto fileResourceMimeTypeDto);

    }
}