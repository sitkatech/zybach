using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public static class PrismMonthlySyncs
{
    public static async Task<List<PrismMonthlySyncSimpleDto>> ListSimpleByYearAndDataType(ZybachDbContext dbContext, int year, PrismDataTypeDto prismDataType)
    {
        var syncRecords = await dbContext.PrismMonthlySyncs
            .AsNoTracking()
            .Where(x => x.Year == year && x.PrismDataTypeID == prismDataType.PrismDataTypeID)
            .Select(x => new PrismMonthlySyncSimpleDto
            {
                PrismMonthlySyncID = x.PrismMonthlySyncID,

                PrismDataTypeID = x.PrismDataTypeID,
                PrismDataTypeName = x.PrismDataType.PrismDataTypeName,
                PrismDataTypeDisplayName = x.PrismDataType.PrismDataTypeDisplayName,

                PrismSyncStatusID = x.PrismSyncStatusID,
                PrismSyncStatusName = x.PrismSyncStatus.PrismSyncStatusName,
                PrismSyncStatusDisplayName = x.PrismSyncStatus.PrismSyncStatusDisplayName,

                RunoffCalculationStatusID = x.RunoffCalculationStatusID,
                RunoffCalculationStatusName = x.RunoffCalculationStatus.RunoffCalculationStatusName,
                RunoffCalculationStatusDisplayName = x.RunoffCalculationStatus.RunoffCalculationStatusDisplayName,

                Year = x.Year,
                Month = x.Month,

                LastSynchronizedDate = x.LastSynchronizedDate,
                LastSynchronizedByUserFullName = x.LastSynchronizedByUser != null
                    ? x.LastSynchronizedByUser.FullName
                    : null,

                LastRunoffCalculationDate = x.LastRunoffCalculationDate,
                LastRunoffCalculatedByUserFullName = x.LastRunoffCalculatedByUser != null
                    ? x.LastRunoffCalculatedByUser.FullName
                    : null,

                FinalizeDate = x.FinalizeDate,
                FinalizedByUserFullName = x.FinalizeByUser != null
                    ? x.FinalizeByUser.FullName
                    : null,
            })
            .ToListAsync();

        return syncRecords;
    }

    public static async Task<PrismMonthlySyncSimpleDto> GetSimpleByYearMonthAndDataType(ZybachDbContext dbContext, int year, int month, PrismDataType prismDataType)
    {
        var syncRecord = await dbContext.PrismMonthlySyncs
            .AsNoTracking()
            .Where(x => x.Year == year && x.Month == month && x.PrismDataTypeID == prismDataType.PrismDataTypeID)
            .Select(x => new PrismMonthlySyncSimpleDto
            {
                PrismMonthlySyncID = x.PrismMonthlySyncID,

                PrismDataTypeID = x.PrismDataTypeID,
                PrismDataTypeName = x.PrismDataType.PrismDataTypeName,
                PrismDataTypeDisplayName = x.PrismDataType.PrismDataTypeDisplayName,

                PrismSyncStatusID = x.PrismSyncStatusID,
                PrismSyncStatusName = x.PrismSyncStatus.PrismSyncStatusName,
                PrismSyncStatusDisplayName = x.PrismSyncStatus.PrismSyncStatusDisplayName,

                RunoffCalculationStatusID = x.RunoffCalculationStatusID,
                RunoffCalculationStatusName = x.RunoffCalculationStatus.RunoffCalculationStatusName,
                RunoffCalculationStatusDisplayName = x.RunoffCalculationStatus.RunoffCalculationStatusDisplayName,

                Year = x.Year,
                Month = x.Month,

                LastSynchronizedDate = x.LastSynchronizedDate,
                LastSynchronizedByUserFullName = x.LastSynchronizedByUser != null
                    ? x.LastSynchronizedByUser.FullName
                    : null,

                LastRunoffCalculationDate = x.LastRunoffCalculationDate,
                LastRunoffCalculatedByUserFullName = x.LastRunoffCalculatedByUser != null
                    ? x.LastRunoffCalculatedByUser.FullName
                    : null,

                FinalizeDate = x.FinalizeDate,
                FinalizedByUserFullName = x.FinalizeByUser != null
                    ? x.FinalizeByUser.FullName
                    : null,
            })
            .FirstOrDefaultAsync();

        return syncRecord;
    }

    public static async Task<PrismMonthlySyncDto> Finalize(ZybachDbContext dbContext, UserDto callingUser, int year, int month, PrismDataType prismDataType)
    {
        var syncRecord = await dbContext.PrismMonthlySyncs
            .Where(x => x.Year == year && x.Month == month && x.PrismDataTypeID == prismDataType.PrismDataTypeID)
            .FirstOrDefaultAsync();

        syncRecord.FinalizeDate = DateTime.UtcNow;
        syncRecord.FinalizeByUserID = callingUser.UserID;

        dbContext.Update(syncRecord);
        await dbContext.SaveChangesAsync();

        return syncRecord.AsDto();
    }

    public static async Task<PrismMonthlySyncDto> UpdateStatus(ZybachDbContext dbContext, UserDto callingUser, int year, int month, PrismDataType prismDataType, PrismSyncStatus prismStatus)
    {
        var syncRecord = await dbContext.PrismMonthlySyncs
            .Where(x => x.Year == year && x.Month == month && x.PrismDataTypeID == prismDataType.PrismDataTypeID)
            .FirstOrDefaultAsync();

        syncRecord.PrismSyncStatusID = prismStatus.PrismSyncStatusID;

        if (prismStatus == PrismSyncStatus.InProgress)
        {
            syncRecord.LastSynchronizedByUserID = callingUser.UserID;
            syncRecord.LastSynchronizedDate = DateTime.UtcNow;
        }

        dbContext.Update(syncRecord);
        await dbContext.SaveChangesAsync();

        return syncRecord.AsDto();
    }

    public static async Task<PrismMonthlySyncDto> UpdateRunoffCalculationStatus(ZybachDbContext dbContext, UserDto callingUser, int year, int month, RunoffCalculationStatus runoffCalculationStatus, string error = null)
    {
        var syncRecord = await dbContext.PrismMonthlySyncs
            .Where(x => x.Year == year && x.Month == month && x.PrismDataTypeID == PrismDataType.ppt.PrismDataTypeID)
            .FirstOrDefaultAsync();

        syncRecord.RunoffCalculationStatusID = runoffCalculationStatus.RunoffCalculationStatusID;

        if (runoffCalculationStatus == RunoffCalculationStatus.InProgress)
        {
            syncRecord.LastRunoffCalculatedByUserID = callingUser.UserID;
            syncRecord.LastRunoffCalculationDate = DateTime.UtcNow;
        }

        if (runoffCalculationStatus == RunoffCalculationStatus.Failed)
        {
            syncRecord.LastRunoffCalculationError = error;
        }

        dbContext.Update(syncRecord);
        await dbContext.SaveChangesAsync();

        return syncRecord.AsDto();
    }
}