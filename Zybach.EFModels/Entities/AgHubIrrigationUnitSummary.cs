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

        public const double AcreInchesToGallonsConversionRate = 27154.2857;

        public int AgHubIrrigationUnitID { get; set; }

        public decimal? TotalEvapotranspirationInches { get; set; }
        public decimal? TotalPrecipitationInches { get; set; }

        public double? FlowMeterPumpedVolumeGallonsTotal { get; set; }
        public double? ContinuityMeterPumpedVolumeGallonsTotal { get; set; }
        public double? ElectricalUsagePumpedVolumeGallonsTotal { get; set; }

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
                    TotalEvapotranspirationGallons = x.TotalEvapotranspirationInches * (decimal?)y.IrrigationUnitAreaInAcres * (decimal?)AcreInchesToGallonsConversionRate,

                    TotalPrecipitationInches = x.TotalPrecipitationInches,
                    TotalPrecipitationGallons = x.TotalPrecipitationInches * (decimal?)y.IrrigationUnitAreaInAcres * (decimal?)AcreInchesToGallonsConversionRate,

                    FlowMeterPumpedVolumeGallons = x.FlowMeterPumpedVolumeGallonsTotal,
                    FlowMeterPumpedDepthInches = (x.FlowMeterPumpedVolumeGallonsTotal / AcreInchesToGallonsConversionRate) / y.IrrigationUnitAreaInAcres,

                    ContinuityMeterPumpedVolumeGallons = x.ContinuityMeterPumpedVolumeGallonsTotal,
                    ContinuityMeterPumpedDepthInches = (x.ContinuityMeterPumpedVolumeGallonsTotal / AcreInchesToGallonsConversionRate) / y.IrrigationUnitAreaInAcres,

                    ElectricalUsagePumpedVolumeGallons = x.ElectricalUsagePumpedVolumeGallonsTotal,
                    ElectricalUsagePumpedDepthInches = (x.ElectricalUsagePumpedVolumeGallonsTotal / AcreInchesToGallonsConversionRate) / y.IrrigationUnitAreaInAcres,
                })
                .OrderBy(x => x.WellTPID);

            return ahiuSummaryDtos;
        }

    }

}