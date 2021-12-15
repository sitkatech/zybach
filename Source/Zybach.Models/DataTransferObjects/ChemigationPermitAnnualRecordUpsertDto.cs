using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitAnnualRecordUpsertDto
    {
        [Required]
        public int ChemigationPermitAnnualRecordStatusID { get; set; }
        [Required]
        public string ApplicantName { get; set; }
        [Required]
        public string PivotName { get; set; }
        [Required]
        public int RecordYear { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DatePaid { get; set; }
        [Required]
        public int ChemigationInjectionUnitTypeID { get; set; }
        [RegularExpression(@"^\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Phone numbers must be submitted in 10 digit format with optional hyphens or spaces")]
        public string ApplicantPhone { get; set; }
        [RegularExpression(@"^\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Phone numbers must be submitted in 10 digit format with optional hyphens or spaces")]
        public string ApplicantMobilePhone { get; set; }
        public string ApplicantMailingAddress { get; set; }
        public string ApplicantEmail { get; set; }
        public string ApplicantCity { get; set; }
        public string ApplicantState { get; set; }
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Zip codes must be formatted in either 5 digit or hyphenated 5+4 digit format")]
        public string ApplicantZipCode { get; set; }
        public decimal? NDEEAmount { get; set; }
        public List<ChemigationPermitAnnualRecordChemicalFormulationUpsertDto> ChemicalFormulations { get; set; }
        public List<ChemigationPermitAnnualRecordApplicatorUpsertDto> Applicators { get; set; }
    }
}