using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    public partial class AgHubIrrigationUnitMonthlyWaterVolumeSummary
    {
        public AgHubIrrigationUnitMonthlyWaterVolumeSummary()
        {
        }

        public int AgHubIrrigationUnitID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public double? PumpedVolumeAcreFeet { get; set; }
        public decimal? EvapotranspirationAcreFeet { get; set; }
        public decimal? PrecipitationAcreFeet { get; set; }
        
        public static IEnumerable<AgHubIrrigationUnitMonthlyWaterVolumeSummary> List(ZybachDbContext dbContext)
        {
            return dbContext.MonthlyWaterVolumeSummaries
                .FromSqlRaw($"EXECUTE dbo.pAgHubIrrigationUnitMonthlyWaterVolumeSummaries")
                .ToList();
        }
    }
}