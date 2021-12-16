using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public class GenerateReportsByModelDto
    {
        public int ReportTemplateModelID { get; set; }
        public List<int> ModelIDList { get; set; }
    }

}