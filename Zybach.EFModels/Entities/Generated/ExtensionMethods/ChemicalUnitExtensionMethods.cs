//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemicalUnit]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemicalUnitExtensionMethods
    {
        public static ChemicalUnitDto AsDto(this ChemicalUnit chemicalUnit)
        {
            var chemicalUnitDto = new ChemicalUnitDto()
            {
                ChemicalUnitID = chemicalUnit.ChemicalUnitID,
                ChemicalUnitName = chemicalUnit.ChemicalUnitName,
                ChemicalUnitPluralName = chemicalUnit.ChemicalUnitPluralName,
                ChemicalUnitLowercaseShortName = chemicalUnit.ChemicalUnitLowercaseShortName
            };
            DoCustomMappings(chemicalUnit, chemicalUnitDto);
            return chemicalUnitDto;
        }

        static partial void DoCustomMappings(ChemicalUnit chemicalUnit, ChemicalUnitDto chemicalUnitDto);

        public static ChemicalUnitSimpleDto AsSimpleDto(this ChemicalUnit chemicalUnit)
        {
            var chemicalUnitSimpleDto = new ChemicalUnitSimpleDto()
            {
                ChemicalUnitID = chemicalUnit.ChemicalUnitID,
                ChemicalUnitName = chemicalUnit.ChemicalUnitName,
                ChemicalUnitPluralName = chemicalUnit.ChemicalUnitPluralName,
                ChemicalUnitLowercaseShortName = chemicalUnit.ChemicalUnitLowercaseShortName
            };
            DoCustomSimpleDtoMappings(chemicalUnit, chemicalUnitSimpleDto);
            return chemicalUnitSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemicalUnit chemicalUnit, ChemicalUnitSimpleDto chemicalUnitSimpleDto);
    }
}