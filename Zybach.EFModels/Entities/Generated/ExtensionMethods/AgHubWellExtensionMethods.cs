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
                Well = agHubWell.Well.AsDto(),
                WellTPNRDPumpRate = agHubWell.WellTPNRDPumpRate,
                TPNRDPumpRateUpdated = agHubWell.TPNRDPumpRateUpdated,
                WellConnectedMeter = agHubWell.WellConnectedMeter,
                WellAuditPumpRate = agHubWell.WellAuditPumpRate,
                AuditPumpRateUpdated = agHubWell.AuditPumpRateUpdated,
                AuditPumpRateTested = agHubWell.AuditPumpRateTested,
                HasElectricalData = agHubWell.HasElectricalData,
                RegisteredPumpRate = agHubWell.RegisteredPumpRate,
                RegisteredUpdated = agHubWell.RegisteredUpdated,
                AgHubRegisteredUser = agHubWell.AgHubRegisteredUser,
                FieldName = agHubWell.FieldName,
                AgHubIrrigationUnit = agHubWell.AgHubIrrigationUnit?.AsDto()
            };
            DoCustomMappings(agHubWell, agHubWellDto);
            return agHubWellDto;
        }

        static partial void DoCustomMappings(AgHubWell agHubWell, AgHubWellDto agHubWellDto);

        public static AgHubWellSimpleDto AsSimpleDto(this AgHubWell agHubWell)
        {
            var agHubWellSimpleDto = new AgHubWellSimpleDto()
            {
                AgHubWellID = agHubWell.AgHubWellID,
                WellID = agHubWell.WellID,
                WellTPNRDPumpRate = agHubWell.WellTPNRDPumpRate,
                TPNRDPumpRateUpdated = agHubWell.TPNRDPumpRateUpdated,
                WellConnectedMeter = agHubWell.WellConnectedMeter,
                WellAuditPumpRate = agHubWell.WellAuditPumpRate,
                AuditPumpRateUpdated = agHubWell.AuditPumpRateUpdated,
                AuditPumpRateTested = agHubWell.AuditPumpRateTested,
                HasElectricalData = agHubWell.HasElectricalData,
                RegisteredPumpRate = agHubWell.RegisteredPumpRate,
                RegisteredUpdated = agHubWell.RegisteredUpdated,
                AgHubRegisteredUser = agHubWell.AgHubRegisteredUser,
                FieldName = agHubWell.FieldName,
                AgHubIrrigationUnitID = agHubWell.AgHubIrrigationUnitID
            };
            DoCustomSimpleDtoMappings(agHubWell, agHubWellSimpleDto);
            return agHubWellSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(AgHubWell agHubWell, AgHubWellSimpleDto agHubWellSimpleDto);
    }
}