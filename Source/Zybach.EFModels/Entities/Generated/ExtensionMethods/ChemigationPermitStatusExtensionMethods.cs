//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitStatus]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationPermitStatusExtensionMethods
    {
        public static ChemigationPermitStatusDto AsDto(this ChemigationPermitStatus chemigationPermitStatus)
        {
            var chemigationPermitStatusDto = new ChemigationPermitStatusDto()
            {
                ChemigationPermitStatusID = chemigationPermitStatus.ChemigationPermitStatusID,
                ChemigationPermitStatusName = chemigationPermitStatus.ChemigationPermitStatusName,
                ChemigationPermitStatusDisplayName = chemigationPermitStatus.ChemigationPermitStatusDisplayName
            };
            DoCustomMappings(chemigationPermitStatus, chemigationPermitStatusDto);
            return chemigationPermitStatusDto;
        }

        static partial void DoCustomMappings(ChemigationPermitStatus chemigationPermitStatus, ChemigationPermitStatusDto chemigationPermitStatusDto);

        public static ChemigationPermitStatusSimpleDto AsSimpleDto(this ChemigationPermitStatus chemigationPermitStatus)
        {
            var chemigationPermitStatusSimpleDto = new ChemigationPermitStatusSimpleDto()
            {
                ChemigationPermitStatusID = chemigationPermitStatus.ChemigationPermitStatusID,
                ChemigationPermitStatusName = chemigationPermitStatus.ChemigationPermitStatusName,
                ChemigationPermitStatusDisplayName = chemigationPermitStatus.ChemigationPermitStatusDisplayName
            };
            DoCustomSimpleDtoMappings(chemigationPermitStatus, chemigationPermitStatusSimpleDto);
            return chemigationPermitStatusSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationPermitStatus chemigationPermitStatus, ChemigationPermitStatusSimpleDto chemigationPermitStatusSimpleDto);
    }
}