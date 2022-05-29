using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationInspectionType")]
    [Index("ChemigationInspectionTypeDisplayName", Name = "AK_ChemigationInspectionType_ChemigationInspectionTypeDisplayName", IsUnique = true)]
    [Index("ChemigationInspectionTypeName", Name = "AK_ChemigationInspectionType_ChemigationInspectionTypeName", IsUnique = true)]
    public partial class ChemigationInspectionType
    {
        public ChemigationInspectionType()
        {
            ChemigationInspections = new HashSet<ChemigationInspection>();
        }

        [Key]
        public int ChemigationInspectionTypeID { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string ChemigationInspectionTypeName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string ChemigationInspectionTypeDisplayName { get; set; }

        [InverseProperty("ChemigationInspectionType")]
        public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; }
    }
}
