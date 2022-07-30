//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnitGeometry]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubIrrigationUnitGeometryExtensionMethods
    {
        public static AgHubIrrigationUnitGeometryDto AsDto(this AgHubIrrigationUnitGeometry agHubIrrigationUnitGeometry)
        {
            var agHubIrrigationUnitGeometryDto = new AgHubIrrigationUnitGeometryDto()
            {
                AgHubIrrigationUnitGeometryID = agHubIrrigationUnitGeometry.AgHubIrrigationUnitGeometryID,
                AgHubIrrigationUnit = agHubIrrigationUnitGeometry.AgHubIrrigationUnit.AsDto()
            };
            DoCustomMappings(agHubIrrigationUnitGeometry, agHubIrrigationUnitGeometryDto);
            return agHubIrrigationUnitGeometryDto;
        }

        static partial void DoCustomMappings(AgHubIrrigationUnitGeometry agHubIrrigationUnitGeometry, AgHubIrrigationUnitGeometryDto agHubIrrigationUnitGeometryDto);

        public static AgHubIrrigationUnitGeometrySimpleDto AsSimpleDto(this AgHubIrrigationUnitGeometry agHubIrrigationUnitGeometry)
        {
            var agHubIrrigationUnitGeometrySimpleDto = new AgHubIrrigationUnitGeometrySimpleDto()
            {
                AgHubIrrigationUnitGeometryID = agHubIrrigationUnitGeometry.AgHubIrrigationUnitGeometryID,
                AgHubIrrigationUnitID = agHubIrrigationUnitGeometry.AgHubIrrigationUnitID
            };
            DoCustomSimpleDtoMappings(agHubIrrigationUnitGeometry, agHubIrrigationUnitGeometrySimpleDto);
            return agHubIrrigationUnitGeometrySimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(AgHubIrrigationUnitGeometry agHubIrrigationUnitGeometry, AgHubIrrigationUnitGeometrySimpleDto agHubIrrigationUnitGeometrySimpleDto);
    }
}