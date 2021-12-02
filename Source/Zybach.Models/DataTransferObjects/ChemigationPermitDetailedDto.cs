using System;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitDetailedDto
    {
        public int ChemigationPermitID { get; set; }
        public int ChemigationPermitNumber { get; set; }
        public ChemigationPermitStatusDto ChemigationPermitStatus { get; set; }
        public DateTime DateCreated { get; set; }
        public string TownshipRangeSection { get; set; }
        public ChemigationCountyDto ChemigationCounty { get; set; }

        public ChemigationPermitAnnualRecordDetailedDto LatestAnnualRecord { get; set; }
    }
}