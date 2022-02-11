using System;
using System.Collections.Generic;
using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.ReportTemplates.Models
{
    public class ReportTemplateWellInspectionModel : ReportTemplateBaseModel
    {

        public WellSimpleDto Well { get; set; }

        public List<WaterLevelInspectionSimpleDto> WaterLevelInspections { get; set; }
        public List<WaterQualityInspectionSimpleDto> WaterQualityInspections { get; set; }

        public ReportTemplateWellInspectionModel(WellInspectionDetailedDto wellWithInspections)
        {
            Well = wellWithInspections.Well;
            WaterLevelInspections = wellWithInspections.WaterLevelInspections;
            WaterQualityInspections = wellWithInspections.WaterQualityInspections;

        }

        /// <summary>
        /// Used in SharpDocx template
        /// </summary>
        /// <returns></returns>
        public List<ReportTemplateWaterLevelInspectionModel> GetWaterLevelInspections()
        {
            return WaterLevelInspections.Select(x => new ReportTemplateWaterLevelInspectionModel(x)).OrderByDescending(x => x.WaterLevelInspection.InspectionDate).ToList();
        }

        /// <summary>
        /// Used in SharpDocx template
        /// </summary>
        /// <returns></returns>
        public List<ReportTemplateWaterQualityInspectionModel> GetWaterQualityInspections()
        {
            return WaterQualityInspections.Select(x => new ReportTemplateWaterQualityInspectionModel(x)).OrderByDescending(x => x.WaterQualityInspection.InspectionDate).ToList();
        }
    }

}




