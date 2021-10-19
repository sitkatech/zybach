using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("StreamFlowZone")]
    [Index(nameof(StreamFlowZoneName), Name = "AK_StreamFlowZone_StreamFlowZoneName", IsUnique = true)]
    public partial class StreamFlowZone
    {
        public StreamFlowZone()
        {
            AgHubWells = new HashSet<AgHubWell>();
        }

        [Key]
        public int StreamFlowZoneID { get; set; }
        [Required]
        [StringLength(100)]
        public string StreamFlowZoneName { get; set; }
        [Required]
        [Column(TypeName = "geometry")]
        public Geometry StreamFlowZoneGeometry { get; set; }
        public double StreamFlowZoneArea { get; set; }

        [InverseProperty(nameof(AgHubWell.StreamflowZone))]
        public virtual ICollection<AgHubWell> AgHubWells { get; set; }
    }
}
