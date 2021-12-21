using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermitExtensionMethods
    {
        static partial void DoCustomMappings(ChemigationPermit chemigationPermit, ChemigationPermitDto chemigationPermitDto)
        {
            chemigationPermitDto.ChemigationPermitNumberDisplay = chemigationPermit.ChemigationPermitNumberDisplay;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationPermit chemigationPermit,
            ChemigationPermitSimpleDto chemigationPermitSimpleDto)
        {
            chemigationPermitSimpleDto.ChemigationPermitNumberDisplay = chemigationPermit.ChemigationPermitNumberDisplay;
        }

        public static ChemigationPermitDetailedDto AsDetailedDto(this ChemigationPermit chemigationPermit,
            ChemigationPermitAnnualRecordDetailedDto chemigationPermitAnnualRecordDetailedDto)
        {
            var chemigationPermitDetailedDto = new ChemigationPermitDetailedDto()
            {
                ChemigationPermitID = chemigationPermit.ChemigationPermitID,
                ChemigationPermitNumber = chemigationPermit.ChemigationPermitNumber,
                ChemigationPermitNumberDisplay = chemigationPermit.ChemigationPermitNumberDisplay,
                ChemigationPermitStatus = chemigationPermit.ChemigationPermitStatus.AsSimpleDto(),
                DateCreated = chemigationPermit.DateCreated,
                ChemigationCounty = chemigationPermit.ChemigationCounty.AsSimpleDto(),
                Well = chemigationPermit.Well?.AsSimpleDto(),
                LatestAnnualRecord = chemigationPermitAnnualRecordDetailedDto
            };
            return chemigationPermitDetailedDto;
        }
    }
}   