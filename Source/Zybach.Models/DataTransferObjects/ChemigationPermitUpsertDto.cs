using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitUpsertDto
    {
        public int ChemigationPermitNumber { get; set; }
        [Required]
        public int ChemigationPermitStatusID { get; set; }
        public string TownshipRangeSection { get; set; }
        public int ChemigationCountyID { get; set; }
    }
}