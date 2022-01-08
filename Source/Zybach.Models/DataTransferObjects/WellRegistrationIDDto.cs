using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class WellRegistrationIDDto
    {
        [Required] 
        public string WellRegistrationID { get; set; }
    }
}
