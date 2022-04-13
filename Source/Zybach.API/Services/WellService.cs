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

        public List<WellWithSensorSummaryDto> GetAghubAndGeoOptixWells()
        {
            var wells = Wells.ListAsWellWithSensorSummaryDto(_dbContext);
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
                .Where(x => x.Sensors.Any(y => y.SensorTypeID == (int)Sensors.SensorTypeEnum.WellPressure))
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
            var wellWithSensorSummaryDtos = GetAghubAndGeoOptixWells();
            var firstReadingDateTimes = WellSensorMeasurements.GetFirstReadingDateTimes(_dbContext);
            var robustReviewDtos = wellWithSensorSummaryDtos.Select(wellWithSensorSummaryDto => CreateRobustReviewDto(wellWithSensorSummaryDto, firstReadingDateTimes)).ToList();
            return robustReviewDtos.Where(x => x != null).ToList();
        }

        private RobustReviewDto CreateRobustReviewDto(WellWithSensorSummaryDto wellWithSensorSummaryDto, Dictionary<string, DateTime> firstReadingDateTimes)
        {
            var wellRegistrationID = wellWithSensorSummaryDto.WellRegistrationID;
            if (!firstReadingDateTimes.ContainsKey(wellRegistrationID))
            {
                return null;
            }

            string dataSource;
            List<WellSensorMeasurementDto> wellSensorMeasurementDtos;
            if (wellWithSensorSummaryDto.HasElectricalData)
            {
                wellSensorMeasurementDtos = WellSensorMeasurements.GetWellSensorMeasurementsForWellByMeasurementType(
                    _dbContext,
                    wellRegistrationID, MeasurementTypeEnum.ElectricalUsage);
                dataSource = MeasurementTypes.ElectricalUsage;
            }
            else
            {
                const string continuityMeter = MeasurementTypes.ContinuityMeter;
                wellSensorMeasurementDtos =
                    WellSensorMeasurements.GetWellSensorMeasurementsForWellAndSensorsByMeasurementType(_dbContext,
                        wellRegistrationID,
                        new List<MeasurementTypeEnum>
                            {MeasurementTypeEnum.ContinuityMeter, MeasurementTypeEnum.FlowMeter},
                        wellWithSensorSummaryDto.Sensors.Where(y => y.SensorType == continuityMeter));
                dataSource = continuityMeter;
            }

            var monthlyPumpedVolume = wellSensorMeasurementDtos.GroupBy(x => x.MeasurementDate.ToString("yyyyMM"))
                .Select(x =>
                    new MonthlyPumpedVolume(x.First().ReadingYear, x.First().ReadingMonth,
                        x.Sum(y => y.MeasurementValue))).ToList();

            var point = (Point)((Feature)wellWithSensorSummaryDto.Location).Geometry;
            var robustReviewDto = new RobustReviewDto
            {
                WellRegistrationID = wellRegistrationID,
                WellTPID = wellWithSensorSummaryDto.WellTPID,
                Latitude = point.Coordinates.Latitude,
                Longitude = point.Coordinates.Longitude,
                DataSource = dataSource,
                MonthlyPumpedVolumeGallons = monthlyPumpedVolume
            };

            return robustReviewDto;
        }

    }
}