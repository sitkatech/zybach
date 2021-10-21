using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Models;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class WellController : SitkaController<WellController>
    {
        private readonly GeoOptixService _geoOptixService;
        private readonly WellService _wellService;

        public WellController(ZybachDbContext dbContext, ILogger<WellController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, GeoOptixService geoOptixService, WellService wellService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _geoOptixService = geoOptixService;
            _wellService = wellService;
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

        [HttpGet("/api/wells/{wellRegistrationID}/details")]
        [ZybachViewFeature]
        public async Task<WellDetailDto> GetWellDetails([FromRoute] string wellRegistrationID)
        {
            var geooptixWell = await _geoOptixService.GetWellSummary(wellRegistrationID);
            var agHubWell = Well.FindByWellRegistrationIDAsWellWithSensorSummaryDto(_dbContext, wellRegistrationID);

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
                Location = agHubWell?.Location ?? geooptixWell?.Location,
                LandownerName = agHubWell?.LandownerName,
                FieldName = agHubWell?.FieldName

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
        [ZybachViewFeature]
        public async Task<List<InstallationRecordDto>> GetInstallationRecordForWell([FromRoute] string wellRegistrationID)
        {
            return await _geoOptixService.GetInstallationRecords(wellRegistrationID);
        }

        [HttpGet("/api/wells/{wellRegistrationID}/installation/{installationCanonicalName}/photo/{photoCanonicalName}")]
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

        /// <summary>
        /// Comprehensive data download to support Robust Review processes
        /// </summary>
        /// <returns></returns>
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


    }
}