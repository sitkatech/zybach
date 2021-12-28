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
    [Table("ChemigationInspectionType")]
    [Index(nameof(ChemigationInspectionTypeDisplayName), Name = "AK_ChemigationInspectionType_ChemigationInspectionTypeDisplayName", IsUnique = true)]
    [Index(nameof(ChemigationInspectionTypeName), Name = "AK_ChemigationInspectionType_ChemigationInspectionTypeName", IsUnique = true)]
    public partial class ChemigationInspectionType
    {
        public ChemigationInspectionType()
        {
            ChemigationInspections = new HashSet<ChemigationInspections>();
        }

        [Key]
        public int ChemigationInspectionTypeID { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationInspectionTypeName { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationInspectionTypeDisplayName { get; set; }

        [InverseProperty(nameof(Entities.ChemigationInspections.ChemigationInspectionType))]
        public virtual ICollection<ChemigationInspections> ChemigationInspections { get; set; }
    }
}
