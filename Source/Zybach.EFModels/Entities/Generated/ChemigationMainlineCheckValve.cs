using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationMainlineCheckValve")]
    [Index(nameof(ChemigationMainlineCheckValveDisplayName), Name = "AK_ChemigationMainlineCheckValve_ChemigationMainlineCheckValveDisplayName", IsUnique = true)]
    [Index(nameof(ChemigationMainlineCheckValveName), Name = "AK_ChemigationMainlineCheckValve_ChemigationMainlineCheckValveName", IsUnique = true)]
    public partial class ChemigationMainlineCheckValve
    {
        public ChemigationMainlineCheckValve()
        {
            ChemigationInspections = new HashSet<ChemigationInspection>();
        }

        [Key]
        public int ChemigationMainlineCheckValveID { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationMainlineCheckValveName { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationMainlineCheckValveDisplayName { get; set; }

        [InverseProperty(nameof(ChemigationInspection.ChemigationMainlineCheckValve))]
        public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; }
    }
}
