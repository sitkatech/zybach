using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DroolTool.EFModels.Entities
{
    public partial class BackboneSegmentType
    {
        public BackboneSegmentType()
        {
            BackboneSegment = new HashSet<BackboneSegment>();
        }

        [Key]
        public int BackboneSegmentTypeID { get; set; }
        [Required]
        [StringLength(20)]
        public string BackboneSegmentTypeName { get; set; }
        [Required]
        [StringLength(20)]
        public string BackboneSegmentTypeDisplayName { get; set; }

        [InverseProperty("BackboneSegmentType")]
        public virtual ICollection<BackboneSegment> BackboneSegment { get; set; }
    }
}
