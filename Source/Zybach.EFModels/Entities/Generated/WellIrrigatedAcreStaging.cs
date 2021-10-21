using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("WellIrrigatedAcreStaging")]
    public partial class WellIrrigatedAcreStaging
    {
        [Key]
        public int WellIrrigatedAcreStagingID { get; set; }
        [Required]
        [StringLength(100)]
        public string WellRegistrationID { get; set; }
        public int IrrigationYear { get; set; }
        public double Acres { get; set; }
    }
}
