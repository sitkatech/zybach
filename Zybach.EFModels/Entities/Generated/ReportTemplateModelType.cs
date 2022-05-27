using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ReportTemplateModelType")]
    [Index(nameof(ReportTemplateModelTypeDisplayName), Name = "AK_ReportTemplateModelType_ReportTemplateModelTypeDisplayName", IsUnique = true)]
    [Index(nameof(ReportTemplateModelTypeName), Name = "AK_ReportTemplateModelType_ReportTemplateModelTypeName", IsUnique = true)]
    public partial class ReportTemplateModelType
    {
        public ReportTemplateModelType()
        {
            ReportTemplates = new HashSet<ReportTemplate>();
        }

        [Key]
        public int ReportTemplateModelTypeID { get; set; }
        [Required]
        [StringLength(100)]
        public string ReportTemplateModelTypeName { get; set; }
        [Required]
        [StringLength(100)]
        public string ReportTemplateModelTypeDisplayName { get; set; }
        [Required]
        [StringLength(250)]
        public string ReportTemplateModelTypeDescription { get; set; }

        [InverseProperty(nameof(ReportTemplate.ReportTemplateModelType))]
        public virtual ICollection<ReportTemplate> ReportTemplates { get; set; }
    }
}
