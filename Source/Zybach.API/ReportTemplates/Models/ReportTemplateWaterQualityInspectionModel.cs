using Zybach.Models.DataTransferObjects;

namespace Zybach.API.ReportTemplates.Models
{
    public class ReportTemplateWaterQualityInspectionModel
    {
        public WaterQualityInspectionSimpleDto WaterQualityInspection { get; set; }

        public ReportTemplateWaterQualityInspectionModel(WaterQualityInspectionSimpleDto waterQualityInspection)
        {
            WaterQualityInspection = waterQualityInspection;
        }
    }
}