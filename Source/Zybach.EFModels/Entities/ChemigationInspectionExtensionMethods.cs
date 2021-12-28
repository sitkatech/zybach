using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationInspectionExtensionMethods
    {
        static partial void DoCustomSimpleDtoMappings(ChemigationInspections chemigationInspections,
            ChemigationInspectionSimpleDto chemigationInspectionSimpleDto)
        {
            chemigationInspectionSimpleDto.ChemigationPermitNumber = chemigationInspections.ChemigationPermitAnnualRecord
                .ChemigationPermit.ChemigationPermitNumber;
            chemigationInspectionSimpleDto.ChemigationPermitNumberDisplay = chemigationInspections
                .ChemigationPermitAnnualRecord.ChemigationPermit.ChemigationPermitNumberDisplay;
            chemigationInspectionSimpleDto.County = chemigationInspections.ChemigationPermitAnnualRecord
                .ChemigationPermit.County.CountyDisplayName;
            chemigationInspectionSimpleDto.TownshipRangeSection =
                chemigationInspections.ChemigationPermitAnnualRecord.TownshipRangeSection;
            chemigationInspectionSimpleDto.ChemigationInspectionTypeName = chemigationInspections.ChemigationInspectionType?.ChemigationInspectionTypeDisplayName;
            chemigationInspectionSimpleDto.ChemigationInspectionStatusName = chemigationInspections.ChemigationInspectionStatus.ChemigationInspectionStatusDisplayName;
            chemigationInspectionSimpleDto.ChemigationMainlineCheckValveName = chemigationInspections.ChemigationMainlineCheckValve?.ChemigationMainlineCheckValveDisplayName;
            chemigationInspectionSimpleDto.ChemigationLowPressureValveName = chemigationInspections.ChemigationLowPressureValve?.ChemigationLowPressureValveDisplayName;
            chemigationInspectionSimpleDto.ChemigationInjectionValveName = chemigationInspections.ChemigationInjectionValve?.ChemigationInjectionValveDisplayName;
            chemigationInspectionSimpleDto.ChemigationInspectionFailureReasonName = chemigationInspections.ChemigationInspectionFailureReason?.ChemigationInspectionFailureReasonDisplayName;
            chemigationInspectionSimpleDto.TillageName = chemigationInspections.Tillage?.TillageDisplayName;
            chemigationInspectionSimpleDto.CropTypeName = chemigationInspections.CropType?.CropTypeDisplayName;
            chemigationInspectionSimpleDto.Inspector = chemigationInspections.InspectorUser?.AsSimpleDto();
        }

    }
}