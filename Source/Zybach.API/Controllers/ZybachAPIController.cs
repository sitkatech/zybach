using System;
using System.Collections.Generic;
using System.Linq;
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
        public ZybachAPIController(ZybachDbContext dbContext, ILogger<ZybachAPIController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
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
                var measurementTypeEnum = MeasurementTypeEnum.ContinuityMeter;
                var query = _dbContext.WellSensorMeasurements.Include(x => x.MeasurementType).AsNoTracking().Where(
                    x =>
                        x.MeasurementTypeID == (int)measurementTypeEnum
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

                var apiResult = new ApiResult<StructuredResults>
                {
                    Status = "success"
                };

                var firstReadingDates = WellSensorMeasurement.GetFirstReadingDateTimesPerSensorForWells(_dbContext, measurementTypeEnum, wellRegistrationIDs);
                apiResult.Result = StructureResults(wellSensorMeasurementDtos, aghubWells, startDate, endDate, firstReadingDates, wellRegistrationIDs);
                return apiResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private static StructuredResults StructureResults(List<WellSensorMeasurementDto> results,
            Dictionary<string, int> aghubWells, DateTime startDate, DateTime endDate,
            List<WellSensorReadingDateDto> firstReadingDates, List<string> wellRegistrationIDs)
        {
            var volumesByWell = new List<VolumeByWell>();
            foreach (var wellRegistrationID in wellRegistrationIDs)
            {
                var currentWellResults = results.Where(x => x.WellRegistrationID == wellRegistrationID)
                    .OrderBy(x => x.MeasurementDate).ToList();
                var volumeByWell = new VolumeByWell
                {
                    WellRegistrationID = wellRegistrationID
                };
                var pumpingRateGallonsPerMinute = aghubWells.ContainsKey(wellRegistrationID) ? aghubWells[wellRegistrationID] : 0;
                volumeByWell.IntervalVolumes = CreateIntervalVolumesAndZeroFillMissingDays(wellRegistrationID,
                    currentWellResults, startDate, endDate,
                    pumpingRateGallonsPerMinute,
                    firstReadingDates.Where(x => x.WellRegistrationID == wellRegistrationID));
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
            DateTime endDate, int pumpingRateGallonsPerMinute, IEnumerable<WellSensorReadingDateDto> wellSensorReadingDates)
        {
            var intervalVolumeDtos = new List<IntervalVolumeDto>();
            var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1).ToList();

            foreach (var wellSensorReadingDate in wellSensorReadingDates.OrderBy(x => x.WellRegistrationID).ThenBy(x => x.SensorName))
            {
                if (wellSensorReadingDate.FirstReadingDate <= startDate)
                {
                    var sensorName = wellSensorReadingDate.SensorName;
                    var measurementValues = wellSensorMeasurementDtos
                        .Where(x => x.SensorName == sensorName).ToDictionary(
                            x => x.MeasurementDate.ToShortDateString(), x => x.MeasurementValue);
                    var dtos = dateRange.Select(a =>
                    {
                        var dateTime = startDate.AddDays(a);
                        var gallons = measurementValues.ContainsKey(dateTime.ToShortDateString())
                            ? measurementValues[dateTime.ToShortDateString()]
                            : 0;
                        return new IntervalVolumeDto(wellRegistrationID, dateTime, gallons,
                            InfluxDBService.SensorTypes.ContinuityMeter, sensorName, pumpingRateGallonsPerMinute);
                    });
                    intervalVolumeDtos.AddRange(dtos);
                }
            }

            return intervalVolumeDtos;
        }
    }
}