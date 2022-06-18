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
            var sensorMessageAges = await _influxDbService.GetLastMessageAgeBySensor();

            var wellSensorMeasurementsBySensor =
                _dbContext.WellSensorMeasurements.AsNoTracking().ToList().ToLookup(x => x.SensorName);

            var currentDate = DateTime.Now;

            foreach (var sensorSimpleDto in sensorSimpleDtos)
            {
                var messageAge = sensorMessageAges.ContainsKey(sensorSimpleDto.SensorName)
                        ? sensorMessageAges[sensorSimpleDto.SensorName]
                        : (int?)null;

                sensorSimpleDto.MessageAge = messageAge;

                if (wellSensorMeasurementsBySensor.Contains(sensorSimpleDto.SensorName))
                {
                    sensorSimpleDto.FirstReadingDate = wellSensorMeasurementsBySensor[sensorSimpleDto.SensorName].Min(x => x.MeasurementDateInPacificTime);
                    var numberOfDays = currentDate.Subtract(sensorSimpleDto.FirstReadingDate.Value).Days;
                    // correlates with ZeroFillMissingDaysAsDto
                    sensorSimpleDto.LastReadingDate = sensorSimpleDto.FirstReadingDate.Value.AddDays(numberOfDays - 1);
                    sensorSimpleDto.LastVoltageReading = wellSensorMeasurementsBySensor[sensorSimpleDto.SensorName]
                        .Where(x => x.MeasurementTypeID == MeasurementType.BatteryVoltage.MeasurementTypeID).MaxBy(x => x.MeasurementDate)?.MeasurementValue;
                }
            }

            return sensorSimpleDtos;
        }

        [HttpGet("/sensors/search/{sensorName}")]
        [ZybachViewFeature]
        public ActionResult<List<string>> SearchBySensorName([FromRoute] string sensorName)
        {
            var sensors = Sensors.SearchBySensorName(_dbContext, sensorName);
            return Ok(sensors.Select(x => x.SensorName).OrderBy(x => x));
        }

        [HttpGet("/sensors/{sensorID}")]
        [ZybachViewFeature]
        public async Task<ActionResult<SensorSimpleDto>> GetByID([FromRoute] int sensorID)
        {
            if (GetSensorAndThrowIfNotFound(sensorID, out var sensor, out var actionResult)) return actionResult;

            var sensorSimpleDto = sensor.AsSimpleDto();
            var sensorMessageAges = await _influxDbService.GetLastMessageAgeBySensor();
            var messageAge = sensorMessageAges.ContainsKey(sensorSimpleDto.SensorName)
                ? sensorMessageAges[sensorSimpleDto.SensorName]
                : (int?)null;
            sensorSimpleDto.MessageAge = messageAge;

            sensorSimpleDto.OpenSupportTickets = SupportTickets.GetSupportTicketsImpl(_dbContext)
                .Where(x => x.SupportTicketStatusID != (int)SupportTicketStatusEnum.Resolved && x.SensorID == sensor.SensorID)
                .Select(x => x.AsSimpleDto()).ToList();

            var wellSensorMeasurements = WellSensorMeasurements.GetWellSensorMeasurementsImpl(_dbContext)
                .Where(x => x.SensorName == sensorSimpleDto.SensorName).ToList();

            sensorSimpleDto.FirstReadingDate = wellSensorMeasurements.Any() ? wellSensorMeasurements.Min(x => x.MeasurementDate) : null;
            sensorSimpleDto.LastReadingDate = wellSensorMeasurements.Any() ? wellSensorMeasurements.Max(x => x.MeasurementDate) : null;
            var sensorAnomalies = _dbContext.SensorAnomalies
                .Where(x => x.SensorID == sensorID).ToList();
            var sensorMeasurementDtos = WellSensorMeasurements.ZeroFillMissingDaysAsSensorMeasurementDto2(wellSensorMeasurements.Where(x => x.MeasurementType.MeasurementTypeID != (int) MeasurementTypeEnum.BatteryVoltage).ToList(), sensorAnomalies, sensorSimpleDto);
            

            sensorSimpleDto.SensorMeasurements = sensorMeasurementDtos;
            var batteryVoltages = wellSensorMeasurements.Where(x => x.MeasurementTypeID == (int) MeasurementTypeEnum.BatteryVoltage).OrderByDescending(x => x.MeasurementDate).ToList();
            var lastVoltageReading = batteryVoltages.Any() ? batteryVoltages.FirstOrDefault()?.MeasurementValue : null;
            sensorSimpleDto.LastVoltageReading = lastVoltageReading;

            return sensorSimpleDto;
        }


        [HttpGet("/sensors/{sensorID}/chartSpec")]
        [ZybachViewFeature]
        public ActionResult GetChartSpecForSensorByID([FromRoute] int sensorID)
        {
            if (GetSensorAndThrowIfNotFound(sensorID, out var sensor, out var actionResult)) return actionResult;

            var vegaLiteChartSpec = VegaSpecUtilities.GetSensorDetailChartSpecOld(sensor);

            return Ok(vegaLiteChartSpec);
        }


        [HttpGet("/sensors/byWellID/{wellID}")]
        [UserViewFeature]
        public ActionResult<List<SensorSimpleDto>> GetSensorsByWellID([FromRoute] int wellID)
        {
            var sensorSimples = new List<SensorSimpleDto>();
            var sensors = Sensors.ListByWellID(_dbContext, wellID);

            foreach (var sensor in sensors)
            {
                var wellSensorMeasurementDtos = WellSensorMeasurements.ListBySensorAsSensorMeasurementDto(_dbContext, sensor);
                var sensorSimple = sensor.AsSimpleDto();
                sensorSimple.FirstReadingDate = wellSensorMeasurementDtos.Any() ? wellSensorMeasurementDtos.Min(x => x.MeasurementDate) : null;
                sensorSimple.LastReadingDate = wellSensorMeasurementDtos.Any() ? wellSensorMeasurementDtos.Max(x => x.MeasurementDate) : null;
                sensorSimple.SensorMeasurements = wellSensorMeasurementDtos;
                sensorSimples.Add(sensorSimple);
            }

            return sensorSimples;
        }

        private bool GetSensorAndThrowIfNotFound(int sensorID, out Sensor sensor, out ActionResult actionResult)
        {
            sensor = Sensors.GetByID(_dbContext, sensorID);
            return ThrowNotFound(sensor, "Sensor", sensorID, out actionResult);
        }
    }
}
