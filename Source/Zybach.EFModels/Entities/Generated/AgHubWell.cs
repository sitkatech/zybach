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
    [Index(nameof(WellRegistrationID), Name = "AK_AgHubWell_WellRegistrationID", IsUnique = true)]
    [Index(nameof(WellTPID), Name = "AK_AgHubWell_WellTPID", IsUnique = true)]
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
        [Required]
        [StringLength(100)]
        public string WellTPID { get; set; }
        [Required]
        [Column(TypeName = "geometry")]
        public Geometry WellGeometry { get; set; }
        public int? TPNRDPumpRate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? TPNRDPumpRateUpdated { get; set; }
        public bool? WellConnectedMeter { get; set; }
        public int? WellAuditPumpRate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditPumpRateUpdated { get; set; }
        public bool? HasElectricalData { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FetchDate { get; set; }

        [InverseProperty(nameof(AgHubWellIrrigatedAcre.AgHubWell))]
        public virtual ICollection<AgHubWellIrrigatedAcre> AgHubWellIrrigatedAcres { get; set; }
    }
}
