using System;
using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitAnnualRecordUpsertDto
    {
        [Required] 
        public int ChemigationPermitID { get; set; }
        [Required]
        public int ChemigationPermitStatusID { get; set; }
        [Required]
        public string ApplicantFirstName { get; set; }
        [Required]
        public string ApplicantLastName { get; set; }
        [Required]
        public string PivotName { get; set; }
        [Required]
        public int RecordYear { get; set; }
    }
}