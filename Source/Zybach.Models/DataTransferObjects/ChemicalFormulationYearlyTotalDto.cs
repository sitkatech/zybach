namespace Zybach.Models.DataTransferObjects
{
    public class ChemicalFormulationYearlyTotalDto
    {
        public int RecordYear{ get; set; }
        public string ChemicalFormulation { get; set; }
        public decimal TotalApplied { get; set; }
        public ChemicalUnitDto ChemicalUnit { get; set; }
        public decimal AcresTreated { get; set; }
    }
}
