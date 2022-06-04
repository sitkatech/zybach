//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecordApplicator]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class ChemigationPermitAnnualRecordApplicatorDto
    {
        public int ChemigationPermitAnnualRecordApplicatorID { get; set; }
        public ChemigationPermitAnnualRecordDto ChemigationPermitAnnualRecord { get; set; }
        public string ApplicatorName { get; set; }
        public int? CertificationNumber { get; set; }
        public int? ExpirationYear { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
    }

    public partial class ChemigationPermitAnnualRecordApplicatorSimpleDto
    {
        public int ChemigationPermitAnnualRecordApplicatorID { get; set; }
        public int ChemigationPermitAnnualRecordID { get; set; }
        public string ApplicatorName { get; set; }
        public int? CertificationNumber { get; set; }
        public int? ExpirationYear { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
    }

}