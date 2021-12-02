using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitAnnualRecordApplicatorUpsertDto
    {
        public int ChemigationPermitAnnualRecordApplicatorID { get; set; }
        public int ChemigationPermitAnnualRecordID { get; set; }
        public string ApplicatorName { get; set; }
        public int CertificationNumber { get; set; }
        public int ExpirationYear { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone numbers must be submitted in 10 digit format with optional hyphens or spaces")]
        public string HomePhone { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone numbers must be submitted in 10 digit format with optional hyphens or spaces")]
        public string MobilePhone { get; set; }
    }
}