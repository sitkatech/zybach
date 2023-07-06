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
        public SensorStatusController(ZybachDbContext dbContext, ILogger<SensorStatusController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }


        [HttpGet("/sensorStatus")]
        [ZybachViewFeature]
        public async Task<List<WellWithSensorSimpleDto>> GetSensorMessageAges()
        {
            var wellSummariesWithSensors = Wells.ListAsWellWithSensorSimpleDto(_dbContext)
                .Where(x => x.Sensors.Any(y => y.SensorTypeID != SensorType.ElectricalUsage.SensorTypeID)).ToList();
            var vSensors = _dbContext.vSensors.AsNoTracking().ToDictionary(x => x.SensorName);
            return wellSummariesWithSensors.Select(well => new WellWithSensorSimpleDto
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
                        var vSensor = vSensors.ContainsKey(sensor.SensorName) ? vSensors[sensor.SensorName] : null;
                        return new SensorSimpleDto
                        {
                            SensorName = sensor.SensorName,
                            SensorID = sensor.SensorID,
                            LastMessageAgeInHours = vSensor?.LastMessageAgeInHours,
                            LastReadingDate = vSensor?.LastReadingDate,
                            LastVoltageReading = vSensor?.LastVoltageReading,
                            LastVoltageReadingDate = vSensor?.LastVoltageReadingDate,
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
        public async Task<WellWithSensorSimpleDto> GetSensorMessageAgesForWell([FromRoute] int wellID)
        {
            var well = Wells.GetByIDAsWellWithSensorSimpleDto(_dbContext, wellID);
            var vSensors = _dbContext.vSensors.AsNoTracking().Where(x => x.WellID == wellID).ToDictionary(x => x.SensorName);
            return new WellWithSensorSimpleDto
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
                        var vSensor = vSensors.ContainsKey(sensor.SensorName) ? vSensors[sensor.SensorName] : null;
                        return new SensorSimpleDto
                        {
                            SensorName = sensor.SensorName,
                            SensorID = sensor.SensorID,
                            LastMessageAgeInHours = vSensor?.LastMessageAgeInHours,
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