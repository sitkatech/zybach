using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationInspectionExtensionMethods
    {
        static partial void DoCustomSimpleDtoMappings(ChemigationInspection chemigationInspection,
            ChemigationInspectionSimpleDto chemigationInspectionSimpleDto)
        {
            chemigationInspectionSimpleDto.ChemigationPermitNumber = chemigationInspection.ChemigationPermitAnnualRecord
                .ChemigationPermit.ChemigationPermitNumber;
            chemigationInspectionSimpleDto.ChemigationPermitNumberDisplay = chemigationInspection
                .ChemigationPermitAnnualRecord.ChemigationPermit.ChemigationPermitNumberDisplay;
            chemigationInspectionSimpleDto.County = chemigationInspection.ChemigationPermitAnnualRecord
                .ChemigationPermit.County.CountyDisplayName;
            chemigationInspectionSimpleDto.TownshipRangeSection =
                chemigationInspection.ChemigationPermitAnnualRecord.TownshipRangeSection;
            chemigationInspectionSimpleDto.ChemigationInspectionTypeName = chemigationInspection.ChemigationInspectionType?.ChemigationInspectionTypeDisplayName;
            chemigationInspectionSimpleDto.ChemigationInspectionStatusName = chemigationInspection.ChemigationInspectionStatus.ChemigationInspectionStatusDisplayName;
            chemigationInspectionSimpleDto.ChemigationMainlineCheckValveName = chemigationInspection.ChemigationMainlineCheckValve?.ChemigationMainlineCheckValveDisplayName;
            chemigationInspectionSimpleDto.ChemigationLowPressureValveName = chemigationInspection.ChemigationLowPressureValve?.ChemigationLowPressureValveDisplayName;
            chemigationInspectionSimpleDto.ChemigationInjectionValveName = chemigationInspection.ChemigationInjectionValve?.ChemigationInjectionValveDisplayName;
            chemigationInspectionSimpleDto.TillageName = chemigationInspection.Tillage?.TillageDisplayName;
            chemigationInspectionSimpleDto.CropTypeName = chemigationInspection.CropType?.CropTypeDisplayName;
            chemigationInspectionSimpleDto.Inspector = chemigationInspection.InspectorUser?.AsSimpleDto();
        }

    }
}