using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("AgHubIrrigationUnit")]
    [Index("WellTPID", Name = "AK_AgHubIrrigationUnit_WellTPID", IsUnique = true)]
    public partial class AgHubIrrigationUnit
    {
        public AgHubIrrigationUnit()
        {
            AgHubIrrigationUnitGeometries = new HashSet<AgHubIrrigationUnitGeometry>();
            AgHubIrrigationUnitWaterYearMonthETData = new HashSet<AgHubIrrigationUnitWaterYearMonthETDatum>();
            AgHubIrrigationUnitWaterYearMonthPrecipitationData = new HashSet<AgHubIrrigationUnitWaterYearMonthPrecipitationDatum>();
            AgHubWells = new HashSet<AgHubWell>();
        }

        [Key]
        public int AgHubIrrigationUnitID { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string WellTPID { get; set; }
        public double? IrrigationUnitAreaInAcres { get; set; }

        [InverseProperty("AgHubIrrigationUnit")]
        public virtual ICollection<AgHubIrrigationUnitGeometry> AgHubIrrigationUnitGeometries { get; set; }
        [InverseProperty("AgHubIrrigationUnit")]
        public virtual ICollection<AgHubIrrigationUnitWaterYearMonthETDatum> AgHubIrrigationUnitWaterYearMonthETData { get; set; }
        [InverseProperty("AgHubIrrigationUnit")]
        public virtual ICollection<AgHubIrrigationUnitWaterYearMonthPrecipitationDatum> AgHubIrrigationUnitWaterYearMonthPrecipitationData { get; set; }
        [InverseProperty("AgHubIrrigationUnit")]
        public virtual ICollection<AgHubWell> AgHubWells { get; set; }
    }
}
