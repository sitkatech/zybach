//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnit]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubIrrigationUnitExtensionMethods
    {
        public static AgHubIrrigationUnitDto AsDto(this AgHubIrrigationUnit agHubIrrigationUnit)
        {
            var agHubIrrigationUnitDto = new AgHubIrrigationUnitDto()
            {
                AgHubIrrigationUnitID = agHubIrrigationUnit.AgHubIrrigationUnitID,
                WellTPID = agHubIrrigationUnit.WellTPID,
                IrrigationUnitAreaInAcres = agHubIrrigationUnit.IrrigationUnitAreaInAcres
            };
            DoCustomMappings(agHubIrrigationUnit, agHubIrrigationUnitDto);
            return agHubIrrigationUnitDto;
        }

        static partial void DoCustomMappings(AgHubIrrigationUnit agHubIrrigationUnit, AgHubIrrigationUnitDto agHubIrrigationUnitDto);

        public static AgHubIrrigationUnitSimpleDto AsSimpleDto(this AgHubIrrigationUnit agHubIrrigationUnit)
        {
            var agHubIrrigationUnitSimpleDto = new AgHubIrrigationUnitSimpleDto()
            {
                AgHubIrrigationUnitID = agHubIrrigationUnit.AgHubIrrigationUnitID,
                WellTPID = agHubIrrigationUnit.WellTPID,
                IrrigationUnitAreaInAcres = agHubIrrigationUnit.IrrigationUnitAreaInAcres
            };
            DoCustomSimpleDtoMappings(agHubIrrigationUnit, agHubIrrigationUnitSimpleDto);
            return agHubIrrigationUnitSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(AgHubIrrigationUnit agHubIrrigationUnit, AgHubIrrigationUnitSimpleDto agHubIrrigationUnitSimpleDto);
    }
}