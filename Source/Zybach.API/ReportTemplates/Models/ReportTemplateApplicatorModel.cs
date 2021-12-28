using Zybach.EFModels.Entities;

namespace Zybach.API.ReportTemplates.Models
{
    public class ReportTemplateApplicatorModel : ReportTemplateBaseModel
    {
        public string ApplicatorName { get; set; }
        public int? CertificationNumber { get; set; }
        public int? ExpirationYear { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }

        public ReportTemplateApplicatorModel(ChemigationPermitAnnualRecordApplicator applicator)
        {
            ApplicatorName = applicator.ApplicatorName;
            CertificationNumber = applicator.CertificationNumber;
            ExpirationYear = applicator.ExpirationYear;
            HomePhone = applicator.HomePhone;
            MobilePhone = applicator.MobilePhone;
        }
    }
}
