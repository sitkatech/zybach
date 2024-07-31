using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public static class AgHubIrrigatedAcres
{
    public static async Task<List<AgHubWellIrrigatedAcreSimpleDto>> ListSimpleForIrrigationUnitID(ZybachDbContext dbContext, int irrigationUnitID)
    {
        var irrigatedAcres = await dbContext.AgHubWellIrrigatedAcres
            .AsNoTracking()
            .Where(x => x.AgHubWell.AgHubIrrigationUnitID == irrigationUnitID)
            .Select(x => x.AsSimpleDto())
            .ToListAsync();

        return irrigatedAcres;
    }
}