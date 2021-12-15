using System.Collections.Generic;
using System.Linq;
using Zybach.API.ReportTemplates.Models;
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
        public List<ChemigationPermitAnnualRecordApplicator> Applicators { get; set; }

        public ReportTemplateChemigationPermitAnnualRecordModel(ChemigationPermitAnnualRecord chemigationPermitAnnualRecord, ZybachDbContext dbContext)
        {
            ChemigationPermitAnnualRecordID = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID;
            ChemigationPermitNumber = ChemigationPermitAnnualRecord.GetChemigationPermitAnnualRecordsImpl(dbContext)
                .FirstOrDefault(x => x.ChemigationPermitID == chemigationPermitAnnualRecord.ChemigationPermitID)
                .ChemigationPermit.ChemigationPermitNumberDisplay;
            TownshipRangeSection = dbContext.ChemigationPermits
                .FirstOrDefault(x => x.ChemigationPermitID == chemigationPermitAnnualRecord.ChemigationPermitID)
                .TownshipRangeSection;
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
            Applicators = dbContext.ChemigationPermitAnnualRecordApplicators
                .Where(x => x.ChemigationPermitAnnualRecordID ==
                            chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID)
                .ToList();
            County = ChemigationPermitAnnualRecord.GetChemigationPermitAnnualRecordsImpl(dbContext)
                .FirstOrDefault(x => x.ChemigationPermitID == chemigationPermitAnnualRecord.ChemigationPermitID)
                .ChemigationPermit.ChemigationCounty.ChemigationCountyDisplayName;
            WellName = ChemigationPermitAnnualRecord.GetChemigationPermitAnnualRecordsImpl(dbContext)
                .FirstOrDefault(x => x.ChemigationPermitID == chemigationPermitAnnualRecord.ChemigationPermitID)
                .ChemigationPermit.Well.WellRegistrationID;
        }

        public List<ReportTemplateApplicatorModel> GetApplicators()
        {
            return Applicators.Select(x => new ReportTemplateApplicatorModel(x)).OrderBy(x => x.ApplicatorName).ToList();
        }
    }
}
