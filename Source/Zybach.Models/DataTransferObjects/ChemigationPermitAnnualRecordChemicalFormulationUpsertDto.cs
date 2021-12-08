using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitAnnualRecordChemicalFormulationUpsertDto
    {
        public int ChemigationPermitAnnualRecordChemicalFormulationID { get; set; }
        public int ChemigationPermitAnnualRecordID { get; set; }
        public int ChemicalFormulationID { get; set; }
        public int ChemicalUnitID { get; set; }
        [Range(0, 999999, ErrorMessage = "Maximum quantity allowed is 999,999")]
        public decimal? TotalApplied { get; set; }
        [Range(0, 999999, ErrorMessage = "Maximum quantity allowed is 999,999")]
        public decimal AcresTreated { get; set; }
    }
}