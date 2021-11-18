using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

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