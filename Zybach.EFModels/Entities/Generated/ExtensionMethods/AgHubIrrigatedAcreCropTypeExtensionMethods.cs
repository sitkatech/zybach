//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigatedAcreCropType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubIrrigatedAcreCropTypeExtensionMethods
    {
        public static AgHubIrrigatedAcreCropTypeDto AsDto(this AgHubIrrigatedAcreCropType agHubIrrigatedAcreCropType)
        {
            var agHubIrrigatedAcreCropTypeDto = new AgHubIrrigatedAcreCropTypeDto()
            {
                AgHubIrrigatedAcreCropTypeID = agHubIrrigatedAcreCropType.AgHubIrrigatedAcreCropTypeID,
                AgHubIrrigatedAcreCropTypeName = agHubIrrigatedAcreCropType.AgHubIrrigatedAcreCropTypeName,
                MapColor = agHubIrrigatedAcreCropType.MapColor,
                SortOrder = agHubIrrigatedAcreCropType.SortOrder
            };
            DoCustomMappings(agHubIrrigatedAcreCropType, agHubIrrigatedAcreCropTypeDto);
            return agHubIrrigatedAcreCropTypeDto;
        }

        static partial void DoCustomMappings(AgHubIrrigatedAcreCropType agHubIrrigatedAcreCropType, AgHubIrrigatedAcreCropTypeDto agHubIrrigatedAcreCropTypeDto);

        public static AgHubIrrigatedAcreCropTypeSimpleDto AsSimpleDto(this AgHubIrrigatedAcreCropType agHubIrrigatedAcreCropType)
        {
            var agHubIrrigatedAcreCropTypeSimpleDto = new AgHubIrrigatedAcreCropTypeSimpleDto()
            {
                AgHubIrrigatedAcreCropTypeID = agHubIrrigatedAcreCropType.AgHubIrrigatedAcreCropTypeID,
                AgHubIrrigatedAcreCropTypeName = agHubIrrigatedAcreCropType.AgHubIrrigatedAcreCropTypeName,
                MapColor = agHubIrrigatedAcreCropType.MapColor,
                SortOrder = agHubIrrigatedAcreCropType.SortOrder
            };
            DoCustomSimpleDtoMappings(agHubIrrigatedAcreCropType, agHubIrrigatedAcreCropTypeSimpleDto);
            return agHubIrrigatedAcreCropTypeSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(AgHubIrrigatedAcreCropType agHubIrrigatedAcreCropType, AgHubIrrigatedAcreCropTypeSimpleDto agHubIrrigatedAcreCropTypeSimpleDto);
    }
}