using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("Well")]
    public partial class Well
    {
        public Well()
        {
            ChemigationPermits = new HashSet<ChemigationPermit>();
            Sensors = new HashSet<Sensor>();
            WaterQualityInspections = new HashSet<WaterQualityInspection>();
        }

        [Key]
        public int WellID { get; set; }
        [Required]
        [StringLength(100)]
        public string WellRegistrationID { get; set; }
        [Required]
        [Column(TypeName = "geometry")]
        public Geometry WellGeometry { get; set; }
        public int? StreamflowZoneID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastUpdateDate { get; set; }

        [ForeignKey(nameof(StreamflowZoneID))]
        [InverseProperty(nameof(StreamFlowZone.Wells))]
        public virtual StreamFlowZone StreamflowZone { get; set; }
        [InverseProperty("Well")]
        public virtual AgHubWell AgHubWell { get; set; }
        [InverseProperty("Well")]
        public virtual GeoOptixWell GeoOptixWell { get; set; }
        [InverseProperty(nameof(ChemigationPermit.Well))]
        public virtual ICollection<ChemigationPermit> ChemigationPermits { get; set; }
        [InverseProperty(nameof(Sensor.Well))]
        public virtual ICollection<Sensor> Sensors { get; set; }
        [InverseProperty(nameof(WaterQualityInspection.Well))]
        public virtual ICollection<WaterQualityInspection> WaterQualityInspections { get; set; }
    }
}
