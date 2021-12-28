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
    [Table("CropType")]
    [Index(nameof(CropTypeDisplayName), Name = "AK_CropType_CropTypeDisplayName", IsUnique = true)]
    [Index(nameof(CropTypeName), Name = "AK_CropType_CropTypeName", IsUnique = true)]
    public partial class CropType
    {
        public CropType()
        {
            ChemigationInspections = new HashSet<ChemigationInspections>();
        }

        [Key]
        public int CropTypeID { get; set; }
        [Required]
        [StringLength(50)]
        public string CropTypeName { get; set; }
        [Required]
        [StringLength(50)]
        public string CropTypeDisplayName { get; set; }

        [InverseProperty(nameof(Entities.ChemigationInspections.CropType))]
        public virtual ICollection<ChemigationInspections> ChemigationInspections { get; set; }
    }
}
