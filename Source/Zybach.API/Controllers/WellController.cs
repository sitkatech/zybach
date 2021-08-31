using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly GeoOptixService _geoOptixService;

        public WellController(ZybachDbContext dbContext, ILogger<WellController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, GeoOptixService geoOptixService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _geoOptixService = geoOptixService;
        }


        /**
        * Returns an array of all Wells in the Water Data Program registered in GeoOptix
        */
        [HttpGet("/api/wells")]
        [ApiExplorerSettings(IgnoreApi = true)]
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
        public ApiResult<StructuredResults> GetPumpedVolume([FromQuery] string startDate, [FromQuery] List<string> filter, [FromQuery] string endDate, [FromQuery] int? interval)
        {
            return GetPumpedVolumeImpl(filter, startDate, endDate);
        }


        [HttpGet("/api/wells/download/robustReviewScenarioJson")]
        [ApiExplorerSettings(IgnoreApi = true)]
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

            var wellWithSensorSummaryDtos = wells.Values
                .Where(x => (x.HasElectricalData ?? false) || x.Sensors.Any(y => y.SensorType == InfluxDBService.SensorTypes.ContinuityMeter))
                .ToList();
            var firstReadingDateTimes = WellSensorMeasurement.GetFirstReadingDateTimes(_dbContext);
            var robustReviewDtos = wellWithSensorSummaryDtos.Select(wellWithSensorSummaryDto => CreateRobustReviewDto(wellWithSensorSummaryDto, firstReadingDateTimes)).ToList();
            return robustReviewDtos.Where(x => x != null).ToList();
        }

        private RobustReviewDto CreateRobustReviewDto(WellWithSensorSummaryDto wellWithSensorSummaryDto,
            Dictionary<string, DateTime> firstReadingDateTimes)
        {
            try
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
                    wellSensorMeasurementDtos = WellSensorMeasurement.GetWellSensorMeasurementsForWellByMeasurementType(_dbContext,
                        wellRegistrationID, MeasurementTypeEnum.ElectricalUsage);
                    dataSource = InfluxDBService.SensorTypes.ElectricalUsage;
                }
                else
                {
                    const string continuityMeter = InfluxDBService.SensorTypes.ContinuityMeter;
                    wellSensorMeasurementDtos = WellSensorMeasurement.GetWellSensorMeasurementsForWellAndSensorsByMeasurementType(_dbContext,
                        wellRegistrationID, new List<MeasurementTypeEnum>{ MeasurementTypeEnum.ContinuityMeter, MeasurementTypeEnum.FlowMeter },
                        wellWithSensorSummaryDto.Sensors.Where(y => y.SensorType == continuityMeter));
                    dataSource = continuityMeter;
                }

                var monthlyPumpedVolume = wellSensorMeasurementDtos.GroupBy(x => x.MeasurementDate.ToString("yyyyMM"))
                    .Select(x =>
                        new MonthlyPumpedVolume(x.First().ReadingYear, x.First().ReadingMonth,
                            x.Sum(y => y.MeasurementValue))).ToList();

                var point = (Point) wellWithSensorSummaryDto.Location.Geometry;
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
            catch (Exception e)
            {
                var i = 9;
                throw;
            }
        }

        //[HttpGet("/api/wells/download/robustReviewScenarioJson")]
        //// todo: get apikey security
        //// @Security(SecurityType.API_KEY)
        //public async Task<object> GetWell([FromRoute] string wellRegistrationID)
        //{
        //    var well = await _geoOptixService.GetWellAsAbbreviatedWellDataResponse(wellRegistrationID);
        //    return new
        //    {
        //        Status = "success",
        //        Result = well
        //    };
        //}

        [HttpGet("/api/wells/{wellRegistrationID}/pumpedVolume")]
        public object GetPumpedVolume([FromQuery] string startDate, [FromRoute] string wellRegistrationID, [FromQuery] string endDate)
        {
            return GetPumpedVolumeImpl(new List<string>{wellRegistrationID}, startDate, endDate);
        }


        [HttpGet("/api/wells/{wellRegistrationID}/details")]
        public async Task<WellDetailDto> GetWellDetails([FromRoute] string wellRegistrationID)
        {
            var geooptixWell = await _geoOptixService.GetWellSummary(wellRegistrationID);
            var agHubWell = AgHubWell.FindByWellRegistrationIDAsWellWithSensorSummaryDto(_dbContext, wellRegistrationID);

            if (geooptixWell == null && agHubWell == null)
            {
                throw new Exception($"Well with {wellRegistrationID} not found!");
            }
            var hasElectricalData = agHubWell != null && (agHubWell.HasElectricalData ?? false);

            var well = new WellDetailDto
            {
                WellRegistrationID = agHubWell?.WellRegistrationID ?? geooptixWell?.WellRegistrationID,
                WellTPID = agHubWell?.WellTPID,
                IrrigatedAcresPerYear = agHubWell?.IrrigatedAcresPerYear,
                Location = agHubWell?.Location ?? geooptixWell?.Location
            };

            var firstReadingDate = WellSensorMeasurement.GetFirstReadingDateTimeForWell(_dbContext, wellRegistrationID);
            var lastReadingDate = WellSensorMeasurement.GetLastReadingDateTimeForWell(_dbContext, wellRegistrationID);

            well.InGeoOptix = geooptixWell != null;
            well.HasElectricalData = hasElectricalData;
            well.FirstReadingDate = firstReadingDate;
            well.LastReadingDate = lastReadingDate;


            var sensors = await _geoOptixService.GetSensorSummariesForWell(wellRegistrationID);
            well.Sensors = sensors;

            var annualPumpedVolumes = new List<AnnualPumpedVolume>();

            annualPumpedVolumes.AddRange(GetAnnualPumpedVolumeForWellAndSensorType(wellRegistrationID, sensors, InfluxDBService.SensorTypes.FlowMeter, MeasurementTypeEnum.FlowMeter));
            annualPumpedVolumes.AddRange(GetAnnualPumpedVolumeForWellAndSensorType(wellRegistrationID, sensors, InfluxDBService.SensorTypes.ContinuityMeter, MeasurementTypeEnum.ContinuityMeter));

            if (hasElectricalData)
            {
                var wellSensorMeasurementDtos = WellSensorMeasurement.GetWellSensorMeasurementsForWellByMeasurementType(_dbContext, wellRegistrationID, MeasurementTypeEnum.ElectricalUsage);
                var pumpedVolumes = wellSensorMeasurementDtos.GroupBy(x => x.ReadingYear)
                    .Select(x => new AnnualPumpedVolume(x.Key, x.Sum(y => y.MeasurementValue),
                        InfluxDBService.SensorTypes.ElectricalUsage)).ToList();

                annualPumpedVolumes.AddRange(pumpedVolumes);
            }
            well.AnnualPumpedVolume = annualPumpedVolumes;
            return well;
        }

        [HttpGet("/api/wells/{wellRegistrationID}/installation")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ZybachViewFeature]
        public async Task<List<InstallationRecordDto>> GetInstallationRecordForWell([FromRoute] string wellRegistrationID)
        {
            return await _geoOptixService.GetInstallationRecords(wellRegistrationID);
        }

        [HttpGet("/api/wells/{wellRegistrationID}/installation/{installationCanonicalName}/photo/{photoCanonicalName}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ZybachViewFeature]
        public async Task<IActionResult> GetPhoto([FromRoute] string wellRegistrationID, [FromRoute] string installationCanonicalName, [FromRoute] string photoCanonicalName)
        {
            try
            {
                var photoBuffer = await _geoOptixService.GetPhoto(wellRegistrationID, installationCanonicalName,
                    photoCanonicalName);
                return File(photoBuffer, "image/jpeg");
            }
            catch
            {
                return NoContent();
            }
        }

        private List<AnnualPumpedVolume> GetAnnualPumpedVolumeForWellAndSensorType(string wellRegistrationID, List<SensorSummaryDto> sensors, string sensorType, MeasurementTypeEnum measurementTypeEnum)
        {
            var sensorTypeSensors = sensors.Where(x => x.SensorType == sensorType).ToList();

            if (!sensorTypeSensors.Any())
            {
                return new List<AnnualPumpedVolume>();
            }

            var wellSensorMeasurementDtos = WellSensorMeasurement.GetWellSensorMeasurementsForWellAndSensorsByMeasurementType(_dbContext, wellRegistrationID, measurementTypeEnum, sensorTypeSensors);

            var annualPumpedVolumes = wellSensorMeasurementDtos.GroupBy(x => x.ReadingYear)
                .Select(x => new AnnualPumpedVolume(x.Key,x.Sum(y => y.MeasurementValue), sensorType)).ToList();

            return annualPumpedVolumes;
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
                var results = _dbContext.WellSensorMeasurements.Include(x => x.MeasurementType).AsNoTracking().Where(
                        x =>
                            wellRegistrationIDs.Contains(x.WellRegistrationID)
                            && x.MeasurementTypeID == (int) MeasurementTypeEnum.ContinuityMeter
                            && x.ReadingYear >= startDate.Year && x.ReadingMonth >= startDate.Month &&
                            x.ReadingDay >= startDate.Day
                            && x.ReadingYear <= endDate.Year && x.ReadingMonth <= endDate.Month &&
                            x.ReadingDay <= endDate.Day
                    ).OrderBy(x => x.ReadingYear).ThenBy(x => x.ReadingMonth).ThenBy(x => x.ReadingDay)
                    .Select(x => x.AsDto()).ToList();

                var aghubWells = _dbContext.AgHubWells.AsNoTracking().Where(x =>
                        wellRegistrationIDs.Contains(x.WellRegistrationID)).ToList()
                    .ToDictionary(x => x.WellRegistrationID, x => x.PumpingRateGallonsPerMinute);

                return new ApiResult<StructuredResults>
                {
                    Status = "success",
                    Result = results.Any() ? StructureResults(results, aghubWells, startDate, endDate) : null
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static StructuredResults StructureResults(List<WellSensorMeasurementDto> results,
            Dictionary<string, int> aghubWells, DateTime startDate, DateTime endDate)
        {
            var distinctWells = results.Select(x => x.WellRegistrationID).Distinct().ToList();
            var volumesByWell = new List<VolumeByWell>();
            foreach (var wellRegistrationID in distinctWells)
            {
                var currentWellResults = results.Where(x => x.WellRegistrationID == wellRegistrationID).ToList();
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

    public class ApiResult<T>
    {
        public string Status { get; set; }

        public T Result { get; set; }
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
        public string IntervalStart { get; set; }
        public string IntervalEnd { get; set; }
        public int WellCount { get; set; }
        public List<VolumeByWell> VolumesByWell { get; set; }
    }

    public class VolumeByWell
    {
        public string WellRegistrationID { get; set; }
        public int IntervalCount => IntervalVolumes?.Count ?? 0;
        public List<IntervalVolumeDto> IntervalVolumes { get; set; }
    }

    public class IntervalVolumeDto
    {
        public IntervalVolumeDto()
        {
        }

        public IntervalVolumeDto(WellSensorMeasurementDto wellSensorMeasurementDto, int pumpingRateGallonsPerMinute)
        {
            WellRegistrationID = wellSensorMeasurementDto.WellRegistrationID;
            MeasurementDate = wellSensorMeasurementDto.MeasurementDate.ToString("yyyy-MM-dd");
            MeasurementType = wellSensorMeasurementDto.MeasurementType.MeasurementTypeDisplayName;
            SensorName = wellSensorMeasurementDto.SensorName;
            MeasurementValueGallons = Convert.ToInt32(Math.Round(wellSensorMeasurementDto.MeasurementValue, 0));
            PumpingRateGallonsPerMinute = pumpingRateGallonsPerMinute;
        }

        public IntervalVolumeDto(string wellRegistrationID, DateTime measurementDate, double measurementValue, string measurementType, string sensorName, int pumpingRateGallonsPerMinute)
        {
            WellRegistrationID = wellRegistrationID;
            MeasurementDate = measurementDate.ToString("yyyy-MM-dd");
            MeasurementType = measurementType;
            SensorName = sensorName;
            MeasurementValueGallons = Convert.ToInt32(Math.Round(measurementValue, 0));
            PumpingRateGallonsPerMinute = pumpingRateGallonsPerMinute;
        }

        public string WellRegistrationID { get; set; }
        public string MeasurementDate { get; set; }
        public string MeasurementType { get; set; }
        public string SensorName { get; set; }
        public int MeasurementValueGallons { get; set; }
        public int PumpingRateGallonsPerMinute { get; set; }
    }
}