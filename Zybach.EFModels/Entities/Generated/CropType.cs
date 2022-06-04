using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("CropType")]
    [Index("CropTypeDisplayName", Name = "AK_CropType_CropTypeDisplayName", IsUnique = true)]
    [Index("CropTypeName", Name = "AK_CropType_CropTypeName", IsUnique = true)]
    public partial class CropType
    {
        public CropType()
        {
            ChemigationInspections = new HashSet<ChemigationInspection>();
            WaterLevelInspections = new HashSet<WaterLevelInspection>();
            WaterQualityInspections = new HashSet<WaterQualityInspection>();
        }

        [Key]
        public int CropTypeID { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string CropTypeName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string CropTypeDisplayName { get; set; }

        [InverseProperty("CropType")]
        public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; }
        [InverseProperty("CropType")]
        public virtual ICollection<WaterLevelInspection> WaterLevelInspections { get; set; }
        [InverseProperty("CropType")]
        public virtual ICollection<WaterQualityInspection> WaterQualityInspections { get; set; }
    }
}
