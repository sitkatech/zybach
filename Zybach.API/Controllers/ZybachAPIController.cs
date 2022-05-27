using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zybach.API.Models;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiKey]
    [ApiController]
    public class ZybachAPIController : SitkaApiController<ZybachAPIController>
    {
        public ZybachAPIController(ZybachDbContext dbContext, ILogger<ZybachAPIController> logger) : base(dbContext, logger)
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
                var query = _dbContext.WellSensorMeasurements.Include(x => x.MeasurementType).AsNoTracking().Where(
                    x => x.MeasurementTypeID == (int)measurementTypeEnum
                         && 10000 * x.ReadingYear + 100 * x.ReadingMonth + x.ReadingDay >=
                         startDateAsInteger
                         && 10000 * x.ReadingYear + 100 * x.ReadingMonth + x.ReadingDay <=
                         endDateAsInteger
                );
                List<WellSensorMeasurementDto> wellSensorMeasurementDtos;
                if (wellRegistrationIDs != null && wellRegistrationIDs.Any())
                {
                    var wellSensorMeasurements = query.Where(x => wellRegistrationIDs.Contains(x.WellRegistrationID)).ToList();
                    wellSensorMeasurementDtos = wellSensorMeasurements.Select(x => x.AsDto()).ToList();
                }
                else
                {
                    wellSensorMeasurementDtos = query.Select(x => x.AsDto()).ToList();
                }

                var wells = _dbContext.AgHubWells.Include(x => x.Well).AsNoTracking().Where(x =>
                        wellRegistrationIDs.Contains(x.Well.WellRegistrationID)).ToList()
                    .ToDictionary(x => x.Well.WellRegistrationID, x => x.PumpingRateGallonsPerMinute);

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
                var currentWellResults = results.Where(x => x.WellRegistrationID == wellRegistrationID)
                    .OrderBy(x => x.MeasurementDate).ToList();
                var volumeByWell = new VolumeByWell
                {
                    WellRegistrationID = wellRegistrationID
                };
                var pumpingRateGallonsPerMinute = wells.ContainsKey(wellRegistrationID) ? wells[wellRegistrationID] : 0;
                volumeByWell.IntervalVolumes = CreateIntervalVolumesAndZeroFillMissingDays(wellRegistrationID,
                    currentWellResults, startDate, endDate,
                    pumpingRateGallonsPerMinute,
                    firstReadingDates.Where(x => x.WellRegistrationID == wellRegistrationID).ToList());
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
                            MeasurementTypes.ContinuityMeter);
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
    }
}