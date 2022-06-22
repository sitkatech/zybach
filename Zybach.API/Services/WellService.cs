using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.Extensions.Logging;
using Zybach.API.Models;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Services
{
    public class WellService
    {
        private readonly ZybachDbContext _dbContext;
        private readonly ILogger<WellService> _logger;

        public WellService(ZybachDbContext dbContext, ILogger<WellService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public List<WellWithSensorSimpleDto> GetAghubAndGeoOptixWells()
        {
            var wells = Wells.ListAsWellWithSensorSimpleDto(_dbContext);
            var lastReadingDateTimes = WellSensorMeasurements.GetLastReadingDateTimes(_dbContext);
            var firstReadingDateTimes = WellSensorMeasurements.GetFirstReadingDateTimes(_dbContext);
            wells.ForEach(x =>
            {
                x.LastReadingDate = lastReadingDateTimes.ContainsKey(x.WellRegistrationID)
                    ? lastReadingDateTimes[x.WellRegistrationID]
                    : (DateTime?)null;
                x.FirstReadingDate = firstReadingDateTimes.ContainsKey(x.WellRegistrationID)
                    ? firstReadingDateTimes[x.WellRegistrationID]
                    : (DateTime?)null;
            });

            return wells;
        }

        public List<WellWaterLevelMapSummaryDto> GetWellPressureWellsForWaterLevelSummary()
        {
            var wells = Wells.ListAsWaterLevelMapSummaryDtos(_dbContext)
                .Where(x => x.Sensors.Any(y => y.SensorTypeID == (int)SensorTypeEnum.WellPressure))
                .ToList();
            var lastReadingDateTimes = WellSensorMeasurements.GetLastReadingDateTimes(_dbContext);
            wells.ForEach(x =>
            {
                x.LastReadingDate = lastReadingDateTimes.ContainsKey(x.WellRegistrationID)
                    ? lastReadingDateTimes[x.WellRegistrationID]
                    : (DateTime?)null;
            });

            return wells;
        }

        public List<RobustReviewDto> GetRobustReviewDtos()
        {
            var wellWithSensorSimpleDtos = GetAghubAndGeoOptixWells();
            var firstReadingDateTimes = WellSensorMeasurements.GetFirstReadingDateTimes(_dbContext);
            var robustReviewDtos = wellWithSensorSimpleDtos.Select(x => CreateRobustReviewDto(x, firstReadingDateTimes)).ToList();
            return robustReviewDtos.Where(x => x != null).ToList();
        }

        private RobustReviewDto CreateRobustReviewDto(WellWithSensorSimpleDto wellWithSensorSimpleDto, Dictionary<string, DateTime> firstReadingDateTimes)
        {
            var wellRegistrationID = wellWithSensorSimpleDto.WellRegistrationID;
            if (!firstReadingDateTimes.ContainsKey(wellRegistrationID))
            {
                return null;
            }

            string dataSource;
            List<SensorMeasurementDto> sensorMeasurementDtos;
            if (wellWithSensorSimpleDto.HasElectricalData)
            {
                sensorMeasurementDtos = WellSensorMeasurements.GetElectricalUsagesForWell(
                    _dbContext,
                    wellRegistrationID).Select(x => new SensorMeasurementDto(SensorType.ElectricalUsage.SensorTypeDisplayName, x.MeasurementDate, x.MeasurementValue, x.MeasurementValueString, false)).ToList();
                dataSource = SensorType.ElectricalUsage.SensorTypeDisplayName;
            }
            else
            {
                var continuityMeter = SensorType.ContinuityMeter.SensorTypeDisplayName;
                sensorMeasurementDtos =
                    WellSensorMeasurements.GetWellSensorMeasurementsForWellAndSensors(_dbContext,
                        wellRegistrationID,
                        wellWithSensorSimpleDto.Sensors.Where(y => y.SensorTypeName == continuityMeter));
                dataSource = continuityMeter;
            }

            var monthlyPumpedVolume = sensorMeasurementDtos.GroupBy(x => x.MeasurementDate.ToString("yyyyMM"))
                .Select(x =>
                    new MonthlyPumpedVolume(x.First().MeasurementDate.Year, x.First().MeasurementDate.Month,
                        x.Sum(y => y.MeasurementValue ?? 0))).ToList();

            var point = (Point)((Feature)wellWithSensorSimpleDto.Location).Geometry;
            var robustReviewDto = new RobustReviewDto
            {
                WellRegistrationID = wellRegistrationID,
                WellTPID = wellWithSensorSimpleDto.WellTPID,
                Latitude = point.Coordinates.Latitude,
                Longitude = point.Coordinates.Longitude,
                DataSource = dataSource,
                MonthlyPumpedVolumeGallons = monthlyPumpedVolume
            };

            return robustReviewDto;
        }

    }
}