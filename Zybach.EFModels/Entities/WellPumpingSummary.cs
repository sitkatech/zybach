using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class WellPumpingSummary
    {
        public WellPumpingSummary()
        {
        }

        public const double GallonsToInchesConversionRate = 27154;

        public int WellID { get; set; }
        public string WellRegistrationID { get; set; }
        public string OwnerName { get; set; }
        public double? Acres { get; set; }
        public int? MostRecentSupportTicketID { get; set; }
        public string MostRecentSupportTicketTitle { get; set; }
        public int? WellAuditPumpRate { get; set; }
        public int? RegisteredPumpRate { get; set; }
        public int? WellTPNRDPumpRate { get; set; }
        public string FlowMeters { get; set; }
        public string ContinuityMeters { get; set; }
        public string ElectricalUsage { get; set; }
        public double? FlowMeterPumpedVolumeGallons { get; set; }
        public double? ContinuityMeterPumpedVolumeGallons { get; set; }
        public double? ElectricalUsagePumpedVolumeGallons { get; set; }

        public static IEnumerable<WellPumpingSummaryDto> GetForDateRange(ZybachDbContext dbContext, string startDate, string endDate)
        {
            var wellPumpingSummaries = dbContext.WellPumpingSummaries
                .FromSqlRaw($"EXECUTE dbo.pWellPumpingSummary @startDate, @endDate", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate))
                .ToList();

            var wellPumpingSummaryDtos = wellPumpingSummaries.OrderBy(x => x.WellRegistrationID).Select(x => new WellPumpingSummaryDto()
            {
                WellID = x.WellID,
                WellRegistrationID = x.WellRegistrationID,
                OwnerName = x.OwnerName,
                MostRecentSupportTicketID = x.MostRecentSupportTicketID,
                MostRecentSupportTicketTitle = x.MostRecentSupportTicketTitle,
                PumpingRateGallonsPerMinute = x.WellAuditPumpRate ?? x.RegisteredPumpRate ?? x.WellTPNRDPumpRate ?? 0,
                PumpingRateSource = x.WellAuditPumpRate != null ? "Audited" :
                    x.RegisteredPumpRate != null ? "Registered" :
                    x.WellTPNRDPumpRate != null ? "District" : "",
                FlowMeters = x.FlowMeters,
                ContinuityMeters = x.ContinuityMeters,
                ElectricalUsage = x.ElectricalUsage,
                FlowMeterPumpedVolumeGallons = x.FlowMeterPumpedVolumeGallons,
                FlowMeterPumpedDepthInches = (x.FlowMeterPumpedVolumeGallons / GallonsToInchesConversionRate) / x.Acres,
                ContinuityMeterPumpedVolumeGallons = x.ContinuityMeterPumpedVolumeGallons,
                ContinuityMeterPumpedDepthInches = (x.ContinuityMeterPumpedVolumeGallons / GallonsToInchesConversionRate) / x.Acres,
                ElectricalUsagePumpedVolumeGallons = x.ElectricalUsagePumpedVolumeGallons,
                ElectricalUsagePumpedDepthInches = (x.ElectricalUsagePumpedVolumeGallons / GallonsToInchesConversionRate) / x.Acres,
            });

            return wellPumpingSummaryDtos;
        }
    }
}