using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum")]
    public partial class AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum
    {
        [Key]
        public int AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumID { get; set; }
        public int AgHubIrrigationUnitID { get; set; }
        public int WaterYearMonthID { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? EvapotranspirationAcreFeet { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? EvapotranspirationInches { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? PrecipitationAcreFeet { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? PrecipitationInches { get; set; }

        [ForeignKey(nameof(AgHubIrrigationUnitID))]
        [InverseProperty("AgHubIrrigationUnitWaterYearMonthETAndPrecipitationData")]
        public virtual AgHubIrrigationUnit AgHubIrrigationUnit { get; set; }
        [ForeignKey(nameof(WaterYearMonthID))]
        [InverseProperty("AgHubIrrigationUnitWaterYearMonthETAndPrecipitationData")]
        public virtual WaterYearMonth WaterYearMonth { get; set; }
    }
}
