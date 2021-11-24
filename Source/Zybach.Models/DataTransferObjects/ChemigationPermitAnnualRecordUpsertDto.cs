using System;
using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitAnnualRecordUpsertDto
    {
        [Required]
        public int ChemigationPermitAnnualRecordStatusID { get; set; }
        [Required]
        public string ApplicantFirstName { get; set; }
        [Required]
        public string ApplicantLastName { get; set; }
        [Required]
        public string PivotName { get; set; }
        [Required]
        public int RecordYear { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DatePaid { get; set; }
        [Required]
        public int ChemigationInjectionUnitTypeID { get; set; }
        public string ApplicantPhone { get; set; }
        public string ApplicantMobilePhone { get; set; }
        public string ApplicantMailingAddress { get; set; }
        public string ApplicantEmail { get; set; }
        public string ApplicantCity { get; set; }
        public string ApplicantState { get; set; }
        public int ApplicantZipCode { get; set; }
    }
}