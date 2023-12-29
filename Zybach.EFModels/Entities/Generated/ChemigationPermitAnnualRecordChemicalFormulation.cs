using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("ChemigationPermitAnnualRecordChemicalFormulation")]
[Index("ChemigationPermitAnnualRecordID", "ChemicalFormulationID", "ChemicalUnitID", Name = "AK_ChemigationPermitAnnualRecordChemicalFormulation_ChemigationPermitAnnualRecordID_ChemicalFormulationID_ChemicalUnitID", IsUnique = true)]
public partial class ChemigationPermitAnnualRecordChemicalFormulation
{
    [Key]
    public int ChemigationPermitAnnualRecordChemicalFormulationID { get; set; }

    public int ChemigationPermitAnnualRecordID { get; set; }

    public int ChemicalFormulationID { get; set; }

    public int ChemicalUnitID { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? TotalApplied { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal AcresTreated { get; set; }

    [ForeignKey("ChemicalFormulationID")]
    [InverseProperty("ChemigationPermitAnnualRecordChemicalFormulations")]
    public virtual ChemicalFormulation ChemicalFormulation { get; set; }

    [ForeignKey("ChemicalUnitID")]
    [InverseProperty("ChemigationPermitAnnualRecordChemicalFormulations")]
    public virtual ChemicalUnit ChemicalUnit { get; set; }

    [ForeignKey("ChemigationPermitAnnualRecordID")]
    [InverseProperty("ChemigationPermitAnnualRecordChemicalFormulations")]
    public virtual ChemigationPermitAnnualRecord ChemigationPermitAnnualRecord { get; set; }
}
