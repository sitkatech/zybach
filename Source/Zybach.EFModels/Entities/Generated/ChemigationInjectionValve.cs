using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationInjectionValve")]
    [Index(nameof(ChemigationInjectionValveDisplayName), Name = "AK_ChemigationInjectionValve_ChemigationInjectionValveDisplayName", IsUnique = true)]
    [Index(nameof(ChemigationInjectionValveName), Name = "AK_ChemigationInjectionValve_ChemigationInjectionValveName", IsUnique = true)]
    public partial class ChemigationInjectionValve
    {
        public ChemigationInjectionValve()
        {
            ChemigationInspections = new HashSet<ChemigationInspection>();
        }

        [Key]
        public int ChemigationInjectionValveID { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationInjectionValveName { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationInjectionValveDisplayName { get; set; }

        [InverseProperty(nameof(ChemigationInspection.ChemigationInjectionValve))]
        public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; }
    }
}
