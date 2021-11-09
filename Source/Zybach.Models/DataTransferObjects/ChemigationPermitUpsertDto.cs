using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitUpsertDto
    {
        [Required]
        public int ChemigationPermitNumber { get; set; }
        [Required]
        public int ChemigationPermitStatusID { get; set; }
        [Required]
        public DateTime DateReceived { get; set; }
        [Required]
        public string TownshipRangeSection { get; set; }
    }
}