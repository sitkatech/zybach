using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationInspectionStatus")]
    [Index("ChemigationInspectionStatusDisplayName", Name = "AK_ChemigationInspectionStatus_ChemigationInspectionStatusDisplayName", IsUnique = true)]
    [Index("ChemigationInspectionStatusName", Name = "AK_ChemigationInspectionStatus_ChemigationInspectionStatusName", IsUnique = true)]
    public partial class ChemigationInspectionStatus
    {
        public ChemigationInspectionStatus()
        {
            ChemigationInspections = new HashSet<ChemigationInspection>();
        }

        [Key]
        public int ChemigationInspectionStatusID { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string ChemigationInspectionStatusName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string ChemigationInspectionStatusDisplayName { get; set; }

        [InverseProperty("ChemigationInspectionStatus")]
        public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; }
    }
}
