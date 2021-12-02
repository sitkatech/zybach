using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermitAnnualRecordWellExtensionMethods
    {
        static partial void DoCustomSimpleDtoMappings(ChemigationPermitAnnualRecordWell chemigationPermitAnnualRecordWell,
            ChemigationPermitAnnualRecordWellSimpleDto chemigationPermitAnnualRecordWellSimpleDto)
        {
            chemigationPermitAnnualRecordWellSimpleDto.WellRegistrationID = chemigationPermitAnnualRecordWell.Well.WellRegistrationID;
        }
    }
}