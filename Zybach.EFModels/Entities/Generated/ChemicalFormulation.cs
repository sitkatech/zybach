using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("ChemicalFormulation")]
    [Index("ChemicalFormulationDisplayName", Name = "AK_ChemicalFormulation_ChemicalFormulationDisplayName", IsUnique = true)]
    [Index("ChemicalFormulationName", Name = "AK_ChemicalFormulation_ChemicalFormulationName", IsUnique = true)]
    public partial class ChemicalFormulation
    {
        public ChemicalFormulation()
        {
            ChemigationPermitAnnualRecordChemicalFormulations = new HashSet<ChemigationPermitAnnualRecordChemicalFormulation>();
        }

        [Key]
        public int ChemicalFormulationID { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string ChemicalFormulationName { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string ChemicalFormulationDisplayName { get; set; }

        [InverseProperty("ChemicalFormulation")]
        public virtual ICollection<ChemigationPermitAnnualRecordChemicalFormulation> ChemigationPermitAnnualRecordChemicalFormulations { get; set; }
    }
}
