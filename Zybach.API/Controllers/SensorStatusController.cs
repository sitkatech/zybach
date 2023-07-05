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
            var wellSummariesWithSensors = Wells.ListAsWellWithSensorSimpleDto(_dbContext)
                .Where(x => x.Sensors.Any(y => y.SensorTypeID != SensorType.ElectricalUsage.SensorTypeID)).ToList();
            var sensorMessageAgesBySensorName = PaigeWirelessPulses.GetLastMessageAgesBySensorName(_dbContext);

            var wellSensorMeasurements = _dbContext.vWellSensorMeasurementFirstAndLatestForSensors.AsNoTracking().ToList();
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
                        var lastReadingDate = wellSensorMeasurementsBySensorExcludingBatteryVoltage.Contains(sensor.SensorName)
                            ? wellSensorMeasurementsBySensorExcludingBatteryVoltage[sensor.SensorName].Max(x => x.LastReadingDate) : null;
                        var lastVoltageReading = batteryVoltagesBySensor.Contains(sensor.SensorName)
                            ? batteryVoltagesBySensor[sensor.SensorName].MaxBy(x => x.LastReadingDate) : null;

                        var messageAge = sensorMessageAgesBySensorName.TryGetValue(sensor.SensorName, out var value1)
                            ? value1 : (int?)null;

                        if (messageAge == null && lastReadingDate != null)
                        {
                            var lastReadingDateAge = DateTime.UtcNow - (DateTime)lastReadingDate;
                            messageAge = (int)lastReadingDateAge.TotalMinutes;
                        }

                        return new SensorMessageAgeDto
                        {
                            SensorName = sensor.SensorName,
                            SensorID = sensor.SensorID,
                            MessageAge = messageAge,
                            LastReadingDate = lastReadingDate,
                            LastVoltageReading = lastVoltageReading?.LatestMeasurementValue,
                            LastVoltageReadingDate = lastVoltageReading?.LastReadingDate,
                            SensorTypeID = sensor.SensorTypeID,
                            SensorTypeName = sensor.SensorTypeName,
                            IsActive = sensor.IsActive,
                            MostRecentSupportTicketID = sensor.MostRecentSupportTicketID,
                            MostRecentSupportTicketTitle = sensor.MostRecentSupportTicketTitle,
                            ContinuityMeterStatus = sensor.ContinuityMeterStatus,
                            SnoozeStartDate = sensor.SnoozeStartDate
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
                        var messageAge = PaigeWirelessPulses.GetLastMessageAgeBySensorName(_dbContext, sensor.SensorName);

                        return new SensorMessageAgeDto
                        {
                            SensorName = sensor.SensorName,
                            SensorID = sensor.SensorID,
                            MessageAge = messageAge,
                            SensorTypeID = sensor.SensorTypeID,
                            SensorTypeName = sensor.SensorTypeName,
                            IsActive = sensor.IsActive,
                            SnoozeStartDate = sensor.SnoozeStartDate
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