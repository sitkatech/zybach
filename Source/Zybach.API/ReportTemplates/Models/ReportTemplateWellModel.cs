using Zybach.EFModels.Entities;

namespace Zybach.API.ReportTemplates.Models
{
    public class ReportTemplateWellModel : ReportTemplateBaseModel
    {
        public string WellRegistrationID { get; set; }

        public ReportTemplateWellModel(Well well)
        {
            WellRegistrationID = well.WellRegistrationID;
        }
    }
}