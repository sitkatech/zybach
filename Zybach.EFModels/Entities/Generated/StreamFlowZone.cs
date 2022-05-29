using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Zybach.EFModels.Entities
{
    [Table("StreamFlowZone")]
    [Index("StreamFlowZoneName", Name = "AK_StreamFlowZone_StreamFlowZoneName", IsUnique = true)]
    public partial class StreamFlowZone
    {
        public StreamFlowZone()
        {
            Wells = new HashSet<Well>();
        }

        [Key]
        public int StreamFlowZoneID { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string StreamFlowZoneName { get; set; }
        [Required]
        [Column(TypeName = "geometry")]
        public Geometry StreamFlowZoneGeometry { get; set; }
        public double StreamFlowZoneArea { get; set; }

        [InverseProperty("StreamflowZone")]
        public virtual ICollection<Well> Wells { get; set; }
    }
}
