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
        public int RecordYear { get; set; }
        public ChemigationPermitAnnualRecordStatusDto ChemigationPermitAnnualRecordStatus { get; set; }
        public string PivotName { get; set; }
        public ChemigationInjectionUnitTypeDto ChemigationInjectionUnitType { get; set; }
        public string ApplicantFirstName { get; set; }
        public string ApplicantLastName { get; set; }
        public string ApplicantMailingAddress { get; set; }
        public string ApplicantCity { get; set; }
        public string ApplicantState { get; set; }
        public string ApplicantZipCode { get; set; }
        public string ApplicantPhone { get; set; }
        public string ApplicantMobilePhone { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DatePaid { get; set; }
        public string ApplicantEmail { get; set; }
        public decimal? NDEEAmount { get; set; }
        public string TownshipRangeSection { get; set; }
        public string ApplicantCompany { get; set; }
        public string AnnualNotes { get; set; }
    }

    public partial class ChemigationPermitAnnualRecordSimpleDto
    {
        public int ChemigationPermitAnnualRecordID { get; set; }
        public int ChemigationPermitID { get; set; }
        public int RecordYear { get; set; }
        public int ChemigationPermitAnnualRecordStatusID { get; set; }
        public string PivotName { get; set; }
        public int ChemigationInjectionUnitTypeID { get; set; }
        public string ApplicantFirstName { get; set; }
        public string ApplicantLastName { get; set; }
        public string ApplicantMailingAddress { get; set; }
        public string ApplicantCity { get; set; }
        public string ApplicantState { get; set; }
        public string ApplicantZipCode { get; set; }
        public string ApplicantPhone { get; set; }
        public string ApplicantMobilePhone { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DatePaid { get; set; }
        public string ApplicantEmail { get; set; }
        public decimal? NDEEAmount { get; set; }
        public string TownshipRangeSection { get; set; }
        public string ApplicantCompany { get; set; }
        public string AnnualNotes { get; set; }
    }

}