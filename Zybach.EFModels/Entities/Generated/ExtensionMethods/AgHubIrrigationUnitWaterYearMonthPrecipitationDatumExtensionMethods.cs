//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnitWaterYearMonthPrecipitationDatum]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubIrrigationUnitWaterYearMonthPrecipitationDatumExtensionMethods
    {
        public static AgHubIrrigationUnitWaterYearMonthPrecipitationDatumDto AsDto(this AgHubIrrigationUnitWaterYearMonthPrecipitationDatum agHubIrrigationUnitWaterYearMonthPrecipitationDatum)
        {
            var agHubIrrigationUnitWaterYearMonthPrecipitationDatumDto = new AgHubIrrigationUnitWaterYearMonthPrecipitationDatumDto()
            {
                AgHubIrrigationUnitWaterYearMonthPrecipitationDatumID = agHubIrrigationUnitWaterYearMonthPrecipitationDatum.AgHubIrrigationUnitWaterYearMonthPrecipitationDatumID,
                AgHubIrrigationUnit = agHubIrrigationUnitWaterYearMonthPrecipitationDatum.AgHubIrrigationUnit.AsDto(),
                WaterYearMonth = agHubIrrigationUnitWaterYearMonthPrecipitationDatum.WaterYearMonth.AsDto(),
                PrecipitationAcreFeet = agHubIrrigationUnitWaterYearMonthPrecipitationDatum.PrecipitationAcreFeet,
                PrecipitationInches = agHubIrrigationUnitWaterYearMonthPrecipitationDatum.PrecipitationInches
            };
            DoCustomMappings(agHubIrrigationUnitWaterYearMonthPrecipitationDatum, agHubIrrigationUnitWaterYearMonthPrecipitationDatumDto);
            return agHubIrrigationUnitWaterYearMonthPrecipitationDatumDto;
        }

        static partial void DoCustomMappings(AgHubIrrigationUnitWaterYearMonthPrecipitationDatum agHubIrrigationUnitWaterYearMonthPrecipitationDatum, AgHubIrrigationUnitWaterYearMonthPrecipitationDatumDto agHubIrrigationUnitWaterYearMonthPrecipitationDatumDto);

        public static AgHubIrrigationUnitWaterYearMonthPrecipitationDatumSimpleDto AsSimpleDto(this AgHubIrrigationUnitWaterYearMonthPrecipitationDatum agHubIrrigationUnitWaterYearMonthPrecipitationDatum)
        {
            var agHubIrrigationUnitWaterYearMonthPrecipitationDatumSimpleDto = new AgHubIrrigationUnitWaterYearMonthPrecipitationDatumSimpleDto()
            {
                AgHubIrrigationUnitWaterYearMonthPrecipitationDatumID = agHubIrrigationUnitWaterYearMonthPrecipitationDatum.AgHubIrrigationUnitWaterYearMonthPrecipitationDatumID,
                AgHubIrrigationUnitID = agHubIrrigationUnitWaterYearMonthPrecipitationDatum.AgHubIrrigationUnitID,
                WaterYearMonthID = agHubIrrigationUnitWaterYearMonthPrecipitationDatum.WaterYearMonthID,
                PrecipitationAcreFeet = agHubIrrigationUnitWaterYearMonthPrecipitationDatum.PrecipitationAcreFeet,
                PrecipitationInches = agHubIrrigationUnitWaterYearMonthPrecipitationDatum.PrecipitationInches
            };
            DoCustomSimpleDtoMappings(agHubIrrigationUnitWaterYearMonthPrecipitationDatum, agHubIrrigationUnitWaterYearMonthPrecipitationDatumSimpleDto);
            return agHubIrrigationUnitWaterYearMonthPrecipitationDatumSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(AgHubIrrigationUnitWaterYearMonthPrecipitationDatum agHubIrrigationUnitWaterYearMonthPrecipitationDatum, AgHubIrrigationUnitWaterYearMonthPrecipitationDatumSimpleDto agHubIrrigationUnitWaterYearMonthPrecipitationDatumSimpleDto);
    }
}