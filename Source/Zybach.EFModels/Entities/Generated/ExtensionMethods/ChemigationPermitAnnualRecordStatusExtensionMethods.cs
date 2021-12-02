//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecordStatus]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationPermitAnnualRecordStatusExtensionMethods
    {
        public static ChemigationPermitAnnualRecordStatusDto AsDto(this ChemigationPermitAnnualRecordStatus chemigationPermitAnnualRecordStatus)
        {
            var chemigationPermitAnnualRecordStatusDto = new ChemigationPermitAnnualRecordStatusDto()
            {
                ChemigationPermitAnnualRecordStatusID = chemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusID,
                ChemigationPermitAnnualRecordStatusName = chemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusName,
                ChemigationPermitAnnualRecordStatusDisplayName = chemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusDisplayName
            };
            DoCustomMappings(chemigationPermitAnnualRecordStatus, chemigationPermitAnnualRecordStatusDto);
            return chemigationPermitAnnualRecordStatusDto;
        }

        static partial void DoCustomMappings(ChemigationPermitAnnualRecordStatus chemigationPermitAnnualRecordStatus, ChemigationPermitAnnualRecordStatusDto chemigationPermitAnnualRecordStatusDto);

        public static ChemigationPermitAnnualRecordStatusSimpleDto AsSimpleDto(this ChemigationPermitAnnualRecordStatus chemigationPermitAnnualRecordStatus)
        {
            var chemigationPermitAnnualRecordStatusSimpleDto = new ChemigationPermitAnnualRecordStatusSimpleDto()
            {
                ChemigationPermitAnnualRecordStatusID = chemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusID,
                ChemigationPermitAnnualRecordStatusName = chemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusName,
                ChemigationPermitAnnualRecordStatusDisplayName = chemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusDisplayName
            };
            DoCustomSimpleDtoMappings(chemigationPermitAnnualRecordStatus, chemigationPermitAnnualRecordStatusSimpleDto);
            return chemigationPermitAnnualRecordStatusSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationPermitAnnualRecordStatus chemigationPermitAnnualRecordStatus, ChemigationPermitAnnualRecordStatusSimpleDto chemigationPermitAnnualRecordStatusSimpleDto);
    }
}