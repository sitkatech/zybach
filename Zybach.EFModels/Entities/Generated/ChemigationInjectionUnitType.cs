using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationInjectionUnitType")]
    [Index(nameof(ChemigationInjectionUnitTypeDisplayName), Name = "AK_ChemigationInjectionUnitType_ChemigationInjectionUnitTypeDisplayName", IsUnique = true)]
    [Index(nameof(ChemigationInjectionUnitTypeName), Name = "AK_ChemigationInjectionUnitType_ChemigationInjectionUnitTypeName", IsUnique = true)]
    public partial class ChemigationInjectionUnitType
    {
        public ChemigationInjectionUnitType()
        {
            ChemigationPermitAnnualRecords = new HashSet<ChemigationPermitAnnualRecord>();
        }

        [Key]
        public int ChemigationInjectionUnitTypeID { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationInjectionUnitTypeName { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationInjectionUnitTypeDisplayName { get; set; }

        [InverseProperty(nameof(ChemigationPermitAnnualRecord.ChemigationInjectionUnitType))]
        public virtual ICollection<ChemigationPermitAnnualRecord> ChemigationPermitAnnualRecords { get; set; }
    }
}
