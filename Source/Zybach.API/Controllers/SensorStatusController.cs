using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SensorStatusController : SitkaController<SensorStatusController>
    {
        private readonly InfluxDBService _influxDbService;
        private readonly WellService _wellService;


        public SensorStatusController(ZybachDbContext dbContext, ILogger<SensorStatusController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, InfluxDBService influxDbService, WellService wellService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _influxDbService = influxDbService;
            _wellService = wellService;
        }


        [HttpGet("/api/sensorStatus")]
        [ZybachViewFeature]
        public async Task<List<WellWithSensorMessageAgeDto>> GetSensorMessageAges()
        {
            var wellSummariesWithSensors = _wellService.GetAghubAndGeoOptixWells()
                .Where(x => x.Sensors.Any(y => y.SensorType != MeasurementTypes.ElectricalUsage)).ToList();
            var sensorMessageAges = await _influxDbService.GetLastMessageAgeBySensor();

            return wellSummariesWithSensors.Select(well => new WellWithSensorMessageAgeDto
            {
                AgHubRegisteredUser = well.AgHubRegisteredUser,
                FieldName = well.FieldName,
                WellRegistrationID = well.WellRegistrationID,
                Location = well.Location,
                Sensors = well.Sensors.Where(x=>x.SensorType != MeasurementTypes.ElectricalUsage).Select(sensor =>
                {
                    try
                    {
                        var messageAge = sensorMessageAges.ContainsKey(sensor.SensorName)
                            ? sensorMessageAges[sensor.SensorName]
                            : (int?) null;
                        return new SensorMessageAgeDto
                        {
                            SensorName = sensor.SensorName,
                            MessageAge = messageAge,
                            SensorType = sensor.SensorType,
                            IsActive = sensor.IsActive
                        };
                    }
                    catch
                    {
                        return null;
                    }
                }).ToList()
            }).ToList();
        }

        [HttpGet("/api/sensorStatus/{wellRegistrationID}")]
        [ZybachViewFeature]
        public async Task<WellWithSensorMessageAgeDto> GetSensorMessageAgesForWell([FromRoute] string wellRegistrationID)
        {
            var well = Wells.GetAsWellWithSensorSummaryDtoByWellRegistrationID(_dbContext, wellRegistrationID);
            var sensorMessageAges = await _influxDbService.GetLastMessageAgeBySensor();

            return new WellWithSensorMessageAgeDto
            {
                AgHubRegisteredUser = well.AgHubRegisteredUser,
                FieldName = well.FieldName,
                WellRegistrationID = well.WellRegistrationID,
                Location = well.Location,
                Sensors = well.Sensors.Where(x => x.SensorType != MeasurementTypes.ElectricalUsage).Select(sensor =>
                {
                    try
                    {
                        var messageAge = sensorMessageAges.ContainsKey(sensor.SensorName)
                            ? sensorMessageAges[sensor.SensorName]
                            : (int?)null;
                        return new SensorMessageAgeDto
                        {
                            SensorName = sensor.SensorName,
                            MessageAge = messageAge,
                            SensorType = sensor.SensorType,
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

        [HttpPut("/api/sensorStatus/enableDisable")]
        [ZybachViewFeature]
        public IActionResult UpdateSensorIsActive([FromBody] SensorSummaryDto sensorSummaryDto)
        {
            // var sensor = Sensors.GetBySensorName(_dbContext, sensorSummaryDto.SensorName);
            var sensor = _dbContext.Sensors.SingleOrDefault(x => x.SensorName.Equals(sensorSummaryDto.SensorName));
            if (sensor == null)
            {
                throw new Exception($"Sensor with Sensor Name {sensorSummaryDto.SensorName} not found!");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            sensor.IsActive = sensorSummaryDto.IsActive;
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}