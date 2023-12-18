using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("ChemicalUnit")]
[Index("ChemicalUnitLowercaseShortName", Name = "AK_ChemicalUnit_ChemicalUnitLowercaseShortName", IsUnique = true)]
[Index("ChemicalUnitName", Name = "AK_ChemicalUnit_ChemicalUnitName", IsUnique = true)]
[Index("ChemicalUnitPluralName", Name = "AK_ChemicalUnit_ChemicalUnitPluralName", IsUnique = true)]
public partial class ChemicalUnit
{
    [Key]
    public int ChemicalUnitID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string ChemicalUnitName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string ChemicalUnitPluralName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string ChemicalUnitLowercaseShortName { get; set; }

    [InverseProperty("ChemicalUnit")]
    public virtual ICollection<ChemigationPermitAnnualRecordChemicalFormulation> ChemigationPermitAnnualRecordChemicalFormulations { get; set; } = new List<ChemigationPermitAnnualRecordChemicalFormulation>();
}
