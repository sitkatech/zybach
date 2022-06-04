//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WaterLevelMeasuringEquipment]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WaterLevelMeasuringEquipmentExtensionMethods
    {
        public static WaterLevelMeasuringEquipmentDto AsDto(this WaterLevelMeasuringEquipment waterLevelMeasuringEquipment)
        {
            var waterLevelMeasuringEquipmentDto = new WaterLevelMeasuringEquipmentDto()
            {
                WaterLevelMeasuringEquipmentID = waterLevelMeasuringEquipment.WaterLevelMeasuringEquipmentID,
                WaterLevelMeasuringEquipmentName = waterLevelMeasuringEquipment.WaterLevelMeasuringEquipmentName,
                WaterLevelMeasuringEquipmentDisplayName = waterLevelMeasuringEquipment.WaterLevelMeasuringEquipmentDisplayName
            };
            DoCustomMappings(waterLevelMeasuringEquipment, waterLevelMeasuringEquipmentDto);
            return waterLevelMeasuringEquipmentDto;
        }

        static partial void DoCustomMappings(WaterLevelMeasuringEquipment waterLevelMeasuringEquipment, WaterLevelMeasuringEquipmentDto waterLevelMeasuringEquipmentDto);

        public static WaterLevelMeasuringEquipmentSimpleDto AsSimpleDto(this WaterLevelMeasuringEquipment waterLevelMeasuringEquipment)
        {
            var waterLevelMeasuringEquipmentSimpleDto = new WaterLevelMeasuringEquipmentSimpleDto()
            {
                WaterLevelMeasuringEquipmentID = waterLevelMeasuringEquipment.WaterLevelMeasuringEquipmentID,
                WaterLevelMeasuringEquipmentName = waterLevelMeasuringEquipment.WaterLevelMeasuringEquipmentName,
                WaterLevelMeasuringEquipmentDisplayName = waterLevelMeasuringEquipment.WaterLevelMeasuringEquipmentDisplayName
            };
            DoCustomSimpleDtoMappings(waterLevelMeasuringEquipment, waterLevelMeasuringEquipmentSimpleDto);
            return waterLevelMeasuringEquipmentSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(WaterLevelMeasuringEquipment waterLevelMeasuringEquipment, WaterLevelMeasuringEquipmentSimpleDto waterLevelMeasuringEquipmentSimpleDto);
    }
}