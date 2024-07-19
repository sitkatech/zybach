//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[BlobResource]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class BlobResourceExtensionMethods
    {
        public static BlobResourceDto AsDto(this BlobResource blobResource)
        {
            var blobResourceDto = new BlobResourceDto()
            {
                BlobResourceID = blobResource.BlobResourceID,
                BlobResourceGUID = blobResource.BlobResourceGUID,
                BlobResourceCanonicalName = blobResource.BlobResourceCanonicalName,
                OriginalBaseFilename = blobResource.OriginalBaseFilename,
                OriginalFileExtension = blobResource.OriginalFileExtension,
                CreateUser = blobResource.CreateUser.AsDto(),
                CreateDate = blobResource.CreateDate
            };
            DoCustomMappings(blobResource, blobResourceDto);
            return blobResourceDto;
        }

        static partial void DoCustomMappings(BlobResource blobResource, BlobResourceDto blobResourceDto);

        public static BlobResourceSimpleDto AsSimpleDto(this BlobResource blobResource)
        {
            var blobResourceSimpleDto = new BlobResourceSimpleDto()
            {
                BlobResourceID = blobResource.BlobResourceID,
                BlobResourceGUID = blobResource.BlobResourceGUID,
                BlobResourceCanonicalName = blobResource.BlobResourceCanonicalName,
                OriginalBaseFilename = blobResource.OriginalBaseFilename,
                OriginalFileExtension = blobResource.OriginalFileExtension,
                CreateUserID = blobResource.CreateUserID,
                CreateDate = blobResource.CreateDate
            };
            DoCustomSimpleDtoMappings(blobResource, blobResourceSimpleDto);
            return blobResourceSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(BlobResource blobResource, BlobResourceSimpleDto blobResourceSimpleDto);
    }
}