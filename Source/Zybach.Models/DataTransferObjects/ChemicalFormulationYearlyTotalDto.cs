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
        public decimal TotalApplied { get; set; }
        public ChemicalUnitDto ChemicalUnit { get; set; }
        public decimal AcresTreated { get; set; }
    }
}
