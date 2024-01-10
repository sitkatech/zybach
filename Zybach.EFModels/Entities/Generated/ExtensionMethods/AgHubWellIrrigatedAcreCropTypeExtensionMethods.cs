//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWellIrrigatedAcreCropType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubWellIrrigatedAcreCropTypeExtensionMethods
    {
        public static AgHubWellIrrigatedAcreCropTypeDto AsDto(this AgHubWellIrrigatedAcreCropType agHubWellIrrigatedAcreCropType)
        {
            var agHubWellIrrigatedAcreCropTypeDto = new AgHubWellIrrigatedAcreCropTypeDto()
            {
                AgHubWellIrrigatedAcreCropTypeID = agHubWellIrrigatedAcreCropType.AgHubWellIrrigatedAcreCropTypeID,
                AgHubWellIrrigatedAcreCropTypeName = agHubWellIrrigatedAcreCropType.AgHubWellIrrigatedAcreCropTypeName,
                MapColor = agHubWellIrrigatedAcreCropType.MapColor
            };
            DoCustomMappings(agHubWellIrrigatedAcreCropType, agHubWellIrrigatedAcreCropTypeDto);
            return agHubWellIrrigatedAcreCropTypeDto;
        }

        static partial void DoCustomMappings(AgHubWellIrrigatedAcreCropType agHubWellIrrigatedAcreCropType, AgHubWellIrrigatedAcreCropTypeDto agHubWellIrrigatedAcreCropTypeDto);

        public static AgHubWellIrrigatedAcreCropTypeSimpleDto AsSimpleDto(this AgHubWellIrrigatedAcreCropType agHubWellIrrigatedAcreCropType)
        {
            var agHubWellIrrigatedAcreCropTypeSimpleDto = new AgHubWellIrrigatedAcreCropTypeSimpleDto()
            {
                AgHubWellIrrigatedAcreCropTypeID = agHubWellIrrigatedAcreCropType.AgHubWellIrrigatedAcreCropTypeID,
                AgHubWellIrrigatedAcreCropTypeName = agHubWellIrrigatedAcreCropType.AgHubWellIrrigatedAcreCropTypeName,
                MapColor = agHubWellIrrigatedAcreCropType.MapColor
            };
            DoCustomSimpleDtoMappings(agHubWellIrrigatedAcreCropType, agHubWellIrrigatedAcreCropTypeSimpleDto);
            return agHubWellIrrigatedAcreCropTypeSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(AgHubWellIrrigatedAcreCropType agHubWellIrrigatedAcreCropType, AgHubWellIrrigatedAcreCropTypeSimpleDto agHubWellIrrigatedAcreCropTypeSimpleDto);
    }
}