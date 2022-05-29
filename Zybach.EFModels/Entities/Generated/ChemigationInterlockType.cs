using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationInterlockType")]
    [Index("ChemigationInterlockTypeDisplayName", Name = "AK_ChemigationInterlockType_ChemigationInterlockTypeDisplayName", IsUnique = true)]
    [Index("ChemigationInterlockTypeName", Name = "AK_ChemigationInterlockType_ChemigationInterlockTypeName", IsUnique = true)]
    public partial class ChemigationInterlockType
    {
        public ChemigationInterlockType()
        {
            ChemigationInspections = new HashSet<ChemigationInspection>();
        }

        [Key]
        public int ChemigationInterlockTypeID { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string ChemigationInterlockTypeName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string ChemigationInterlockTypeDisplayName { get; set; }

        [InverseProperty("ChemigationInterlockType")]
        public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; }
    }
}
