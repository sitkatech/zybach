//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellStaging]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WellStagingExtensionMethods
    {
        public static WellStagingDto AsDto(this WellStaging wellStaging)
        {
            var wellStagingDto = new WellStagingDto()
            {
                WellStagingID = wellStaging.WellStagingID,
                WellRegistrationID = wellStaging.WellRegistrationID,
                WellTPID = wellStaging.WellTPID,
                WellTPNRDPumpRate = wellStaging.WellTPNRDPumpRate,
                TPNRDPumpRateUpdated = wellStaging.TPNRDPumpRateUpdated,
                WellConnectedMeter = wellStaging.WellConnectedMeter,
                WellAuditPumpRate = wellStaging.WellAuditPumpRate,
                AuditPumpRateUpdated = wellStaging.AuditPumpRateUpdated,
                RegisteredPumpRate = wellStaging.RegisteredPumpRate,
                RegisteredUpdated = wellStaging.RegisteredUpdated,
                HasElectricalData = wellStaging.HasElectricalData,
                AgHubRegisteredUser = wellStaging.AgHubRegisteredUser,
                FieldName = wellStaging.FieldName
            };
            DoCustomMappings(wellStaging, wellStagingDto);
            return wellStagingDto;
        }

        static partial void DoCustomMappings(WellStaging wellStaging, WellStagingDto wellStagingDto);

    }
}