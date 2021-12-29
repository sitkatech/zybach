//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInterlockType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationInterlockTypeExtensionMethods
    {
        public static ChemigationInterlockTypeDto AsDto(this ChemigationInterlockType chemigationInterlockType)
        {
            var chemigationInterlockTypeDto = new ChemigationInterlockTypeDto()
            {
                ChemigationInterlockTypeID = chemigationInterlockType.ChemigationInterlockTypeID,
                ChemigationInterlockTypeName = chemigationInterlockType.ChemigationInterlockTypeName,
                ChemigationInterlockTypeDisplayName = chemigationInterlockType.ChemigationInterlockTypeDisplayName
            };
            DoCustomMappings(chemigationInterlockType, chemigationInterlockTypeDto);
            return chemigationInterlockTypeDto;
        }

        static partial void DoCustomMappings(ChemigationInterlockType chemigationInterlockType, ChemigationInterlockTypeDto chemigationInterlockTypeDto);

        public static ChemigationInterlockTypeSimpleDto AsSimpleDto(this ChemigationInterlockType chemigationInterlockType)
        {
            var chemigationInterlockTypeSimpleDto = new ChemigationInterlockTypeSimpleDto()
            {
                ChemigationInterlockTypeID = chemigationInterlockType.ChemigationInterlockTypeID,
                ChemigationInterlockTypeName = chemigationInterlockType.ChemigationInterlockTypeName,
                ChemigationInterlockTypeDisplayName = chemigationInterlockType.ChemigationInterlockTypeDisplayName
            };
            DoCustomSimpleDtoMappings(chemigationInterlockType, chemigationInterlockTypeSimpleDto);
            return chemigationInterlockTypeSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationInterlockType chemigationInterlockType, ChemigationInterlockTypeSimpleDto chemigationInterlockTypeSimpleDto);
    }
}