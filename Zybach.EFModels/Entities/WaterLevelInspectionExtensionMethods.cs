using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class WaterLevelInspectionExtensionMethods
    {
        static partial void DoCustomSimpleDtoMappings(WaterLevelInspection waterLevelInspection,
            WaterLevelInspectionSimpleDto waterLevelInspectionSimpleDto)
        {
            waterLevelInspectionSimpleDto.Well = waterLevelInspection.Well.AsSimpleDto();
            waterLevelInspectionSimpleDto.CropTypeName = waterLevelInspection.CropType?.CropTypeDisplayName;
            waterLevelInspectionSimpleDto.Inspector = waterLevelInspection.InspectorUser?.AsSimpleDto();
            waterLevelInspectionSimpleDto.InspectionYear = waterLevelInspection.InspectionDate.Year;
        }

        public static WaterLevelInspectionSummaryDto AsSummaryDto(this WaterLevelInspection waterLevelInspection)
        {
            var waterLevelInspectionSummaryDto = new WaterLevelInspectionSummaryDto()
            {
                WaterLevelInspectionID = waterLevelInspection.WaterLevelInspectionID,
                InspectionDate = waterLevelInspection.InspectionDate,
                Measurement = waterLevelInspection.Measurement,
                MeasuringEquipment = waterLevelInspection.MeasuringEquipment,
                InspectionNotes = waterLevelInspection.InspectionNotes
            };

            return waterLevelInspectionSummaryDto;
        }

        public static WaterLevelInspectionForVegaChartDto AsVegaChartDto(this WaterLevelInspection waterLevelInspection)
        {
            var waterLevelInspectionForVegaChartDto = new WaterLevelInspectionForVegaChartDto()
            {
                WellID = waterLevelInspection.WellID,
                InspectionDate = waterLevelInspection.InspectionDate,
                Measurement = waterLevelInspection.Measurement
            };

            return waterLevelInspectionForVegaChartDto;
        }
    }
}