using System;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class WaterQualityInspectionExtensionMethods
    {
        static partial void DoCustomSimpleDtoMappings(WaterQualityInspection waterQualityInspection,
            WaterQualityInspectionSimpleDto waterQualityInspectionSimpleDto)
        {
            waterQualityInspectionSimpleDto.Well = waterQualityInspection.Well.AsSimpleDto();
            waterQualityInspectionSimpleDto.WaterQualityInspectionTypeName = waterQualityInspection.WaterQualityInspectionType.WaterQualityInspectionTypeDisplayName;
            waterQualityInspectionSimpleDto.CropTypeName = waterQualityInspection.CropType?.CropTypeDisplayName;
            waterQualityInspectionSimpleDto.Inspector = waterQualityInspection.InspectorUser?.AsSimpleDto();
            waterQualityInspectionSimpleDto.InspectionYear = waterQualityInspection.InspectionDate.Year;
        }

        public static WaterQualityInspectionSummaryDto AsSummaryDto(this WaterQualityInspection waterQualityInspection)
        {
            var waterQualityInspectionSummaryDto = new WaterQualityInspectionSummaryDto()
            {
                WaterQualityInspectionID = waterQualityInspection.WaterQualityInspectionID,
                InspectionDate = waterQualityInspection.InspectionDate,
                InspectionType = waterQualityInspection.WaterQualityInspectionType.WaterQualityInspectionTypeDisplayName,
                InspectionNotes = waterQualityInspection.InspectionNotes
            };

            return waterQualityInspectionSummaryDto;
        }

        public static WaterQualityInspectionForVegaChartDto AsVegaChartDto(this WaterQualityInspection waterQualityInspection, decimal mostRecentLabNitrates)
        {
            var waterQualityInspectionForVegaChartDto = new WaterQualityInspectionForVegaChartDto()
            {
                WellID = waterQualityInspection.WellID,
                InspectionDate = waterQualityInspection.InspectionDate,
                LabNitrates = waterQualityInspection.LabNitrates,
                MostRecentDateLabNitrates = mostRecentLabNitrates
            };

            return waterQualityInspectionForVegaChartDto;
        }

        public static class ClearinghouseConstants
        {
            // Setting these here to keep track
            // Per KE 1-10-22, we are only tracking nitrates at this time and others are following hard-coded Beehive values
            public const string DefaultContaminant = "Nitrate";
            public const decimal DefaultReportingLimit = 0.10m;
            public const string DefaultLaboratory = "Ward";
        }

        public static ClearinghouseWaterQualityInspectionDto AsClearinghouseDto(
            this WaterQualityInspection waterQualityInspection)
        {
            var clearinghouseWaterQualityInspectionDto = new ClearinghouseWaterQualityInspectionDto()
            {
                WellRegistrationID = waterQualityInspection.Well.WellRegistrationID,
                WellNickname = waterQualityInspection.Well.WellNickname,
                Clearinghouse = waterQualityInspection.Well.Clearinghouse,
                Contaminant = ClearinghouseConstants.DefaultContaminant,
                SamplingDate = waterQualityInspection.InspectionDate,
                LabConcentration = waterQualityInspection.LabNitrates,
                FieldConcentration = waterQualityInspection.FieldNitrates,
                ReportingLimit = ClearinghouseConstants.DefaultReportingLimit,
                Laboratory = ClearinghouseConstants.DefaultLaboratory,
                IsReplacement = waterQualityInspection.Well.IsReplacement,
                RequiresChemigationInspection = waterQualityInspection.Well.RequiresChemigation,
                WaterQualityInspectionType = waterQualityInspection.WaterQualityInspectionType.WaterQualityInspectionTypeDisplayName,
                TownshipRangeSection = waterQualityInspection.Well.TownshipRangeSection,
                WellDepth = waterQualityInspection.Well.WellDepth,
                WellParticipation = waterQualityInspection.Well.WellParticipation?.WellParticipationDisplayName,
                WellUse = waterQualityInspection.Well.WellUse?.WellUseDisplayName,
                WellPreLevel = waterQualityInspection.PreWaterLevel,
                WellPostLevel = waterQualityInspection.PostWaterLevel,
                ScreenInterval = waterQualityInspection.Well.ScreenInterval
            };

            return clearinghouseWaterQualityInspectionDto;
        }
    }
}