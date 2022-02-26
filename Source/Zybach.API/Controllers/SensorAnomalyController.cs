using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
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
    public class SensorAnomalyController : SitkaController<SensorAnomalyController>
    {
        public SensorAnomalyController(ZybachDbContext dbContext, ILogger<SensorAnomalyController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, GeoOptixService geoOptixService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        { }

        [HttpGet("/api/sensorAnomalies")]
        [ZybachViewFeature]
        public ActionResult<List<SensorAnomalyDto>> List()
        {
            var sensorAnomalyDtos = SensorAnomalies.ListAsSimpleDto(_dbContext);
            return Ok(sensorAnomalyDtos);
        }

        [HttpPost("/api/sensorAnomalies/new")]
        [ZybachEditFeature]
        public IActionResult NewSensorAnomaly([FromBody] SensorAnomalySimpleDto sensorAnomalySimpleDto)
        {
            if (!VerifySensorExists(_dbContext, sensorAnomalySimpleDto.SensorID))
            {
                return BadRequest(ModelState);
            }

            if (!ValidateStartAndEndDates(sensorAnomalySimpleDto))
            {
                return BadRequest(ModelState);
            }

            SensorAnomalies.CreateNew(_dbContext, sensorAnomalySimpleDto);
            return Ok();
        }

        [HttpPut("/api/sensorAnomalies/update")]
        [ZybachEditFeature]
        public IActionResult UpdateSensorAnomaly([FromBody] SensorAnomalySimpleDto sensorAnomalySimpleDto)
        {

            var sensorAnomaly = VerifySensorAndGetSensorAnomalyIfExists(_dbContext, sensorAnomalySimpleDto);
            if (sensorAnomaly == null)
            {
                return BadRequest(ModelState);
            }

            if (!ValidateStartAndEndDates(sensorAnomalySimpleDto))
            {
                return BadRequest(ModelState);
            }

            SensorAnomalies.Update(_dbContext, sensorAnomaly, sensorAnomalySimpleDto);
            return Ok();
        }

        [HttpDelete("/api/sensorAnomalies/delete")]
        [ZybachEditFeature]
        public IActionResult DeleteSensorAnomaly([FromBody] SensorAnomalySimpleDto sensorAnomalySimpleDto)
        {
            var sensorAnomaly = VerifySensorAndGetSensorAnomalyIfExists(_dbContext, sensorAnomalySimpleDto);
            if (sensorAnomaly == null)
            {
                return BadRequest(ModelState);
            }

            SensorAnomalies.Delete(_dbContext, sensorAnomaly);
            return Ok();
        }

        private bool VerifySensorExists(ZybachDbContext dbContext, int sensorID)
        {
            var sensor = _dbContext.Sensors.SingleOrDefault(x => x.SensorID == sensorID);

            if (sensor != null)
            {
                return true;
            }

            ModelState.AddModelError("Sensor", $"Sensor with an ID of {sensorID} does not exist.");
            return false;
        }

        private SensorAnomaly VerifySensorAndGetSensorAnomalyIfExists(ZybachDbContext dbContext, SensorAnomalySimpleDto sensorAnomalySimpleDto)
        {
            if (!VerifySensorExists(dbContext, sensorAnomalySimpleDto.SensorID))
            {
                return null;
            }

            var sensorAnomaly = dbContext.SensorAnomalies.SingleOrDefault(x => x.SensorAnomalyID == sensorAnomalySimpleDto.SensorAnomalyID);
            if (sensorAnomaly != null)
            {
                return sensorAnomaly;
            }

            ModelState.AddModelError("Sensor", $"Sensor anomaly with an ID of {sensorAnomalySimpleDto.SensorAnomalyID} does not exist.");
            return null;
        }

        private bool ValidateStartAndEndDates(SensorAnomalySimpleDto sensorAnomalySimpleDto)
        {
            return true;
        }
    }
}