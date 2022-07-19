using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Zybach.EFModels.Entities
{
    [Table("AgHubIrrigationUnitGeometry")]
    [Index("AgHubIrrigationUnitID", Name = "AK_AgHubIrrigationUnitGeometry_AgHubIrrigationUnitID", IsUnique = true)]
    public partial class AgHubIrrigationUnitGeometry
    {
        [Key]
        public int AgHubIrrigationUnitGeometryID { get; set; }
        public int AgHubIrrigationUnitID { get; set; }
        [Required]
        [Column(TypeName = "geometry")]
        public Geometry IrrigationUnitGeometry { get; set; }

        [ForeignKey("AgHubIrrigationUnitID")]
        [InverseProperty("AgHubIrrigationUnitGeometry")]
        public virtual AgHubIrrigationUnit AgHubIrrigationUnit { get; set; }
    }
}
