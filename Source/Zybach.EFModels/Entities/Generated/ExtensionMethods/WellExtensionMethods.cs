//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[Well]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WellExtensionMethods
    {
        public static WellDto AsDto(this Well well)
        {
            var wellDto = new WellDto()
            {
                WellID = well.WellID,
                WellRegistrationID = well.WellRegistrationID,
                WellTPID = well.WellTPID,
                WellTPNRDPumpRate = well.WellTPNRDPumpRate,
                TPNRDPumpRateUpdated = well.TPNRDPumpRateUpdated,
                WellConnectedMeter = well.WellConnectedMeter,
                WellAuditPumpRate = well.WellAuditPumpRate,
                AuditPumpRateUpdated = well.AuditPumpRateUpdated,
                HasElectricalData = well.HasElectricalData,
                FetchDate = well.FetchDate,
                RegisteredPumpRate = well.RegisteredPumpRate,
                RegisteredUpdated = well.RegisteredUpdated,
                StreamflowZone = well.StreamflowZone?.AsDto(),
                LandownerName = well.LandownerName,
                FieldName = well.FieldName
            };
            DoCustomMappings(well, wellDto);
            return wellDto;
        }

        static partial void DoCustomMappings(Well well, WellDto wellDto);

    }
}