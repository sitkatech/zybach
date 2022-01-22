using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace Zybach.API.Controllers
{
    public class ReportTemplateUpdateDto
    {
        public IFormFile FileResource { get; set; }
        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [Required]
        public int ReportTemplateModelID { get; set; }
    }
}