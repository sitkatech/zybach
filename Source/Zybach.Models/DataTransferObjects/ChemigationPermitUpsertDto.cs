using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitUpsertDto
    {
        [Required]
        public int ChemigationPermitStatusID { get; set; }
        [Required]
        public int CountyID { get; set; }
    }
}