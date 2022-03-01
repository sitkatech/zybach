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
using Zybach.Models.DataTransferObjects.User;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class SensorAnomalyController : SitkaController<SensorAnomalyController>
    {
        public SensorAnomalyController(ZybachDbContext dbContext, ILogger<SensorAnomalyController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, GeoOptixService geoOptixService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        { }

        [HttpGet("/api/sensorAnomalies")]
        [ZybachViewFeature]
        public ActionResult<List<SensorAnomalySimpleDto>> List()
        {
            var sensorAnomalySimpleDtos = SensorAnomalies.ListAsSimpleDto(_dbContext);
            return Ok(sensorAnomalySimpleDtos);
        }

        [HttpGet("/api/sensorAnomalies/{sensorAnomalyID}")]
        [ZybachViewFeature]
        public ActionResult<SensorAnomalySimpleDto> GetByID([FromRoute] int sensorAnomalyID)
        {
            var sensorAnomalySimpleDto = SensorAnomalies.GetByIDAsSimpleDto(_dbContext, sensorAnomalyID);
            return Ok(sensorAnomalySimpleDto);
        }

        [HttpPost("/api/sensorAnomalies/new")]
        [ZybachEditFeature]
        public IActionResult NewSensorAnomaly([FromBody] SensorAnomalyUpsertDto sensorAnomalyUpsertDto)
        {
            var sensor = _dbContext.Sensors.SingleOrDefault(x => x.SensorID == sensorAnomalyUpsertDto.SensorID);
            if (sensor == null)
            {
                return BadRequest();
            }

            if (!ValidateStartAndEndDates(sensorAnomalyUpsertDto))
            {
                return BadRequest(ModelState);
            }

            SensorAnomalies.CreateNew(_dbContext, sensorAnomalyUpsertDto);
            return Ok();
        }

        [HttpPost("/api/sensorAnomalies/update")]
        [ZybachEditFeature]
        public IActionResult UpdateSensorAnomaly([FromBody] SensorAnomalyUpsertDto sensorAnomalyUpsertDto)
        {

            var sensorAnomaly = _dbContext.SensorAnomalies.SingleOrDefault(x => x.SensorAnomalyID == sensorAnomalyUpsertDto.SensorAnomalyID);

            if (sensorAnomaly == null || sensorAnomaly.SensorID != sensorAnomalyUpsertDto.SensorID)
            {
                return BadRequest();
            }

            if (!ValidateStartAndEndDates(sensorAnomalyUpsertDto))
            {
                return BadRequest(ModelState);
            }

            SensorAnomalies.Update(_dbContext, sensorAnomaly, sensorAnomalyUpsertDto);
            return Ok();
        }

        [HttpDelete("/api/sensorAnomalies/{sensorAnomalyID}")]
        [ZybachEditFeature]
        public IActionResult DeleteSensorAnomaly([FromRoute] int sensorAnomalyID)
        {
            var sensorAnomaly = _dbContext.SensorAnomalies.SingleOrDefault(x => x.SensorAnomalyID == sensorAnomalyID);
            if (sensorAnomaly == null)
            {
                return BadRequest();
            }

            SensorAnomalies.Delete(_dbContext, sensorAnomaly);
            return Ok();
        }

        private bool ValidateStartAndEndDates(SensorAnomalyUpsertDto sensorAnomalyUpsertDto)
        {
            if (sensorAnomalyUpsertDto.StartDate <= sensorAnomalyUpsertDto.EndDate)
            {
                return true;
            }

            ModelState.AddModelError("End Date", "End date must be equal to or later than start date.");
            return false;
        }
    }
}