using System.Collections.Generic;

namespace Zybach.API.ReportTemplates.Models
{
    public class ReportTemplateWellBaseViewModel 
    {
        public string ReportTitle { get; set; }
        public List<ReportTemplateWellModel> ReportModel { get; set; }
    }

    // Other base view models would go here if we created a new model type
}