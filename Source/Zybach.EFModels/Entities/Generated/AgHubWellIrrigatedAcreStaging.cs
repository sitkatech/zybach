using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("AgHubWellIrrigatedAcreStaging")]
    public partial class AgHubWellIrrigatedAcreStaging
    {
        [Key]
        public int AgHubWellIrrigatedAcreStagingID { get; set; }
        [Required]
        [StringLength(100)]
        public string WellRegistrationID { get; set; }
        public int IrrigationYear { get; set; }
        public double Acres { get; set; }
    }
}
