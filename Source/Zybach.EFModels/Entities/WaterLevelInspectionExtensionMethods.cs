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
        }
    }
}