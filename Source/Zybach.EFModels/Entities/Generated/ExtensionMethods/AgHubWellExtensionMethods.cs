//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWell]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubWellExtensionMethods
    {
        public static AgHubWellDto AsDto(this AgHubWell agHubWell)
        {
            var agHubWellDto = new AgHubWellDto()
            {
                AgHubWellID = agHubWell.AgHubWellID,
                WellRegistrationID = agHubWell.WellRegistrationID,
                WellTPID = agHubWell.WellTPID,
                TPNRDPumpRate = agHubWell.TPNRDPumpRate,
                TPNRDPumpRateUpdated = agHubWell.TPNRDPumpRateUpdated,
                WellConnectedMeter = agHubWell.WellConnectedMeter,
                WellAuditPumpRate = agHubWell.WellAuditPumpRate,
                AuditPumpRateUpdated = agHubWell.AuditPumpRateUpdated,
                HasElectricalData = agHubWell.HasElectricalData,
                FetchDate = agHubWell.FetchDate
            };
            DoCustomMappings(agHubWell, agHubWellDto);
            return agHubWellDto;
        }

        static partial void DoCustomMappings(AgHubWell agHubWell, AgHubWellDto agHubWellDto);

    }
}