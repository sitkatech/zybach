using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("AgHubWellIrrigatedAcre")]
    [Index(nameof(AgHubWellID), nameof(IrrigationYear), Name = "AK_AgHubWellIrrigatedAcre_AgHubWellID_IrrigationYear", IsUnique = true)]
    public partial class AgHubWellIrrigatedAcre
    {
        [Key]
        public int AgHubWellIrrigatedAcreID { get; set; }
        public int AgHubWellID { get; set; }
        public int IrrigationYear { get; set; }
        public double Acres { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FetchDate { get; set; }

        [ForeignKey(nameof(AgHubWellID))]
        [InverseProperty("AgHubWellIrrigatedAcres")]
        public virtual AgHubWell AgHubWell { get; set; }
    }
}
