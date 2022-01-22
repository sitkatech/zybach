//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInspectionType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationInspectionTypeExtensionMethods
    {
        public static ChemigationInspectionTypeDto AsDto(this ChemigationInspectionType chemigationInspectionType)
        {
            var chemigationInspectionTypeDto = new ChemigationInspectionTypeDto()
            {
                ChemigationInspectionTypeID = chemigationInspectionType.ChemigationInspectionTypeID,
                ChemigationInspectionTypeName = chemigationInspectionType.ChemigationInspectionTypeName,
                ChemigationInspectionTypeDisplayName = chemigationInspectionType.ChemigationInspectionTypeDisplayName
            };
            DoCustomMappings(chemigationInspectionType, chemigationInspectionTypeDto);
            return chemigationInspectionTypeDto;
        }

        static partial void DoCustomMappings(ChemigationInspectionType chemigationInspectionType, ChemigationInspectionTypeDto chemigationInspectionTypeDto);

        public static ChemigationInspectionTypeSimpleDto AsSimpleDto(this ChemigationInspectionType chemigationInspectionType)
        {
            var chemigationInspectionTypeSimpleDto = new ChemigationInspectionTypeSimpleDto()
            {
                ChemigationInspectionTypeID = chemigationInspectionType.ChemigationInspectionTypeID,
                ChemigationInspectionTypeName = chemigationInspectionType.ChemigationInspectionTypeName,
                ChemigationInspectionTypeDisplayName = chemigationInspectionType.ChemigationInspectionTypeDisplayName
            };
            DoCustomSimpleDtoMappings(chemigationInspectionType, chemigationInspectionTypeSimpleDto);
            return chemigationInspectionTypeSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationInspectionType chemigationInspectionType, ChemigationInspectionTypeSimpleDto chemigationInspectionTypeSimpleDto);
    }
}