using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitAnnualRecordUpsertDto
    {
        [Display(Name = "Status")]
        [Required]
        public int ChemigationPermitAnnualRecordStatusID { get; set; }
        [Display(Name = "Fee Type")]
        [Required(ErrorMessage = "Fee Type is required")]
        public int ChemigationPermitAnnualRecordFeeTypeID { get; set; }
        public string ApplicantFirstName { get; set; }
        public string ApplicantLastName { get; set; }
        public string ApplicantCompany { get; set; }
        public string PivotName { get; set; }
        [Display(Name = "Year")]
        [Required]
        public int RecordYear { get; set; }
        [Display(Name = "Township-Range-Section")]
        [Required]
        public string TownshipRangeSection { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DatePaid { get; set; }
        public DateTime? DateApproved { get; set; }
        [Display(Name = "Injection Unit Type")]
        [Required]
        public int ChemigationInjectionUnitTypeID { get; set; }
        [Display(Name = "Home Phone")]
        [RegularExpression(@"^\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Phone numbers must be submitted in 10 digit format with optional hyphens or spaces")]
        public string ApplicantPhone { get; set; }
        [Display(Name = "Mobile Phone")]
        [RegularExpression(@"^\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Phone numbers must be submitted in 10 digit format with optional hyphens or spaces")]
        public string ApplicantMobilePhone { get; set; }
        [Display(Name = "Address")]
        [Required]
        [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
        public string ApplicantMailingAddress { get; set; }
        public string ApplicantEmail { get; set; }
        [Display(Name = "City")]
        [Required]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string ApplicantCity { get; set; }
        [Display(Name = "State")]
        [Required]
        [StringLength(20, ErrorMessage = "State cannot exceed 20 characters.")]
        public string ApplicantState { get; set; }
        [Display(Name = "Zip Code")]
        [Required]
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Zip codes must be formatted in either 5 digit or hyphenated 5+4 digit format")]
        public string ApplicantZipCode { get; set; }
        public decimal? NDEEAmount { get; set; }
        public string AnnualNotes { get; set; }
        public List<ChemigationPermitAnnualRecordChemicalFormulationUpsertDto> ChemicalFormulations { get; set; }
        public List<ChemigationPermitAnnualRecordApplicatorUpsertDto> Applicators { get; set; }
    }
}