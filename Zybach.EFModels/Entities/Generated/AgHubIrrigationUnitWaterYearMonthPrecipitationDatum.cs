using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("AgHubIrrigationUnitWaterYearMonthPrecipitationDatum")]
    public partial class AgHubIrrigationUnitWaterYearMonthPrecipitationDatum
    {
        [Key]
        public int AgHubIrrigationUnitWaterYearMonthPrecipitationDatumID { get; set; }
        public int AgHubIrrigationUnitID { get; set; }
        public int WaterYearMonthID { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? PrecipitationAcreFeet { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? PrecipitationInches { get; set; }

        [ForeignKey(nameof(AgHubIrrigationUnitID))]
        [InverseProperty("AgHubIrrigationUnitWaterYearMonthPrecipitationData")]
        public virtual AgHubIrrigationUnit AgHubIrrigationUnit { get; set; }
        [ForeignKey(nameof(WaterYearMonthID))]
        [InverseProperty("AgHubIrrigationUnitWaterYearMonthPrecipitationData")]
        public virtual WaterYearMonth WaterYearMonth { get; set; }
    }
}
