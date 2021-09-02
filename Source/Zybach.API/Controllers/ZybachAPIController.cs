using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Models;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiKey]
    [ApiController]
    public class ZybachAPIController : SitkaController<ZybachAPIController>
    {
        private readonly WellService _wellService;

        public ZybachAPIController(ZybachDbContext dbContext, ILogger<ZybachAPIController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, WellService wellService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _wellService = wellService;
        }

        /// <summary>
        /// Returns a time series representing pumped volume at a well or series of wells, summed daily for a given date range.
        /// Each point in the output time series represents the total pumped volume for the given day.
        /// </summary>
        /// <remarks>
        /// Sample requests:
        ///
        ///     Returns data for ALL wells from 8/1/2021 to 8/31/2021
        ///     GET /api/wells/pumpedVolume?startDate=2021-08-01&amp;endDate=2021-08-31
        ///
        ///     Returns data for Well Registration IDs G-056157, G-097457, G-110920 from 8/1/2021 to 8/31/2021
        ///     GET /api/wells/pumpedVolume?startDate=2021-08-01&amp;endDate=2021-08-31&amp;wellRegistrationID=G-056157&amp;wellRegistrationID=G-097457&amp;wellRegistrationID=G-110920
        /// </remarks>
        /// <param name="wellRegistrationID">The Well Registration ID(s) for the requested Well(s). If left blank, will bring back data for every Well that has reported data within the time range.</param>
        /// <param name="startDate">The start date for the report in yyyy-MM-dd format (eg. 2020-06-23)</param>
        /// <param name="endDate">The end date for the report in yyyy-MM-dd format (eg. 2020-06-23)</param>
        [Produces("application/json")]
        [HttpGet("/api/wells/pumpedVolume")]
        public ApiResult<StructuredResults> GetPumpedVolume([FromQuery] List<string> wellRegistrationID, [FromQuery] string startDate, [FromQuery] string endDate)
        {
            return GetPumpedVolumeImpl(wellRegistrationID, startDate, endDate);
        }

        /// <summary>
        /// Returns a time series representing pumped volume at a well, summed daily for a given date range.
        /// Each point in the output time series represents the total pumped volume for the given day.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/wells/G-110920/pumpedVolume?startDate=2021-08-01&amp;endDate=2021-08-31
        /// </remarks>
        /// <param name="wellRegistrationID">The Well Registration ID for the requested Well.</param>
        /// <param name="startDate">The start date for the report in yyyy-MM-dd format (eg. 2020-06-23)</param>
        /// <param name="endDate">The end date for the report in yyyy-MM-dd format (eg. 2020-06-23)</param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet("/api/wells/{wellRegistrationID}/pumpedVolume")]
        public ApiResult<StructuredResults> GetPumpedVolume([FromRoute] string wellRegistrationID, [FromQuery] string startDate, [FromQuery] string endDate)
        {
            return GetPumpedVolumeImpl(new List<string> { wellRegistrationID }, startDate, endDate);
        }

        private ApiResult<StructuredResults> GetPumpedVolumeImpl(List<string> wellRegistrationIDs, string startDateString, string endDateString)
        {
            if (string.IsNullOrWhiteSpace(endDateString))
            {
                endDateString = DateTime.Today.ToShortDateString();
            }

            if (!DateTime.TryParse(startDateString, out var startDate))
            {
                throw new ArgumentException(
                    "Start date is not a valid Date string in ISO 8601 format. Please enter a valid date string ");
            }

            if (!DateTime.TryParse(endDateString, out var endDate))
            {
                throw new ArgumentException(
                    "End date is not a valid Date string in ISO 8601 format. Please enter a valid date string ");
            }

            if (startDate > endDate)
            {
                throw new ArgumentOutOfRangeException("startDate",
                    "Start date occurs after End date. Please ensure that Start Date occurs before End date");
            }

            try
            {
                var query = _dbContext.WellSensorMeasurements.Include(x => x.MeasurementType).AsNoTracking().Where(
                    x =>
                        x.MeasurementTypeID == (int)MeasurementTypeEnum.ContinuityMeter
                        && x.ReadingYear >= startDate.Year && x.ReadingMonth >= startDate.Month &&
                        x.ReadingDay >= startDate.Day
                        && x.ReadingYear <= endDate.Year && x.ReadingMonth <= endDate.Month &&
                        x.ReadingDay <= endDate.Day
                );
                List<WellSensorMeasurementDto> wellSensorMeasurementDtos;
                if (wellRegistrationIDs != null && wellRegistrationIDs.Any())
                {
                    wellSensorMeasurementDtos = query.Where(x => wellRegistrationIDs.Contains(x.WellRegistrationID)).Select(x => x.AsDto()).ToList();
                }
                else
                {
                    wellSensorMeasurementDtos = query.Select(x => x.AsDto()).ToList();
                }

                var aghubWells = _dbContext.AgHubWells.AsNoTracking().Where(x =>
                        wellRegistrationIDs.Contains(x.WellRegistrationID)).ToList()
                    .ToDictionary(x => x.WellRegistrationID, x => x.PumpingRateGallonsPerMinute);

                return new ApiResult<StructuredResults>
                {
                    Status = "success",
                    Result = query.Any() ? StructureResults(wellSensorMeasurementDtos, aghubWells, startDate, endDate) : null
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Comprehensive data download to support Robust Review processes
        /// </summary>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet("/api/wells/download/robustReviewScenarioJson")]
        public async Task<List<RobustReviewDto>> GetRobustReviewJsonFile()
        {
            var wellWithSensorSummaryDtos = await _wellService.GetAghubAndGeoOptixWells();
            var firstReadingDateTimes = WellSensorMeasurement.GetFirstReadingDateTimes(_dbContext);
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
            if (wellWithSensorSummaryDto.HasElectricalData ?? false)
            {
                wellSensorMeasurementDtos = WellSensorMeasurement.GetWellSensorMeasurementsForWellByMeasurementType(
                    _dbContext,
                    wellRegistrationID, MeasurementTypeEnum.ElectricalUsage);
                dataSource = InfluxDBService.SensorTypes.ElectricalUsage;
            }
            else
            {
                const string continuityMeter = InfluxDBService.SensorTypes.ContinuityMeter;
                wellSensorMeasurementDtos =
                    WellSensorMeasurement.GetWellSensorMeasurementsForWellAndSensorsByMeasurementType(_dbContext,
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

            var point = (Point)wellWithSensorSummaryDto.Location.Geometry;
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


        private static StructuredResults StructureResults(List<WellSensorMeasurementDto> results,
            Dictionary<string, int> aghubWells, DateTime startDate, DateTime endDate)
        {
            var distinctWells = results.Select(x => x.WellRegistrationID).Distinct().ToList();
            var volumesByWell = new List<VolumeByWell>();
            foreach (var wellRegistrationID in distinctWells)
            {
                var currentWellResults = results.Where(x => x.WellRegistrationID == wellRegistrationID)
                    .OrderBy(x => x.MeasurementDate).ToList();
                var volumeByWell = new VolumeByWell
                {
                    WellRegistrationID = wellRegistrationID,
                    IntervalVolumes = CreateIntervalVolumesAndZeroFillMissingDays(wellRegistrationID,
                        currentWellResults, startDate, endDate,
                        aghubWells.ContainsKey(wellRegistrationID) ? aghubWells[wellRegistrationID] : 0)
                };
                volumesByWell.Add(volumeByWell);
            }

            return new StructuredResults
            {
                IntervalCountTotal = volumesByWell.Sum(x => x.IntervalCount),
                IntervalStart = startDate.ToString("yyyy-MM-dd"),
                IntervalEnd = endDate.ToString("yyyy-MM-dd"),
                WellCount = distinctWells.Count,
                VolumesByWell = volumesByWell
            };
        }

        private static List<IntervalVolumeDto> CreateIntervalVolumesAndZeroFillMissingDays(
            string wellRegistrationID, List<WellSensorMeasurementDto> wellSensorMeasurementDtos, DateTime startDate,
            DateTime endDate, int pumpingRateGallonsPerMinute)
        {
            var intervalVolumeDtos = new List<IntervalVolumeDto>();
            if (!wellSensorMeasurementDtos.Any())
            {
                return intervalVolumeDtos;
            }

            var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1).ToList();

            var sensorNames = wellSensorMeasurementDtos.Select(x => x.SensorName).Distinct().ToList();
            foreach (var sensorName in sensorNames)
            {
                var measurementValues = wellSensorMeasurementDtos.Where(x => x.SensorName == sensorName).ToDictionary(
                    x => x.MeasurementDate.ToShortDateString(), x => x.MeasurementValue);
                var dtos = dateRange.Select(a =>
                {
                    var dateTime = startDate.AddDays(a);
                    var gallons = measurementValues.ContainsKey(dateTime.ToShortDateString()) ? measurementValues[dateTime.ToShortDateString()] : 0;
                    return new IntervalVolumeDto(wellRegistrationID, dateTime, gallons, InfluxDBService.SensorTypes.ContinuityMeter, sensorName, pumpingRateGallonsPerMinute);
                });
                intervalVolumeDtos.AddRange(dtos);
            }

            return intervalVolumeDtos;
        }
    }
}