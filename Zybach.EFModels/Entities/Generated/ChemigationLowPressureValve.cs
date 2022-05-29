using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationLowPressureValve")]
    [Index("ChemigationLowPressureValveDisplayName", Name = "AK_ChemigationLowPressureValve_ChemigationLowPressureValveDisplayName", IsUnique = true)]
    [Index("ChemigationLowPressureValveName", Name = "AK_ChemigationLowPressureValve_ChemigationLowPressureValveName", IsUnique = true)]
    public partial class ChemigationLowPressureValve
    {
        public ChemigationLowPressureValve()
        {
            ChemigationInspections = new HashSet<ChemigationInspection>();
        }

        [Key]
        public int ChemigationLowPressureValveID { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string ChemigationLowPressureValveName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string ChemigationLowPressureValveDisplayName { get; set; }

        [InverseProperty("ChemigationLowPressureValve")]
        public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; }
    }
}
