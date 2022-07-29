using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    public partial class MonthlyWaterVolumeSummary
    {
        public MonthlyWaterVolumeSummary()
        {
        }

        public int AgHubIrrigationUnitID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public double? ContinuityMeterPumpedVolumeAcreFeet { get; set; }
        public double? ElectricalUsagePumpedVolumeAcreFeet { get; set; }
        public double? FlowMeterPumpedVolumeAcreFeet { get; set; }
        public decimal? EvapotranspirationAcreFeet { get; set; }
        public decimal? PrecipitationAcreFeet { get; set; }
        
        public static IEnumerable<MonthlyWaterVolumeSummary> AggregateMonthlyWaterVolumesByIrrigationUnit(ZybachDbContext dbContext)
        {
            return dbContext.MonthlyWaterVolumeSummaries
                .FromSqlRaw($"EXECUTE dbo.pMonthlyWaterVolumeSummaries")
                .ToList();
        }
    }
}