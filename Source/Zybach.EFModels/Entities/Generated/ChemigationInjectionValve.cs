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
    [Table("ChemigationInjectionValve")]
    [Index(nameof(ChemigationInjectionValveDisplayName), Name = "AK_ChemigationInjectionValve_ChemigationInjectionValveDisplayName", IsUnique = true)]
    [Index(nameof(ChemigationInjectionValveName), Name = "AK_ChemigationInjectionValve_ChemigationInjectionValveName", IsUnique = true)]
    public partial class ChemigationInjectionValve
    {
        public ChemigationInjectionValve()
        {
            ChemigationInspections = new HashSet<ChemigationInspections>();
        }

        [Key]
        public int ChemigationInjectionValveID { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationInjectionValveName { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationInjectionValveDisplayName { get; set; }

        [InverseProperty(nameof(Entities.ChemigationInspections.ChemigationInjectionValve))]
        public virtual ICollection<ChemigationInspections> ChemigationInspections { get; set; }

    }
}
