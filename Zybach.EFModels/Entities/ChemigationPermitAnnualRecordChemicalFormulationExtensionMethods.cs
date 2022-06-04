using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermitAnnualRecordChemicalFormulationExtensionMethods
    {
        static partial void DoCustomSimpleDtoMappings(ChemigationPermitAnnualRecordChemicalFormulation chemigationPermitAnnualRecordChemicalFormulation, ChemigationPermitAnnualRecordChemicalFormulationSimpleDto chemigationPermitAnnualRecordChemicalFormulationSimpleDto)
        {
            chemigationPermitAnnualRecordChemicalFormulationSimpleDto.ChemicalUnitName = chemigationPermitAnnualRecordChemicalFormulation.ChemicalUnit.ChemicalUnitPluralName;
            chemigationPermitAnnualRecordChemicalFormulationSimpleDto.ChemicalFormulationName = chemigationPermitAnnualRecordChemicalFormulation.ChemicalFormulation.ChemicalFormulationDisplayName;
        }
    }
}