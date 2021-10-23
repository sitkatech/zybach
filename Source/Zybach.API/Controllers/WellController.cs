using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Feature;
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
        public object GetWells()
        {
            var wells= Wells.ListAsDtos(_dbContext);

            return new
            { 
                Status = "success",
                Result = wells
            };
        }

        [HttpGet("/api/wells/{wellRegistrationID}/details")]
        [ZybachViewFeature]
        public WellDetailDto GetWellDetails([FromRoute] string wellRegistrationID)
        {
            var well = Wells.GetByWellRegistrationID(_dbContext, wellRegistrationID);

            if (well == null)
            {
                throw new Exception($"Well with {wellRegistrationID} not found!");
            }

            var wellDetailDto = new WellDetailDto
            {
                WellRegistrationID = well.WellRegistrationID,
                Location = new Feature(new Point(new Position(well.WellGeometry.Coordinate.Y, well.WellGeometry.Coordinate.X)))
            };

            var agHubWell = well.AgHubWell;
            if (agHubWell != null)
            {
                wellDetailDto.WellTPID = agHubWell.WellTPID;
                wellDetailDto.IrrigatedAcresPerYear = agHubWell.AgHubWellIrrigatedAcres.Select(x => new IrrigatedAcresPerYearDto { Acres = x.Acres, Year = x.IrrigationYear }).ToList();
                wellDetailDto.AgHubRegisteredUser = agHubWell.AgHubRegisteredUser;
                wellDetailDto.FieldName = agHubWell.FieldName;
                wellDetailDto.HasElectricalData = agHubWell.HasElectricalData;
            }
            else
            {
                wellDetailDto.HasElectricalData = false;
            }

            var firstReadingDate = WellSensorMeasurement.GetFirstReadingDateTimeForWell(_dbContext, wellRegistrationID);
            var lastReadingDate = WellSensorMeasurement.GetLastReadingDateTimeForWell(_dbContext, wellRegistrationID);

            wellDetailDto.InGeoOptix = well.GeoOptixWell != null;
            wellDetailDto.FirstReadingDate = firstReadingDate;
            wellDetailDto.LastReadingDate = lastReadingDate;

            var sensors = well.Sensors.Select(x => new SensorSummaryDto()
            {
                SensorName = x.SensorName,
                SensorType = x.SensorType.SensorTypeDisplayName,
                WellRegistrationID = wellRegistrationID
            }).ToList();
            wellDetailDto.Sensors = sensors;

            var annualPumpedVolumes = new List<AnnualPumpedVolume>();

            annualPumpedVolumes.AddRange(GetAnnualPumpedVolumeForWellAndSensorType(wellRegistrationID, sensors, MeasurementTypes.FlowMeter, MeasurementTypeEnum.FlowMeter));
            annualPumpedVolumes.AddRange(GetAnnualPumpedVolumeForWellAndSensorType(wellRegistrationID, sensors, MeasurementTypes.ContinuityMeter, MeasurementTypeEnum.ContinuityMeter));

            if (wellDetailDto.HasElectricalData)
            {
                var wellSensorMeasurementDtos = WellSensorMeasurement.GetWellSensorMeasurementsForWellByMeasurementType(_dbContext, wellRegistrationID, MeasurementTypeEnum.ElectricalUsage);
                var pumpedVolumes = wellSensorMeasurementDtos.GroupBy(x => x.ReadingYear)
                    .Select(x => new AnnualPumpedVolume(x.Key, x.Sum(y => y.MeasurementValue),
                        MeasurementTypes.ElectricalUsage)).ToList();

                annualPumpedVolumes.AddRange(pumpedVolumes);
            }
            wellDetailDto.AnnualPumpedVolume = annualPumpedVolumes;
            return wellDetailDto;
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
        public List<RobustReviewDto> GetRobustReviewJsonFile()
        {
            var wellWithSensorSummaryDtos = _wellService.GetAghubAndGeoOptixWells();
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
            if (wellWithSensorSummaryDto.HasElectricalData)
            {
                wellSensorMeasurementDtos = WellSensorMeasurement.GetWellSensorMeasurementsForWellByMeasurementType(
                    _dbContext,
                    wellRegistrationID, MeasurementTypeEnum.ElectricalUsage);
                dataSource = MeasurementTypes.ElectricalUsage;
            }
            else
            {
                const string continuityMeter = MeasurementTypes.ContinuityMeter;
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

        [HttpPost("/api/wells/new")]
        [AdminFeature]
        public IActionResult NewWell([FromBody] WellNewDto wellNewDto)
        {
            var existingWell = Wells.GetByWellRegistrationID(_dbContext, wellNewDto.WellRegistrationID);
            if (existingWell != null)
            {
                ModelState.AddModelError("Well Registration ID", $"'{wellNewDto.WellRegistrationID}' already exists!");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wellDto = Wells.CreateNew(_dbContext, wellNewDto);
            return Ok(wellDto);
        }
    }
}