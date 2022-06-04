using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class WellNewDto
    {
        [Required]
        public string WellRegistrationID { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }
    }
}