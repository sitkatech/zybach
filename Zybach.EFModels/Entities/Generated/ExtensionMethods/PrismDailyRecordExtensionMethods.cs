//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PrismDailyRecord]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class PrismDailyRecordExtensionMethods
    {
        public static PrismDailyRecordDto AsDto(this PrismDailyRecord prismDailyRecord)
        {
            var prismDailyRecordDto = new PrismDailyRecordDto()
            {
                PrismDailyRecordID = prismDailyRecord.PrismDailyRecordID,
                PrismMonthlySync = prismDailyRecord.PrismMonthlySync.AsDto(),
                PrismSyncStatus = prismDailyRecord.PrismSyncStatus.AsDto(),
                PrismDataType = prismDailyRecord.PrismDataType.AsDto(),
                BlobResource = prismDailyRecord.BlobResource?.AsDto(),
                Year = prismDailyRecord.Year,
                Month = prismDailyRecord.Month,
                Day = prismDailyRecord.Day,
                ErrorMessage = prismDailyRecord.ErrorMessage
            };
            DoCustomMappings(prismDailyRecord, prismDailyRecordDto);
            return prismDailyRecordDto;
        }

        static partial void DoCustomMappings(PrismDailyRecord prismDailyRecord, PrismDailyRecordDto prismDailyRecordDto);

        public static PrismDailyRecordSimpleDto AsSimpleDto(this PrismDailyRecord prismDailyRecord)
        {
            var prismDailyRecordSimpleDto = new PrismDailyRecordSimpleDto()
            {
                PrismDailyRecordID = prismDailyRecord.PrismDailyRecordID,
                PrismMonthlySyncID = prismDailyRecord.PrismMonthlySyncID,
                PrismSyncStatusID = prismDailyRecord.PrismSyncStatusID,
                PrismDataTypeID = prismDailyRecord.PrismDataTypeID,
                BlobResourceID = prismDailyRecord.BlobResourceID,
                Year = prismDailyRecord.Year,
                Month = prismDailyRecord.Month,
                Day = prismDailyRecord.Day,
                ErrorMessage = prismDailyRecord.ErrorMessage
            };
            DoCustomSimpleDtoMappings(prismDailyRecord, prismDailyRecordSimpleDto);
            return prismDailyRecordSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(PrismDailyRecord prismDailyRecord, PrismDailyRecordSimpleDto prismDailyRecordSimpleDto);
    }
}