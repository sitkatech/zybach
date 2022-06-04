using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitAnnualRecordApplicatorUpsertDto
    {
        public int ChemigationPermitAnnualRecordApplicatorID { get; set; }
        public int ChemigationPermitAnnualRecordID { get; set; }
        [Display(Name = "Applicator Name")]
        [Required]
        public string ApplicatorName { get; set; }
        [Display(Name = "Certification Number")]
        [Required]
        public int? CertificationNumber { get; set; }
        [Display(Name = "Expiration Year")]
        [Required]
        public int? ExpirationYear { get; set; }
        [Display(Name = "Home Phone")]
        [RegularExpression(@"^\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Phone numbers must be submitted in 10 digit format with optional hyphens or spaces")]
        public string HomePhone { get; set; }
        [Display(Name = "Mobile Phone")]
        [RegularExpression(@"^\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Phone numbers must be submitted in 10 digit format with optional hyphens or spaces")]
        public string MobilePhone { get; set; }
    }
}