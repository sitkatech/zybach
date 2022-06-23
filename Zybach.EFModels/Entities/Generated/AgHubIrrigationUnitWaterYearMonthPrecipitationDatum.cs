using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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
        public decimal? PrecipitationInches { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? PrecipitationAcreInches { get; set; }

        [ForeignKey("AgHubIrrigationUnitID")]
        [InverseProperty("AgHubIrrigationUnitWaterYearMonthPrecipitationData")]
        public virtual AgHubIrrigationUnit AgHubIrrigationUnit { get; set; }
        [ForeignKey("WaterYearMonthID")]
        [InverseProperty("AgHubIrrigationUnitWaterYearMonthPrecipitationData")]
        public virtual WaterYearMonth WaterYearMonth { get; set; }
    }
}
