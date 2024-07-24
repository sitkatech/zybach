//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PrismMonthlySync]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class PrismMonthlySyncExtensionMethods
    {
        public static PrismMonthlySyncDto AsDto(this PrismMonthlySync prismMonthlySync)
        {
            var prismMonthlySyncDto = new PrismMonthlySyncDto()
            {
                PrismMonthlySyncID = prismMonthlySync.PrismMonthlySyncID,
                PrismSyncStatus = prismMonthlySync.PrismSyncStatus.AsDto(),
                PrismDataType = prismMonthlySync.PrismDataType.AsDto(),
                Year = prismMonthlySync.Year,
                Month = prismMonthlySync.Month,
                LastSynchronizedDate = prismMonthlySync.LastSynchronizedDate,
                LastSynchronizedByUser = prismMonthlySync.LastSynchronizedByUser?.AsDto(),
                FinalizeDate = prismMonthlySync.FinalizeDate,
                FinalizeByUser = prismMonthlySync.FinalizeByUser?.AsDto()
            };
            DoCustomMappings(prismMonthlySync, prismMonthlySyncDto);
            return prismMonthlySyncDto;
        }

        static partial void DoCustomMappings(PrismMonthlySync prismMonthlySync, PrismMonthlySyncDto prismMonthlySyncDto);

        public static PrismMonthlySyncSimpleDto AsSimpleDto(this PrismMonthlySync prismMonthlySync)
        {
            var prismMonthlySyncSimpleDto = new PrismMonthlySyncSimpleDto()
            {
                PrismMonthlySyncID = prismMonthlySync.PrismMonthlySyncID,
                PrismSyncStatusID = prismMonthlySync.PrismSyncStatusID,
                PrismDataTypeID = prismMonthlySync.PrismDataTypeID,
                Year = prismMonthlySync.Year,
                Month = prismMonthlySync.Month,
                LastSynchronizedDate = prismMonthlySync.LastSynchronizedDate,
                LastSynchronizedByUserID = prismMonthlySync.LastSynchronizedByUserID,
                FinalizeDate = prismMonthlySync.FinalizeDate,
                FinalizeByUserID = prismMonthlySync.FinalizeByUserID
            };
            DoCustomSimpleDtoMappings(prismMonthlySync, prismMonthlySyncSimpleDto);
            return prismMonthlySyncSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(PrismMonthlySync prismMonthlySync, PrismMonthlySyncSimpleDto prismMonthlySyncSimpleDto);
    }
}