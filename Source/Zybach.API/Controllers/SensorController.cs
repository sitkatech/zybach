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
    public class SensorController : SitkaController<SensorController>
    {
        private readonly InfluxDBService _influxDbService;

        public SensorController(ZybachDbContext dbContext, ILogger<SensorController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, InfluxDBService influxDbService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _influxDbService = influxDbService;
        }

        [HttpGet("/api/sensors")]
        [ZybachViewFeature]
        public async Task<List<SensorSimpleDto>> ListSensors()
        {
            var sensorSimpleDtos = Sensors.ListAsSimpleDto(_dbContext);
            var sensorMessageAges = await _influxDbService.GetLastMessageAgeBySensor();

            foreach (var sensorSimpleDto in sensorSimpleDtos)
            {
                var messageAge = sensorMessageAges.ContainsKey(sensorSimpleDto.SensorName)
                        ? sensorMessageAges[sensorSimpleDto.SensorName]
                        : (int?)null;

                sensorSimpleDto.MessageAge = messageAge;
            }

            return sensorSimpleDtos;
        }

        [HttpGet("/api/sensors/{sensorID}")]
        [ZybachViewFeature]
        public async Task<ActionResult<SensorSimpleDto>> GetSensorByIDAsSimpleDto([FromRoute] int sensorID)
        {
            var sensorSimpleDto = Sensors.GetBySensorIDAsSimpleDto(_dbContext, sensorID);
            if (sensorSimpleDto == null)
            {
                ModelState.AddModelError("Sensor ID", $"Sensor with ID '{sensorID}' not found!");
                return BadRequest(ModelState);
            }

            var sensorMessageAges = await _influxDbService.GetLastMessageAgeBySensor();
            var messageAge = sensorMessageAges.ContainsKey(sensorSimpleDto.SensorName)
                ? sensorMessageAges[sensorSimpleDto.SensorName]
                : (int?)null;
            sensorSimpleDto.MessageAge = messageAge;

            return sensorSimpleDto;
        }
    }
}
