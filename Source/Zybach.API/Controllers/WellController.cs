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

        public WellController(ZybachDbContext dbContext, ILogger<WellController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, GeoOptixService geoOptixService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _geoOptixService = geoOptixService;
        }

        [HttpGet("/api/wellUses")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<WellUseDto>> GetWellUses()
        {
            var wellUseDtos = WellUses.ListAsDto(_dbContext);
            return Ok(wellUseDtos);
        }

        [HttpGet("/api/wellParticipations")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<WellUseDto>> GetWellParticipations()
        {
            var wellParticipationDtos = WellParticipations.ListAsDto(_dbContext);
            return Ok(wellParticipationDtos);
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

        [HttpGet("/api/wells/search/{wellRegistrationID}/requiresChemigation")]
        [ZybachViewFeature]
        public ActionResult<List<string>> SearchByWellRegistrationIDRequiresChemigation([FromRoute] string wellRegistrationID)
        {
            return Ok(Wells.SearchByWellRegistrationIDRequiresChemigation(_dbContext, wellRegistrationID));
        }

        [HttpGet("/api/wells/{wellID}/waterLevelInspections")]
        [ZybachViewFeature]
        public ActionResult<List<WaterLevelInspectionSummaryDto>> GetWaterLevelInspectionSummariesByWellID([FromRoute] int wellID)
        {
            var waterLevelInspectionSummaryDtos = WaterLevelInspections.ListByWellIDAsSummaryDto(_dbContext, wellID);
            return Ok(waterLevelInspectionSummaryDtos);
        }

        [HttpGet("/api/wells/{wellID}/waterQualityInspections")]
        [ZybachViewFeature]
        public ActionResult<List<WaterQualityInspectionSummaryDto>> GetWaterQualityInspectionSummariesByWellID([FromRoute] int wellID)
        {
            var waterQualityInspectionSummaryDtos = WaterQualityInspections.ListByWellIDAsSummaryDto(_dbContext, wellID);
            return Ok(waterQualityInspectionSummaryDtos);
        }

        [HttpGet("/api/wells/{wellID}")]
        [ZybachViewFeature]
        public ActionResult<WellDetailDto> GetWellDetails([FromRoute] int wellID)
        {
            if (GetWellAndThrowIfNotFound(wellID, out var well, out var actionResult)) return actionResult;

            var wellRegistrationID = well.WellRegistrationID;
            var wellDetailDto = new WellDetailDto
            {
                WellID = well.WellID,
                WellRegistrationID = wellRegistrationID,
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
                ScreenInterval = well.ScreenInterval,
                ScreenDepth = well.ScreenDepth,
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
                SensorID = x.SensorID,
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

        private bool GetWellAndThrowIfNotFound(int wellID, out Well well, out ActionResult actionResult)
        {
            well = Wells.GetByID(_dbContext, wellID);
            return ThrowNotFound(well, "Well", wellID, out actionResult);
        }

        private bool GetWellWithTrackingAndThrowIfNotFound(int wellID, out Well well, out ActionResult actionResult)
        {
            well = Wells.GetByIDWithTracking(_dbContext, wellID);
            return ThrowNotFound(well, "Well", wellID, out actionResult);
        }

        [HttpPut("/api/wells/{wellID}/editRegistrationID")]
        [AdminFeature]
        public ActionResult UpsertWellRegistrationID([FromRoute] int wellID, [FromBody] WellRegistrationIDDto wellRegistrationIDDto)
        {
            if (GetWellWithTrackingAndThrowIfNotFound(wellID, out var well, out var actionResult)) return actionResult;

            well.WellRegistrationID = wellRegistrationIDDto.WellRegistrationID;
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpGet("/api/wells/{wellID}/contactInfo")]
        [ZybachViewFeature]
        public ActionResult<WellContactInfoDto> GetWellContactDetails([FromRoute] int wellID)
        {
            if (GetWellAndThrowIfNotFound(wellID, out var well, out var actionResult)) return actionResult;

            var wellContactInfoDto = new WellContactInfoDto()
            {
                WellRegistrationID = well.WellRegistrationID,
                TownshipRangeSection = well.TownshipRangeSection,
                County = well.County?.CountyDisplayName,
                CountyID = well.County?.CountyID,
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
                WellNickname = well.WellNickname,
                Notes = well.Notes
            };

            return Ok(wellContactInfoDto);
        }

        [HttpPut("/api/wells/{wellID}/contactInfo")]
        [AdminFeature]
        public ActionResult UpsertWellContactDetails([FromRoute] int wellID, [FromBody] WellContactInfoDto wellContactInfoDto)
        {
            if (GetWellWithTrackingAndThrowIfNotFound(wellID, out var well, out var actionResult)) return actionResult;

            Wells.MapFromContactUpsert(well, wellContactInfoDto);
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpGet("/api/wells/{wellID}/participationInfo")]
        [ZybachViewFeature]
        public ActionResult<WellParticipationInfoDto> GetWellParticipationDetails([FromRoute] int wellID)
        {
            if (GetWellAndThrowIfNotFound(wellID, out var well, out var actionResult)) return actionResult;

            var waterQualityInspectionTypeIDs = well.WellWaterQualityInspectionTypes
                .Select(x => x.WaterQualityInspectionTypeID)
                .ToList();

            var wellParticipationInfoDto = new WellParticipationInfoDto()
            {
                WellRegistrationID = well.WellRegistrationID,
                WellParticipationID = well.WellParticipationID,
                WellParticipationName = well.WellParticipation?.WellParticipationDisplayName,
                WellUseID = well.WellUseID,
                WellUseName = well.WellUse?.WellUseDisplayName, 
                RequiresChemigation = well.RequiresChemigation,
                RequiresWaterLevelInspection = well.RequiresWaterLevelInspection,
                WaterQualityInspectionTypeIDs = waterQualityInspectionTypeIDs,
                IsReplacement = well.IsReplacement, 
                WellDepth = well.WellDepth,
                Clearinghouse = well.Clearinghouse,
                PageNumber = well.PageNumber, 
                SiteName = well.SiteName,
                SiteNumber = well.SiteNumber,
                ScreenInterval = well.ScreenInterval,
                ScreenDepth = well.ScreenDepth
            };

            return Ok(wellParticipationInfoDto);
        }

        [HttpPut("/api/wells/{wellID}/participationInfo")]
        [AdminFeature]
        public ActionResult UpsertWellParticipationDetails([FromRoute] int wellID, [FromBody] WellParticipationInfoDto wellParticipationInfoDto)
        {
            if (GetWellWithTrackingAndThrowIfNotFound(wellID, out var well, out var actionResult)) return actionResult;
            Wells.MapFromParticipationUpsert(well, wellParticipationInfoDto);
            UpdateWaterQualityInspectionTypesForWell(well, wellParticipationInfoDto);
            _dbContext.SaveChanges();
            return Ok();
        }

        private void UpdateWaterQualityInspectionTypesForWell(Well well, WellParticipationInfoDto wellParticipationInfoDto)
        {
            _dbContext.WellWaterQualityInspectionTypes.RemoveRange(
                _dbContext.WellWaterQualityInspectionTypes.Where(x => x.WellID == well.WellID));

            foreach (var waterQualityInspectionTypeID in wellParticipationInfoDto.WaterQualityInspectionTypeIDs)
            {
                var wellWaterQualityInspectionType = new WellWaterQualityInspectionType()
                {
                    WellID = well.WellID,
                    WaterQualityInspectionTypeID = waterQualityInspectionTypeID
                };
                _dbContext.WellWaterQualityInspectionTypes.Add(wellWaterQualityInspectionType);
            }
        }

        [HttpGet("/api/wells/{wellID}/installation")]
        [ZybachViewFeature]
        public async Task<ActionResult<List<InstallationRecordDto>>> GetInstallationRecordForWell([FromRoute] int wellID)
        {
            if (GetWellWithTrackingAndThrowIfNotFound(wellID, out var well, out var actionResult)) return actionResult;
            return await _geoOptixService.GetInstallationRecords(well.WellRegistrationID);
        }

        [HttpGet("/api/wells/{wellID}/installation/{installationCanonicalName}/photo/{photoCanonicalName}")]
        [ZybachViewFeature]
        public async Task<IActionResult> GetPhoto([FromRoute] int wellID, [FromRoute] string installationCanonicalName, [FromRoute] string photoCanonicalName)
        {
            if (GetWellWithTrackingAndThrowIfNotFound(wellID, out var well, out var actionResult)) return actionResult;
            try
            {
                var photoBuffer = await _geoOptixService.GetPhoto(well.WellRegistrationID, installationCanonicalName,
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
            var existingWell = Wells.GetByWellRegistrationIDWithTracking(_dbContext, wellNewDto.WellRegistrationID);
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

        [HttpGet("/api/wells/{wellID}/chemigationPermits")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationPermitDetailedDto>> ListChemigationPermits([FromRoute] int wellID)
        {
            var chemigationPermitDetailedDtos = ChemigationPermits.GetByWellIDAsDetailedDto(_dbContext, wellID);
            return Ok(chemigationPermitDetailedDtos);
        }
    }
}