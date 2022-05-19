//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnitWaterYearMonthETDatum]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubIrrigationUnitWaterYearMonthETDatumExtensionMethods
    {
        public static AgHubIrrigationUnitWaterYearMonthETDatumDto AsDto(this AgHubIrrigationUnitWaterYearMonthETDatum agHubIrrigationUnitWaterYearMonthETDatum)
        {
            var agHubIrrigationUnitWaterYearMonthETDatumDto = new AgHubIrrigationUnitWaterYearMonthETDatumDto()
            {
                AgHubIrrigationUnitWaterYearMonthETDatumID = agHubIrrigationUnitWaterYearMonthETDatum.AgHubIrrigationUnitWaterYearMonthETDatumID,
                AgHubIrrigationUnit = agHubIrrigationUnitWaterYearMonthETDatum.AgHubIrrigationUnit.AsDto(),
                WaterYearMonth = agHubIrrigationUnitWaterYearMonthETDatum.WaterYearMonth.AsDto(),
                EvapotranspirationRateInches = agHubIrrigationUnitWaterYearMonthETDatum.EvapotranspirationRateInches,
                EvapotranspirationRateAcreFeet = agHubIrrigationUnitWaterYearMonthETDatum.EvapotranspirationRateAcreFeet,
                PrecipitationAcreFeet = agHubIrrigationUnitWaterYearMonthETDatum.PrecipitationAcreFeet,
                PrecipitationInches = agHubIrrigationUnitWaterYearMonthETDatum.PrecipitationInches
            };
            DoCustomMappings(agHubIrrigationUnitWaterYearMonthETDatum, agHubIrrigationUnitWaterYearMonthETDatumDto);
            return agHubIrrigationUnitWaterYearMonthETDatumDto;
        }

        static partial void DoCustomMappings(AgHubIrrigationUnitWaterYearMonthETDatum agHubIrrigationUnitWaterYearMonthETDatum, AgHubIrrigationUnitWaterYearMonthETDatumDto agHubIrrigationUnitWaterYearMonthETDatumDto);

        public static AgHubIrrigationUnitWaterYearMonthETDatumSimpleDto AsSimpleDto(this AgHubIrrigationUnitWaterYearMonthETDatum agHubIrrigationUnitWaterYearMonthETDatum)
        {
            var agHubIrrigationUnitWaterYearMonthETDatumSimpleDto = new AgHubIrrigationUnitWaterYearMonthETDatumSimpleDto()
            {
                AgHubIrrigationUnitWaterYearMonthETDatumID = agHubIrrigationUnitWaterYearMonthETDatum.AgHubIrrigationUnitWaterYearMonthETDatumID,
                AgHubIrrigationUnitID = agHubIrrigationUnitWaterYearMonthETDatum.AgHubIrrigationUnitID,
                WaterYearMonthID = agHubIrrigationUnitWaterYearMonthETDatum.WaterYearMonthID,
                EvapotranspirationRateInches = agHubIrrigationUnitWaterYearMonthETDatum.EvapotranspirationRateInches,
                EvapotranspirationRateAcreFeet = agHubIrrigationUnitWaterYearMonthETDatum.EvapotranspirationRateAcreFeet,
                PrecipitationAcreFeet = agHubIrrigationUnitWaterYearMonthETDatum.PrecipitationAcreFeet,
                PrecipitationInches = agHubIrrigationUnitWaterYearMonthETDatum.PrecipitationInches
            };
            DoCustomSimpleDtoMappings(agHubIrrigationUnitWaterYearMonthETDatum, agHubIrrigationUnitWaterYearMonthETDatumSimpleDto);
            return agHubIrrigationUnitWaterYearMonthETDatumSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(AgHubIrrigationUnitWaterYearMonthETDatum agHubIrrigationUnitWaterYearMonthETDatum, AgHubIrrigationUnitWaterYearMonthETDatumSimpleDto agHubIrrigationUnitWaterYearMonthETDatumSimpleDto);
    }
}