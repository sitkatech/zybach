using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public class AgHubIrrigationUnitSummary
    {
        public AgHubIrrigationUnitSummary()
        {
        }

        public int AgHubIrrigationUnitID { get; set; }

        public decimal? TotalEvapotranspirationInches { get; set; }
        public decimal? TotalPrecipitationInches { get; set; }

        public double? TotalEvapotranspirationGallons { get; set; }
        public double? TotalPrecipitationGallons { get; set; }

        public double? FlowMeterPumpedVolumeGallonsTotal { get; set; }
        public double? ContinuityMeterPumpedVolumeGallonsTotal { get; set; }
        public double? ElectricalUsagePumpedVolumeGallonsTotal { get; set; }

        public double? FlowMeterPumpedDepthInchesTotal { get; set; }
        public double? ContinuityMeterPumpedDepthInchesTotal { get; set; }
        public double? ElectricalUsagePumpedDepthInchesTotal { get; set; }

        public static IEnumerable<AgHubIrrigationUnitSummaryDto> GetForDateRange(ZybachDbContext dbContext, int startDateYear, int startDateMonth, int endDateYear, int endDateMonth)
        {
            var ahiuSummaries = dbContext.AgHubIrrigationUnitSummaries
                .FromSqlRaw(
                    $"EXECUTE dbo.pAgHubIrrigationUnitSummariesByDateRange @startDateMonth, @startDateYear, @endDateMonth, @endDateYear",
                    new SqlParameter("startDateMonth", startDateMonth),
                    new SqlParameter("startDateYear", startDateYear),
                    new SqlParameter("endDateMonth", endDateMonth),
                    new SqlParameter("endDateYear", endDateYear)
                    )
                .ToList();

            var irrigationUnitSimples = AgHubIrrigationUnits.ListAsSimpleDto(dbContext);

            var ahiuSummaryDtos = ahiuSummaries.Join(irrigationUnitSimples,
                x => x.AgHubIrrigationUnitID, y => y.AgHubIrrigationUnitID,
                (x, y) => new AgHubIrrigationUnitSummaryDto()
                {
                    AgHubIrrigationUnitID = x.AgHubIrrigationUnitID,
                    WellTPID = y.WellTPID,
                    IrrigationUnitAreaInAcres = y.IrrigationUnitAreaInAcres,
                    AssociatedWells = y.AssociatedWells,

                    TotalEvapotranspirationInches = x.TotalEvapotranspirationInches,
                    TotalEvapotranspirationGallons = x.TotalEvapotranspirationGallons,

                    TotalPrecipitationInches = x.TotalPrecipitationInches,
                    TotalPrecipitationGallons = x.TotalPrecipitationGallons,

                    FlowMeterPumpedVolumeGallons = x.FlowMeterPumpedVolumeGallonsTotal,
                    FlowMeterPumpedDepthInches = x.FlowMeterPumpedDepthInchesTotal,

                    ContinuityMeterPumpedVolumeGallons = x.ContinuityMeterPumpedVolumeGallonsTotal,
                    ContinuityMeterPumpedDepthInches = x.ContinuityMeterPumpedDepthInchesTotal,

                    ElectricalUsagePumpedVolumeGallons = x.ElectricalUsagePumpedVolumeGallonsTotal,
                    ElectricalUsagePumpedDepthInches = x.ElectricalUsagePumpedDepthInchesTotal
                })
                .OrderBy(x => x.WellTPID);
            
            return ahiuSummaryDtos;
        }

    }

}