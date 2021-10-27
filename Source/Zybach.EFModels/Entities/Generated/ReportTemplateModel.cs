using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ReportTemplateModel")]
    [Index(nameof(ReportTemplateModelDisplayName), Name = "AK_ReportTemplateModel_ReportTemplateModelDisplayName", IsUnique = true)]
    [Index(nameof(ReportTemplateModelName), Name = "AK_ReportTemplateModel_ReportTemplateModelName", IsUnique = true)]
    public partial class ReportTemplateModel
    {
        public ReportTemplateModel()
        {
            ReportTemplates = new HashSet<ReportTemplate>();
        }

        [Key]
        public int ReportTemplateModelID { get; set; }
        [Required]
        [StringLength(100)]
        public string ReportTemplateModelName { get; set; }
        [Required]
        [StringLength(100)]
        public string ReportTemplateModelDisplayName { get; set; }
        [Required]
        [StringLength(250)]
        public string ReportTemplateModelDescription { get; set; }

        [InverseProperty(nameof(ReportTemplate.ReportTemplateModel))]
        public virtual ICollection<ReportTemplate> ReportTemplates { get; set; }
    }
}
