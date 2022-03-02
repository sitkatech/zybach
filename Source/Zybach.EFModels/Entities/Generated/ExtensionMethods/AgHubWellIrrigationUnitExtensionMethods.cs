//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWellIrrigationUnit]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubWellIrrigationUnitExtensionMethods
    {
        public static AgHubWellIrrigationUnitDto AsDto(this AgHubWellIrrigationUnit agHubWellIrrigationUnit)
        {
            var agHubWellIrrigationUnitDto = new AgHubWellIrrigationUnitDto()
            {
                AgHubWellIrrigationUnitID = agHubWellIrrigationUnit.AgHubWellIrrigationUnitID,
                AgHubWell = agHubWellIrrigationUnit.AgHubWell.AsDto()
            };
            DoCustomMappings(agHubWellIrrigationUnit, agHubWellIrrigationUnitDto);
            return agHubWellIrrigationUnitDto;
        }

        static partial void DoCustomMappings(AgHubWellIrrigationUnit agHubWellIrrigationUnit, AgHubWellIrrigationUnitDto agHubWellIrrigationUnitDto);

        public static AgHubWellIrrigationUnitSimpleDto AsSimpleDto(this AgHubWellIrrigationUnit agHubWellIrrigationUnit)
        {
            var agHubWellIrrigationUnitSimpleDto = new AgHubWellIrrigationUnitSimpleDto()
            {
                AgHubWellIrrigationUnitID = agHubWellIrrigationUnit.AgHubWellIrrigationUnitID,
                AgHubWellID = agHubWellIrrigationUnit.AgHubWellID
            };
            DoCustomSimpleDtoMappings(agHubWellIrrigationUnit, agHubWellIrrigationUnitSimpleDto);
            return agHubWellIrrigationUnitSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(AgHubWellIrrigationUnit agHubWellIrrigationUnit, AgHubWellIrrigationUnitSimpleDto agHubWellIrrigationUnitSimpleDto);
    }
}