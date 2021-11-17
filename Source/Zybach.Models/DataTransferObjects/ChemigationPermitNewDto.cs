using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitNewDto
    {
        [Required]
        public int ChemigationPermitNumber { get; set; }
        [Required]
        public int ChemigationPermitStatusID { get; set; }
        [Required]
        public string TownshipRangeSection { get; set; }
        [Required]
        public int ChemigationCountyID { get; set; }
        [Required]
        public decimal TotalAcresTreated { get; set; }
        [Required]
        public int ChemigationInjectionUnitTypeID { get; set; }
        [Required]
        public string ApplicantFirstName { get; set; }
        [Required]
        public string ApplicantLastName { get; set; }
        public string ApplicantPhone { get; set; }
        public string ApplicantMobilePhone { get; set; }
        [Required]
        public string ApplicantMailingAddress { get; set; }
        [Required]
        public string ApplicantCity { get; set; }
        [Required]
        public string ApplicantState { get; set; }
        [Required]
        public int ApplicantZipCode { get; set; }
        [Required] 
        public string PivotName { get; set; }
    }
}
