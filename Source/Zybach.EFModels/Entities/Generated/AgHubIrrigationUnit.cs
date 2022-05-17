using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("AgHubIrrigationUnit")]
    [Index(nameof(WellTPID), Name = "AK_AgHubIrrigationUnit_WellTPID", IsUnique = true)]
    public partial class AgHubIrrigationUnit
    {
        public AgHubIrrigationUnit()
        {
            AgHubIrrigationUnitWaterYearMonthETData = new HashSet<AgHubIrrigationUnitWaterYearMonthETDatum>();
            AgHubWells = new HashSet<AgHubWell>();
        }

        [Key]
        public int AgHubIrrigationUnitID { get; set; }
        [Required]
        [StringLength(100)]
        public string WellTPID { get; set; }
        [Column(TypeName = "geometry")]
        public Geometry IrrigationUnitGeometry { get; set; }
        public double? IrrigationUnitAreaInAcres { get; set; }

        [InverseProperty(nameof(AgHubIrrigationUnitWaterYearMonthETDatum.AgHubIrrigationUnit))]
        public virtual ICollection<AgHubIrrigationUnitWaterYearMonthETDatum> AgHubIrrigationUnitWaterYearMonthETData { get; set; }
        [InverseProperty(nameof(AgHubWell.AgHubIrrigationUnit))]
        public virtual ICollection<AgHubWell> AgHubWells { get; set; }
    }
}
