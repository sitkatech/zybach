//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[FileResource]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class FileResourceExtensionMethods
    {
        public static FileResourceDto AsDto(this FileResource fileResource)
        {
            var fileResourceDto = new FileResourceDto()
            {
                FileResourceID = fileResource.FileResourceID,
                FileResourceMimeType = fileResource.FileResourceMimeType.AsDto(),
                OriginalBaseFilename = fileResource.OriginalBaseFilename,
                OriginalFileExtension = fileResource.OriginalFileExtension,
                FileResourceGUID = fileResource.FileResourceGUID,
                FileResourceData = fileResource.FileResourceData,
                CreateUser = fileResource.CreateUser.AsDto(),
                CreateDate = fileResource.CreateDate
            };
            DoCustomMappings(fileResource, fileResourceDto);
            return fileResourceDto;
        }

        static partial void DoCustomMappings(FileResource fileResource, FileResourceDto fileResourceDto);

    }
}