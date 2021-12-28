//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInspection]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationInspectionExtensionMethods
    {
        public static ChemigationInspectionDto AsDto(this ChemigationInspections chemigationInspections)
        {
            var chemigationInspectionDto = new ChemigationInspectionDto()
            {
                ChemigationInspectionID = chemigationInspections.ChemigationInspectionID,
                ChemigationPermitAnnualRecord = chemigationInspections.ChemigationPermitAnnualRecord.AsDto(),
                ChemigationInspectionStatus = chemigationInspections.ChemigationInspectionStatus.AsDto(),
                ChemigationInspectionType = chemigationInspections.ChemigationInspectionType?.AsDto(),
                InspectionDate = chemigationInspections.InspectionDate,
                InspectorUser = chemigationInspections.InspectorUser?.AsDto(),
                ChemigationMainlineCheckValve = chemigationInspections.ChemigationMainlineCheckValve?.AsDto(),
                HasVacuumReliefValve = chemigationInspections.HasVacuumReliefValve,
                HasInspectionPort = chemigationInspections.HasInspectionPort,
                ChemigationLowPressureValve = chemigationInspections.ChemigationLowPressureValve?.AsDto(),
                ChemigationInjectionValve = chemigationInspections.ChemigationInjectionValve?.AsDto(),
                Tillage = chemigationInspections.Tillage?.AsDto(),
                CropType = chemigationInspections.CropType?.AsDto(),
                InspectionNotes = chemigationInspections.InspectionNotes,
                ChemigationInspectionFailureReason = chemigationInspections.ChemigationInspectionFailureReason?.AsDto()
            };
            DoCustomMappings(chemigationInspections, chemigationInspectionDto);
            return chemigationInspectionDto;
        }

        static partial void DoCustomMappings(ChemigationInspections chemigationInspections, ChemigationInspectionDto chemigationInspectionDto);

        public static ChemigationInspectionSimpleDto AsSimpleDto(this ChemigationInspections chemigationInspections)
        {
            var chemigationInspectionSimpleDto = new ChemigationInspectionSimpleDto()
            {
                ChemigationInspectionID = chemigationInspections.ChemigationInspectionID,
                ChemigationPermitAnnualRecordID = chemigationInspections.ChemigationPermitAnnualRecordID,
                ChemigationInspectionStatusID = chemigationInspections.ChemigationInspectionStatusID,
                ChemigationInspectionTypeID = chemigationInspections.ChemigationInspectionTypeID,
                InspectionDate = chemigationInspections.InspectionDate,
                InspectorUserID = chemigationInspections.InspectorUserID,
                ChemigationMainlineCheckValveID = chemigationInspections.ChemigationMainlineCheckValveID,
                HasVacuumReliefValve = chemigationInspections.HasVacuumReliefValve,
                HasInspectionPort = chemigationInspections.HasInspectionPort,
                ChemigationLowPressureValveID = chemigationInspections.ChemigationLowPressureValveID,
                ChemigationInjectionValveID = chemigationInspections.ChemigationInjectionValveID,
                TillageID = chemigationInspections.TillageID,
                CropTypeID = chemigationInspections.CropTypeID,
                InspectionNotes = chemigationInspections.InspectionNotes,
                ChemigationInspectionFailureReasonID = chemigationInspections.ChemigationInspectionFailureReasonID
            };
            DoCustomSimpleDtoMappings(chemigationInspections, chemigationInspectionSimpleDto);
            return chemigationInspectionSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationInspections chemigationInspections, ChemigationInspectionSimpleDto chemigationInspectionSimpleDto);
    }
}