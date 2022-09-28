using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("AgHubWellIrrigatedAcreStaging")]
    public partial class AgHubWellIrrigatedAcreStaging
    {
        [Key]
        public int AgHubWellIrrigatedAcreStagingID { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string WellRegistrationID { get; set; }
        public int IrrigationYear { get; set; }
        public double Acres { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string CropType { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Tillage { get; set; }
    }
}
