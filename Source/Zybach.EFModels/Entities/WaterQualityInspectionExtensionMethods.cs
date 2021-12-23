using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class WaterQualityInspectionExtensionMethods
    {
        static partial void DoCustomSimpleDtoMappings(WaterQualityInspection waterQualityInspection,
            WaterQualityInspectionSimpleDto waterQualityInspectionSimpleDto)
        {
            waterQualityInspectionSimpleDto.Well = waterQualityInspection.Well.AsSimpleDto();
            waterQualityInspectionSimpleDto.CropTypeName = waterQualityInspection.CropType?.CropTypeDisplayName;
            waterQualityInspectionSimpleDto.Inspector = waterQualityInspection.InspectorUser?.AsSimpleDto();
        }
    }
}