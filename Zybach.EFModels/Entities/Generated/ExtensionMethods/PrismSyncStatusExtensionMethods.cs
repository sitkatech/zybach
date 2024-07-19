//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PrismSyncStatus]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class PrismSyncStatusExtensionMethods
    {
        public static PrismSyncStatusDto AsDto(this PrismSyncStatus prismSyncStatus)
        {
            var prismSyncStatusDto = new PrismSyncStatusDto()
            {
                PrismSyncStatusID = prismSyncStatus.PrismSyncStatusID,
                PrismSyncStatusName = prismSyncStatus.PrismSyncStatusName,
                PrismSyncStatusDisplayName = prismSyncStatus.PrismSyncStatusDisplayName
            };
            DoCustomMappings(prismSyncStatus, prismSyncStatusDto);
            return prismSyncStatusDto;
        }

        static partial void DoCustomMappings(PrismSyncStatus prismSyncStatus, PrismSyncStatusDto prismSyncStatusDto);

        public static PrismSyncStatusSimpleDto AsSimpleDto(this PrismSyncStatus prismSyncStatus)
        {
            var prismSyncStatusSimpleDto = new PrismSyncStatusSimpleDto()
            {
                PrismSyncStatusID = prismSyncStatus.PrismSyncStatusID,
                PrismSyncStatusName = prismSyncStatus.PrismSyncStatusName,
                PrismSyncStatusDisplayName = prismSyncStatus.PrismSyncStatusDisplayName
            };
            DoCustomSimpleDtoMappings(prismSyncStatus, prismSyncStatusSimpleDto);
            return prismSyncStatusSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(PrismSyncStatus prismSyncStatus, PrismSyncStatusSimpleDto prismSyncStatusSimpleDto);
    }
}