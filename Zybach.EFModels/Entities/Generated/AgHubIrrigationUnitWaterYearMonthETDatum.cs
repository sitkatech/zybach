using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("AgHubIrrigationUnitWaterYearMonthETDatum")]
    public partial class AgHubIrrigationUnitWaterYearMonthETDatum
    {
        [Key]
        public int AgHubIrrigationUnitWaterYearMonthETDatumID { get; set; }
        public int AgHubIrrigationUnitID { get; set; }
        public int WaterYearMonthID { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? EvapotranspirationInches { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? EvapotranspirationAcreInches { get; set; }

        [ForeignKey("AgHubIrrigationUnitID")]
        [InverseProperty("AgHubIrrigationUnitWaterYearMonthETData")]
        public virtual AgHubIrrigationUnit AgHubIrrigationUnit { get; set; }
        [ForeignKey("WaterYearMonthID")]
        [InverseProperty("AgHubIrrigationUnitWaterYearMonthETData")]
        public virtual WaterYearMonth WaterYearMonth { get; set; }
    }
}
