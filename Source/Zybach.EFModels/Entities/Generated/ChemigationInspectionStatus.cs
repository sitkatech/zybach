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
    [Table("ChemigationInspectionStatus")]
    [Index(nameof(ChemigationInspectionStatusDisplayName), Name = "AK_ChemigationInspectionStatus_ChemigationInspectionStatusDisplayName", IsUnique = true)]
    [Index(nameof(ChemigationInspectionStatusName), Name = "AK_ChemigationInspectionStatus_ChemigationInspectionStatusName", IsUnique = true)]
    public partial class ChemigationInspectionStatus
    {
        public ChemigationInspectionStatus()
        {
            ChemigationInspections = new HashSet<ChemigationInspections>();
        }

        [Key]
        public int ChemigationInspectionStatusID { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationInspectionStatusName { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationInspectionStatusDisplayName { get; set; }

        [InverseProperty(nameof(Entities.ChemigationInspections.ChemigationInspectionStatus))]
        public virtual ICollection<ChemigationInspections> ChemigationInspections { get; set; }

    }
}
