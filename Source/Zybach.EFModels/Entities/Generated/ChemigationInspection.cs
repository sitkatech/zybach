using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationInspection")]
    public partial class ChemigationInspection
    {
        [Key]
        public int ChemigationInspectionID { get; set; }
        [Required]
        [StringLength(100)]
        public string WellRegistrationID { get; set; }
        [Required]
        [StringLength(100)]
        public string ProtocolCanonicalName { get; set; }
        [Required]
        [StringLength(100)]
        public string Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastUpdate { get; set; }
    }
}
