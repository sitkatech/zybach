using Zybach.Models.DataTransferObjects;

namespace Zybach.API.ReportTemplates.Models
{
    public class ReportTemplateWaterLevelInspectionModel : ReportTemplateBaseModel
    {
        public WaterLevelInspectionSimpleDto WaterLevelInspection { get; set; }

        public ReportTemplateWaterLevelInspectionModel(WaterLevelInspectionSimpleDto waterLevelInspection)
        {
            WaterLevelInspection = waterLevelInspection;
        }
    }
}
