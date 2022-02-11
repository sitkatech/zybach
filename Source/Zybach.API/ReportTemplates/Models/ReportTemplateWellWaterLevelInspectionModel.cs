using System.Collections.Generic;
using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.ReportTemplates.Models
{
    public class ReportTemplateWellWaterLevelInspectionModel : ReportTemplateBaseModel

    {
    public WellSimpleDto Well { get; set; }

    public List<WaterLevelInspectionSimpleDto> WaterLevelInspections { get; set; }

    public ReportTemplateWellWaterLevelInspectionModel(
        WellWaterLevelInspectionDetailedDto wellWithWaterLevelInspections)
    {
        Well = wellWithWaterLevelInspections.Well;
        WaterLevelInspections = wellWithWaterLevelInspections.WaterLevelInspections;
    }

    /// <summary>
    /// Used in SharpDocx template
    /// </summary>
    /// <returns></returns>
    public List<ReportTemplateWaterLevelInspectionModel> GetWaterLevelInspections()
    {
        return WaterLevelInspections.Select(x => new ReportTemplateWaterLevelInspectionModel(x))
            .OrderByDescending(x => x.WaterLevelInspection.InspectionDate).ToList();
    }
    }
}
