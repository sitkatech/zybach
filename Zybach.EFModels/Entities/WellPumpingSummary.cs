using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
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
        public SupportTicketSimpleDto MostRecentSupportTicket { get; set; }
        public bool HasContinuityMeter { get; set; }
        public bool HasElectricalUsage { get; set; }
        public double? FlowMeterPumpedVolume { get; set; }
        public double? ContinuityMeterPumpedVolume { get; set; }
        public double? ElectricalUsagePumpedVolume { get; set; }

        public static IEnumerable<WellPumpingSummaryDto> GetForDateRange(ZybachDbContext dbContext, DateTime startDate, DateTime endDate)
        {
            //var wellPumpingSummaries = dbContext.WellPumpingSummaries
            //    .FromSqlRaw($"EXECUTE dbo.pWellPumpingSummary @startDate, @endDate", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate))
            //    .ToList();

            //var wellPumpingSummaryDtos = wellPumpingSummaries.OrderBy(x => x.WellRegistrationID).Select(x => new WellPumpingSummaryDto()
            //{
            //    WellID = x.WellID,
            //    WellRegistrationID = x.WellRegistrationID,
            //    OwnerName = x.OwnerName,
            //    MostRecentSupportTicket = x.MostRecentSupportTicket,
            //    HasContinuityMeter = x.HasContinuityMeter,
            //    HasElectricalUsage = x.HasElectricalUsage,
            //    FlowMeterPumpedVolume = x.FlowMeterPumpedVolume,
            //    ContinuityMeterPumpedVolume = x.ContinuityMeterPumpedVolume,
            //    ElectricalUsagePumpedVolume = x.ElectricalUsagePumpedVolume
            //});

            //return wellPumpingSummaryDtos;

            return new List<WellPumpingSummaryDto>();
        }
    }
}