//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumExtensionMethods
    {
        public static AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumDto AsDto(this AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum)
        {
            var agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumDto = new AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumDto()
            {
                AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumID = agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum.AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumID,
                AgHubIrrigationUnit = agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum.AgHubIrrigationUnit.AsDto(),
                WaterYearMonth = agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum.WaterYearMonth.AsDto(),
                EvapotranspirationAcreFeet = agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum.EvapotranspirationAcreFeet,
                EvapotranspirationInches = agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum.EvapotranspirationInches,
                PrecipitationAcreFeet = agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum.PrecipitationAcreFeet,
                PrecipitationInches = agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum.PrecipitationInches
            };
            DoCustomMappings(agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum, agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumDto);
            return agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumDto;
        }

        static partial void DoCustomMappings(AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum, AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumDto agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumDto);

        public static AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumSimpleDto AsSimpleDto(this AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum)
        {
            var agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumSimpleDto = new AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumSimpleDto()
            {
                AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumID = agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum.AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumID,
                AgHubIrrigationUnitID = agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum.AgHubIrrigationUnitID,
                WaterYearMonthID = agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum.WaterYearMonthID,
                EvapotranspirationAcreFeet = agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum.EvapotranspirationAcreFeet,
                EvapotranspirationInches = agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum.EvapotranspirationInches,
                PrecipitationAcreFeet = agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum.PrecipitationAcreFeet,
                PrecipitationInches = agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum.PrecipitationInches
            };
            DoCustomSimpleDtoMappings(agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum, agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumSimpleDto);
            return agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum, AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumSimpleDto agHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumSimpleDto);
    }
}