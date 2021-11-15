//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermit]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class ChemigationPermitDto
    {
        public int ChemigationPermitID { get; set; }
        public int ChemigationPermitNumber { get; set; }
        public ChemigationPermitStatusDto ChemigationPermitStatus { get; set; }
        public DateTime DateCreated { get; set; }
        public string TownshipRangeSection { get; set; }
        public ChemigationCountyDto ChemigationCounty { get; set; }
    }
}