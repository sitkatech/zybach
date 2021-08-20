//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWellStaging]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubWellStagingExtensionMethods
    {
        public static AgHubWellStagingDto AsDto(this AgHubWellStaging agHubWellStaging)
        {
            var agHubWellStagingDto = new AgHubWellStagingDto()
            {
                AgHubWellStagingID = agHubWellStaging.AgHubWellStagingID,
                WellRegistrationID = agHubWellStaging.WellRegistrationID,
                WellTPID = agHubWellStaging.WellTPID,
                TPNRDPumpRate = agHubWellStaging.TPNRDPumpRate,
                TPNRDPumpRateUpdated = agHubWellStaging.TPNRDPumpRateUpdated,
                WellConnectedMeter = agHubWellStaging.WellConnectedMeter,
                WellAuditPumpRate = agHubWellStaging.WellAuditPumpRate,
                AuditPumpRateUpdated = agHubWellStaging.AuditPumpRateUpdated,
                HasElectricalData = agHubWellStaging.HasElectricalData,
                FetchDate = agHubWellStaging.FetchDate
            };
            DoCustomMappings(agHubWellStaging, agHubWellStagingDto);
            return agHubWellStagingDto;
        }

        static partial void DoCustomMappings(AgHubWellStaging agHubWellStaging, AgHubWellStagingDto agHubWellStagingDto);

    }
}