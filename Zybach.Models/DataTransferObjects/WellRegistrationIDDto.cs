using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class WellRegistrationIDDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Registration ID cannot exceed 100 characters")]
        public string WellRegistrationID { get; set; }
    }
}
