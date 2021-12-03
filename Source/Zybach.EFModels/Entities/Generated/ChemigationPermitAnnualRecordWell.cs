using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationPermitAnnualRecordWell")]
    [Index(nameof(ChemigationPermitAnnualRecordID), nameof(WellID), Name = "AK_ChemigationPermitAnnualRecordWell_ChemigationPermitAnnualRecordID_WellID", IsUnique = true)]
    public partial class ChemigationPermitAnnualRecordWell
    {
        [Key]
        public int ChemigationPermitAnnualRecordWellID { get; set; }
        public int ChemigationPermitAnnualRecordID { get; set; }
        public int WellID { get; set; }

        [ForeignKey(nameof(ChemigationPermitAnnualRecordID))]
        [InverseProperty("ChemigationPermitAnnualRecordWells")]
        public virtual ChemigationPermitAnnualRecord ChemigationPermitAnnualRecord { get; set; }
        [ForeignKey(nameof(WellID))]
        [InverseProperty("ChemigationPermitAnnualRecordWells")]
        public virtual Well Well { get; set; }
    }
}
