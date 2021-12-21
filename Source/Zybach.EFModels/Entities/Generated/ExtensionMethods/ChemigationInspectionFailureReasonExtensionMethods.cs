//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInspectionFailureReason]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationInspectionFailureReasonExtensionMethods
    {
        public static ChemigationInspectionFailureReasonDto AsDto(this ChemigationInspectionFailureReason chemigationInspectionFailureReason)
        {
            var chemigationInspectionFailureReasonDto = new ChemigationInspectionFailureReasonDto()
            {
                ChemigationInspectionFailureReasonID = chemigationInspectionFailureReason.ChemigationInspectionFailureReasonID,
                ChemigationInspectionFailureReasonName = chemigationInspectionFailureReason.ChemigationInspectionFailureReasonName,
                ChemigationInspectionFailureReasonDisplayName = chemigationInspectionFailureReason.ChemigationInspectionFailureReasonDisplayName
            };
            DoCustomMappings(chemigationInspectionFailureReason, chemigationInspectionFailureReasonDto);
            return chemigationInspectionFailureReasonDto;
        }

        static partial void DoCustomMappings(ChemigationInspectionFailureReason chemigationInspectionFailureReason, ChemigationInspectionFailureReasonDto chemigationInspectionFailureReasonDto);

        public static ChemigationInspectionFailureReasonSimpleDto AsSimpleDto(this ChemigationInspectionFailureReason chemigationInspectionFailureReason)
        {
            var chemigationInspectionFailureReasonSimpleDto = new ChemigationInspectionFailureReasonSimpleDto()
            {
                ChemigationInspectionFailureReasonID = chemigationInspectionFailureReason.ChemigationInspectionFailureReasonID,
                ChemigationInspectionFailureReasonName = chemigationInspectionFailureReason.ChemigationInspectionFailureReasonName,
                ChemigationInspectionFailureReasonDisplayName = chemigationInspectionFailureReason.ChemigationInspectionFailureReasonDisplayName
            };
            DoCustomSimpleDtoMappings(chemigationInspectionFailureReason, chemigationInspectionFailureReasonSimpleDto);
            return chemigationInspectionFailureReasonSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationInspectionFailureReason chemigationInspectionFailureReason, ChemigationInspectionFailureReasonSimpleDto chemigationInspectionFailureReasonSimpleDto);
    }
}