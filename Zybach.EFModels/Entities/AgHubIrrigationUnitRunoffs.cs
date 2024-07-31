using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public static class AgHubIrrigationUnitRunoffs
{
    public static async Task<List<AgHubIrrigationUnitRunoffSimpleDto>> ListSimpleForIrrigationUnitID(ZybachDbContext dbContext, int irrigationUnitID)
    {
        var runoffs = await dbContext.AgHubIrrigationUnitRunoffs
            .Where(x => x.AgHubIrrigationUnitID == irrigationUnitID)
            .OrderBy(x => x.Year).ThenBy(x => x.Month).ThenBy(x => x.Day)
            .Select(x => new AgHubIrrigationUnitRunoffSimpleDto
            {
                AgHubIrrigationUnitRunoffID = x.AgHubIrrigationUnitRunoffID,
                AgHubIrrigationUnitID = x.AgHubIrrigationUnitID,
                WellTPID = x.AgHubIrrigationUnit.WellTPID,
                Year = x.Year,
                Month = x.Month,
                Day = x.Day,
                CropType = x.CropType,
                Tillage = x.Tillage,
                CurveNumber = x.CurveNumber,
                Precipitation = x.Precipitation,
                Area = x.Area,
                RunoffDepth = x.RunoffDepth,
                RunoffVolume = x.RunoffVolume
            })
            .ToListAsync();

        return runoffs;
    }

    public static async Task<List<AgHubIrrigationUnitRunoffSimpleDto>> ListSimpleForYear(ZybachDbContext dbContext, int year)
    {
        var runoffs = await dbContext.AgHubIrrigationUnitRunoffs
            .Where(x => x.Year == year)
            .OrderBy(x => x.Year).ThenBy(x => x.Month).ThenBy(x => x.Day)
            .Select(x => new AgHubIrrigationUnitRunoffSimpleDto
            {
                AgHubIrrigationUnitRunoffID = x.AgHubIrrigationUnitRunoffID,
                AgHubIrrigationUnitID = x.AgHubIrrigationUnitID,
                WellTPID = x.AgHubIrrigationUnit.WellTPID,
                Year = x.Year,
                Month = x.Month,
                Day = x.Day,
                CropType = x.CropType,
                Tillage = x.Tillage,
                CurveNumber = x.CurveNumber,
                Precipitation = x.Precipitation,
                Area = x.Area,
                RunoffDepth = x.RunoffDepth,
                RunoffVolume = x.RunoffVolume
            })
            .ToListAsync();

        return runoffs;
    }
}