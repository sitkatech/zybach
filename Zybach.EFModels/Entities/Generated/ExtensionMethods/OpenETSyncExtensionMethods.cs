//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[OpenETSync]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class OpenETSyncExtensionMethods
    {
        public static OpenETSyncDto AsDto(this OpenETSync openETSync)
        {
            var openETSyncDto = new OpenETSyncDto()
            {
                OpenETSyncID = openETSync.OpenETSyncID,
                OpenETDataType = openETSync.OpenETDataType.AsDto(),
                Year = openETSync.Year,
                Month = openETSync.Month,
                FinalizeDate = openETSync.FinalizeDate
            };
            DoCustomMappings(openETSync, openETSyncDto);
            return openETSyncDto;
        }

        static partial void DoCustomMappings(OpenETSync openETSync, OpenETSyncDto openETSyncDto);

        public static OpenETSyncSimpleDto AsSimpleDto(this OpenETSync openETSync)
        {
            var openETSyncSimpleDto = new OpenETSyncSimpleDto()
            {
                OpenETSyncID = openETSync.OpenETSyncID,
                OpenETDataTypeID = openETSync.OpenETDataTypeID,
                Year = openETSync.Year,
                Month = openETSync.Month,
                FinalizeDate = openETSync.FinalizeDate
            };
            DoCustomSimpleDtoMappings(openETSync, openETSyncSimpleDto);
            return openETSyncSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(OpenETSync openETSync, OpenETSyncSimpleDto openETSyncSimpleDto);
    }
}