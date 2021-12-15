using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitAnnualRecordApplicatorUpsertDto
    {
        public int ChemigationPermitAnnualRecordApplicatorID { get; set; }
        public int ChemigationPermitAnnualRecordID { get; set; }
        [Required]
        public string ApplicatorName { get; set; }
        [Required]
        public int? CertificationNumber { get; set; }
        [Required]
        public int? ExpirationYear { get; set; }
        [RegularExpression(@"^\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Phone numbers must be submitted in 10 digit format with optional hyphens or spaces")]
        public string HomePhone { get; set; }
        [RegularExpression(@"^\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Phone numbers must be submitted in 10 digit format with optional hyphens or spaces")]
        public string MobilePhone { get; set; }
    }
}