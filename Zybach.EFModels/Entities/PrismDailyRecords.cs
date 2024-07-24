using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public static class PrismDailyRecords
{
    public static async Task<List<PrismDailyRecordSimpleDto>> ListSimpleByYearMonthAndDataType(ZybachDbContext dbContext, int year, int month, PrismDataType prismDataType)
    {
        var dailyRecords = await dbContext.PrismDailyRecords
            .AsNoTracking()
            .Where(x => x.Year == year && x.Month == month && x.PrismDataTypeID == prismDataType.PrismDataTypeID)
            .Select(x => new PrismDailyRecordSimpleDto
            {
                PrismDailyRecordID = x.PrismDailyRecordID,
                Year = x.Year,
                Month = x.Month,
                Day = x.Day,
                BlobResourceID = x.BlobResourceID, 
                BlobResourceCanonicalName = x.BlobResource.BlobResourceCanonicalName,
                BlobFileName = x.BlobResource.OriginalBaseFilename
            })
            .ToListAsync();

        return dailyRecords;
    }
}