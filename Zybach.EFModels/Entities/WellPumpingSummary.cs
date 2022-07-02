using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Rio.EFModels.Entities
{
    public partial class WellPumpingSummary
    {
        public WellPumpingSummary()
        {
        }

        public int WellID { get; set; }
        public string WellRegistrationID { get; set; }
        public string OwnerName { get; set; }
        public int? MostRecentSupportTicketID { get; set; }
        public string MostRecentSupportTicketTitle { get; set; }
        public int? HasFlowMeter { get; set; }
        public int? HasContinuityMeter { get; set; }
        public int? HasElectricalUsage { get; set; }
        public double? FlowMeterPumpedVolume { get; set; }
        public double? ContinuityMeterPumpedVolume { get; set; }
        public double? ElectricalUsagePumpedVolume { get; set; }
        public double? FlowMeterContinuityMeterDifference { get; set; }
        public double? FlowMeterElectricalUsageDifference { get; set; }

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
                HasFlowMeter = x.HasFlowMeter > 0,
                HasContinuityMeter = x.HasContinuityMeter > 0,
                HasElectricalUsage = x.HasElectricalUsage > 0,
                FlowMeterPumpedVolume = x.FlowMeterPumpedVolume,
                ContinuityMeterPumpedVolume = x.ContinuityMeterPumpedVolume,
                ElectricalUsagePumpedVolume = x.ElectricalUsagePumpedVolume,
                FlowMeterContinuityMeterDifference = x.FlowMeterContinuityMeterDifference,
                FlowMeterElectricalUsageDifference = x.FlowMeterElectricalUsageDifference
            });

            return wellPumpingSummaryDtos;
        }
    }
}