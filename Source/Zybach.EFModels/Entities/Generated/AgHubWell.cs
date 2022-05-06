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
    [Index(nameof(WellID), Name = "AK_AgHubWell_WellID", IsUnique = true)]
    public partial class AgHubWell
    {
        public AgHubWell()
        {
            AgHubWellIrrigatedAcres = new HashSet<AgHubWellIrrigatedAcre>();
        }

        [Key]
        public int AgHubWellID { get; set; }
        public int WellID { get; set; }
        [Required]
        [Column(TypeName = "geometry")]
        public Geometry AgHubWellGeometry { get; set; }
        public int? WellTPNRDPumpRate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? TPNRDPumpRateUpdated { get; set; }
        public bool WellConnectedMeter { get; set; }
        public int? WellAuditPumpRate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditPumpRateUpdated { get; set; }
        public bool HasElectricalData { get; set; }
        public int? RegisteredPumpRate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RegisteredUpdated { get; set; }
        [StringLength(100)]
        public string AgHubRegisteredUser { get; set; }
        [StringLength(100)]
        public string FieldName { get; set; }
        public int? AgHubIrrigationUnitID { get; set; }

        [ForeignKey(nameof(AgHubIrrigationUnitID))]
        [InverseProperty("AgHubWells")]
        public virtual AgHubIrrigationUnit AgHubIrrigationUnit { get; set; }
        [ForeignKey(nameof(WellID))]
        [InverseProperty("AgHubWell")]
        public virtual Well Well { get; set; }
        [InverseProperty(nameof(AgHubWellIrrigatedAcre.AgHubWell))]
        public virtual ICollection<AgHubWellIrrigatedAcre> AgHubWellIrrigatedAcres { get; set; }
    }
}
