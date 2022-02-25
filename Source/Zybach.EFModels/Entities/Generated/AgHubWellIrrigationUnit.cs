using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("AgHubWellIrrigationUnit")]
    [Index(nameof(AgHubWellID), Name = "AK_AgHubWellIrrigationUnit_AgHubWellID", IsUnique = true)]
    public partial class AgHubWellIrrigationUnit
    {
        [Key]
        public int AgHubWellIrrigationUnitID { get; set; }
        public int AgHubWellID { get; set; }
        [Required]
        [Column(TypeName = "geometry")]
        public Geometry IrrigationUnitGeometry { get; set; }

        [ForeignKey(nameof(AgHubWellID))]
        [InverseProperty("AgHubWellIrrigationUnit")]
        public virtual AgHubWell AgHubWell { get; set; }
    }
}
