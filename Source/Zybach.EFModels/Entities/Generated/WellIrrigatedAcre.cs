using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("WellIrrigatedAcre")]
    [Index(nameof(WellID), nameof(IrrigationYear), Name = "AK_WellIrrigatedAcre_WellID_IrrigationYear", IsUnique = true)]
    public partial class WellIrrigatedAcre
    {
        [Key]
        public int WellIrrigatedAcreID { get; set; }
        public int WellID { get; set; }
        public int IrrigationYear { get; set; }
        public double Acres { get; set; }

        [ForeignKey(nameof(WellID))]
        [InverseProperty("WellIrrigatedAcres")]
        public virtual Well Well { get; set; }
    }
}
