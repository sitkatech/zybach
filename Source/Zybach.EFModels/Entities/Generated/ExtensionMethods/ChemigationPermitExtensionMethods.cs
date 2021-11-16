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
                TotalAcresTreated = chemigationPermit.TotalAcresTreated,
                DateCreated = chemigationPermit.DateCreated,
                TownshipRangeSection = chemigationPermit.TownshipRangeSection,
                ChemigationCounty = chemigationPermit.ChemigationCounty.AsDto()
            };
            DoCustomMappings(chemigationPermit, chemigationPermitDto);
            return chemigationPermitDto;
        }

        static partial void DoCustomMappings(ChemigationPermit chemigationPermit, ChemigationPermitDto chemigationPermitDto);

    }
}