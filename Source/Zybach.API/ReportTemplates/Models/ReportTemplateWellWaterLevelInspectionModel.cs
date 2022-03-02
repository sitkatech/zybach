using System;
using System.Collections.Generic;
using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.ReportTemplates.Models
{
    public class ReportTemplateWellWaterLevelInspectionModel : ReportTemplateBaseModel

    {
        public int WellID { get; set; }
        public string WellRegistrationID { get; set; }
        public string WellNickname { get; set; }
        public string WellParticipationName { get; set; }
        public string TownshipRangeSection { get; set; }
        public decimal? WellDepth { get; set; }
        public string Clearinghouse { get; set; }
        public string SiteName { get; set; }
        public string SiteNumber { get; set; }
        public string ScreenInterval { get; set; }
        public decimal? ScreenDepth { get; set; }
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
        public decimal WaterLevelLast { get; set; }

        public decimal WaterLevelHighestDepth { get; set; }
        public string WaterLevelHighestDepthDate { get; set; }

        public decimal WaterLevelLowestDepth { get; set; }
        public string WaterLevelLowestDepthDate { get; set; }

        public decimal WaterLevelAverage { get; set; }
        public string WaterLevelsChartImagePath { get; set; }


        public ReportTemplateWellWaterLevelInspectionModel(
            WellWaterLevelInspectionDetailedDto wellWithWaterLevelInspections)
        {
            WellID = wellWithWaterLevelInspections.Well.WellID;
            WellRegistrationID = wellWithWaterLevelInspections.Well.WellRegistrationID;
            WellNickname = wellWithWaterLevelInspections.Well.WellNickname;
            WellParticipationName = wellWithWaterLevelInspections.Well.WellParticipationName;
            TownshipRangeSection = wellWithWaterLevelInspections.Well.TownshipRangeSection;
            WellDepth = wellWithWaterLevelInspections.Well.WellDepth;
            Clearinghouse = wellWithWaterLevelInspections.Well.Clearinghouse;
            SiteName = wellWithWaterLevelInspections.Well.SiteName;
            SiteNumber = wellWithWaterLevelInspections.Well.SiteNumber;
            ScreenInterval = wellWithWaterLevelInspections.Well.ScreenInterval;
            ScreenDepth = wellWithWaterLevelInspections.Well.ScreenDepth;
            OwnerName = wellWithWaterLevelInspections.Well.OwnerName;
            OwnerAddress = wellWithWaterLevelInspections.Well.OwnerAddress;
            OwnerCity = wellWithWaterLevelInspections.Well.OwnerCity;
            OwnerState = wellWithWaterLevelInspections.Well.OwnerState;
            OwnerZipCode = wellWithWaterLevelInspections.Well.OwnerZipCode;
            AdditionalContactName = wellWithWaterLevelInspections.Well.AdditionalContactName;
            AdditionalContactAddress = wellWithWaterLevelInspections.Well.AdditionalContactAddress;
            AdditionalContactCity = wellWithWaterLevelInspections.Well.AdditionalContactCity;
            AdditionalContactState = wellWithWaterLevelInspections.Well.AdditionalContactState;
            AdditionalContactZipCode = wellWithWaterLevelInspections.Well.AdditionalContactZipCode;

            WaterLevelInspections = wellWithWaterLevelInspections.WaterLevelInspections;

            if (WaterLevelInspections is { Count: > 0 })
            {
                var inspectionsWithDepthMeasurements = WaterLevelInspections.Where(x => x.Measurement != null);

                var firstDepthMeasurement = inspectionsWithDepthMeasurements
                    .OrderBy(x => x.InspectionDate).First();

                var lastDepthMeasurement = inspectionsWithDepthMeasurements
                    .OrderByDescending(x => x.InspectionDate).First();

                var highestDepthMeasurement = inspectionsWithDepthMeasurements
                    .OrderByDescending(x => x.Measurement).First();

                var lowestDepthMeasurement = inspectionsWithDepthMeasurements
                    .OrderBy(x => x.Measurement).First();

                WaterLevelFirstDate = firstDepthMeasurement.InspectionDate.ToShortDateString();
                WaterLevelLastDate = lastDepthMeasurement.InspectionDate.ToShortDateString();
                WaterLevelLast = Math.Round(lastDepthMeasurement.Measurement.Value, 2);
                
                WaterLevelHighestDepthDate = highestDepthMeasurement.InspectionDate.ToShortDateString();
                WaterLevelHighestDepth = Math.Round(highestDepthMeasurement.Measurement.Value, 2);
                
                WaterLevelLowestDepthDate = lowestDepthMeasurement.InspectionDate.ToShortDateString();
                WaterLevelLowestDepth = Math.Round(lowestDepthMeasurement.Measurement.Value, 2);
                
                WaterLevelAverage = Math.Round(WaterLevelInspections.Where(x => x.Measurement != null).Average(x => x.Measurement.Value), 2);
            }
            else
            {
                WaterLevelInspections = new List<WaterLevelInspectionSimpleDto>();
            }

            WaterLevelsChartImagePath = $"{wellWithWaterLevelInspections.Well.WellID}-waterLevelsChart.png";
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
