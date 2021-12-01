using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public partial class ChemigationPermitAnnualRecordApplicatorUpsertDto
    {
        public int ChemigationPermitAnnualRecordApplicatorID { get; set; }
        public int ChemigationPermitAnnualRecordID { get; set; }
        public string ApplicatorName { get; set; }
        public int CertificationNumber { get; set; }
        [Range(2020, 2050, ErrorMessage = "Expiration Year needs to be between 2020 and 20250")]
        public int ExpirationYear { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone numbers must be submitted in 10 digit format with optional hyphens or spaces")]
        public string HomePhone { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone numbers must be submitted in 10 digit format with optional hyphens or spaces")]
        public string MobilePhone { get; set; }
    }
}