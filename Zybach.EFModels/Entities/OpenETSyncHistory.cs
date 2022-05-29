using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class OpenETSyncHistory
    {
        public static OpenETSyncHistoryDto CreateNew(ZybachDbContext dbContext, int waterYearMonthID, int openETDataTypeID)
        {
            var waterYearMonth = dbContext.WaterYearMonths.Single(x => x.WaterYearMonthID == waterYearMonthID);
            
            var openETSyncHistoryToAdd = new OpenETSyncHistory()
            {
                OpenETSyncResultTypeID = (int)OpenETSyncResultTypeEnum.Created,
                WaterYearMonthID = waterYearMonthID,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                OpenETDataTypeID = openETDataTypeID
            };

            dbContext.OpenETSyncHistories.Add(openETSyncHistoryToAdd);
            dbContext.SaveChanges();
            dbContext.Entry(openETSyncHistoryToAdd).Reload();

            return GetByOpenETSyncHistoryID(dbContext, openETSyncHistoryToAdd.OpenETSyncHistoryID);
        }

        public static OpenETSyncHistoryDto GetByOpenETSyncHistoryID(ZybachDbContext dbContext, int openETSyncHistoryID)
        {
            return dbContext.OpenETSyncHistories
                .Include(x => x.WaterYearMonth)
                .SingleOrDefault(x => x.OpenETSyncHistoryID == openETSyncHistoryID).AsDto();
        }
        public static OpenETSyncHistoryDto UpdateOpenETSyncEntityByID(ZybachDbContext zybachDbContext, int openETSyncHistoryID, OpenETSyncResultTypeEnum resultType)
        {
            return UpdateOpenETSyncEntityByID(zybachDbContext, openETSyncHistoryID, resultType, null);
        }

        public static OpenETSyncHistoryDto UpdateOpenETSyncEntityByID(ZybachDbContext zybachDbContext, int openETSyncHistoryID, OpenETSyncResultTypeEnum resultType, string errorMessage)
        {
            return UpdateOpenETSyncEntityByID(zybachDbContext, openETSyncHistoryID, resultType, errorMessage, null);
        }

        public static OpenETSyncHistoryDto UpdateOpenETSyncEntityByID(ZybachDbContext zybachDbContext, int openETSyncHistoryID, OpenETSyncResultTypeEnum resultType, string errorMessage, string googleBucketFileRetrievalURL)
        {
            var openETSyncHistory =
                zybachDbContext.OpenETSyncHistories.Single(x => x.OpenETSyncHistoryID == openETSyncHistoryID);

            openETSyncHistory.UpdateDate = DateTime.UtcNow;
            openETSyncHistory.OpenETSyncResultTypeID = (int)resultType;
            if (resultType == OpenETSyncResultTypeEnum.Failed)
            {
                openETSyncHistory.ErrorMessage = errorMessage;
            }

            //Once this is set it should never change
            if (String.IsNullOrWhiteSpace(openETSyncHistory.GoogleBucketFileRetrievalURL))
            {
                openETSyncHistory.GoogleBucketFileRetrievalURL = googleBucketFileRetrievalURL;
            }
            
            zybachDbContext.SaveChanges();
            zybachDbContext.Entry(openETSyncHistory).Reload();

            return GetByOpenETSyncHistoryID(zybachDbContext, openETSyncHistory.OpenETSyncHistoryID);
        }

        public static List<OpenETSyncHistoryDto> List(ZybachDbContext dbContext)
        {
            return dbContext.OpenETSyncHistories
                .Include(x => x.WaterYearMonth)
                .OrderByDescending(x => x.CreateDate).Select(x => x.AsDto()).ToList();
        }
    }
}
