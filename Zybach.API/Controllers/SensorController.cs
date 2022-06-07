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

            var wellSensorMeasurementDtos = WellSensorMeasurements.ListBySensorAsDto(_dbContext, sensorSimpleDto.SensorName);
            sensorSimpleDto.FirstReadingDate = wellSensorMeasurementDtos.Any() ? wellSensorMeasurementDtos.Min(x => x.MeasurementDate) : null;
            sensorSimpleDto.LastReadingDate = wellSensorMeasurementDtos.Any() ? wellSensorMeasurementDtos.Max(x => x.MeasurementDate) : null;
            sensorSimpleDto.WellSensorMeasurements = wellSensorMeasurementDtos;

            return sensorSimpleDto;
        }

        [HttpGet("/sensors/byWellID/{wellID}")]
        [UserViewFeature]
        public ActionResult<List<SensorSimpleDto>> GetSensorsByWellID([FromRoute] int wellID)
        {
            var sensors = Sensors.ListByWellIDAsSimpleDto(_dbContext, wellID);

            foreach (var sensor in sensors)
            {
                var wellSensorMeasurementDtos = WellSensorMeasurements.ListBySensorAsDto(_dbContext, sensor.SensorName);
                sensor.FirstReadingDate = wellSensorMeasurementDtos.Any() ? wellSensorMeasurementDtos.Min(x => x.MeasurementDate) : null;
                sensor.LastReadingDate = wellSensorMeasurementDtos.Any() ? wellSensorMeasurementDtos.Max(x => x.MeasurementDate) : null;
                sensor.WellSensorMeasurements = wellSensorMeasurementDtos;
            }

            return sensors;
        }

        private bool GetSensorAndThrowIfNotFound(int sensorID, out Sensor sensor, out ActionResult actionResult)
        {
            sensor = Sensors.GetByID(_dbContext, sensorID);
            return ThrowNotFound(sensor, "Sensor", sensorID, out actionResult);
        }
    }
}
