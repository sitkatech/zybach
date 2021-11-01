using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace Zybach.API.Controllers
{
    public class ReportTemplateUpdateDto
    {
        [Required]
        public int ReportTemplateID { get; set; }
        public IFormFile FileResource { get; set; }
        [Required]
        public string DisplayName { get; set; }
        public string Description { get; set; }
        [Required]
        public int ReportTemplateModelID { get; set; }
    }
}