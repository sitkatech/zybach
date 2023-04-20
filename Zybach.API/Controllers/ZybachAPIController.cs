using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ZybachAPIController : SitkaApiController<ZybachAPIController>
    {
        private readonly InfluxDBService _influxDbService;
        private readonly ZybachConfiguration _zybachConfiguration;

        public ZybachAPIController(ZybachDbContext dbContext, ILogger<ZybachAPIController> logger, InfluxDBService influxDbService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger)
        {
            _influxDbService = influxDbService;
            _zybachConfiguration = zybachConfiguration.Value;
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
        public StructuredResultsDto GetPumpedVolume([FromQuery] List<string> wellRegistrationID, [FromQuery] string startDate, [FromQuery] string endDate)
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
        public StructuredResultsDto GetPumpedVolume([FromRoute] string wellRegistrationID, [FromQuery] string startDate, [FromQuery] string endDate)
        {
            return GetPumpedVolumeImpl(new List<string> { wellRegistrationID }, startDate, endDate);
        }

        private StructuredResultsDto GetPumpedVolumeImpl(List<string> wellRegistrationIDs, string startDateString, string endDateString)
        {
            // sometimes there can be well registration IDs with different cases e.g: G-032254 vs g-032254
            // lets start by adjusting everything to be uppercase for this string comparison.
            wellRegistrationIDs = wellRegistrationIDs.Select(x => x.ToUpper()).ToList();

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
                var measurementTypeEnum = MeasurementTypeEnum.ContinuityMeter;
                var startDateAsInteger = int.Parse(startDate.ToString("yyyyMMdd"));
                var endDateAsInteger = int.Parse(endDate.ToString("yyyyMMdd"));
                var query = _dbContext.WellSensorMeasurements.AsNoTracking().Where(
                    x => x.MeasurementTypeID == (int)measurementTypeEnum
                         && 10000 * x.ReadingYear + 100 * x.ReadingMonth + x.ReadingDay >=
                         startDateAsInteger
                         && 10000 * x.ReadingYear + 100 * x.ReadingMonth + x.ReadingDay <=
                         endDateAsInteger
                );
                List<WellSensorMeasurementDto> wellSensorMeasurementDtos;
                if (wellRegistrationIDs != null && wellRegistrationIDs.Any())
                {
                    var wellSensorMeasurements = query.Where(x => wellRegistrationIDs.Contains(x.WellRegistrationID.ToUpper())).ToList();
                    wellSensorMeasurementDtos = wellSensorMeasurements.Select(x => x.AsDto()).ToList();
                }
                else
                {
                    wellSensorMeasurementDtos = query.Select(x => x.AsDto()).ToList();
                }

                var wells = _dbContext.AgHubWells.Include(x => x.Well).AsNoTracking().Where(x =>
                        wellRegistrationIDs.Contains(x.Well.WellRegistrationID.ToUpper())).ToList()
                    .ToDictionary(x => x.Well.WellRegistrationID.ToUpper(), x => x.PumpingRateGallonsPerMinute);

                var apiResult = new StructuredResultsDto
                {
                    Status = "success"
                };

                var firstReadingDates = WellSensorMeasurements.GetFirstReadingDateTimesPerSensorForWells(_dbContext, measurementTypeEnum, wellRegistrationIDs);
                apiResult.Result = StructureResults(wellSensorMeasurementDtos, wells, startDate, endDate, firstReadingDates, wellRegistrationIDs);
                return apiResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private static StructuredResults StructureResults(List<WellSensorMeasurementDto> results,
            Dictionary<string, int> wells, DateTime startDate, DateTime endDate,
            List<WellSensorReadingDateDto> firstReadingDates, List<string> wellRegistrationIDs)
        {
            var volumesByWell = new List<VolumeByWell>();
            foreach (var wellRegistrationID in wellRegistrationIDs)
            {
                var currentWellResults = results.Where(x => x.WellRegistrationID.ToUpper() == wellRegistrationID)
                    .OrderBy(x => x.MeasurementDate).ToList();
                var volumeByWell = new VolumeByWell
                {
                    WellRegistrationID = wellRegistrationID
                };
                var pumpingRateGallonsPerMinute = wells.ContainsKey(wellRegistrationID) ? wells[wellRegistrationID] : 0;
                volumeByWell.IntervalVolumes = CreateIntervalVolumesAndZeroFillMissingDays(wellRegistrationID,
                    currentWellResults, startDate, endDate,
                    pumpingRateGallonsPerMinute,
                    firstReadingDates.Where(x => x.WellRegistrationID.ToUpper() == wellRegistrationID).ToList());
                volumesByWell.Add(volumeByWell);
            }

            return new StructuredResults
            {
                IntervalCountTotal = volumesByWell.Sum(x => x.IntervalCount),
                IntervalStart = startDate.ToString("yyyy-MM-dd"),
                IntervalEnd = endDate.ToString("yyyy-MM-dd"),
                WellCount = volumesByWell.Select(x => x.WellRegistrationID).Distinct().Count(),
                VolumesByWell = volumesByWell
            };
        }

        private static List<IntervalVolumeDto> CreateIntervalVolumesAndZeroFillMissingDays(string wellRegistrationID,
            List<WellSensorMeasurementDto> wellSensorMeasurementDtos, DateTime startDate,
            DateTime endDate, int pumpingRateGallonsPerMinute, List<WellSensorReadingDateDto> wellSensorReadingDates)
        {
            var intervalVolumeDtos = new List<IntervalVolumeDto>();
            var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1).ToList();

                foreach (var i in dateRange)
                {
                    var dateTime = startDate.AddDays(i);
                    if (wellSensorReadingDates.Any(x => x.FirstReadingDate <= dateTime))
                    {
                        var intervalVolumeDto = new IntervalVolumeDto(wellRegistrationID, dateTime,
                            wellSensorMeasurementDtos
                                .Where(x => x.MeasurementDate.ToShortDateString() == dateTime.ToShortDateString())
                                .ToList(),
                            MeasurementType.ContinuityMeter.MeasurementTypeDisplayName);
                        var dailySensorVolumeDtos = new List<DailySensorVolumeDto>();
                        foreach (var wellSensorReadingDate in wellSensorReadingDates.OrderBy(x => x.SensorName))
                        {
                            if (wellSensorReadingDate.FirstReadingDate <= dateTime)
                            {
                                var sensorName = wellSensorReadingDate.SensorName;
                                var wellSensorMeasurementDto = wellSensorMeasurementDtos.SingleOrDefault(x =>
                                    x.SensorName.Equals(sensorName, StringComparison.InvariantCultureIgnoreCase) &&
                                    x.MeasurementDate.ToShortDateString() == dateTime.ToShortDateString());
                                var gallons = wellSensorMeasurementDto?.MeasurementValue ?? 0;
                                dailySensorVolumeDtos.Add(new DailySensorVolumeDto(gallons, sensorName,
                                    pumpingRateGallonsPerMinute, wellSensorMeasurementDto?.IsAnomalous ?? false));
                            }
                        }

                        intervalVolumeDto.SensorVolumes = dailySensorVolumeDtos;
                        intervalVolumeDtos.Add(intervalVolumeDto);
                    }
                }

            return intervalVolumeDtos;
        }

        /// <summary>
        /// Migrates Influx records associated with the specified Well Pressure sensor from the first specified well to the second.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/sensors/PW010834/migrateWaterLevelReadings/G-043540/G-193253
        /// </remarks>
        /// <param name="sensorName">The device number for the sensor readings to be migrated.</param>
        /// <param name="fromWellRegistrationID">The registration ID for the well which the specified sensor readings should be migrated from.</param>
        /// <param name="toWellRegistrationID">The registration ID for the well which the specified sensor readings should be migrated to.</param>
        /// <param name="startDate">The start date for the readings to be migrated in yyyy-MM-dd format (eg. 2022-06-23). If no date is provided, 2018-01-01 will be used by default.</param>
        /// <param name="endDate">The end date for the readings to be migrated in yyyy-MM-dd format (eg. 2022-07-01). If no date is provided, the current date will be used by default.</param>
        /// <param name="bucket">The bucket to be used for the Influx operations. If no bucket is provided, the main bucket will be used by default.</param>
        /// 
        /// <returns>A count of the successfully migrated Influx records.</returns>
        /// 
        [HttpPut("/api/sensors/{sensorName}/migrateWaterLevelReadings/{fromWellRegistrationID}/{toWellRegistrationID}")]
        public async Task<ActionResult<int>> MigrateSensorReadings([FromRoute] string sensorName, [FromRoute] string fromWellRegistrationID, 
            [FromRoute] string toWellRegistrationID, [FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string bucket)
        {
            var sensor = Sensors.GetBySensorName(_dbContext, sensorName);
            if (sensor == null)
            {
                ModelState.AddModelError("sensorName", $"No sensor matching the sensor name {sensorName} was found in the Groundwater Managers Platform database.");
            }

            var fromWell = Wells.GetByWellRegistrationID(_dbContext, fromWellRegistrationID);
            if (fromWell == null)
            {
                ModelState.AddModelError("fromWellRegistrationID", $"No well matching the well registration ID {fromWellRegistrationID} was found in the Groundwater Managers Platform database.");
            }

            var toWell = Wells.GetByWellRegistrationID(_dbContext, toWellRegistrationID);
            if (toWell == null)
            {
                ModelState.AddModelError("toWellRegistrationID", $"No well matching the well registration ID {toWellRegistrationID} was found in the Groundwater Managers Platform database.");
            }

            if (!ModelState.IsValid)
            {
                return NotFound(ModelState);
            }

            if (sensor.SensorTypeID != (int)SensorTypeEnum.WellPressure)
            {
                ModelState.AddModelError("sensorName", $"The specified sensor is a(n) {sensor.SensorType.SensorTypeDisplayName} sensor. A Well Pressure sensor is expected.");
            }

            DateTime queryStartDate;
            if (string.IsNullOrWhiteSpace(startDate))
            {
                queryStartDate = new DateTime(2018, 01, 01);
            }
            else if (!DateTime.TryParse(startDate, out queryStartDate))
            {
                ModelState.AddModelError("startDate", "Start date is not a valid Date string in ISO 8601 format. Please enter a valid date string ");
            }

            DateTime queryEndDate;
            if (string.IsNullOrWhiteSpace(endDate))
            {
                queryEndDate = DateTime.Today;
            }
            else if (!DateTime.TryParse(endDate, out queryEndDate))
            {
                ModelState.AddModelError("endDate", "End date is not a valid Date string in ISO 8601 format. Please enter a valid date string ");
            }

            if (queryStartDate > queryEndDate)
            {
                ModelState.AddModelError("startDate", "Start Date occurs after End Date. Please ensure that Start Date occurs before End Date.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int insertedRecordsCount;

            try
            {
                bucket ??= _zybachConfiguration.INFLUX_BUCKET;

                var wellPressureReadings = await _influxDbService.GetWellPressureReadingsByRegistrationIDAndSensorName(fromWellRegistrationID, sensorName, queryStartDate, queryEndDate, bucket);
                wellPressureReadings.ForEach(x => x.RegistrationID = toWellRegistrationID);

                await _influxDbService.WriteMeasurementReadingsToInfluxDb(wellPressureReadings, bucket);
                await _influxDbService.DeletePressureSensorReadingsByRegistrationIDAndSensorName(fromWellRegistrationID, sensorName, queryStartDate, queryEndDate, bucket);

                insertedRecordsCount = wellPressureReadings.Count;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Ok($"{insertedRecordsCount} Influx records found & migrated to new well");
        }
        
        [HttpPost("api/sensors/{sensorName}/pulse")]
        public ActionResult CreatePaigeWirelessPulse([FromRoute] string sensorName, [FromBody] string eventMessage)
        {
            PaigeWirelessPulses.Create(_dbContext, sensorName, eventMessage);
            return Ok();
        }
    }
}