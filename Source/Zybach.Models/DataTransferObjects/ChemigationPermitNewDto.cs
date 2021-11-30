using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitNewDto
    {
        [Required]
        public int ChemigationPermitNumber { get; set; }
        [Required]
        public int ChemigationPermitStatusID { get; set; }
        [Required]
        public string TownshipRangeSection { get; set; }
        [Required]
        public int ChemigationCountyID { get; set; }
        [Required]
        public ChemigationPermitAnnualRecordUpsertDto ChemigationPermitAnnualRecord { get; set; }
    }
}
