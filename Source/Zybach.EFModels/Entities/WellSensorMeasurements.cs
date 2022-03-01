using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class WellSensorMeasurements
    {
        public static List<WellSensorMeasurementDto> GetWellSensorMeasurementsByMeasurementTypeAndYear(
            ZybachDbContext dbContext, MeasurementTypeEnum measurementTypeEnum, int year)
        {
            return GetWellSensorMeasurementsImpl(dbContext)
                .Where(x => x.MeasurementTypeID == (int)measurementTypeEnum
                            && x.ReadingYear == year
                ).Select(x => x.AsDto())
                .ToList();
        }

        private static IQueryable<WellSensorMeasurement> GetWellSensorMeasurementsImpl(ZybachDbContext dbContext)
        {
            return dbContext.WellSensorMeasurements.Include(x => x.MeasurementType).AsNoTracking();
        }

        public static List<WellSensorMeasurementDto> GetWellSensorMeasurementsForWellAndSensorsByMeasurementType(
            ZybachDbContext dbContext, string wellRegistrationID, MeasurementTypeEnum measurementTypeEnum,
            IEnumerable<SensorSummaryDto> sensorTypeSensors)
        {
            var sensorNames = sensorTypeSensors.Select(y => y.SensorName);
            return GetWellSensorMeasurementsImpl(dbContext)
                .Where(x => x.WellRegistrationID == wellRegistrationID &&
                            x.MeasurementTypeID == (int)measurementTypeEnum &&
                            sensorNames.Contains(x.SensorName)).Select(x => x.AsDto())
                .ToList();
        }

        public static List<WellSensorMeasurementDto> GetWellSensorMeasurementsForWellAndSensorsByMeasurementType(
            ZybachDbContext dbContext, string wellRegistrationID, List<MeasurementTypeEnum> measurementTypeEnums,
            IEnumerable<SensorSummaryDto> sensorTypeSensors)
        {
            var measurementTypeIDs = measurementTypeEnums.Select(x => (int) x);
            var sensorNames = sensorTypeSensors.Select(y => y.SensorName);
            return GetWellSensorMeasurementsImpl(dbContext)
                .Where(x => x.WellRegistrationID == wellRegistrationID &&
                            measurementTypeIDs.Contains(x.MeasurementTypeID) &&
                            sensorNames.Contains(x.SensorName)).Select(x => x.AsDto())
                .ToList();
        }

        public static List<WellSensorMeasurementDto> GetWellSensorMeasurementsForWellByMeasurementType(
            ZybachDbContext dbContext, string wellRegistrationID, MeasurementTypeEnum measurementTypeEnum)
        {
            return GetWellSensorMeasurementsImpl(dbContext)
                .Where(x => x.WellRegistrationID == wellRegistrationID &&
                            x.MeasurementTypeID == (int)measurementTypeEnum).Select(x => x.AsDto())
                .ToList();
        }

        public static DateTime? GetFirstReadingDateTimeForWell(ZybachDbContext dbContext, string wellRegistrationID)
        {
            var wellSensorMeasurements = dbContext.WellSensorMeasurements.Where(x => x.WellRegistrationID == wellRegistrationID).ToList();
            return wellSensorMeasurements.Any() ? wellSensorMeasurements.Min(x => x.MeasurementDate) : null;
        }

        public static DateTime? GetLastReadingDateTimeForWell(ZybachDbContext dbContext, string wellRegistrationID)
        {
            var wellSensorMeasurements = dbContext.WellSensorMeasurements.Where(x => x.WellRegistrationID == wellRegistrationID).ToList();
            return wellSensorMeasurements.Any() ? wellSensorMeasurements.Max(x => x.MeasurementDate) : null;
        }

        public static Dictionary<string, DateTime> GetFirstReadingDateTimes(ZybachDbContext dbContext)
        {
            return dbContext.WellSensorMeasurements.ToList().GroupBy(x => x.WellRegistrationID).ToDictionary(x => x.Key, x => x.Min(y => y.MeasurementDate));
        }

        public static Dictionary<string, DateTime> GetLastReadingDateTimes(ZybachDbContext dbContext)
        {
            return dbContext.WellSensorMeasurements.ToList().GroupBy(x => x.WellRegistrationID).ToDictionary(x => x.Key, x => x.Max(y => y.MeasurementDate));
        }

        public static List<WellSensorReadingDateDto> GetFirstReadingDateTimesPerSensorForWells(ZybachDbContext dbContext, MeasurementTypeEnum measurementTypeEnum, List<string> wellRegistrationIDs)
        {
            var firstReadingDateTimesPerSensorForWells = dbContext.WellSensorMeasurements.Where(x => x.MeasurementTypeID == (int) measurementTypeEnum && wellRegistrationIDs.Contains(x.WellRegistrationID)).ToList()
                .GroupBy(x => new { x.WellRegistrationID, x.SensorName});

            var wellSensorReadingDates = firstReadingDateTimesPerSensorForWells.Select(x =>
                    new WellSensorReadingDateDto(x.Key.WellRegistrationID, x.Key.SensorName,
                        x.Min(y => y.MeasurementDate)))
                .ToList();
            return wellSensorReadingDates;
        }

        public static List<WellSensorMeasurementDto> ListBySensorAsDto(ZybachDbContext dbContext, string sensorName)
        {
            var wellSensorMeasurements = GetWellSensorMeasurementsImpl(dbContext)
                .Where(x => x.SensorName == sensorName).ToList();

            var anomalousDates = SensorAnomalies.GetAnomolousDatesBySensorName(dbContext, sensorName);
            wellSensorMeasurements = wellSensorMeasurements.Where(x => !anomalousDates.Contains(x.MeasurementDate)).ToList();
            
            return ZeroFillMissingDaysAsDto(wellSensorMeasurements);
        }

        private static List<WellSensorMeasurementDto> ZeroFillMissingDaysAsDto(
            List<WellSensorMeasurement> wellSensorMeasurements)
        {
            if (!wellSensorMeasurements.Any())
            {
                return new List<WellSensorMeasurementDto>();
            }

            var measurementTypeDto = wellSensorMeasurements.First().MeasurementType.AsDto();
            var sensorName = wellSensorMeasurements.First().SensorName;
            var units = measurementTypeDto.MeasurementTypeID == (int)MeasurementTypeEnum.WellPressure ? "feet" : "gallons";
            var measurementValues = wellSensorMeasurements.ToLookup(
                x => x.MeasurementDate.ToShortDateString());
            var startDate = wellSensorMeasurements.Min(x => x.MeasurementDateInPacificTime);
            var endDate = DateTime.Today;
            var list = Enumerable.Range(0, (endDate - startDate).Days + 1)
                .ToList();
            return list.Select(a =>
            {
                var measurementDate = startDate.AddDays(a);
                var measurementValue = measurementValues.Contains(measurementDate.ToShortDateString()) ? measurementValues[measurementDate.ToShortDateString()].Sum(x => x.MeasurementValue) : 0;
                return new WellSensorMeasurementDto(measurementTypeDto, sensorName, measurementDate, measurementValue, $"{measurementValue:N1} {units}");
            }).ToList();
        }


    }
}