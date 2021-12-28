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
    [Table("ChemigationLowPressureValve")]
    [Index(nameof(ChemigationLowPressureValveDisplayName), Name = "AK_ChemigationLowPressureValve_ChemigationLowPressureValveDisplayName", IsUnique = true)]
    [Index(nameof(ChemigationLowPressureValveName), Name = "AK_ChemigationLowPressureValve_ChemigationLowPressureValveName", IsUnique = true)]
    public partial class ChemigationLowPressureValve
    {
        public ChemigationLowPressureValve()
        {
            ChemigationInspections = new HashSet<ChemigationInspections>();
        }

        [Key]
        public int ChemigationLowPressureValveID { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationLowPressureValveName { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationLowPressureValveDisplayName { get; set; }

        [InverseProperty(nameof(Entities.ChemigationInspections.ChemigationLowPressureValve))]
        public virtual ICollection<ChemigationInspections> ChemigationInspections { get; set; }
    }
}
