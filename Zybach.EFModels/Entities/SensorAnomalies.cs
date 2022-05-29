using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.EFModels.Util;
using Zybach.Models.DataTransferObjects;
using Zybach.Models.DataTransferObjects.User;

namespace Zybach.EFModels.Entities
{
    public static class SensorAnomalies
    {
        private static IQueryable<SensorAnomaly> GetSensorAnomaliesImpl(ZybachDbContext dbContext)
        {
            return dbContext.SensorAnomalies
                .Include(x => x.Sensor).ThenInclude(x => x.Well)
                .AsNoTracking();
        }

        public static List<SensorAnomalySimpleDto> ListAsSimpleDto(ZybachDbContext dbContext)
        {
            return GetSensorAnomaliesImpl(dbContext)
                .OrderByDescending(x => x.EndDate)
                .Select(x => x.AsSimpleDto())
                .ToList();
        }

        public static List<DateTime> GetAnomolousDatesBySensorName(ZybachDbContext dbContext, string sensorName)
        {
            var anomalousDates = new List<DateTime>();
            var sensorAnomalies = GetSensorAnomaliesImpl(dbContext)
                .Where(x => x.Sensor.SensorName == sensorName);

            foreach (var sensorAnomaly in sensorAnomalies)
            {
                var anomalousDateRange = Enumerable.Range(0, (sensorAnomaly.EndDate - sensorAnomaly.StartDate).Days + 1)
                    .Select(d => sensorAnomaly.StartDate.AddDays(d));

                anomalousDates.AddRange(anomalousDateRange);
            }

            return anomalousDates;
        }

        public static SensorAnomalySimpleDto GetByIDAsSimpleDto(ZybachDbContext dbContext, int sensorAnomalyID)
        {
            var sensorAnomaly = GetSensorAnomaliesImpl(dbContext).SingleOrDefault(x => x.SensorAnomalyID == sensorAnomalyID);
            return sensorAnomaly?.AsSimpleDto();
        }

        public static void CreateNew(ZybachDbContext dbContext, SensorAnomalyUpsertDto sensorAnomalyUpsertDto)
        {
            var sensorAnomaly = dbContext.SensorAnomalies.Add(new SensorAnomaly()
            {
                SensorID = sensorAnomalyUpsertDto.SensorID,
                StartDate = sensorAnomalyUpsertDto.StartDate.GetValueOrDefault(),
                EndDate = sensorAnomalyUpsertDto.EndDate.GetValueOrDefault(),
                Notes = sensorAnomalyUpsertDto.Notes
            }).Entity;

            var sensor = dbContext.Sensors.FirstOrDefault(x => x.SensorID == sensorAnomaly.SensorID);

            var measurementsForSensor = dbContext.WellSensorMeasurements
                .Where(x => x.SensorName == sensor.SensorName).AsEnumerable();

            var measurementsInCurrentDateRange = measurementsForSensor.Where(x =>
                x.MeasurementDate.IsDateInRange(sensorAnomaly.StartDate, sensorAnomaly.EndDate)).ToList();

            foreach (var wellSensorMeasurement in measurementsInCurrentDateRange)
            {
                wellSensorMeasurement.IsAnomalous = true;
            }

            dbContext.SaveChanges();
        }

        private static void UpdateWellSensorMeasurementsAnomalousFlag(ZybachDbContext dbContext,
            SensorAnomaly sensorAnomaly, SensorAnomalyUpsertDto sensorAnomalyUpsertDto)
        {
            var sensor = dbContext.Sensors.FirstOrDefault(x => x.SensorID == sensorAnomaly.SensorID);

            var measurementsForSensor = dbContext.WellSensorMeasurements
                .Where(x => x.SensorName == sensor.SensorName).AsEnumerable();

            var measurementsInCurrentDateRange = measurementsForSensor.Where(x =>
                                            x.MeasurementDate.IsDateInRange(sensorAnomaly.StartDate, sensorAnomaly.EndDate)).ToList();

            // ReSharper disable twice PossibleInvalidOperationException
            var measurementsInNewDateRange = measurementsForSensor.Where(x =>
                    x.MeasurementDate.IsDateInRange(sensorAnomalyUpsertDto.StartDate.Value, sensorAnomalyUpsertDto.EndDate.Value)).ToList();
            
            foreach (var wellSensorMeasurement in measurementsInCurrentDateRange)
            {
                wellSensorMeasurement.IsAnomalous = measurementsInNewDateRange.Contains(wellSensorMeasurement);
            }

            dbContext.UpdateRange(measurementsInCurrentDateRange);
        }

        public static void Update(ZybachDbContext dbContext, SensorAnomaly sensorAnomaly, SensorAnomalyUpsertDto sensorAnomalyUpsertDto)
        {
            UpdateWellSensorMeasurementsAnomalousFlag(dbContext, sensorAnomaly, sensorAnomalyUpsertDto);

            sensorAnomaly.StartDate = sensorAnomalyUpsertDto.StartDate.GetValueOrDefault();
            sensorAnomaly.EndDate = sensorAnomalyUpsertDto.EndDate.GetValueOrDefault();
            sensorAnomaly.Notes = sensorAnomalyUpsertDto.Notes;

            dbContext.SaveChanges();
        }

        public static void Delete(ZybachDbContext dbContext, SensorAnomaly sensorAnomaly)
        {
            var sensor = dbContext.Sensors.FirstOrDefault(x => x.SensorID == sensorAnomaly.SensorID);

            var measurementsForSensor = dbContext.WellSensorMeasurements
                .Where(x => x.SensorName == sensor.SensorName).AsEnumerable();

            var measurementsInCurrentDateRange = measurementsForSensor.Where(x =>
                x.MeasurementDate.IsDateInRange(sensorAnomaly.StartDate, sensorAnomaly.EndDate)).ToList();

            foreach (var wellSensorMeasurement in measurementsInCurrentDateRange)
            {
                wellSensorMeasurement.IsAnomalous = false;
            }

            dbContext.SensorAnomalies.Remove(sensorAnomaly);
            
            dbContext.SaveChanges();
        }
    }
}