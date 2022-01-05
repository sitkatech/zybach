using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Feature;
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
        private readonly GeoOptixService _geoOptixService;
        private readonly WellService _wellService;

        public WellController(ZybachDbContext dbContext, ILogger<WellController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, GeoOptixService geoOptixService, WellService wellService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _geoOptixService = geoOptixService;
            _wellService = wellService;
        }

        [HttpGet("/api/wells/search/{wellRegistrationID}")]
        [ZybachViewFeature]
        public ActionResult<List<string>> SearchByWellRegistrationID([FromRoute] string wellRegistrationID)
        {
            var wellSimpleDtos = Wells.SearchByWellRegistrationID(_dbContext, wellRegistrationID);
            return Ok(wellSimpleDtos.Select(x => x.WellRegistrationID).OrderBy(x => x));
        }

        [HttpGet("/api/wells/search/{wellRegistrationID}/hasInspectionType")]
        [ZybachViewFeature]
        public ActionResult<List<string>> SearchByWellRegistrationIDHasInspectionType([FromRoute] string wellRegistrationID)
        {
            return Ok(Wells.SearchByWellRegistrationIDHasInspectionType(_dbContext, wellRegistrationID));
        }

        [HttpGet("/api/wells/{wellRegistrationID}")]
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
                Location = new Feature(new Point(new Position(well.WellGeometry.Coordinate.Y, well.WellGeometry.Coordinate.X))),
                InAgHub = well.AgHubWell != null,
                InGeoOptix = well.GeoOptixWell != null,
                TownshipRangeSection = well.TownshipRangeSection,
                County = well.County?.CountyDisplayName,
                WellParticipationID = well.WellParticipationID,
                WellParticipationName = well.WellParticipation?.WellParticipationDisplayName,
                WellUseID = well.WellUseID,
                WellUseName = well.WellUse?.WellUseDisplayName,
                RequiresChemigation = well.RequiresChemigation,
                RequiresWaterLevelInspection = well.RequiresWaterLevelInspection,
                WellDepth = well.WellDepth,
                Clearinghouse = well.Clearinghouse,
                PageNumber = well.PageNumber,
                SiteName = well.SiteName,
                SiteNumber = well.SiteNumber,
                OwnerName = well.OwnerName,
                OwnerAddress = well.OwnerAddress,
                OwnerCity = well.OwnerCity,
                OwnerState = well.OwnerState,
                OwnerZipCode = well.OwnerZipCode,
                AdditionalContactName = well.AdditionalContactName,
                AdditionalContactAddress = well.AdditionalContactAddress,
                AdditionalContactCity = well.AdditionalContactCity,
                AdditionalContactState = well.AdditionalContactState,
                AdditionalContactZipCode = well.AdditionalContactZipCode,
                IsReplacement = well.IsReplacement,
                WellNickname = well.WellNickname,
                Notes = well.Notes,
                WaterQualityInspectionTypes = string.Join(", ", well.WellWaterQualityInspectionTypes.Select(x => x.WaterQualityInspectionType.WaterQualityInspectionTypeDisplayName)),
            };

            var agHubWell = well.AgHubWell;
            if (agHubWell != null)
            {
                wellDetailDto.WellTPID = agHubWell.WellTPID;
                wellDetailDto.IrrigatedAcresPerYear = agHubWell.AgHubWellIrrigatedAcres.Select(x => new IrrigatedAcresPerYearDto { Acres = x.Acres, Year = x.IrrigationYear }).ToList();
                wellDetailDto.AgHubRegisteredUser = agHubWell.AgHubRegisteredUser;
                wellDetailDto.FieldName = agHubWell.FieldName;
                wellDetailDto.HasElectricalData = agHubWell.HasElectricalData;
                wellDetailDto.InAgHub = true;
            }
            else
            {
                wellDetailDto.HasElectricalData = false;
                wellDetailDto.InAgHub = false;
            }

            var firstReadingDate = WellSensorMeasurement.GetFirstReadingDateTimeForWell(_dbContext, wellRegistrationID);
            var lastReadingDate = WellSensorMeasurement.GetLastReadingDateTimeForWell(_dbContext, wellRegistrationID);

            wellDetailDto.FirstReadingDate = firstReadingDate;
            wellDetailDto.LastReadingDate = lastReadingDate;

            var sensors = well.Sensors.Select(x => new SensorSummaryDto()
            {
                SensorName = x.SensorName,
                SensorType = x.SensorType.SensorTypeDisplayName,
                WellRegistrationID = wellRegistrationID,
                IsActive = x.IsActive
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

        [HttpGet("/api/wells/{wellRegistrationID}/chemigationPermits")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationPermitDetailedDto>> ListChemigationPermits([FromRoute] string wellRegistrationID)
        {
            var chemigationPermitDetailedDtos = ChemigationPermitAnnualRecords.GetByWellRegistrationIDAsDetailedDto(_dbContext, wellRegistrationID);
            return Ok(chemigationPermitDetailedDtos);
        }
    }
}