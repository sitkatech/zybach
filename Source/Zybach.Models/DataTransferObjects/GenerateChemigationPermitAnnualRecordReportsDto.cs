using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public class GenerateChemigationPermitAnnualRecordReportsDto
    {
        public int ReportTemplateID { get; set; }
        public List<int> ChemigationPermitAnnualRecordIDList { get; set; }
    }
}