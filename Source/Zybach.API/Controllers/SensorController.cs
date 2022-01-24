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
        public ActionResult<SensorDto> GetSensorByIDAsDto([FromRoute] int sensorID)
        {
            var sensorDto = Sensors.GetBySensorID(_dbContext, sensorID);
            return RequireNotNullThrowNotFound(sensorDto, "Sensor", sensorID);
        }
    }
}
