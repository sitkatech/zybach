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
                WellTPNRDPumpRate = agHubWellStaging.WellTPNRDPumpRate,
                TPNRDPumpRateUpdated = agHubWellStaging.TPNRDPumpRateUpdated,
                WellConnectedMeter = agHubWellStaging.WellConnectedMeter,
                WellAuditPumpRate = agHubWellStaging.WellAuditPumpRate,
                AuditPumpRateUpdated = agHubWellStaging.AuditPumpRateUpdated,
                RegisteredPumpRate = agHubWellStaging.RegisteredPumpRate,
                RegisteredUpdated = agHubWellStaging.RegisteredUpdated,
                HasElectricalData = agHubWellStaging.HasElectricalData,
                AgHubRegisteredUser = agHubWellStaging.AgHubRegisteredUser,
                FieldName = agHubWellStaging.FieldName
            };
            DoCustomMappings(agHubWellStaging, agHubWellStagingDto);
            return agHubWellStagingDto;
        }

        static partial void DoCustomMappings(AgHubWellStaging agHubWellStaging, AgHubWellStagingDto agHubWellStagingDto);

        public static AgHubWellStagingSimpleDto AsSimpleDto(this AgHubWellStaging agHubWellStaging)
        {
            var agHubWellStagingSimpleDto = new AgHubWellStagingSimpleDto()
            {
                AgHubWellStagingID = agHubWellStaging.AgHubWellStagingID,
                WellRegistrationID = agHubWellStaging.WellRegistrationID,
                WellTPID = agHubWellStaging.WellTPID,
                WellTPNRDPumpRate = agHubWellStaging.WellTPNRDPumpRate,
                TPNRDPumpRateUpdated = agHubWellStaging.TPNRDPumpRateUpdated,
                WellConnectedMeter = agHubWellStaging.WellConnectedMeter,
                WellAuditPumpRate = agHubWellStaging.WellAuditPumpRate,
                AuditPumpRateUpdated = agHubWellStaging.AuditPumpRateUpdated,
                RegisteredPumpRate = agHubWellStaging.RegisteredPumpRate,
                RegisteredUpdated = agHubWellStaging.RegisteredUpdated,
                HasElectricalData = agHubWellStaging.HasElectricalData,
                AgHubRegisteredUser = agHubWellStaging.AgHubRegisteredUser,
                FieldName = agHubWellStaging.FieldName
            };
            DoCustomSimpleDtoMappings(agHubWellStaging, agHubWellStagingSimpleDto);
            return agHubWellStagingSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(AgHubWellStaging agHubWellStaging, AgHubWellStagingSimpleDto agHubWellStagingSimpleDto);
    }
}