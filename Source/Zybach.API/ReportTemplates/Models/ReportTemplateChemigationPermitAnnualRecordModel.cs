using System.Collections.Generic;
using System.Linq;
using Zybach.EFModels.Entities;

namespace Zybach.API.ReportTemplates.Models
{
    public class ReportTemplateChemigationPermitAnnualRecordModel : ReportTemplateBaseModel
    {
        public int ChemigationPermitAnnualRecordID { get; set; }
        public string ChemigationPermitNumber { get; set; }
        public string TownshipRangeSection { get; set; }
        public string County { get; set; }
        public int RecordYear { get; set; }
        public string PivotName { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantMailingAddress { get; set; }
        public string ApplicantCity { get; set; }
        public string ApplicantState { get; set; }
        public string ApplicantZipCode { get; set; }
        public string ApplicantPhone { get; set; }
        public string ApplicantMobilePhone { get; set; }
        public string ApplicantEmail { get; set; }
        public string WellName { get; set; }
        public string WellLatitude { get; set; }
        public string WellLongitude{ get; set; }

        public List<ChemigationPermitAnnualRecordApplicator> Applicators { get; set; }

        public ReportTemplateChemigationPermitAnnualRecordModel(ChemigationPermitAnnualRecord chemigationPermitAnnualRecord)
        {
            ChemigationPermitAnnualRecordID = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID;
            ChemigationPermitNumber = chemigationPermitAnnualRecord.ChemigationPermit.ChemigationPermitNumberDisplay;
            TownshipRangeSection = chemigationPermitAnnualRecord.TownshipRangeSection;
            RecordYear = chemigationPermitAnnualRecord.RecordYear;
            PivotName = chemigationPermitAnnualRecord.PivotName;
            ApplicantName = chemigationPermitAnnualRecord.ApplicantName;
            ApplicantMailingAddress = chemigationPermitAnnualRecord.ApplicantMailingAddress;
            ApplicantCity = chemigationPermitAnnualRecord.ApplicantCity;
            ApplicantState = chemigationPermitAnnualRecord.ApplicantState;
            ApplicantZipCode = chemigationPermitAnnualRecord.ApplicantZipCode;
            ApplicantPhone = chemigationPermitAnnualRecord.ApplicantPhone;
            ApplicantMobilePhone = chemigationPermitAnnualRecord.ApplicantMobilePhone;
            ApplicantEmail = chemigationPermitAnnualRecord.ApplicantEmail;
            Applicators = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordApplicators.ToList();
            County = chemigationPermitAnnualRecord.ChemigationPermit.County.CountyDisplayName;
            WellName = chemigationPermitAnnualRecord.ChemigationPermit.Well.WellRegistrationID;
            WellLatitude = chemigationPermitAnnualRecord.ChemigationPermit.Well.Latitude.ToString("F4");
            WellLongitude = chemigationPermitAnnualRecord.ChemigationPermit.Well.Longitude.ToString("F4");
        }
        
        public List<ReportTemplateApplicatorModel> GetApplicators()
        {
            return Applicators.Select(x => new ReportTemplateApplicatorModel(x)).OrderBy(x => x.ApplicatorName).ToList();
        }
    }
}
