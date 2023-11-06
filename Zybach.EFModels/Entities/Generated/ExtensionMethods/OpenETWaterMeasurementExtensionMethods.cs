//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[OpenETWaterMeasurement]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class OpenETWaterMeasurementExtensionMethods
    {
        public static OpenETWaterMeasurementDto AsDto(this OpenETWaterMeasurement openETWaterMeasurement)
        {
            var openETWaterMeasurementDto = new OpenETWaterMeasurementDto()
            {
                OpenETWaterMeasurementID = openETWaterMeasurement.OpenETWaterMeasurementID,
                WellTPID = openETWaterMeasurement.WellTPID,
                OpenETDataType = openETWaterMeasurement.OpenETDataType.AsDto(),
                ReportedDate = openETWaterMeasurement.ReportedDate,
                TransactionDate = openETWaterMeasurement.TransactionDate,
                ReportedValueInches = openETWaterMeasurement.ReportedValueInches,
                ReportedValueAcreFeet = openETWaterMeasurement.ReportedValueAcreFeet,
                IrrigationUnitArea = openETWaterMeasurement.IrrigationUnitArea
            };
            DoCustomMappings(openETWaterMeasurement, openETWaterMeasurementDto);
            return openETWaterMeasurementDto;
        }

        static partial void DoCustomMappings(OpenETWaterMeasurement openETWaterMeasurement, OpenETWaterMeasurementDto openETWaterMeasurementDto);

        public static OpenETWaterMeasurementSimpleDto AsSimpleDto(this OpenETWaterMeasurement openETWaterMeasurement)
        {
            var openETWaterMeasurementSimpleDto = new OpenETWaterMeasurementSimpleDto()
            {
                OpenETWaterMeasurementID = openETWaterMeasurement.OpenETWaterMeasurementID,
                WellTPID = openETWaterMeasurement.WellTPID,
                OpenETDataTypeID = openETWaterMeasurement.OpenETDataTypeID,
                ReportedDate = openETWaterMeasurement.ReportedDate,
                TransactionDate = openETWaterMeasurement.TransactionDate,
                ReportedValueInches = openETWaterMeasurement.ReportedValueInches,
                ReportedValueAcreFeet = openETWaterMeasurement.ReportedValueAcreFeet,
                IrrigationUnitArea = openETWaterMeasurement.IrrigationUnitArea
            };
            DoCustomSimpleDtoMappings(openETWaterMeasurement, openETWaterMeasurementSimpleDto);
            return openETWaterMeasurementSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(OpenETWaterMeasurement openETWaterMeasurement, OpenETWaterMeasurementSimpleDto openETWaterMeasurementSimpleDto);
    }
}