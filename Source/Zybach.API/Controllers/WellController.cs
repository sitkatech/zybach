using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class WellController : SitkaController<WellController>
    {
        private readonly InfluxDBService _influxDbService;
        private readonly GeoOptixService _geoOptixService;

        public WellController(ZybachDbContext dbContext, ILogger<WellController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, InfluxDBService influxDbService, GeoOptixService geoOptixService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _influxDbService = influxDbService;
            _geoOptixService = geoOptixService;
        }


        /**
        * Returns an array of all Wells in the Water Data Program registered in GeoOptix
        */
        [HttpGet("/api/wells")]
        // todo: get apikey security
        // @Security(SecurityType.API_KEY)
        public async Task<object> GetWells()
        {
            // get all wells that either existed in GeoOptix as of the given year or that had electrical estimates as of the given year
            var geoOptixWells = await _geoOptixService.GetWellSummaries();

            return new
            { 
                Status = "success",
                Result = geoOptixWells
            };
        }

        /**
         * Returns a time series representing pumped volume at a well or series of wells, summed on a chosen reporting interval,
         * for a given date range. Each point in the output time series represents the total pumped volume over the previous
         * reporting interval.
         * @param wellRegistrationIDs The Well Registration ID(s) for the requested Well(s). If left blank, will bring back data
         * for every Well that has reported data within the time range.
         * @param startDateString The start date for the report, formatted as an ISO date string (eg. 2020-06-23). If a specific
         * time is requested, must contain a timezone (eg. 2020-06-23T17:24:56+00:00)
         * @param endDateString The end date for the report, formatted as an ISO date string (eg. 2020-06-23). If a specific
         * time is requested, must contain a timezone (eg. 2020-06-23T17:24:56+00:00). Default's to today's date.
         * @param interval The reporting interval, in minutes. Defaults to 60.
         */
        [HttpGet("/api/wells/pumpedVolume")]
        // todo: get apikey security
        // @Security(SecurityType.API_KEY)
        public async Task<object> GetPumpedVolume([FromQuery] string startDate, [FromQuery] List<string> filter, [FromQuery] string endDate, [FromQuery] int? interval)
        {
            return await GetPumpedVolumeImpl(startDate, filter, endDate, interval);
        }

        [HttpGet("/api/wells/download/robustReviewScenarioJson")]
        // todo: get apikey security
        // @Security(SecurityType.API_KEY)
        public async Task<List<RobustReviewDto>> GetRobustReviewJsonFile()
        {
            var wells = await _geoOptixService.GetWellsWithSensors();
            var aghubWells = AgHubWell.GetAgHubWellsAsWellWithSensorSummaryDtos(_dbContext);

            aghubWells.ForEach(x =>
            {
                if (!wells.ContainsKey(x.WellRegistrationID))
                {
                    wells.Add(x.WellRegistrationID, x);
                    return;

                }

                var geoOptixWell = wells[x.WellRegistrationID];
                geoOptixWell.HasElectricalData = x.HasElectricalData;
                geoOptixWell.WellTPID = x.WellTPID;
                var sensors = new List<SensorSummaryDto>();
                sensors.AddRange(x.Sensors);
                sensors.AddRange(geoOptixWell.Sensors);
                geoOptixWell.Sensors = sensors;
            });

            const string continuityMeterString = "Continuity Meter";

            var wellWithSensorSummaryDtos = wells.Values
                .Where(x => (x.HasElectricalData ?? false) || x.Sensors.Any(y => y.SensorType == continuityMeterString)).ToList();
            var robustReviewDtos = new List<RobustReviewDto>();
            foreach (var wellWithSensorSummaryDto in wellWithSensorSummaryDtos)
            {
                robustReviewDtos.Add(await CreateRobustReviewDto(wellWithSensorSummaryDto, continuityMeterString));
            }

            return robustReviewDtos.Where(x => x != null).ToList();
//        if (robustReviewStructure) {
//            req.res?.writeHead(200, {
//            'Content-Type': 'application/octet-stream',
//                "Content-Disposition": "attachment; filename=\"RobustReviewScenario.json\""
//            });
//            //We'll have null objects if they don't have a first reading date, and I'd rather 
//            //do a second filter here than include the firstReadingDate in above filter and then have to get it a second time
//            req.res?.end(JSON.stringify(robustReviewStructure.filter(x => x != null)));
//        } else {
//            req.res?.status(204).end();
//}
        }

        private async Task<RobustReviewDto> CreateRobustReviewDto(WellWithSensorSummaryDto x, string continuityMeterString)
        {
            var firstReadingDate =
                await _influxDbService.GetFirstReadingDateTimeForWell(x.WellRegistrationID);

            if (firstReadingDate == null)
            {
                return null;
            }

            List<MonthlyPumpedVolume> monthlyPumpedVolume;
            string dataSource;
            if (x.HasElectricalData ?? false)
            {
                monthlyPumpedVolume =
                    await _influxDbService.GetMonthlyElectricalBasedFlowEstimate(x.WellRegistrationID,
                        firstReadingDate.Value);
                dataSource = "Electrical Usage";
            }
            else
            {
                monthlyPumpedVolume = await _influxDbService.GetMonthlyPumpedVolumesForSensors(
                    x.Sensors.Where(y => y.SensorType == continuityMeterString).Select(y => y.SensorName)
                        .ToList(), x.WellRegistrationID, firstReadingDate.Value);
                dataSource = continuityMeterString;
            }

            var point = (Point) x.Location.Geometry;
            var robustReviewDto = new RobustReviewDto
            {
                WellRegistrationID = x.WellRegistrationID,
                WellTPID = x.WellTPID,
                // TODO: lookup feature to point to lat long
                Latitude = point.Coordinates.Latitude,
                Longitude = point.Coordinates.Longitude,
                DataSource = dataSource,
                MonthlyPumpedVolumeGallons = monthlyPumpedVolume
            };

            return robustReviewDto;
        }

        [HttpGet("/api/wells/download/robustReviewScenarioJson")]
        // todo: get apikey security
        // @Security(SecurityType.API_KEY)
        public async Task<object> GetWell([FromRoute] string wellRegistrationID)
        {
            var well = await _geoOptixService.GetWellAsAbbreviatedWellDataResponse(wellRegistrationID);
            return new
            {
                Status = "success",
                Result = well
            };
        }

        [HttpGet("/api/wells/{wellRegistrationID}/details")]
        [AdminFeature]
        public async Task<WellDetailDto> GetWellDetails([FromRoute] string wellRegistrationID)
        {
            var geooptixWell = await _geoOptixService.GetWellSummary(wellRegistrationID);
            var agHubWell = AgHubWell.FindByWellRegistrationIDAsWellWithSensorSummaryDto(_dbContext, wellRegistrationID);

            //if (geooptixWell == null && agHubWell == null)
            //{
            //    return NotFound($"Well with {wellRegistrationID} not found!");
            //}
            var hasElectricalData = agHubWell != null && (agHubWell.HasElectricalData ?? false);

            var well = new WellDetailDto();
//                (geooptixWell || agHubWell) as WellDetailDto;

            var firstReadingDate = await _influxDbService.GetFirstReadingDateTimeForWell(wellRegistrationID);
            if (firstReadingDate != null)
            {
                var readingDate = firstReadingDate.Value;
                firstReadingDate = new DateTime(readingDate.Year, readingDate.Month, readingDate.Day);
            }
            var lastReadingDate = await _influxDbService.GetLastReadingDateTimeForWell(wellRegistrationID);

            if (geooptixWell != null)
            {
                well.InGeoOptix = true;
                if (agHubWell != null)
                {
                    well.WellTPID = agHubWell.WellTPID;
                    well.Location = agHubWell.Location;
                    well.IrrigatedAcresPerYear = agHubWell.IrrigatedAcresPerYear;
                }
            }
            else
            {
                well.InGeoOptix = false;
            }

            well.HasElectricalData = hasElectricalData;
            well.FirstReadingDate = firstReadingDate;
            well.LastReadingDate = lastReadingDate;


            var sensors = await _geoOptixService.GetSensorSummariesForWell(wellRegistrationID);
            well.Sensors = sensors;

            var annualPumpedVolume = new List<AnnualPumpedVolume>();

            annualPumpedVolume.AddRange(await GetAnnualPumpedVolumeForWellAndSensorType(sensors, "Flow Meter"));
            annualPumpedVolume.AddRange(await GetAnnualPumpedVolumeForWellAndSensorType(sensors, "Continuity Meter"));

            if (hasElectricalData)
            {
                var annualEstimatedPumpedVolumeForWell = await _influxDbService.GetAnnualEstimatedPumpedVolumeForWell(wellRegistrationID);
                annualPumpedVolume.AddRange(annualEstimatedPumpedVolumeForWell);
            }
            well.AnnualPumpedVolume = annualPumpedVolume;
            return well;
        }

        [HttpGet("/api/wells/{wellRegistrationID}/installation")]
        [AdminFeature]
        public async Task<List<InstallationRecordDto>> GetInstallationRecordForWell([FromRoute] string wellRegistrationID)
        {
            return await _geoOptixService.GetInstallationRecords(wellRegistrationID);
        }

        [HttpGet("/api/wells/{wellRegistrationID}/installation/{installationCanonicalName}/photo/{photoCanonicalName}")]
        [AdminFeature]
        public async Task<IActionResult> GetPhoto([FromRoute] string wellRegistrationID, [FromRoute] string installationCanonicalName, [FromRoute] string photoCanonicalName)
        {
            var photoBuffer = await _geoOptixService.GetPhoto(wellRegistrationID, installationCanonicalName, photoCanonicalName);
            return File(photoBuffer, "image/jpeg");
        }

        private async Task<List<AnnualPumpedVolume>> GetAnnualPumpedVolumeForWellAndSensorType(List<SensorSummaryDto> sensors, string sensorType)
        {
            var sensorTypeSensors = sensors.Where(x => x.SensorType == sensorType).ToList();

            if (!sensorTypeSensors.Any())
            {
                return new List<AnnualPumpedVolume>();
            }

            return await _influxDbService.GetAnnualPumpedVolumesForSensor(sensorTypeSensors.Select(x => x.SensorName).ToList(), sensorType);
        }

        private async Task<object> GetPumpedVolumeImpl(string startDateString, List<string> wellRegistrationIDs,
            string endDateString, int? interval)
        {
            if (string.IsNullOrWhiteSpace(endDateString))
            {
                endDateString = DateTime.Today.ToShortDateString();
            }

            var intervalValue = interval ?? 60;

            //TODO: Validate startDateString and endDateString are valid date strings in ISO 8601 format
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

            if (intervalValue == 0 || intervalValue % 15 != 0)
            {
                throw new ArgumentOutOfRangeException("interval",
                    "Interval must be a number evenly divisible by 15 and greater than 0. Please enter a new interval.");
            }

            if (startDate > endDate)
            {
                throw new ArgumentOutOfRangeException("startDate",
                    "Start date occurs after End date. Please ensure that Start Date occurs before End date");
            }

            try
            {
                var results =
                    await this._influxDbService.GetFlowMeterSeries(wellRegistrationIDs, startDate, endDate,
                        intervalValue);
                return new
                {
                    Status = "success",
                    Result = results.Any() ? StructureResults(results, intervalValue) : null
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static StructuredResults StructureResults(List<InfluxDBService.ResultFromInfluxDB> results, int interval)
        {
            var distinctWells = results.Select(x => x.WellRegistrationID).Distinct().ToList();
            var startDate = results.First().EndTime;
            var endDate = results.Last().EndTime;
            var totalResults = 0;
            var volumesByWell = new List<VolumeByWell>();
            foreach (var wellRegistrationID in distinctWells)
            {
                var currentWellResults = results.Where(x => x.WellRegistrationID == wellRegistrationID).OrderBy(x => x.EndTime).ToList();

                if (currentWellResults.First().EndTime < startDate)
                {
                    startDate = currentWellResults.First().EndTime;
                }

                var aggregatedResults = currentWellResults;

                if (aggregatedResults.Last().EndTime > endDate)
                {
                    endDate = aggregatedResults.Last().EndTime;
                }

                totalResults += aggregatedResults.Count;

                var volumeByWell = new VolumeByWell
                {
                    WellRegistrationID = wellRegistrationID,
                    IntervalCount = aggregatedResults.Count,
                    IntervalVolumes = aggregatedResults
                };
                volumesByWell.Add(volumeByWell);
            }

            //Because we get the intervals back in 15 minute increments, technically our startDate is 15 minutes BEFORE our actual first time
            //Remove this extra piece if we decide we just want the first interval's end date
            startDate = startDate.AddMinutes(-15);
            return new StructuredResults()
            {
                IntervalCountTotal = totalResults,
                IntervalWidthInMinutes = interval,
                IntervalStart = startDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                IntervalEnd = endDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                DurationInMinutes = (endDate - startDate).TotalMinutes,
                WellCount = distinctWells.Count,
                VolumesByWell = volumesByWell
            };
        }

    }

    public class RobustReviewDto
    {
        public string WellRegistrationID { get; set; }
        public string WellTPID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DataSource { get; set; }
        public List<MonthlyPumpedVolume> MonthlyPumpedVolumeGallons { get; set; }
    }

    public class StructuredResults
    {
        public int IntervalCountTotal { get; set; }
        public int IntervalWidthInMinutes { get; set; }
        public string IntervalStart { get; set; }
        public string IntervalEnd { get; set; }
        public double DurationInMinutes { get; set; }
        public int WellCount { get; set; }
        public List<VolumeByWell> VolumesByWell { get; set; }
    }

    public class VolumeByWell
    {
        public string WellRegistrationID { get; set; }
        public int IntervalCount { get; set; }
        public List<InfluxDBService.ResultFromInfluxDB> IntervalVolumes { get; set; }
    }
}