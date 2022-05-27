using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitNewDto
    {
        [Required(ErrorMessage = "Status is required")]
        public int? ChemigationPermitStatusID { get; set; }
        [Required(ErrorMessage = "County is required")]
        public int? CountyID { get; set; }
        public string WellRegistrationID { get; set; }
        [Required]
        public ChemigationPermitAnnualRecordUpsertDto ChemigationPermitAnnualRecord { get; set; }
    }
}
