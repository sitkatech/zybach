using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("WaterYearMonth")]
    [Index(nameof(Year), nameof(Month), Name = "AK_WaterYearMonth_Year_Month", IsUnique = true)]
    public partial class WaterYearMonth
    {
        public WaterYearMonth()
        {
            AgHubIrrigationUnitWaterYearMonthETData = new HashSet<AgHubIrrigationUnitWaterYearMonthETDatum>();
            OpenETSyncHistories = new HashSet<OpenETSyncHistory>();
        }

        [Key]
        public int WaterYearMonthID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FinalizeDate { get; set; }

        [InverseProperty(nameof(AgHubIrrigationUnitWaterYearMonthETDatum.WaterYearMonth))]
        public virtual ICollection<AgHubIrrigationUnitWaterYearMonthETDatum> AgHubIrrigationUnitWaterYearMonthETData { get; set; }
        [InverseProperty(nameof(OpenETSyncHistory.WaterYearMonth))]
        public virtual ICollection<OpenETSyncHistory> OpenETSyncHistories { get; set; }
    }
}
