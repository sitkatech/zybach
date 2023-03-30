using System;
using System.Collections.Generic;
using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.ReportTemplates.Models
{
    public class ReportTemplateWellGroupWaterLevelInspectionModel : ReportTemplateBaseModel
    {
        public int WellGroupID { get; set; }
        public string WellGroupName { get; set; }
        public string WellRegistrationIDs { get; set; }

        public string TownshipRangeSection { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerCity { get; set; }
        public string OwnerState { get; set; }
        public string OwnerZipCode { get; set; }
        public string AdditionalContactName { get; set; }
        public string AdditionalContactAddress { get; set; }
        public string AdditionalContactCity { get; set; }
        public string AdditionalContactState { get; set; }
        public string AdditionalContactZipCode { get; set; }

        public List<WaterLevelInspectionSimpleDto> WaterLevelInspections { get; set; }

        public string WaterLevelFirstDate { get; set; }
        public string WaterLevelLastDate { get; set; }
        public decimal? WaterLevelLast { get; set; }

        public decimal? WaterLevelHighestDepth { get; set; }
        public string WaterLevelHighestDepthDate { get; set; }

        public decimal? WaterLevelLowestDepth { get; set; }
        public string WaterLevelLowestDepthDate { get; set; }

        public decimal? WaterLevelAverage { get; set; }
        public string WaterLevelsChartImagePath { get; set; }


        public ReportTemplateWellGroupWaterLevelInspectionModel(WellGroupWaterLevelInspectionDto wellGroupWithWaterLevelInspections)
        {
            WellGroupID = wellGroupWithWaterLevelInspections.WellGroup.WellGroupID;
            WellGroupName = wellGroupWithWaterLevelInspections.WellGroup.WellGroupName;

            WellRegistrationIDs = string.Join(", ", wellGroupWithWaterLevelInspections.WellGroup.WellGroupWells.Select(x => x.WellRegistrationID));
            
            TownshipRangeSection = wellGroupWithWaterLevelInspections.WellGroup.PrimaryWell.TownshipRangeSection;
            OwnerAddress = wellGroupWithWaterLevelInspections.WellGroup.PrimaryWell.OwnerAddress;
            OwnerCity = wellGroupWithWaterLevelInspections.WellGroup.PrimaryWell.OwnerCity;
            OwnerState = wellGroupWithWaterLevelInspections.WellGroup.PrimaryWell.OwnerState;
            OwnerZipCode = wellGroupWithWaterLevelInspections.WellGroup.PrimaryWell.OwnerZipCode;
            AdditionalContactName = wellGroupWithWaterLevelInspections.WellGroup.PrimaryWell.AdditionalContactName;
            AdditionalContactAddress = wellGroupWithWaterLevelInspections.WellGroup.PrimaryWell.AdditionalContactAddress;
            AdditionalContactCity = wellGroupWithWaterLevelInspections.WellGroup.PrimaryWell.AdditionalContactCity;
            AdditionalContactState = wellGroupWithWaterLevelInspections.WellGroup.PrimaryWell.AdditionalContactState;
            AdditionalContactZipCode = wellGroupWithWaterLevelInspections.WellGroup.PrimaryWell.AdditionalContactZipCode;

            WaterLevelInspections = wellGroupWithWaterLevelInspections.WaterLevelInspections.ToList();

            var inspectionsWithDepthMeasurements = WaterLevelInspections
                .Where(x => x.Measurement.HasValue).ToList();
            if (inspectionsWithDepthMeasurements.Any())
            {
                var firstDepthMeasurement = inspectionsWithDepthMeasurements.MinBy(x => x.InspectionDate);
                var lastDepthMeasurement = inspectionsWithDepthMeasurements.MaxBy(x => x.InspectionDate);

                var lowestDepthMeasurement = inspectionsWithDepthMeasurements.MinBy(x => x.Measurement);
                var highestDepthMeasurement = inspectionsWithDepthMeasurements.MaxBy(x => x.Measurement);

                WaterLevelFirstDate = firstDepthMeasurement?.InspectionDate.ToShortDateString();
                WaterLevelLastDate = lastDepthMeasurement?.InspectionDate.ToShortDateString();
                WaterLevelLast = lastDepthMeasurement != null ? Math.Round(lastDepthMeasurement.Measurement.Value, 2) : null;
                
                WaterLevelHighestDepthDate = highestDepthMeasurement?.InspectionDate.ToShortDateString();
                WaterLevelHighestDepth = highestDepthMeasurement != null ? Math.Round(highestDepthMeasurement.Measurement.Value, 2) : null;
                
                WaterLevelLowestDepthDate = lowestDepthMeasurement?.InspectionDate.ToShortDateString();
                WaterLevelLowestDepth = lowestDepthMeasurement != null ? Math.Round(lowestDepthMeasurement.Measurement.Value, 2) : null;
                
                WaterLevelAverage = Math.Round(inspectionsWithDepthMeasurements.Average(x => x.Measurement.Value), 2);
            }

            WaterLevelsChartImagePath = $"{wellGroupWithWaterLevelInspections.WellGroup.WellGroupID}-waterLevelsChart.png";
        }

        /// <summary>
        /// Used in SharpDocx template
        /// </summary>
        /// <returns></returns>
        public List<ReportTemplateWaterLevelInspectionModel> GetWaterLevelInspections()
        {
            return WaterLevelInspections.Select(x => new ReportTemplateWaterLevelInspectionModel(x))
                .OrderByDescending(x => x.InspectionDate).ToList();
        }
    }
}
