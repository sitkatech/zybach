//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigatedAcreTillageType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubIrrigatedAcreTillageTypeExtensionMethods
    {
        public static AgHubIrrigatedAcreTillageTypeDto AsDto(this AgHubIrrigatedAcreTillageType agHubIrrigatedAcreTillageType)
        {
            var agHubIrrigatedAcreTillageTypeDto = new AgHubIrrigatedAcreTillageTypeDto()
            {
                AgHubIrrigatedAcreTillageTypeID = agHubIrrigatedAcreTillageType.AgHubIrrigatedAcreTillageTypeID,
                AgHubIrrigatedAcreTillageTypeName = agHubIrrigatedAcreTillageType.AgHubIrrigatedAcreTillageTypeName,
                AgHubIrrigatedAcreTillageTypeDisplayName = agHubIrrigatedAcreTillageType.AgHubIrrigatedAcreTillageTypeDisplayName,
                MapColor = agHubIrrigatedAcreTillageType.MapColor
            };
            DoCustomMappings(agHubIrrigatedAcreTillageType, agHubIrrigatedAcreTillageTypeDto);
            return agHubIrrigatedAcreTillageTypeDto;
        }

        static partial void DoCustomMappings(AgHubIrrigatedAcreTillageType agHubIrrigatedAcreTillageType, AgHubIrrigatedAcreTillageTypeDto agHubIrrigatedAcreTillageTypeDto);

        public static AgHubIrrigatedAcreTillageTypeSimpleDto AsSimpleDto(this AgHubIrrigatedAcreTillageType agHubIrrigatedAcreTillageType)
        {
            var agHubIrrigatedAcreTillageTypeSimpleDto = new AgHubIrrigatedAcreTillageTypeSimpleDto()
            {
                AgHubIrrigatedAcreTillageTypeID = agHubIrrigatedAcreTillageType.AgHubIrrigatedAcreTillageTypeID,
                AgHubIrrigatedAcreTillageTypeName = agHubIrrigatedAcreTillageType.AgHubIrrigatedAcreTillageTypeName,
                AgHubIrrigatedAcreTillageTypeDisplayName = agHubIrrigatedAcreTillageType.AgHubIrrigatedAcreTillageTypeDisplayName,
                MapColor = agHubIrrigatedAcreTillageType.MapColor
            };
            DoCustomSimpleDtoMappings(agHubIrrigatedAcreTillageType, agHubIrrigatedAcreTillageTypeSimpleDto);
            return agHubIrrigatedAcreTillageTypeSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(AgHubIrrigatedAcreTillageType agHubIrrigatedAcreTillageType, AgHubIrrigatedAcreTillageTypeSimpleDto agHubIrrigatedAcreTillageTypeSimpleDto);
    }
}