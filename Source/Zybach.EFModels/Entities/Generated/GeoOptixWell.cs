using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("GeoOptixWell")]
    [Index(nameof(WellID), Name = "AK_GeoOptixWell_WellID", IsUnique = true)]
    public partial class GeoOptixWell
    {
        [Key]
        public int GeoOptixWellID { get; set; }
        public int WellID { get; set; }
        [Required]
        [Column(TypeName = "geometry")]
        public Geometry GeoOptixWellGeometry { get; set; }

        [ForeignKey(nameof(WellID))]
        [InverseProperty("GeoOptixWell")]
        public virtual Well Well { get; set; }
    }
}
