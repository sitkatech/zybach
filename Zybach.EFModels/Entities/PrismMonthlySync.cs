using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public partial class PrismMonthlySync
{
    public static async Task<List<PrismMonthlySyncSimpleDto>> Get(ZybachDbContext dbContext, int year, PrismDataTypeDto prismDataType)
    {
        var syncRecords = await dbContext.PrismMonthlySyncs
            .AsNoTracking()
            .Where(x => x.Year == year && x.PrismDataTypeID == prismDataType.PrismDataTypeID)
            .Select(x => new PrismMonthlySyncSimpleDto
            {
                PrismMonthlySyncID = x.PrismMonthlySyncID,
                PrismSyncStatusID = x.PrismSyncStatusID,
                PrismDataTypeID = x.PrismDataTypeID,
                Year = x.Year,
                Month = x.Month,
                FinalizeDate = x.FinalizeDate
            })
            .ToListAsync();

        return syncRecords;
    }

    public static async Task<PrismMonthlySyncSimpleDto> Get(ZybachDbContext dbContext, int year, int month, PrismDataType prismDataType)
    {
        var syncRecord = await dbContext.PrismMonthlySyncs
            .AsNoTracking()
            .Where(x => x.Year == year && x.Month == month && x.PrismDataTypeID == prismDataType.PrismDataTypeID)
            .Select(x => new PrismMonthlySyncSimpleDto
            {
                PrismMonthlySyncID = x.PrismMonthlySyncID,
                PrismSyncStatusID = x.PrismSyncStatusID,
                PrismDataTypeID = x.PrismDataTypeID,
                Year = x.Year,
                Month = x.Month,
                FinalizeDate = x.FinalizeDate
            })
            .FirstOrDefaultAsync();

        return syncRecord;
    }

    public static async Task<PrismMonthlySyncDto> Finalize(ZybachDbContext dbContext, UserDto callingUser, int year, int month, PrismDataType prismDataType, PrismMonthlySyncUpsertDto upsertDto)
    {
        var syncRecord = await dbContext.PrismMonthlySyncs
            .AsNoTracking()
            .Where(x => x.Year == year && x.Month == month && x.PrismDataTypeID == prismDataType.PrismDataTypeID)
            .FirstOrDefaultAsync();

        syncRecord.FinalizeDate = upsertDto.FinalizedDate;
        syncRecord.FinalizeByUserID = callingUser.UserID;

        dbContext.Update(syncRecord);
        await dbContext.SaveChangesAsync();
        
        return syncRecord.AsDto();
    }

    public static async Task<PrismMonthlySyncDto> UpdateStatus(ZybachDbContext dbContext, UserDto callingUser, int year, int month, PrismDataType prismDataType, PrismSyncStatus prismStatus)
    {
        var syncRecord = await dbContext.PrismMonthlySyncs
            .AsNoTracking()
            .Where(x => x.Year == year && x.Month == month && x.PrismDataTypeID == prismDataType.PrismDataTypeID)
            .FirstOrDefaultAsync();

        syncRecord.PrismSyncStatusID = prismStatus.PrismSyncStatusID;

        dbContext.Update(syncRecord);
        await dbContext.SaveChangesAsync();

        return syncRecord.AsDto();
    }
}