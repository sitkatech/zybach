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
            ChemigationInspections = new HashSet<ChemigationInspection>();
        }

        [Key]
        public int ChemigationLowPressureValveID { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationLowPressureValveName { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationLowPressureValveDisplayName { get; set; }

        [InverseProperty(nameof(ChemigationInspection.ChemigationLowPressureValve))]
        public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; }

        public static IEnumerable<ChemigationLowPressureValveDto> List(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationLowPressureValves.AsNoTracking().Select(x => x.AsDto()).ToList();
        }
    }
}
