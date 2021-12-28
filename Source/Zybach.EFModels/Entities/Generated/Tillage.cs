using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("Tillage")]
    [Index(nameof(TillageDisplayName), Name = "AK_Tillage_TillageDisplayName", IsUnique = true)]
    [Index(nameof(TillageName), Name = "AK_Tillage_TillageName", IsUnique = true)]
    public partial class Tillage
    {
        public Tillage()
        {
            ChemigationInspections = new HashSet<ChemigationInspection>();
        }

        [Key]
        public int TillageID { get; set; }
        [Required]
        [StringLength(50)]
        public string TillageName { get; set; }
        [Required]
        [StringLength(50)]
        public string TillageDisplayName { get; set; }

        [InverseProperty(nameof(ChemigationInspection.Tillage))]
        public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; }
    }
}
