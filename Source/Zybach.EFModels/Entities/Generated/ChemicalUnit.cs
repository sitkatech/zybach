using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemicalUnit")]
    [Index(nameof(ChemicalUnitLowercaseShortName), Name = "AK_ChemicalUnit_ChemicalUnitLowercaseShortName", IsUnique = true)]
    [Index(nameof(ChemicalUnitName), Name = "AK_ChemicalUnit_ChemicalUnitName", IsUnique = true)]
    [Index(nameof(ChemicalUnitPluralName), Name = "AK_ChemicalUnit_ChemicalUnitPluralName", IsUnique = true)]
    public partial class ChemicalUnit
    {
        public ChemicalUnit()
        {
            ChemigationPermitAnnualRecordChemicalFormulations = new HashSet<ChemigationPermitAnnualRecordChemicalFormulation>();
        }

        [Key]
        public int ChemicalUnitID { get; set; }
        [StringLength(100)]
        public string ChemicalUnitName { get; set; }
        [StringLength(100)]
        public string ChemicalUnitPluralName { get; set; }
        [StringLength(100)]
        public string ChemicalUnitLowercaseShortName { get; set; }

        [InverseProperty(nameof(ChemigationPermitAnnualRecordChemicalFormulation.ChemicalUnit))]
        public virtual ICollection<ChemigationPermitAnnualRecordChemicalFormulation> ChemigationPermitAnnualRecordChemicalFormulations { get; set; }
    }
}
