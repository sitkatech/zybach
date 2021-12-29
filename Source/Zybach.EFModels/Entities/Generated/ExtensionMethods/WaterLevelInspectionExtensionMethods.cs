//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WaterLevelInspection]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WaterLevelInspectionExtensionMethods
    {
        public static WaterLevelInspectionDto AsDto(this WaterLevelInspection waterLevelInspection)
        {
            var waterLevelInspectionDto = new WaterLevelInspectionDto()
            {
                WaterLevelInspectionID = waterLevelInspection.WaterLevelInspectionID,
                Well = waterLevelInspection.Well.AsDto(),
                InspectionDate = waterLevelInspection.InspectionDate,
                InspectorUser = waterLevelInspection.InspectorUser.AsDto(),
                WaterLevelInspectionStatus = waterLevelInspection.WaterLevelInspectionStatus,
                MeasuringEquipment = waterLevelInspection.MeasuringEquipment,
                Crop = waterLevelInspection.Crop,
                HasOil = waterLevelInspection.HasOil,
                HasBrokenTape = waterLevelInspection.HasBrokenTape,
                Accuracy = waterLevelInspection.Accuracy,
                Method = waterLevelInspection.Method,
                Party = waterLevelInspection.Party,
                SourceAgency = waterLevelInspection.SourceAgency,
                SourceCode = waterLevelInspection.SourceCode,
                TimeDatumCode = waterLevelInspection.TimeDatumCode,
                TimeDatumReliability = waterLevelInspection.TimeDatumReliability,
                LevelTypeCode = waterLevelInspection.LevelTypeCode,
                AgencyCode = waterLevelInspection.AgencyCode,
                Access = waterLevelInspection.Access,
                Hold = waterLevelInspection.Hold,
                Cut = waterLevelInspection.Cut,
                MP = waterLevelInspection.MP,
                Measurement = waterLevelInspection.Measurement,
                IsPrimary = waterLevelInspection.IsPrimary,
                WaterLevel = waterLevelInspection.WaterLevel,
                CropType = waterLevelInspection.CropType?.AsDto(),
                InspectionNotes = waterLevelInspection.InspectionNotes
            };
            DoCustomMappings(waterLevelInspection, waterLevelInspectionDto);
            return waterLevelInspectionDto;
        }

        static partial void DoCustomMappings(WaterLevelInspection waterLevelInspection, WaterLevelInspectionDto waterLevelInspectionDto);

        public static WaterLevelInspectionSimpleDto AsSimpleDto(this WaterLevelInspection waterLevelInspection)
        {
            var waterLevelInspectionSimpleDto = new WaterLevelInspectionSimpleDto()
            {
                WaterLevelInspectionID = waterLevelInspection.WaterLevelInspectionID,
                WellID = waterLevelInspection.WellID,
                InspectionDate = waterLevelInspection.InspectionDate,
                InspectorUserID = waterLevelInspection.InspectorUserID,
                WaterLevelInspectionStatus = waterLevelInspection.WaterLevelInspectionStatus,
                MeasuringEquipment = waterLevelInspection.MeasuringEquipment,
                Crop = waterLevelInspection.Crop,
                HasOil = waterLevelInspection.HasOil,
                HasBrokenTape = waterLevelInspection.HasBrokenTape,
                Accuracy = waterLevelInspection.Accuracy,
                Method = waterLevelInspection.Method,
                Party = waterLevelInspection.Party,
                SourceAgency = waterLevelInspection.SourceAgency,
                SourceCode = waterLevelInspection.SourceCode,
                TimeDatumCode = waterLevelInspection.TimeDatumCode,
                TimeDatumReliability = waterLevelInspection.TimeDatumReliability,
                LevelTypeCode = waterLevelInspection.LevelTypeCode,
                AgencyCode = waterLevelInspection.AgencyCode,
                Access = waterLevelInspection.Access,
                Hold = waterLevelInspection.Hold,
                Cut = waterLevelInspection.Cut,
                MP = waterLevelInspection.MP,
                Measurement = waterLevelInspection.Measurement,
                IsPrimary = waterLevelInspection.IsPrimary,
                WaterLevel = waterLevelInspection.WaterLevel,
                CropTypeID = waterLevelInspection.CropTypeID,
                InspectionNotes = waterLevelInspection.InspectionNotes
            };
            DoCustomSimpleDtoMappings(waterLevelInspection, waterLevelInspectionSimpleDto);
            return waterLevelInspectionSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(WaterLevelInspection waterLevelInspection, WaterLevelInspectionSimpleDto waterLevelInspectionSimpleDto);
    }
}