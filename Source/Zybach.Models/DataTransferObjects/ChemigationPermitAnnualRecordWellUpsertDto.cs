using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitAnnualRecordWellUpsertDto
    {
        public int ChemigationPermitAnnualRecordWellID { get; set; }
        public int ChemigationPermitAnnualRecordID { get; set; }
        public string WellRegistrationID { get; set; }
    }
}