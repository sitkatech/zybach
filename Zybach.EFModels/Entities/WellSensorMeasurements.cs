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
            return dbContext.WellSensorMeasurements.AsNoTracking();
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

        public static List<SensorMeasurementDto> GetWellSensorMeasurementsForWellAndSensors(
            ZybachDbContext dbContext, string wellRegistrationID,
            IEnumerable<Sensor> sensors)
        {
            // we need to filter by sensor names when looking at it from the well level because sensors can be moved from a well and reused.
            var sensorMeasurementDtos = new List<SensorMeasurementDto>();
            foreach (var sensor in sensors)
            {
                sensorMeasurementDtos.AddRange(ListByWellAndSensorAsSensorMeasurementDto(dbContext, wellRegistrationID, sensor.SensorName, sensor.SensorID, sensor.RetirementDate, sensor.GetChartDataSourceName(), sensor.GetChartAnomaliesDataSourceName()));
            }

            return sensorMeasurementDtos;
        }

        /// <summary>
        /// If you have sensor simples and don't care about charts use this
        /// </summary>
        public static List<SensorMeasurementDto> GetWellSensorMeasurementsForWellAndSensorSimples(
            ZybachDbContext dbContext, string wellRegistrationID,
            IEnumerable<SensorSimpleDto> sensors)
        {
            // we need to filter by sensor names when looking at it from the well level because sensors can be moved from a well and reused.
            var sensorMeasurementDtos = new List<SensorMeasurementDto>();
            foreach (var sensor in sensors)
            {
                sensorMeasurementDtos.AddRange(ListByWellAndSensorAsSensorMeasurementDto(dbContext, wellRegistrationID, sensor.SensorName, sensor.SensorID, sensor.RetirementDate, null, null));
            }

            return sensorMeasurementDtos;
        }

        public static List<AnnualPumpedVolume> GetAnnualPumpedVolumeForWellAndSensorType(ZybachDbContext dbContext, string wellRegistrationID,
            List<Sensor> sensors, SensorType sensorType)
        {
            var sensorMeasurementDtos =
                GetWellSensorMeasurementsForWellAndSensors(dbContext,
                    wellRegistrationID, sensors.Where(y => y.SensorTypeID == sensorType.SensorTypeID)).Where(x => !x.IsAnomalous);

            var annualPumpedVolumes = sensorMeasurementDtos.GroupBy(x => x.MeasurementDate.Year)
                .Select(x => new AnnualPumpedVolume(x.Key, x.Sum(y => y.MeasurementValue ?? 0), sensorType.SensorTypeDisplayName)).ToList();

            return annualPumpedVolumes;
        }

        public static List<SensorMeasurementDto> ListByWellAndSensorAsSensorMeasurementDto(ZybachDbContext dbContext, string wellRegistrationID, string sensorName, int sensorID, DateTime? retirementDate, string dataSourceName, string anomalousDataSourceName)
        {
            var wellSensorMeasurements = GetWellSensorMeasurementsImpl(dbContext)
                .Where(x => x.WellRegistrationID == wellRegistrationID && x.SensorName == sensorName)
                .ToList();

            return ListAsSensorMeasurementDtos(dbContext, sensorID, retirementDate, dataSourceName, anomalousDataSourceName, wellSensorMeasurements);
        }

        public static List<SensorMeasurementDto> ListBySensorAsSensorMeasurementDto(ZybachDbContext dbContext, string sensorName, int sensorID, DateTime? retirementDate, string dataSourceName, string anomalousDataSourceName)
        {
            var wellSensorMeasurements = GetWellSensorMeasurementsImpl(dbContext)
                .Where(x => x.SensorName == sensorName).ToList();

            return ListAsSensorMeasurementDtos(dbContext, sensorID, retirementDate, dataSourceName, anomalousDataSourceName, wellSensorMeasurements);
        }

        private static List<SensorMeasurementDto> ListAsSensorMeasurementDtos(ZybachDbContext dbContext, int sensorID, DateTime? retirementDate, string dataSourceName,
            string anomalousDataSourceName, List<WellSensorMeasurement> wellSensorMeasurements)
        {
            var sensorAnomalies = dbContext.SensorAnomalies.Where(x => x.SensorID == sensorID).ToList();
            wellSensorMeasurements = wellSensorMeasurements
                .Where(x => x.MeasurementType.MeasurementTypeID != MeasurementType.BatteryVoltage.MeasurementTypeID).ToList();

            return ZeroFillMissingDaysAsSensorMeasurementDto(wellSensorMeasurements, sensorAnomalies, retirementDate, dataSourceName,
                anomalousDataSourceName);
        }

        public static List<SensorMeasurementDto> ZeroFillMissingDaysAsSensorMeasurementDto(List<WellSensorMeasurement> wellSensorMeasurements, List<SensorAnomaly> sensorAnomalies,
            DateTime? retirementDate, string dataSourceName, string anomalousDataSourceName)
        {
            var allSensorMeasurementDtos = new List<SensorMeasurementDto>();

            if (!wellSensorMeasurements.Any())
            {
                return allSensorMeasurementDtos;
            }

            // first we need to zero fill missing dates, since the sensor data we get from Influx is sparse on purpose
            // to create the chart to display the non-anomalous and anomalous data together
            // we need to create two sets of data here: one for the non-anomalous and one for the anomalous
            // both series will have the same date/value data points
            // for the non anomalous series, we need to flip the values for the anomalous dates to null
            // for the anomalous series, we need to flip the values of the non-anomalous dates to null, except for the value right before the anomalous date range and the value right after, so that it will give the appearance of "connecting" the non-anomalous and anomalous series
            var units = wellSensorMeasurements.First().MeasurementType.UnitsDisplayPlural;
            var measurementValues = wellSensorMeasurements.ToLookup(
                x => x.MeasurementDate.ToShortDateString());
            var startDate = wellSensorMeasurements.Min(x => x.MeasurementDateInPacificTime);
            var endDate = retirementDate ?? DateTime.Today;
            var list = Enumerable.Range(0, (endDate - startDate).Days + 1)
                .ToList();
            var anomalousDateAsShortDateStrings = new List<string>();
            foreach (var sensorAnomaly in sensorAnomalies)
            {
                var anomalousDateRange = Enumerable.Range(0, (sensorAnomaly.EndDate - sensorAnomaly.StartDate).Days + 1)
                    .Select(d => sensorAnomaly.StartDate.AddDays(d).ToShortDateString());

                anomalousDateAsShortDateStrings.AddRange(anomalousDateRange);
            }

            var bookendDatesOfAnomalies =
            sensorAnomalies.Select(x => x.StartDate.AddDays(-1).ToShortDateString())
                .Union(sensorAnomalies.Select(x => x.EndDate.AddDays(1).ToShortDateString()));

            var nonAnomalousSensorMeasurementDtos = list.Select(a =>
            {
                var measurementDate = startDate.AddDays(a);
                var measurementValue = measurementValues.Contains(measurementDate.ToShortDateString()) ? measurementValues[measurementDate.ToShortDateString()].Sum(x => x.MeasurementValue) : 0;
                return new SensorMeasurementDto(dataSourceName, measurementDate, measurementValue, $"{measurementValue:N1} {units}", false);
            }).ToList();

            if (anomalousDateAsShortDateStrings.Any())
            {
                var anomalousSensorMeasurementDtos = nonAnomalousSensorMeasurementDtos.Select(x =>
                {
                    var currentDate = x.MeasurementDate.ToShortDateString();
                    var measurementValue =
                        anomalousDateAsShortDateStrings.Contains(currentDate) ||
                        bookendDatesOfAnomalies.Contains(currentDate)
                            ? x.MeasurementValue
                            : null;
                    var measurementValueString = measurementValue != null ? $"{measurementValue:N1} {units}" : null;
                    return new SensorMeasurementDto(anomalousDataSourceName, x.MeasurementDate, measurementValue,
                        measurementValueString, true);
                }).ToList();

                foreach (var nonAnomalousSensorMeasurementDto in nonAnomalousSensorMeasurementDtos
                             .Where(
                                 x => anomalousDateAsShortDateStrings.Contains(x.MeasurementDate.ToShortDateString()))
                             .ToList())
                {
                    nonAnomalousSensorMeasurementDto.MeasurementValue = null;
                    nonAnomalousSensorMeasurementDto.MeasurementValueString = null;
                }
                allSensorMeasurementDtos.AddRange(anomalousSensorMeasurementDtos);
            }

            allSensorMeasurementDtos.AddRange(nonAnomalousSensorMeasurementDtos);

            return allSensorMeasurementDtos;
        }

        public static List<SensorMeasurementDto> ZeroFillMissingDaysAsSensorMeasurementDtoAlternatePath(
    List<WellSensorMeasurement> wellSensorMeasurements, List<SensorAnomaly> sensorAnomalies, string sensorName, string sensorTypeDisplayName)
        {
            var allSensorMeasurementDtos = new List<SensorMeasurementDto>();

            if (!wellSensorMeasurements.Any())
            {
                return allSensorMeasurementDtos;
            }

            // first we need to zero fill missing dates, since the sensor data we get from Influx is sparse on purpose
            // to create the chart to display the non-anomalous and anomalous data together
            // we need to create two sets of data here: one for the non-anomalous and one for the anomalous
            // both series will have the same date/value data points
            // for the non anomalous series, we need to flip the values for the anomalous dates to null
            // for the anomalous series, we need to flip the values of the non-anomalous dates to null, except for the value right before the anomalous date range and the value right after, so that it will give the appearance of "connecting" the non-anomalous and anomalous series

            var measurementValues = wellSensorMeasurements.ToLookup(
                x => x.MeasurementDate.ToShortDateString());
            var startDate = wellSensorMeasurements.Min(x => x.MeasurementDateInPacificTime);
            var endDate = DateTime.Today;
            var list = Enumerable.Range(0, (endDate - startDate).Days + 1)
                .ToList();
            var anomalousDateAsShortDateStrings = new List<string>();
            foreach (var sensorAnomaly in sensorAnomalies)
            {
                var anomalousDateRange = Enumerable.Range(0, (sensorAnomaly.EndDate - sensorAnomaly.StartDate).Days + 1)
                    .Select(d => sensorAnomaly.StartDate.AddDays(d).ToShortDateString());

                anomalousDateAsShortDateStrings.AddRange(anomalousDateRange);
            }

            var bookendDatesOfAnomalies =
            sensorAnomalies.Select(x => x.StartDate.AddDays(-1).ToShortDateString())
                .Union(sensorAnomalies.Select(x => x.EndDate.AddDays(1).ToShortDateString()));

            var nonAnomalousSensorMeasurementDtos = list.Select(a =>
            {
                var measurementDate = startDate.AddDays(a);
                var measurementDateAsShortDateString = measurementDate.ToShortDateString();
                var measurementValue = measurementValues.Contains(measurementDateAsShortDateString) ? measurementValues[measurementDateAsShortDateString].Sum(x => x.MeasurementValue) : 0;
                var isAnomalous = anomalousDateAsShortDateStrings.Contains(measurementDateAsShortDateString) || bookendDatesOfAnomalies.Contains(measurementDateAsShortDateString);
                return new SensorMeasurementDto($"{sensorName} - {sensorTypeDisplayName}", measurementDate, measurementValue, $"{measurementValue:N1}", isAnomalous);
            }).ToList();

            allSensorMeasurementDtos.AddRange(nonAnomalousSensorMeasurementDtos);

            return allSensorMeasurementDtos;
        }
    }
}