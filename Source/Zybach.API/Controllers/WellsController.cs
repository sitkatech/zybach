using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
namespace Zybach.API.Controllers
{
    [ApiController]
    public class WellsController : ControllerBase
    {
        private readonly ZybachDbContext _dbContext;
        private readonly ILogger<UserController> _logger;
        private readonly ZybachConfiguration _zybachConfiguration;
        private GeoOptixService _geoOptixService;

        public WellsController(ZybachDbContext dbContext, ILogger<UserController> logger, IOptions<ZybachConfiguration> zybachConfigurationOptions)
        {
            _dbContext = dbContext;
            _logger = logger;
            _zybachConfiguration = zybachConfigurationOptions.Value;
            _geoOptixService = new GeoOptixService(_zybachConfiguration, _dbContext);
        }

        /// <summary>
        /// Returns a time series representing pumped volume at a well, averaged on a chosen reporting interval, for a given date range.
        /// Each point in the output time series represents the average pumped volume over the previous reporting interval.
        /// In order for this call to be successful, the Well in question must have a Flow Meter sensor installed.
        /// </summary>
        /// <param name="wellRegistrationID">The Well Registration ID for the requested Well.</param>
        /// <param name="reportingIntervalMinutes">The reporting interval, in minutes</param>
        /// <param name="startDateISO">The start date for the report, formatted as an ISO date string with a timezone (eg. 2020-06-23T17:24:56+00:00)</param>
        /// <param name="endDateISO">The end date for the report, formatted as an ISO date string with a timezone (eg. 2020-06-23T17:24:56+00:00)</param>
        /// <returns>A time series representing the average pumped volume for the given date range.</returns>
        /// <response code="200">Returns the requested time series</response>
        /// <response code="404">Will return this in a number of cases: if the Well is not found, if the Well has no Flow Sensors, if the Flow Sensor has no stored data available, if the TimeSeries that is returned does not have dates that fall within the given date range.</response>
        /// <response code="400">If the inputs are improperly-formatted or the date range or reporting interval are invalid. Error message will describe the invalid parameter(s)</response>
        [HttpGet("/wells/{wellRegistrationID}/pumpedVolume")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [UserViewFeature]
        public async Task<ActionResult<PumpedVolumeDto>> PumpedVolume([FromRoute] string wellRegistrationID,
            [FromQuery] int reportingIntervalMinutes, [FromQuery] string startDateISO, [FromQuery] string endDateISO)
        {
            var startDate = new DateTime();
            var endDate = new DateTime();

            if (!DateTime.TryParseExact(startDateISO, "yyyy-MM-ddTHH:mm:sszzz",CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out startDate))
            {
                var message = "Start Date formatted incorrectly.";
                return BadRequest(message);
            }

            if (!DateTime.TryParseExact(endDateISO, "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out endDate))
            {
                var message = "End Date formatted incorrectly.";
                return BadRequest(message);
            }

            var wellResponse = await _geoOptixService.GetWell(wellRegistrationID);

            if (!wellResponse.IsSuccessStatusCode)
            {
                var message = "An error has occurred";
                return StatusCode((int)wellResponse.StatusCode, message);
            }

            //We know we didn't find a well if this comes back empty
            if ((int)wellResponse.StatusCode == 204)
            {
                var message = "Well not found.";
                return NotFound(message);
            }

            var sensorsResponse = await _geoOptixService.GetSensorsForWell(wellRegistrationID);

            if (!sensorsResponse.IsSuccessStatusCode)
            {
                var message = "An error has occurred";
                return StatusCode((int)sensorsResponse.StatusCode, message);
            }

            var sensorsContents = JsonConvert.DeserializeObject<List<SensorDto>>(await sensorsResponse.Content.ReadAsStringAsync());
            var flowMeterSensor = sensorsContents.FirstOrDefault(x => x.SensorDefinitionDto.SensorType == "FlowMeter");
            if (flowMeterSensor == null)
            {
                var message = "Well has no Flow Sensors on record.";
                return NotFound(message);
            }

            var sensorFoldersResponse = await _geoOptixService.GetSensorFolders(flowMeterSensor.CanonicalName, wellRegistrationID);
            if (!sensorFoldersResponse.IsSuccessStatusCode)
            {
                var message = "An error has occurred";
                return StatusCode((int)sensorFoldersResponse.StatusCode, message);
            }

            var sensorFoldersContents =
                JsonConvert.DeserializeObject<List<FolderDto>>(await sensorFoldersResponse.Content.ReadAsStringAsync());

            if (sensorFoldersContents.Count == 0)
            {
                var message = "Flow Sensor has no stored files on record.";
                return NotFound(message);
            }

            var timeSeriesDataResponse = await _geoOptixService.GetTimeSeriesData(
                sensorFoldersContents[0].CanonicalName, flowMeterSensor.CanonicalName, wellRegistrationID);

            if (!timeSeriesDataResponse.IsSuccessStatusCode)
            {
                var message = "An error has occurred";
                return StatusCode((int)timeSeriesDataResponse.StatusCode, message);
            }
            var timeSeriesDataContentsAsString = await timeSeriesDataResponse.Content.ReadAsStringAsync();
            var timeSeriesDataContents = JsonConvert.DeserializeObject<List<FlowMeterTimePointDto>>($"[{timeSeriesDataContentsAsString.TrimEnd(',')}]");

            var jsonInDates = timeSeriesDataContents.Where(x => x.ReadingTime >= startDate && x.ReadingTime <= endDate).ToList();

            if (jsonInDates.Count == 0)
            {
                var message = "Time series data does not contain values within the date range specified.";
                return NotFound(message);
            }

            var count = 0;
            var sum = (decimal)0;
            var startTime = jsonInDates[0].ReadingTime;
            var pumpedVolumeTimePointDtos = new List<PumpedVolumeTimePoint>();
            jsonInDates.ForEach(x =>
            {
                if (x.ReadingTime.Subtract(startTime).TotalMinutes > reportingIntervalMinutes)
                {
                    var pumpedVolumeTimePointDto = new PumpedVolumeTimePoint()
                    {
                        StartTime = startTime,
                        PumpedVolumeGallons = sum / count
                    };
                    pumpedVolumeTimePointDtos.Add(pumpedVolumeTimePointDto);
                    count = 0;
                    sum = 0;
                    startTime = x.ReadingTime;
                }
                count++;
                sum += x.Gallons;
            });

            return Ok(new PumpedVolumeDto
            {
                ReportingIntervalMinutes = reportingIntervalMinutes,
                PumpedVolumeTimeSeries = pumpedVolumeTimePointDtos
            });
        }
        /// <summary>
        /// Returns a dictionary containing key summary statistics for a given well
        /// </summary>
        /// <param name="wellRegistrationID">The Well Registration ID for the requested Well</param>
        /// <returns>A dictionary containing key summary statistics for a given well</returns>
        /// <response code="200">Returns the requested summary statistics</response>
        /// <response code="404">If the requested Well was not found</response>
        [HttpGet("/wells/{wellRegistrationID}/summaryStatistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [UserViewFeature]
        public async Task<ActionResult<WellSummaryStatisticsDto>> SummaryStatistics([FromRoute] string wellRegistrationID)
        {
            //todo implement endpoint
            //As of 9/23/20 the statistics we'd like to use are not yet available.

            return Ok(new WellSummaryStatisticsDto());
        }

        /// <summary>
        /// Returns a json object with basic information about every well in system
        /// </summary>
        /// <returns>Returns a json object with basic information about every well in system</returns>
        /// <response code="200">Returns the list of wells</response>
        /// <response code="401">If the authentication fails</response>
        /// <response code="403">If Authorization level is insufficient</response>
        [HttpGet("/wells")]
        [UserViewFeature]
        public async Task<ActionResult> GetWells()
        {
            var response = await _geoOptixService.GetWells();
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var contents = await response.Content.ReadAsStringAsync();
            var siteDtos = JsonConvert.DeserializeObject(contents);
            return Ok(siteDtos);
        }
    }
}
