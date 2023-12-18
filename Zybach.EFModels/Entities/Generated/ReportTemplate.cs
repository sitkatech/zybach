using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("ReportTemplate")]
[Index("DisplayName", Name = "AK_ReportTemplate_DisplayName", IsUnique = true)]
public partial class ReportTemplate
{
    [Key]
    public int ReportTemplateID { get; set; }

    public int FileResourceID { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string DisplayName { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string Description { get; set; }

    public int ReportTemplateModelTypeID { get; set; }

    public int ReportTemplateModelID { get; set; }

    [ForeignKey("FileResourceID")]
    [InverseProperty("ReportTemplates")]
    public virtual FileResource FileResource { get; set; }
}
