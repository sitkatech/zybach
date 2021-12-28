//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WaterQualityInspection]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WaterQualityInspectionExtensionMethods
    {
        public static WaterQualityInspectionDto AsDto(this WaterQualityInspection waterQualityInspection)
        {
            var waterQualityInspectionDto = new WaterQualityInspectionDto()
            {
                WaterQualityInspectionID = waterQualityInspection.WaterQualityInspectionID,
                Well = waterQualityInspection.Well.AsDto(),
                WaterQualityInspectionType = waterQualityInspection.WaterQualityInspectionType.AsDto(),
                InspectionDate = waterQualityInspection.InspectionDate,
                InspectorUser = waterQualityInspection.InspectorUser.AsDto(),
                Temperature = waterQualityInspection.Temperature,
                PH = waterQualityInspection.PH,
                Conductivity = waterQualityInspection.Conductivity,
                FieldAlkilinity = waterQualityInspection.FieldAlkilinity,
                FieldNitrates = waterQualityInspection.FieldNitrates,
                LabNitrates = waterQualityInspection.LabNitrates,
                Salinity = waterQualityInspection.Salinity,
                MV = waterQualityInspection.MV,
                Sodium = waterQualityInspection.Sodium,
                Calcium = waterQualityInspection.Calcium,
                Magnesium = waterQualityInspection.Magnesium,
                Potassium = waterQualityInspection.Potassium,
                HydrogenCarbonate = waterQualityInspection.HydrogenCarbonate,
                CalciumCarbonate = waterQualityInspection.CalciumCarbonate,
                Sulfate = waterQualityInspection.Sulfate,
                Chloride = waterQualityInspection.Chloride,
                SiliconDioxide = waterQualityInspection.SiliconDioxide,
                CropType = waterQualityInspection.CropType?.AsDto(),
                PreWaterLevel = waterQualityInspection.PreWaterLevel,
                PostWaterLevel = waterQualityInspection.PostWaterLevel,
                InspectionNotes = waterQualityInspection.InspectionNotes
            };
            DoCustomMappings(waterQualityInspection, waterQualityInspectionDto);
            return waterQualityInspectionDto;
        }

        static partial void DoCustomMappings(WaterQualityInspection waterQualityInspection, WaterQualityInspectionDto waterQualityInspectionDto);

        public static WaterQualityInspectionSimpleDto AsSimpleDto(this WaterQualityInspection waterQualityInspection)
        {
            var waterQualityInspectionSimpleDto = new WaterQualityInspectionSimpleDto()
            {
                WaterQualityInspectionID = waterQualityInspection.WaterQualityInspectionID,
                WellID = waterQualityInspection.WellID,
                WaterQualityInspectionTypeID = waterQualityInspection.WaterQualityInspectionTypeID,
                InspectionDate = waterQualityInspection.InspectionDate,
                InspectorUserID = waterQualityInspection.InspectorUserID,
                Temperature = waterQualityInspection.Temperature,
                PH = waterQualityInspection.PH,
                Conductivity = waterQualityInspection.Conductivity,
                FieldAlkilinity = waterQualityInspection.FieldAlkilinity,
                FieldNitrates = waterQualityInspection.FieldNitrates,
                LabNitrates = waterQualityInspection.LabNitrates,
                Salinity = waterQualityInspection.Salinity,
                MV = waterQualityInspection.MV,
                Sodium = waterQualityInspection.Sodium,
                Calcium = waterQualityInspection.Calcium,
                Magnesium = waterQualityInspection.Magnesium,
                Potassium = waterQualityInspection.Potassium,
                HydrogenCarbonate = waterQualityInspection.HydrogenCarbonate,
                CalciumCarbonate = waterQualityInspection.CalciumCarbonate,
                Sulfate = waterQualityInspection.Sulfate,
                Chloride = waterQualityInspection.Chloride,
                SiliconDioxide = waterQualityInspection.SiliconDioxide,
                CropTypeID = waterQualityInspection.CropTypeID,
                PreWaterLevel = waterQualityInspection.PreWaterLevel,
                PostWaterLevel = waterQualityInspection.PostWaterLevel,
                InspectionNotes = waterQualityInspection.InspectionNotes
            };
            DoCustomSimpleDtoMappings(waterQualityInspection, waterQualityInspectionSimpleDto);
            return waterQualityInspectionSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(WaterQualityInspection waterQualityInspection, WaterQualityInspectionSimpleDto waterQualityInspectionSimpleDto);
    }
}