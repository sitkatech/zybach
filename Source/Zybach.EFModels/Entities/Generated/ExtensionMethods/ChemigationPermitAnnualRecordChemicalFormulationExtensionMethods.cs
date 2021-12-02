//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecordChemicalFormulation]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationPermitAnnualRecordChemicalFormulationExtensionMethods
    {
        public static ChemigationPermitAnnualRecordChemicalFormulationDto AsDto(this ChemigationPermitAnnualRecordChemicalFormulation chemigationPermitAnnualRecordChemicalFormulation)
        {
            var chemigationPermitAnnualRecordChemicalFormulationDto = new ChemigationPermitAnnualRecordChemicalFormulationDto()
            {
                ChemigationPermitAnnualRecordChemicalFormulationID = chemigationPermitAnnualRecordChemicalFormulation.ChemigationPermitAnnualRecordChemicalFormulationID,
                ChemigationPermitAnnualRecord = chemigationPermitAnnualRecordChemicalFormulation.ChemigationPermitAnnualRecord.AsDto(),
                ChemicalFormulation = chemigationPermitAnnualRecordChemicalFormulation.ChemicalFormulation.AsDto(),
                ChemicalUnit = chemigationPermitAnnualRecordChemicalFormulation.ChemicalUnit.AsDto(),
                TotalApplied = chemigationPermitAnnualRecordChemicalFormulation.TotalApplied,
                AcresTreated = chemigationPermitAnnualRecordChemicalFormulation.AcresTreated
            };
            DoCustomMappings(chemigationPermitAnnualRecordChemicalFormulation, chemigationPermitAnnualRecordChemicalFormulationDto);
            return chemigationPermitAnnualRecordChemicalFormulationDto;
        }

        static partial void DoCustomMappings(ChemigationPermitAnnualRecordChemicalFormulation chemigationPermitAnnualRecordChemicalFormulation, ChemigationPermitAnnualRecordChemicalFormulationDto chemigationPermitAnnualRecordChemicalFormulationDto);

        public static ChemigationPermitAnnualRecordChemicalFormulationSimpleDto AsSimpleDto(this ChemigationPermitAnnualRecordChemicalFormulation chemigationPermitAnnualRecordChemicalFormulation)
        {
            var chemigationPermitAnnualRecordChemicalFormulationSimpleDto = new ChemigationPermitAnnualRecordChemicalFormulationSimpleDto()
            {
                ChemigationPermitAnnualRecordChemicalFormulationID = chemigationPermitAnnualRecordChemicalFormulation.ChemigationPermitAnnualRecordChemicalFormulationID,
                ChemigationPermitAnnualRecordID = chemigationPermitAnnualRecordChemicalFormulation.ChemigationPermitAnnualRecordID,
                ChemicalFormulationID = chemigationPermitAnnualRecordChemicalFormulation.ChemicalFormulationID,
                ChemicalUnitID = chemigationPermitAnnualRecordChemicalFormulation.ChemicalUnitID,
                TotalApplied = chemigationPermitAnnualRecordChemicalFormulation.TotalApplied,
                AcresTreated = chemigationPermitAnnualRecordChemicalFormulation.AcresTreated
            };
            DoCustomSimpleDtoMappings(chemigationPermitAnnualRecordChemicalFormulation, chemigationPermitAnnualRecordChemicalFormulationSimpleDto);
            return chemigationPermitAnnualRecordChemicalFormulationSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationPermitAnnualRecordChemicalFormulation chemigationPermitAnnualRecordChemicalFormulation, ChemigationPermitAnnualRecordChemicalFormulationSimpleDto chemigationPermitAnnualRecordChemicalFormulationSimpleDto);
    }
}