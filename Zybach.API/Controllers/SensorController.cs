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
    public class SensorController : SitkaController<SensorController>
    {
        private readonly InfluxDBService _influxDbService;

        public SensorController(ZybachDbContext dbContext, ILogger<SensorController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, InfluxDBService influxDbService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _influxDbService = influxDbService;
        }

        [HttpGet("/sensors")]
        [ZybachViewFeature]
        public async Task<List<SensorSimpleDto>> List()
        {
            var sensorSimpleDtos = Sensors.ListAsSimpleDto(_dbContext);
            var sensorMessageAges = PaigeWirelessPulses.GetLastMessageAgesBySensorName(_dbContext);

            var wellSensorMeasurementsBySensor = _dbContext.WellSensorMeasurements.AsNoTracking().ToList().ToLookup(x => x.SensorName);

            foreach (var sensorSimpleDto in sensorSimpleDtos)
            {
                sensorSimpleDto.MessageAge = sensorMessageAges.ContainsKey(sensorSimpleDto.SensorName) ? sensorMessageAges[sensorSimpleDto.SensorName] : (int?)null; ;

                if (!wellSensorMeasurementsBySensor.Contains(sensorSimpleDto.SensorName)) continue;

                var wellSensorMeasurementsExcludingBatteryVoltage = wellSensorMeasurementsBySensor[sensorSimpleDto.SensorName]
                    .Where(x => x.MeasurementTypeID != (int)MeasurementTypeEnum.BatteryVoltage).ToList();

                sensorSimpleDto.FirstReadingDate = wellSensorMeasurementsBySensor[sensorSimpleDto.SensorName].Min(x => x.MeasurementDate);
                sensorSimpleDto.LastReadingDate = wellSensorMeasurementsExcludingBatteryVoltage.Any() ? wellSensorMeasurementsExcludingBatteryVoltage.Max(x => x.MeasurementDate) : null;
                    
                var lastVoltageReading = wellSensorMeasurementsBySensor[sensorSimpleDto.SensorName]
                    .Where(x => x.MeasurementTypeID == MeasurementType.BatteryVoltage.MeasurementTypeID)
                    .MaxBy(x => x.MeasurementDate);

                sensorSimpleDto.LastVoltageReading = lastVoltageReading?.MeasurementValue;
                sensorSimpleDto.LastVoltageReadingDate = lastVoltageReading?.MeasurementDate;
            }

            return sensorSimpleDtos;
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
        public async Task<ActionResult<SensorSimpleDto>> GetByID([FromRoute] int sensorID)
        {
            if (GetSensorAndThrowIfNotFound(sensorID, out var sensor, out var actionResult)) return actionResult;

            var sensorSimpleDto = sensor.AsSimpleDto();
            sensorSimpleDto.MessageAge = PaigeWirelessPulses.GetLastMessageAgeBySensorName(_dbContext, sensor.SensorName);

            var wellSensorMeasurements = _dbContext.WellSensorMeasurements.AsNoTracking()
                .Where(x => x.SensorName == sensorSimpleDto.SensorName).ToList();
            var wellSensorMeasurementsExcludingBatteryVoltage = wellSensorMeasurements
                .Where(x => x.MeasurementTypeID != (int)MeasurementTypeEnum.BatteryVoltage).ToList();
            
            sensorSimpleDto.FirstReadingDate = wellSensorMeasurements.Any() ? wellSensorMeasurements.Min(x => x.MeasurementDate) : null;
            sensorSimpleDto.LastReadingDate = wellSensorMeasurementsExcludingBatteryVoltage.Any() ? wellSensorMeasurementsExcludingBatteryVoltage.Max(x => x.MeasurementDate) : null;

            var batteryVoltages = wellSensorMeasurements.Where(x => x.MeasurementTypeID == (int) MeasurementTypeEnum.BatteryVoltage).OrderByDescending(x => x.MeasurementDate).ToList();
            var lastVoltageReading = batteryVoltages.Any() ? batteryVoltages.FirstOrDefault() : null;
            sensorSimpleDto.LastVoltageReading = lastVoltageReading?.MeasurementValue;
            sensorSimpleDto.LastVoltageReadingDate = lastVoltageReading?.MeasurementDate;

            if (sensor.SensorTypeID == (int)SensorTypeEnum.ContinuityMeter)
            {
                sensorSimpleDto.LastOnReadingDate = wellSensorMeasurements.Any(x => x.MeasurementValue > 0)
                    ? wellSensorMeasurements.Where(x => x.MeasurementValue > 0).Max(x => x.MeasurementDate)
                    : null;
                sensorSimpleDto.LastOffReadingDate = wellSensorMeasurements.Any(x => x.MeasurementValue == 0) ? 
                    wellSensorMeasurements.Where(x => x.MeasurementValue == 0).Max(x => x.MeasurementDate) : null;
            }
            return sensorSimpleDto;
        }


        [HttpGet("/sensors/{sensorID}/openSupportTickets")]
        [ZybachViewFeature]
        public ActionResult<List<SupportTicketSimpleDto>> ListOpenSupportTickersBySensor([FromRoute] int sensorID)
        {
            if (GetSensorAndThrowIfNotFound(sensorID, out var sensor, out var actionResult)) return actionResult;

            return SupportTickets.GetSupportTicketsImpl(_dbContext)
                .Where(x => x.SupportTicketStatusID != (int)SupportTicketStatusEnum.Resolved && x.SensorID == sensor.SensorID)
                .Select(x => x.AsSimpleDto()).ToList();
        }


        [HttpGet("/sensors/{sensorID}/chartData")]
        [ZybachViewFeature]
        public ActionResult<SensorChartDataDto> GetChartSpecForSensorByID([FromRoute] int sensorID)
        {
            if (GetSensorAndThrowIfNotFound(sensorID, out var sensor, out var actionResult)) return actionResult;
            var vegaLiteChartSpec = VegaSpecUtilities.GetSensorTypeChartSpec(sensor);
            var sensorMeasurements = WellSensorMeasurements.ListBySensorAsSensorMeasurementDto(_dbContext, sensor.SensorName, sensor.SensorID, sensor.RetirementDate, sensor.GetChartDataSourceName(), sensor.GetChartAnomaliesDataSourceName());
            var sensorChartDataDto = new SensorChartDataDto(sensorMeasurements, vegaLiteChartSpec);
            return sensorChartDataDto;
        }

        [HttpPut("/sensors/{sensorID}/snooze")]
        [ZybachViewFeature]
        public ActionResult<DateTime> UpdateSensorSnoozeByID([FromRoute] int sensorID, [FromBody] bool sensorSnoozed)
        {
            var sensor = _dbContext.Sensors.SingleOrDefault(x => x.SensorID == sensorID);
            if (ThrowNotFound(sensor, "Sensor", sensorID, out var actionResult))
            {
                return actionResult;
            }
            
            sensor.SnoozeStartDate = sensorSnoozed ? DateTime.UtcNow : null;
            _dbContext.SaveChanges();

            return Ok(sensor.SnoozeStartDate);
        }


        private bool GetSensorAndThrowIfNotFound(int sensorID, out Sensor sensor, out ActionResult actionResult)
        {
            sensor = Sensors.GetByID(_dbContext, sensorID);
            return ThrowNotFound(sensor, "Sensor", sensorID, out actionResult);
        }
    }
}
