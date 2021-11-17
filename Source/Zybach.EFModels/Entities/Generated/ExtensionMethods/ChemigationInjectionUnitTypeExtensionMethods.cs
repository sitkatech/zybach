//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInjectionUnitType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationInjectionUnitTypeExtensionMethods
    {
        public static ChemigationInjectionUnitTypeDto AsDto(this ChemigationInjectionUnitType chemigationInjectionUnitType)
        {
            var chemigationInjectionUnitTypeDto = new ChemigationInjectionUnitTypeDto()
            {
                ChemigationInjectionUnitTypeID = chemigationInjectionUnitType.ChemigationInjectionUnitTypeID,
                ChemigationInjectionUnitTypeName = chemigationInjectionUnitType.ChemigationInjectionUnitTypeName,
                ChemigationInjectionUnitTypeDisplayName = chemigationInjectionUnitType.ChemigationInjectionUnitTypeDisplayName
            };
            DoCustomMappings(chemigationInjectionUnitType, chemigationInjectionUnitTypeDto);
            return chemigationInjectionUnitTypeDto;
        }

        static partial void DoCustomMappings(ChemigationInjectionUnitType chemigationInjectionUnitType, ChemigationInjectionUnitTypeDto chemigationInjectionUnitTypeDto);

    }
}