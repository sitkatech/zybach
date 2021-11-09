//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermit]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationPermitExtensionMethods
    {
        public static ChemigationPermitDto AsDto(this ChemigationPermit chemigationPermit)
        {
            var chemigationPermitDto = new ChemigationPermitDto()
            {
                ChemigationPermitID = chemigationPermit.ChemigationPermitID,
                ChemigationPermitNumber = chemigationPermit.ChemigationPermitNumber,
                ChemigationPermitStatus = chemigationPermit.ChemigationPermitStatus.AsDto(),
                DateReceived = chemigationPermit.DateReceived,
                TownshipRangeSection = chemigationPermit.TownshipRangeSection
            };
            DoCustomMappings(chemigationPermit, chemigationPermitDto);
            return chemigationPermitDto;
        }

        static partial void DoCustomMappings(ChemigationPermit chemigationPermit, ChemigationPermitDto chemigationPermitDto);

    }
}