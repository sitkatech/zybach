using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public static partial class PrismMonthlySyncExtensionMethods
{
    static partial void DoCustomSimpleDtoMappings(PrismMonthlySync prismMonthlySync, PrismMonthlySyncSimpleDto prismMonthlySyncSimpleDto)
    {
        prismMonthlySyncSimpleDto.PrismSyncStatusName = prismMonthlySync.PrismSyncStatus.PrismSyncStatusName;
        prismMonthlySyncSimpleDto.PrismSyncStatusDisplayName = prismMonthlySync.PrismSyncStatus.PrismSyncStatusDisplayName;
        prismMonthlySyncSimpleDto.PrismDataTypeName = prismMonthlySync.PrismDataType.PrismDataTypeName;
        prismMonthlySyncSimpleDto.PrismDataTypeDisplayName = prismMonthlySync.PrismDataType.PrismDataTypeDisplayName;
        prismMonthlySyncSimpleDto.LastSynchronizedByUserFullName = prismMonthlySync.LastSynchronizedByUser?.FullName;
        prismMonthlySyncSimpleDto.LastRunoffCalculatedByUserFullName = prismMonthlySync.LastSynchronizedByUser?.FullName;
        prismMonthlySyncSimpleDto.FinalizedByUserFullName = prismMonthlySync.FinalizeByUser?.FullName;
    }
}