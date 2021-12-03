using System;
using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationPermitAnnualRecordDetailedDto
    {
        public int ChemigationPermitAnnualRecordID { get; set; }
        public ChemigationPermitDto ChemigationPermit { get; set; }
        public int RecordYear { get; set; }
        public int ChemigationPermitAnnualRecordStatusID { get; set; }
        public string ChemigationPermitAnnualRecordStatusName { get; set; }
        public string PivotName { get; set; }
        public int ChemigationInjectionUnitTypeID { get; set; }
        public string ChemigationInjectionUnitTypeName { get; set; }
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

        public List<ChemigationPermitAnnualRecordChemicalFormulationSimpleDto> ChemicalFormulations { get; set; }
        public List<ChemigationPermitAnnualRecordApplicatorSimpleDto> Applicators { get; set; }
        public List<ChemigationPermitAnnualRecordWellSimpleDto> Wells { get; set; }
    }
}