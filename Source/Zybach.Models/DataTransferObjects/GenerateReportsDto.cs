using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public class GenerateReportsDto
    {
        public int ReportTemplateID { get; set; }
        public List<int> WellIDList { get; set; }
    }
}