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
            .Select(x => new AgHubIrrigationUnitRunoffSimpleDto
            {
                AgHubIrrigationUnitRunoffID = x.AgHubIrrigationUnitRunoffID,
                AgHubIrrigationUnitID = x.AgHubIrrigationUnitID,
                Year = x.Year,
                Month = x.Month,
                Day = x.Day,
                CurveNumber = x.CurveNumber,
                Precipitation = x.Precipitation,
                RunoffDepth = x.RunoffDepth,
                RunoffVolume = x.RunoffVolume
            })
            .ToListAsync();

        return runoffs;
    }
}