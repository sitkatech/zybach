using System;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitCurrentDetailsDto
    {
        public int ChemigationPermitID { get; set; }
        public int ChemigationPermitNumber { get; set; }
        public ChemigationPermitStatusDto ChemigationPermitStatus { get; set; }
        public DateTime DateCreated { get; set; }
        public string TownshipRangeSection { get; set; }
        public string ApplicantFirstName { get; set; }
        public string ApplicantLastName { get; set; }
        public string PivotName { get; set; }
    }
}
