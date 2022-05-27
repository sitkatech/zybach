using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ReportTemplate")]
    [Index(nameof(DisplayName), Name = "AK_ReportTemplate_DisplayName", IsUnique = true)]
    public partial class ReportTemplate
    {
        [Key]
        public int ReportTemplateID { get; set; }
        public int FileResourceID { get; set; }
        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public int ReportTemplateModelTypeID { get; set; }
        public int ReportTemplateModelID { get; set; }

        [ForeignKey(nameof(FileResourceID))]
        [InverseProperty("ReportTemplates")]
        public virtual FileResource FileResource { get; set; }
        [ForeignKey(nameof(ReportTemplateModelID))]
        [InverseProperty("ReportTemplates")]
        public virtual ReportTemplateModel ReportTemplateModel { get; set; }
        [ForeignKey(nameof(ReportTemplateModelTypeID))]
        [InverseProperty("ReportTemplates")]
        public virtual ReportTemplateModelType ReportTemplateModelType { get; set; }
    }
}
