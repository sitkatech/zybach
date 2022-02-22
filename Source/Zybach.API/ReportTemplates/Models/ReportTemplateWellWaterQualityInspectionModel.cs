using System;
using System.Collections.Generic;
using System.Linq;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.ReportTemplates.Models
{
    public class ReportTemplateWellWaterQualityInspectionModel : ReportTemplateBaseModel
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

        public List<WaterQualityInspectionSimpleDto> WaterQualityInspections { get; set; }

        public string NitrateLevelFirstDate { get; set; }
        public string NitrateLevelLastDate { get; set; }
        public decimal NitrateLevelLast { get; set; }

        public decimal NitrateLevelHighest { get; set; }
        public string NitrateLevelHighestDate { get; set; }

        public decimal NitrateLevelLowest { get; set; }
        public string NitrateLevelLowestDate { get; set; }

        public decimal NitrateLevelAverage { get; set; }
        public string NitrateLevelsChartImagePath { get; set; }

        public ReportTemplateWellWaterQualityInspectionModel(WellWaterQualityInspectionDetailedDto wellWithWaterQualityInspections)
        {
            WellID = wellWithWaterQualityInspections.Well.WellID;
            WellRegistrationID = wellWithWaterQualityInspections.Well.WellRegistrationID;
            WellNickname = wellWithWaterQualityInspections.Well.WellNickname;
            WellParticipationName = wellWithWaterQualityInspections.Well.WellParticipationName;
            TownshipRangeSection = wellWithWaterQualityInspections.Well.TownshipRangeSection;
            WellDepth = wellWithWaterQualityInspections.Well.WellDepth;
            Clearinghouse = wellWithWaterQualityInspections.Well.Clearinghouse;
            SiteName = wellWithWaterQualityInspections.Well.SiteName;
            SiteNumber = wellWithWaterQualityInspections.Well.SiteNumber;
            ScreenInterval = wellWithWaterQualityInspections.Well.ScreenInterval;
            ScreenDepth = wellWithWaterQualityInspections.Well.ScreenDepth;
            OwnerName = wellWithWaterQualityInspections.Well.OwnerName;
            OwnerAddress = wellWithWaterQualityInspections.Well.OwnerAddress;
            OwnerCity = wellWithWaterQualityInspections.Well.OwnerCity;
            OwnerState = wellWithWaterQualityInspections.Well.OwnerState;
            OwnerZipCode = wellWithWaterQualityInspections.Well.OwnerZipCode;
            AdditionalContactName = wellWithWaterQualityInspections.Well.AdditionalContactName;
            AdditionalContactAddress = wellWithWaterQualityInspections.Well.AdditionalContactAddress;
            AdditionalContactCity = wellWithWaterQualityInspections.Well.AdditionalContactCity;
            AdditionalContactState = wellWithWaterQualityInspections.Well.AdditionalContactState;
            AdditionalContactZipCode = wellWithWaterQualityInspections.Well.AdditionalContactZipCode;

            WaterQualityInspections = wellWithWaterQualityInspections.WaterQualityInspections;

            if (WaterQualityInspections is { Count: > 0 })
            {
                var inspectionsWithNitrates = WaterQualityInspections.Where(x => x.LabNitrates != null);
                var firstNitrateInspection = inspectionsWithNitrates
                    .OrderBy(x => x.InspectionDate).First();

                var lastNitrateInspection = inspectionsWithNitrates
                    .OrderByDescending(x => x.InspectionDate).First();

                var highestNitrateInspection = inspectionsWithNitrates
                    .OrderByDescending(x => x.LabNitrates).First();

                var lowestNitrateInspection = inspectionsWithNitrates
                    .OrderBy(x => x.LabNitrates).First();

                NitrateLevelFirstDate = firstNitrateInspection.InspectionDate.ToShortDateString();
                NitrateLevelLastDate = lastNitrateInspection.InspectionDate.ToShortDateString();
                NitrateLevelLast = Math.Round(lastNitrateInspection.LabNitrates.Value, 2);

                NitrateLevelHighestDate = highestNitrateInspection.InspectionDate.ToShortDateString();
                NitrateLevelHighest = Math.Round(highestNitrateInspection.LabNitrates.Value, 2);

                NitrateLevelLowestDate = lowestNitrateInspection.InspectionDate.ToShortDateString();
                NitrateLevelLowest = Math.Round(lowestNitrateInspection.LabNitrates.Value, 2);

                NitrateLevelAverage = Math.Round(WaterQualityInspections.Where(x => x.LabNitrates != null).Average(x => x.LabNitrates.Value), 2);
            }
            else
            {
                WaterQualityInspections = new List<WaterQualityInspectionSimpleDto>();
            }

            NitrateLevelsChartImagePath = $"{wellWithWaterQualityInspections.Well.WellID}-nitrateLevelsChart.png";
        }

        /// <summary>
        /// Used in SharpDocx template
        /// </summary>
        /// <returns></returns>
        public List<ReportTemplateWaterQualityInspectionModel> GetNitrateInspections()
        {
            return WaterQualityInspections?.Select(x => new ReportTemplateWaterQualityInspectionModel(x))
                .OrderByDescending(x => x.InspectionDate).ToList();
        }
    }
}
