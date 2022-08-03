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

        public const double AcreInchesToGallonsConversionRate = 27154;

        public int AgHubIrrigationUnitID { get; set; }
        public double IrrigationUnitAreaInAcres { get; set; }

        public decimal? TotalEvapotranspirationInches { get; set; }
        public decimal? TotalPrecipitationInches { get; set; }

        public double? FlowMeterPumpedVolumeGallonsTotal { get; set; }
        public double? ContinuityMeterPumpedVolumeGallonsTotal { get; set; }
        public double? ElectricalUsagePumpedVolumeGallonsTotal { get; set; }

        public static IEnumerable<AgHubIrrigationUnitSummaryDto> GetForDateRange(ZybachDbContext dbContext,
            int startDateYear, int startDateMonth, int endDateYear, int endDateMonth)
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

            var ahiuSummaryDtos = ahiuSummaries.OrderBy(x => x.AgHubIrrigationUnitID).Select(x =>
                new AgHubIrrigationUnitSummaryDto()
                {
                    AgHubIrrigationUnitID = x.AgHubIrrigationUnitID,
                    IrrigationUnitAreaInAcres = x.IrrigationUnitAreaInAcres,

                    TotalEvapotranspirationInches = x.TotalEvapotranspirationInches,
                    TotalPrecipitationInches = x.TotalPrecipitationInches,

                    TotalEvapotranspirationGallons = x.TotalEvapotranspirationInches *
                                                     (decimal?)x.IrrigationUnitAreaInAcres *
                                                     (decimal?)AcreInchesToGallonsConversionRate,
                    TotalPrecipitationGallons = x.TotalPrecipitationInches *
                                                (decimal?)x.IrrigationUnitAreaInAcres *
                                                (decimal?)AcreInchesToGallonsConversionRate,

                    FlowMeterPumpedVolumeGallons = x.FlowMeterPumpedVolumeGallonsTotal,
                    FlowMeterPumpedDepthInches =
                        (x.FlowMeterPumpedVolumeGallonsTotal / AcreInchesToGallonsConversionRate) /
                        x.IrrigationUnitAreaInAcres,
                    ContinuityMeterPumpedVolumeGallons = x.ContinuityMeterPumpedVolumeGallonsTotal,
                    ContinuityMeterPumpedDepthInches =
                        (x.ContinuityMeterPumpedVolumeGallonsTotal / AcreInchesToGallonsConversionRate) /
                        x.IrrigationUnitAreaInAcres,
                    ElectricalUsagePumpedVolumeGallons = x.ElectricalUsagePumpedVolumeGallonsTotal,
                    ElectricalUsagePumpedDepthInches =
                        (x.ElectricalUsagePumpedVolumeGallonsTotal / AcreInchesToGallonsConversionRate) /
                        x.IrrigationUnitAreaInAcres,
                });

            return ahiuSummaryDtos;
        }

    }

}