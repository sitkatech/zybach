using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SensorController : SitkaController<SensorController>
    {
        public SensorController(ZybachDbContext dbContext, ILogger<SensorController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/sensors")]
        [ZybachViewFeature]
        public ActionResult<List<SensorSimpleDto>> List()
        {
            var sensorSimpleDtos = _dbContext.vSensors.AsNoTracking().Select(x => x.AsSimpleDto()).ToList();
            return Ok(sensorSimpleDtos);
        }

        [HttpGet("/sensors/{sensorName}/search")]
        [ZybachViewFeature]
        public ActionResult<List<string>> SearchBySensorName([FromRoute] string sensorName)
        {
            var sensors = Sensors.SearchBySensorName(_dbContext, sensorName);
            return Ok(sensors.Select(x => x.SensorName).OrderBy(x => x));
        }

        [HttpGet("sensors/{sensorName}/pulse")]
        public ActionResult<PaigeWirelessPulseDto> GetLatestSensorPulse([FromRoute] string sensorName)
        {
            var paigeWirelessPulseDto = PaigeWirelessPulses.GetLatestBySensorName(_dbContext, sensorName);
            return Ok(paigeWirelessPulseDto);
        }

        [HttpGet("/sensors/{sensorID}")]
        [ZybachViewFeature]
        public ActionResult<SensorSimpleDto> GetByID([FromRoute] int sensorID)
        {
            if (GetSensorAndThrowIfNotFound(sensorID, out var sensor, out var actionResult)) return actionResult;
            var sensorSimpleDto = _dbContext.vSensors.AsNoTracking().Single(x => x.SensorID == sensorID).AsSimpleDto();
            var wellSensorMeasurements = _dbContext.WellSensorMeasurements.AsNoTracking()
                .Where(x => x.SensorName == sensorSimpleDto.SensorName).ToList();

            if (sensor.SensorTypeID == (int)SensorTypeEnum.ContinuityMeter)
            {
                sensorSimpleDto.LastOnReadingDate = wellSensorMeasurements.Any(x => x.MeasurementValue > 0)
                    ? wellSensorMeasurements.Where(x => x.MeasurementValue > 0).Max(x => x.MeasurementDate)
                    : null;
                sensorSimpleDto.LastOffReadingDate = wellSensorMeasurements.Any(x => x.MeasurementValue == 0) ? 
                    wellSensorMeasurements.Where(x => x.MeasurementValue == 0).Max(x => x.MeasurementDate) : null;
            }

            return Ok(sensorSimpleDto);
        }


        [HttpGet("/sensors/{sensorID}/openSupportTickets")]
        [ZybachViewFeature]
        public ActionResult<List<SupportTicketSimpleDto>> ListOpenSupportTickersBySensor([FromRoute] int sensorID)
        {
            if (GetSensorAndThrowIfNotFound(sensorID, out var sensor, out var actionResult)) return actionResult;

            var openSupportTickets = SupportTickets.GetSupportTicketsImpl(_dbContext)
                .Where(x => x.SupportTicketStatusID != (int)SupportTicketStatusEnum.Resolved && x.SensorID == sensor.SensorID)
                .Select(x => x.AsSimpleDto()).ToList();

            return Ok(openSupportTickets);
        }


        [HttpGet("/sensors/{sensorID}/chartData")]
        [ZybachViewFeature]
        public ActionResult<SensorChartDataDto> GetChartSpecForSensorByID([FromRoute] int sensorID)
        {
            if (GetSensorAndThrowIfNotFound(sensorID, out var sensor, out var actionResult)) return actionResult;

            var vegaLiteChartSpec = VegaSpecUtilities.GetSensorTypeChartSpec(sensor);
            var sensorMeasurements = WellSensorMeasurements.ListBySensorAsSensorMeasurementDto(_dbContext, sensor.SensorName, sensor.SensorID, sensor.RetirementDate, sensor.GetChartDataSourceName(), sensor.GetChartAnomaliesDataSourceName());
            var sensorChartDataDto = new SensorChartDataDto(sensorMeasurements, vegaLiteChartSpec);

            return Ok(sensorChartDataDto);
        }

        [HttpPut("/sensors/{sensorID}/snooze")]
        [ZybachViewFeature]
        public ActionResult<DateTime> UpdateSensorSnoozeByID([FromRoute] int sensorID, [FromBody] bool sensorSnoozed)
        {
            if (GetSensorAndThrowIfNotFound(sensorID, out var sensor, out var actionResult)) return actionResult;
            sensor.SnoozeStartDate = sensorSnoozed ? DateTime.UtcNow : null;
            _dbContext.SaveChanges();

            return Ok(sensor.SnoozeStartDate);
        }


        private bool GetSensorAndThrowIfNotFound(int sensorID, out Sensor sensor, out ActionResult actionResult)
        {
            sensor = _dbContext.Sensors.SingleOrDefault(x => x.SensorID == sensorID);
            return ThrowNotFound(sensor, "Sensor", sensorID, out actionResult);
        }
    }
}
