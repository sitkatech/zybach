//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInspectionStatus]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationInspectionStatusExtensionMethods
    {
        public static ChemigationInspectionStatusDto AsDto(this ChemigationInspectionStatus chemigationInspectionStatus)
        {
            var chemigationInspectionStatusDto = new ChemigationInspectionStatusDto()
            {
                ChemigationInspectionStatusID = chemigationInspectionStatus.ChemigationInspectionStatusID,
                ChemigationInspectionStatusName = chemigationInspectionStatus.ChemigationInspectionStatusName,
                ChemigationInspectionStatusDisplayName = chemigationInspectionStatus.ChemigationInspectionStatusDisplayName
            };
            DoCustomMappings(chemigationInspectionStatus, chemigationInspectionStatusDto);
            return chemigationInspectionStatusDto;
        }

        static partial void DoCustomMappings(ChemigationInspectionStatus chemigationInspectionStatus, ChemigationInspectionStatusDto chemigationInspectionStatusDto);

        public static ChemigationInspectionStatusSimpleDto AsSimpleDto(this ChemigationInspectionStatus chemigationInspectionStatus)
        {
            var chemigationInspectionStatusSimpleDto = new ChemigationInspectionStatusSimpleDto()
            {
                ChemigationInspectionStatusID = chemigationInspectionStatus.ChemigationInspectionStatusID,
                ChemigationInspectionStatusName = chemigationInspectionStatus.ChemigationInspectionStatusName,
                ChemigationInspectionStatusDisplayName = chemigationInspectionStatus.ChemigationInspectionStatusDisplayName
            };
            DoCustomSimpleDtoMappings(chemigationInspectionStatus, chemigationInspectionStatusSimpleDto);
            return chemigationInspectionStatusSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationInspectionStatus chemigationInspectionStatus, ChemigationInspectionStatusSimpleDto chemigationInspectionStatusSimpleDto);
    }
}