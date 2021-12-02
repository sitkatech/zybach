//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemicalFormulation]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemicalFormulationExtensionMethods
    {
        public static ChemicalFormulationDto AsDto(this ChemicalFormulation chemicalFormulation)
        {
            var chemicalFormulationDto = new ChemicalFormulationDto()
            {
                ChemicalFormulationID = chemicalFormulation.ChemicalFormulationID,
                ChemicalFormulationName = chemicalFormulation.ChemicalFormulationName,
                ChemicalFormulationDisplayName = chemicalFormulation.ChemicalFormulationDisplayName
            };
            DoCustomMappings(chemicalFormulation, chemicalFormulationDto);
            return chemicalFormulationDto;
        }

        static partial void DoCustomMappings(ChemicalFormulation chemicalFormulation, ChemicalFormulationDto chemicalFormulationDto);

        public static ChemicalFormulationSimpleDto AsSimpleDto(this ChemicalFormulation chemicalFormulation)
        {
            var chemicalFormulationSimpleDto = new ChemicalFormulationSimpleDto()
            {
                ChemicalFormulationID = chemicalFormulation.ChemicalFormulationID,
                ChemicalFormulationName = chemicalFormulation.ChemicalFormulationName,
                ChemicalFormulationDisplayName = chemicalFormulation.ChemicalFormulationDisplayName
            };
            DoCustomSimpleDtoMappings(chemicalFormulation, chemicalFormulationSimpleDto);
            return chemicalFormulationSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemicalFormulation chemicalFormulation, ChemicalFormulationSimpleDto chemicalFormulationSimpleDto);
    }
}