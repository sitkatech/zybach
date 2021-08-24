using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class WellSensorMeasurement
    {
        public static List<WellSensorMeasurementDto> GetWellSensorMeasurementsByMeasurementType(
    ZybachDbContext dbContext, MeasurementTypeEnum measurementTypeEnum)
        {
            return GetWellSensorMeasurementsImpl(dbContext)
                .Where(x => x.MeasurementTypeID == (int)measurementTypeEnum).Select(x => x.AsDto())
                .ToList();
        }

        public static List<WellSensorMeasurementDto> GetWellSensorMeasurementsByMeasurementTypeAndYear(
            ZybachDbContext dbContext, MeasurementTypeEnum measurementTypeEnum, int year)
        {
            return GetWellSensorMeasurementsImpl(dbContext)
                .Where(x => x.MeasurementTypeID == (int)measurementTypeEnum
                            && x.ReadingDate.Year == year
                ).Select(x => x.AsDto())
                .ToList();
        }

        public static List<WellSensorMeasurementDto> GetWellSensorMeasurementsForSensorsByMeasurementType(
            ZybachDbContext dbContext, MeasurementTypeEnum measurementTypeEnum,
            IEnumerable<SensorSummaryDto> sensorTypeSensors)
        {
            var sensorNames = sensorTypeSensors.Select(y => y.SensorName);
            return GetWellSensorMeasurementsImpl(dbContext)
                .Where(x => x.MeasurementTypeID == (int)measurementTypeEnum &&
                            sensorNames.Contains(x.SensorName)).Select(x => x.AsDto())
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
            return wellSensorMeasurements.Any() ? wellSensorMeasurements.Min(x => x.ReadingDate) : (DateTime?) null;
        }

        public static DateTime? GetLastReadingDateTimeForWell(ZybachDbContext dbContext, string wellRegistrationID)
        {
            var wellSensorMeasurements = dbContext.WellSensorMeasurements.Where(x => x.WellRegistrationID == wellRegistrationID).ToList();
            return wellSensorMeasurements.Any() ? wellSensorMeasurements.Max(x => x.ReadingDate) : (DateTime?)null;
        }

        public static Dictionary<string, DateTime> GetFirstReadingDateTimes(ZybachDbContext dbContext)
        {
            return dbContext.WellSensorMeasurements.ToList().GroupBy(x => x.WellRegistrationID).ToDictionary(x => x.Key, x => x.Min(y => y.ReadingDate));
        }

        public static Dictionary<string, DateTime> GetLastReadingDateTimes(ZybachDbContext dbContext)
        {
            return dbContext.WellSensorMeasurements.ToList().GroupBy(x => x.WellRegistrationID).ToDictionary(x => x.Key, x => x.Max(y => y.ReadingDate));
        }
    }
}