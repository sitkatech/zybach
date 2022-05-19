using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

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
        public decimal? EvapotranspirationRateInches { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? EvapotranspirationRateAcreFeet { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? PrecipitationAcreFeet { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? PrecipitationInches { get; set; }

        [ForeignKey(nameof(AgHubIrrigationUnitID))]
        [InverseProperty("AgHubIrrigationUnitWaterYearMonthETData")]
        public virtual AgHubIrrigationUnit AgHubIrrigationUnit { get; set; }
        [ForeignKey(nameof(WaterYearMonthID))]
        [InverseProperty("AgHubIrrigationUnitWaterYearMonthETData")]
        public virtual WaterYearMonth WaterYearMonth { get; set; }
    }
}
