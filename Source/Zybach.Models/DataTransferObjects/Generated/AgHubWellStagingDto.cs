//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWellStaging]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class AgHubWellStagingDto
    {
        public int AgHubWellStagingID { get; set; }
        public string WellRegistrationID { get; set; }
        public string WellTPID { get; set; }
        public int? TPNRDPumpRate { get; set; }
        public DateTime? TPNRDPumpRateUpdated { get; set; }
        public bool WellConnectedMeter { get; set; }
        public int? WellAuditPumpRate { get; set; }
        public DateTime? AuditPumpRateUpdated { get; set; }
        public bool HasElectricalData { get; set; }
    }
}