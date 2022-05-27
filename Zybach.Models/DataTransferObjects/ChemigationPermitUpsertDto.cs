using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitUpsertDto
    {
        [Required(ErrorMessage = "Status is required")]
        public int? ChemigationPermitStatusID { get; set; }
        [Required(ErrorMessage = "County is required")]
        public int? CountyID { get; set; }
        [Required(ErrorMessage = "Well is required")]
        public string? WellRegistrationID { get; set; }
    }
}