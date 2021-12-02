using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemicalFormulationYearlyTotalDto
    {
        public int RecordYear{ get; set; }
        public string ChemicalFormulation { get; set; }
        [Range(0, 999999, ErrorMessage = "Maximum quantity allowed is 999,999")]
        public decimal TotalApplied { get; set; }
        public ChemicalUnitDto ChemicalUnit { get; set; }
        [Range(0, 999999, ErrorMessage = "Maximum quantity allowed is 999,999")]
        public decimal AcresTreated { get; set; }
    }
}
