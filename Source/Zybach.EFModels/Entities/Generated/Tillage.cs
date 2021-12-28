using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

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
            ChemigationInspections = new HashSet<ChemigationInspections>();
        }

        [Key]
        public int TillageID { get; set; }
        [Required]
        [StringLength(50)]
        public string TillageName { get; set; }
        [Required]
        [StringLength(50)]
        public string TillageDisplayName { get; set; }

        [InverseProperty(nameof(Entities.ChemigationInspections.Tillage))]
        public virtual ICollection<ChemigationInspections> ChemigationInspections { get; set; }
    }
}
