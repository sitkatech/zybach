//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[OpenETSyncHistory]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class OpenETSyncHistoryExtensionMethods
    {
        public static OpenETSyncHistoryDto AsDto(this OpenETSyncHistory openETSyncHistory)
        {
            var openETSyncHistoryDto = new OpenETSyncHistoryDto()
            {
                OpenETSyncHistoryID = openETSyncHistory.OpenETSyncHistoryID,
                WaterYearMonth = openETSyncHistory.WaterYearMonth.AsDto(),
                OpenETSyncResultType = openETSyncHistory.OpenETSyncResultType.AsDto(),
                CreateDate = openETSyncHistory.CreateDate,
                UpdateDate = openETSyncHistory.UpdateDate,
                GoogleBucketFileRetrievalURL = openETSyncHistory.GoogleBucketFileRetrievalURL,
                ErrorMessage = openETSyncHistory.ErrorMessage,
                OpenETDataType = openETSyncHistory.OpenETDataType?.AsDto(),
                OpenETSync = openETSyncHistory.OpenETSync?.AsDto()
            };
            DoCustomMappings(openETSyncHistory, openETSyncHistoryDto);
            return openETSyncHistoryDto;
        }

        static partial void DoCustomMappings(OpenETSyncHistory openETSyncHistory, OpenETSyncHistoryDto openETSyncHistoryDto);

        public static OpenETSyncHistorySimpleDto AsSimpleDto(this OpenETSyncHistory openETSyncHistory)
        {
            var openETSyncHistorySimpleDto = new OpenETSyncHistorySimpleDto()
            {
                OpenETSyncHistoryID = openETSyncHistory.OpenETSyncHistoryID,
                WaterYearMonthID = openETSyncHistory.WaterYearMonthID,
                OpenETSyncResultTypeID = openETSyncHistory.OpenETSyncResultTypeID,
                CreateDate = openETSyncHistory.CreateDate,
                UpdateDate = openETSyncHistory.UpdateDate,
                GoogleBucketFileRetrievalURL = openETSyncHistory.GoogleBucketFileRetrievalURL,
                ErrorMessage = openETSyncHistory.ErrorMessage,
                OpenETDataTypeID = openETSyncHistory.OpenETDataTypeID,
                OpenETSyncID = openETSyncHistory.OpenETSyncID
            };
            DoCustomSimpleDtoMappings(openETSyncHistory, openETSyncHistorySimpleDto);
            return openETSyncHistorySimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(OpenETSyncHistory openETSyncHistory, OpenETSyncHistorySimpleDto openETSyncHistorySimpleDto);
    }
}