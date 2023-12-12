using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public class OpenETSyncHistories
    {
        public static OpenETSyncHistory CreateNew(ZybachDbContext dbContext, int year, int month, int openETDataTypeID)
        {
            var openETSync = dbContext.OpenETSyncs
                .SingleOrDefault(x => x.Year == year && x.Month == month && x.OpenETDataTypeID == openETDataTypeID) ?? new OpenETSync()
            {
                OpenETDataTypeID = openETDataTypeID,
                Year = year,
                Month = month
            };

            var openETSyncHistoryToAdd = new OpenETSyncHistory()
            {
                OpenETSyncResultTypeID = (int)OpenETSyncResultTypeEnum.Created,
                OpenETSync = openETSync,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                OpenETDataTypeID = openETDataTypeID
            };

            dbContext.OpenETSyncHistories.Add(openETSyncHistoryToAdd);
            dbContext.SaveChanges();
            dbContext.Entry(openETSyncHistoryToAdd).Reload();

            return GetByOpenETSyncHistoryID(dbContext, openETSyncHistoryToAdd.OpenETSyncHistoryID);
        }

        public static OpenETSyncHistory GetByOpenETSyncHistoryID(ZybachDbContext dbContext, int openETSyncHistoryID)
        {
            return dbContext.OpenETSyncHistories
                .Include(x => x.OpenETSync)
                .SingleOrDefault(x => x.OpenETSyncHistoryID == openETSyncHistoryID);
        }

        public static async Task<OpenETSyncHistory> UpdateOpenETSyncEntityByID(ZybachDbContext zybachDbContext, int openETSyncHistoryID, OpenETSyncResultTypeEnum resultType)
        {
            return await UpdateOpenETSyncEntityByID(zybachDbContext, openETSyncHistoryID, resultType, null);
        }

        public static async Task<OpenETSyncHistory> UpdateOpenETSyncEntityByID(ZybachDbContext zybachDbContext, int openETSyncHistoryID, OpenETSyncResultTypeEnum resultType, string errorMessage)
        {
            return await UpdateOpenETSyncEntityByID(zybachDbContext, openETSyncHistoryID, resultType, errorMessage, null);
        }

        public static async Task<OpenETSyncHistory> UpdateOpenETSyncEntityByID(ZybachDbContext zybachDbContext, int openETSyncHistoryID, OpenETSyncResultTypeEnum resultType, string errorMessage, string googleBucketFileRetrievalURL)
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
            if (string.IsNullOrWhiteSpace(openETSyncHistory.GoogleBucketFileRetrievalURL))
            {
                openETSyncHistory.GoogleBucketFileRetrievalURL = googleBucketFileRetrievalURL;
            }
            
            await zybachDbContext.SaveChangesAsync();
            await zybachDbContext.Entry(openETSyncHistory).ReloadAsync();

            return GetByOpenETSyncHistoryID(zybachDbContext, openETSyncHistory.OpenETSyncHistoryID);
        }

        public static List<OpenETSyncHistoryDto> List(ZybachDbContext dbContext)
        {
            return dbContext.OpenETSyncHistories
                .Include(x => x.OpenETSync)
                .OrderByDescending(x => x.CreateDate).Select(x => x.AsDto()).ToList();
        }
    }
}
