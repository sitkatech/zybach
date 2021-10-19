using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("AgHubWell")]
    public partial class AgHubWell
    {
        public AgHubWell()
        {
            AgHubWellIrrigatedAcres = new HashSet<AgHubWellIrrigatedAcre>();
        }

        [Key]
        public int AgHubWellID { get; set; }
        [Required]
        [StringLength(100)]
        public string WellRegistrationID { get; set; }
        [StringLength(100)]
        public string WellTPID { get; set; }
        [Required]
        [Column(TypeName = "geometry")]
        public Geometry WellGeometry { get; set; }
        public int? WellTPNRDPumpRate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? TPNRDPumpRateUpdated { get; set; }
        public bool WellConnectedMeter { get; set; }
        public int? WellAuditPumpRate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditPumpRateUpdated { get; set; }
        public bool HasElectricalData { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FetchDate { get; set; }
        public int? RegisteredPumpRate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RegisteredUpdated { get; set; }
        public int? StreamflowZoneID { get; set; }
        [StringLength(100)]
        public string LandownerName { get; set; }
        [StringLength(100)]
        public string FieldName { get; set; }

        [ForeignKey(nameof(StreamflowZoneID))]
        [InverseProperty(nameof(StreamFlowZone.AgHubWells))]
        public virtual StreamFlowZone StreamflowZone { get; set; }
        [InverseProperty(nameof(AgHubWellIrrigatedAcre.AgHubWell))]
        public virtual ICollection<AgHubWellIrrigatedAcre> AgHubWellIrrigatedAcres { get; set; }
    }
}
