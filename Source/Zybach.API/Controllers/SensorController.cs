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
        public SensorController(ZybachDbContext dbContext, ILogger<SensorController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/api/sensors")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<SensorSimpleDto>> ListSensors()
        {
            var sensorSimpleDtos = Sensors.ListAsSimpleDto(_dbContext);
            return Ok(sensorSimpleDtos);
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
