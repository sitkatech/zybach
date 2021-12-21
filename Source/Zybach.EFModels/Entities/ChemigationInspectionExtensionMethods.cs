using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationInspectionExtensionMethods
    {
        static partial void DoCustomSimpleDtoMappings(ChemigationInspection chemigationInspection,
            ChemigationInspectionSimpleDto chemigationInspectionSimpleDto)
        {
            chemigationInspectionSimpleDto.ChemigationInspectionTypeName = chemigationInspection.ChemigationInspectionType.ChemigationInspectionTypeName;
            chemigationInspectionSimpleDto.ChemigationInspectionStatusName = chemigationInspection.ChemigationInspectionStatus.ChemigationInspectionStatusDisplayName;
            chemigationInspectionSimpleDto.ChemigationMainlineCheckValveName = chemigationInspection.ChemigationMainlineCheckValve.ChemigationMainlineCheckValveDisplayName;
            chemigationInspectionSimpleDto.ChemigationLowPressureValveName = chemigationInspection.ChemigationLowPressureValve.ChemigationLowPressureValveDisplayName;
            chemigationInspectionSimpleDto.ChemigationInjectionValveName = chemigationInspection.ChemigationInjectionValve.ChemigationInjectionValveDisplayName;
            chemigationInspectionSimpleDto.TillageName = chemigationInspection.Tillage.TillageDisplayName;
            chemigationInspectionSimpleDto.CropTypeName = chemigationInspection.CropType.CropTypeDisplayName;
        }
    }
}