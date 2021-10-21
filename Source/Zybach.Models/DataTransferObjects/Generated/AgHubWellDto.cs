//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWell]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class AgHubWellDto
    {
        public int AgHubWellID { get; set; }
        public WellDto Well { get; set; }
        public string WellTPID { get; set; }
        public int? WellTPNRDPumpRate { get; set; }
        public DateTime? TPNRDPumpRateUpdated { get; set; }
        public bool WellConnectedMeter { get; set; }
        public int? WellAuditPumpRate { get; set; }
        public DateTime? AuditPumpRateUpdated { get; set; }
        public bool HasElectricalData { get; set; }
        public DateTime FetchDate { get; set; }
        public int? RegisteredPumpRate { get; set; }
        public DateTime? RegisteredUpdated { get; set; }
        public string AgHubRegisteredUser { get; set; }
        public string FieldName { get; set; }
    }
}