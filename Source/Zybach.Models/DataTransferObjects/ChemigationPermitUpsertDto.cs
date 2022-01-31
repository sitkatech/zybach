using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitUpsertDto
    {
        [Required(ErrorMessage = "Status is required")]
        public int? ChemigationPermitStatusID { get; set; }
        [Required(ErrorMessage = "County is required")]
        public int? CountyID { get; set; }
    }
}