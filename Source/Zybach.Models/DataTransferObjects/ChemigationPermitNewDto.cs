using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitNewDto
    {
        [Required]
        public int ChemigationPermitStatusID { get; set; }
        [Required]
        public int CountyID { get; set; }
        public string WellRegistrationID { get; set; }
        [Required]
        public ChemigationPermitAnnualRecordUpsertDto ChemigationPermitAnnualRecord { get; set; }
    }
}
