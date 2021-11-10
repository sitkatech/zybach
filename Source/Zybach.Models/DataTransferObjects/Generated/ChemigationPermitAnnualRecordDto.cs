//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecord]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class ChemigationPermitAnnualRecordDto
    {
        public int ChemigationPermitAnnualRecordID { get; set; }
        public ChemigationPermitDto ChemigationPermit { get; set; }
        public ChemigationPermitAnnualRecordStatusDto ChemigationPermitAnnualRecordStatus { get; set; }
        public string ApplicantFirstName { get; set; }
        public string ApplicantLastName { get; set; }
        public string PivotName { get; set; }
        public int RecordYear { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DatePaid { get; set; }
    }
}