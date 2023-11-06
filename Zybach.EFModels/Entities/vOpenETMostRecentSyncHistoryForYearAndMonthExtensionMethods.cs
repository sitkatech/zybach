using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class vOpenETMostRecentSyncHistoryForYearAndMonthExtensionMethods
    {
        public static OpenETSyncHistoryDto AsOpenETSyncHistoryDto(
            this vOpenETMostRecentSyncHistoryForYearAndMonth vOpenETMostRecentSyncHistoryForYearAndMonth)
        {
            return new OpenETSyncHistoryDto()
            {
                OpenETSyncHistoryID = vOpenETMostRecentSyncHistoryForYearAndMonth.OpenETSyncHistoryID,
                OpenETSyncResultType = vOpenETMostRecentSyncHistoryForYearAndMonth.OpenETSyncResultType.AsDto(),
                OpenETSync = vOpenETMostRecentSyncHistoryForYearAndMonth.OpenETSync.AsDto(),
                CreateDate = vOpenETMostRecentSyncHistoryForYearAndMonth.CreateDate,
                UpdateDate = vOpenETMostRecentSyncHistoryForYearAndMonth.UpdateDate,
                GoogleBucketFileRetrievalURL = vOpenETMostRecentSyncHistoryForYearAndMonth.GoogleBucketFileRetrievalURL,
                ErrorMessage = vOpenETMostRecentSyncHistoryForYearAndMonth.ErrorMessage
            };
        }
    }
}