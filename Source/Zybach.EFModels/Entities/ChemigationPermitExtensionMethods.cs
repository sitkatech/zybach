using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermitExtensionMethods
    {
        public static ChemigationPermitDetailedDto AsDetailedDto(this ChemigationPermit chemigationPermit,
            ChemigationPermitAnnualRecordDetailedDto chemigationPermitAnnualRecordDetailedDto)
        {
            var chemigationPermitDetailedDto = new ChemigationPermitDetailedDto()
            {
                ChemigationPermitID = chemigationPermit.ChemigationPermitID,
                ChemigationPermitNumber = chemigationPermit.ChemigationPermitNumber,
                ChemigationPermitStatus = chemigationPermit.ChemigationPermitStatus.AsDto(),
                DateCreated = chemigationPermit.DateCreated,
                TownshipRangeSection = chemigationPermit.TownshipRangeSection,
                ChemigationCounty = chemigationPermit.ChemigationCounty.AsDto(),
                LatestAnnualRecord = chemigationPermitAnnualRecordDetailedDto
            };
            return chemigationPermitDetailedDto;
        }
    }
}