using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class SensorStatusController : SitkaController<SensorStatusController>
    {
        private readonly InfluxDBService _influxDbService;
        private readonly WellService _wellService;


        public SensorStatusController(ZybachDbContext dbContext, ILogger<SensorStatusController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, InfluxDBService influxDbService, WellService wellService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _influxDbService = influxDbService;
            _wellService = wellService;
        }


        [HttpGet("/sensorStatus")]
        [ZybachViewFeature]
        public async Task<List<WellWithSensorMessageAgeDto>> GetSensorMessageAges()
        {
            var wellSummariesWithSensors = _wellService.GetAghubAndGeoOptixWells()
                .Where(x => x.Sensors.Any(y => y.SensorTypeID != SensorType.ElectricalUsage.SensorTypeID)).ToList();
            var sensorMessageAges = await _influxDbService.GetLastMessageAgeBySensor();

            var wellSensorMeasurements = _dbContext.WellSensorMeasurements.AsNoTracking().ToList();
            var wellSensorMeasurementsBySensorExcludingBatteryVoltage = wellSensorMeasurements.Where(x => x.MeasurementTypeID != (int)MeasurementTypeEnum.BatteryVoltage).ToLookup(x => x.SensorName);
            var batteryVoltagesBySensor = wellSensorMeasurements.Where(x => x.MeasurementTypeID == (int)MeasurementTypeEnum.BatteryVoltage).ToLookup(x => x.SensorName);

            return wellSummariesWithSensors.Select(well => new WellWithSensorMessageAgeDto
            {
                AgHubRegisteredUser = well.AgHubRegisteredUser,
                FieldName = well.FieldName,
                WellID = well.WellID,
                WellRegistrationID = well.WellRegistrationID,
                Location = well.Location,
                Sensors = well.Sensors.Where(x=> x.SensorTypeID != SensorType.ElectricalUsage.SensorTypeID).Select(sensor =>
                {
                    try
                    {
                        var messageAge = sensorMessageAges.ContainsKey(sensor.SensorName)
                            ? sensorMessageAges[sensor.SensorName]
                            : (int?) null;
                        var lastReadingDate = wellSensorMeasurementsBySensorExcludingBatteryVoltage.Contains(sensor.SensorName)
                                ? wellSensorMeasurementsBySensorExcludingBatteryVoltage[sensor.SensorName].Max(x => x.MeasurementDate) : (DateTime?)null;
                        var lastVoltageReading = batteryVoltagesBySensor.Contains(sensor.SensorName) 
                            ? batteryVoltagesBySensor[sensor.SensorName].MaxBy(x => x.MeasurementDate) : null;
                        

                        return new SensorMessageAgeDto
                        {
                            SensorName = sensor.SensorName,
                            SensorID = sensor.SensorID,
                            MessageAge = messageAge,
                            LastReadingDate = lastReadingDate,
                            LastVoltageReading = lastVoltageReading?.MeasurementValue,
                            LastVoltageReadingDate = lastVoltageReading?.MeasurementDate,
                            SensorTypeID = sensor.SensorTypeID,
                            SensorTypeName = sensor.SensorTypeName,
                            IsActive = sensor.IsActive,
                            MostRecentSupportTicketID = sensor.MostRecentSupportTicketID,
                            MostRecentSupportTicketTitle = sensor.MostRecentSupportTicketTitle
                        };
                    }
                    catch
                    {
                        return null;
                    }
                }).ToList()
            }).ToList();
        }

        [HttpGet("/sensorStatus/{wellID}")]
        [ZybachViewFeature]
        public async Task<WellWithSensorMessageAgeDto> GetSensorMessageAgesForWell([FromRoute] int wellID)
        {
            var well = Wells.GetByIDAsWellWithSensorSimpleDto(_dbContext, wellID);
            var sensorMessageAges = await _influxDbService.GetLastMessageAgeBySensor();

            return new WellWithSensorMessageAgeDto
            {
                AgHubRegisteredUser = well.AgHubRegisteredUser,
                FieldName = well.FieldName,
                WellID = well.WellID,
                WellRegistrationID = well.WellRegistrationID,
                Location = well.Location,
                Sensors = well.Sensors.Where(x => x.SensorTypeID != SensorType.ElectricalUsage.SensorTypeID).Select(sensor =>
                {
                    try
                    {
                        var messageAge = sensorMessageAges.ContainsKey(sensor.SensorName)
                            ? sensorMessageAges[sensor.SensorName]
                            : (int?)null;
                        return new SensorMessageAgeDto
                        {
                            SensorName = sensor.SensorName,
                            SensorID = sensor.SensorID,
                            MessageAge = messageAge,
                            SensorTypeID = sensor.SensorTypeID,
                            SensorTypeName = sensor.SensorTypeName,
                            IsActive = sensor.IsActive
                        };
                    }
                    catch
                    {
                        return null;
                    }
                }).ToList()
            };
        }

        [HttpPut("/sensorStatus/enableDisable")]
        [ZybachViewFeature]
        public IActionResult UpdateSensorIsActive([FromBody] SensorSimpleDto sensorSimpleDto)
        {
            var sensor = _dbContext.Sensors.SingleOrDefault(x => x.SensorName.Equals(sensorSimpleDto.SensorName));
            if (sensor == null)
            {
                throw new Exception($"Sensor with Sensor Name {sensorSimpleDto.SensorName} not found!");
            }
            if (!sensorSimpleDto.IsActive)
            {
                if (!sensorSimpleDto.RetirementDate.HasValue)
                {
                    ModelState.AddModelError("Retirement Date", "The Retirement Date field is required.");
                }
                else
                {
                    var currentDate = DateTime.UtcNow;
                    if (DateTime.Compare(sensorSimpleDto.RetirementDate.Value, DateTime.UtcNow) > 0)
                    {
                        ModelState.AddModelError("Retirement Date", "Future retirement dates are not allowed.");
                    }
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            sensor.IsActive = sensorSimpleDto.IsActive;
            sensor.RetirementDate = sensorSimpleDto.RetirementDate;

            _dbContext.SaveChanges();
            return Ok();
        }
    }
}