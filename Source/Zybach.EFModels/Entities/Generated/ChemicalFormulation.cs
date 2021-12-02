using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemicalFormulation")]
    [Index(nameof(ChemicalFormulationDisplayName), Name = "AK_ChemicalFormulation_ChemicalFormulationDisplayName", IsUnique = true)]
    [Index(nameof(ChemicalFormulationName), Name = "AK_ChemicalFormulation_ChemicalFormulationName", IsUnique = true)]
    public partial class ChemicalFormulation
    {
        public ChemicalFormulation()
        {
            ChemigationPermitAnnualRecordChemicalFormulations = new HashSet<ChemigationPermitAnnualRecordChemicalFormulation>();
        }

        [Key]
        public int ChemicalFormulationID { get; set; }
        [StringLength(100)]
        public string ChemicalFormulationName { get; set; }
        [StringLength(100)]
        public string ChemicalFormulationDisplayName { get; set; }

        [InverseProperty(nameof(ChemigationPermitAnnualRecordChemicalFormulation.ChemicalFormulation))]
        public virtual ICollection<ChemigationPermitAnnualRecordChemicalFormulation> ChemigationPermitAnnualRecordChemicalFormulations { get; set; }
    }
}
