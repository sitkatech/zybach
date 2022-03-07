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
        public async Task<List<SensorSimpleDto>> List()
        {
            var sensorSimpleDtos = Sensors.ListAsSimpleDto(_dbContext);
            var sensorMessageAges = await _influxDbService.GetLastMessageAgeBySensor();

            var wellSensorMeasurementDatesBySensor =
                WellSensorMeasurements.ListReadingDatesBySensor(_dbContext);

            var currentDate = DateTime.Now;

            foreach (var sensorSimpleDto in sensorSimpleDtos)
            {
                var messageAge = sensorMessageAges.ContainsKey(sensorSimpleDto.SensorName)
                        ? sensorMessageAges[sensorSimpleDto.SensorName]
                        : (int?)null;

                sensorSimpleDto.MessageAge = messageAge;

                if (wellSensorMeasurementDatesBySensor.ContainsKey(sensorSimpleDto.SensorName))
                {
                    sensorSimpleDto.FirstReadingDate = wellSensorMeasurementDatesBySensor[sensorSimpleDto.SensorName].Min();
                    var numberOfDays = currentDate.Subtract(sensorSimpleDto.FirstReadingDate.Value).Days;
                    // correlates with ZeroFillMissingDaysAsDto
                    sensorSimpleDto.LastReadingDate = sensorSimpleDto.FirstReadingDate.Value.AddDays(numberOfDays - 1);
                }
            }

            return sensorSimpleDtos;
        }

        [HttpGet("/api/sensors/{sensorID}")]
        [ZybachViewFeature]
        public async Task<ActionResult<SensorSimpleDto>> GetByID([FromRoute] int sensorID)
        {
            if (GetSensorSimpleDtoAndThrowIfNotFound(sensorID, out var sensorSimpleDto, out var actionResult)) return actionResult;

            var sensorMessageAges = await _influxDbService.GetLastMessageAgeBySensor();
            var messageAge = sensorMessageAges.ContainsKey(sensorSimpleDto.SensorName)
                ? sensorMessageAges[sensorSimpleDto.SensorName]
                : (int?)null;
            sensorSimpleDto.MessageAge = messageAge;

            var wellSensorMeasurementDtos = WellSensorMeasurements.ListBySensorAsDto(_dbContext, sensorSimpleDto.SensorName);
            sensorSimpleDto.FirstReadingDate = wellSensorMeasurementDtos.Any() ? wellSensorMeasurementDtos.Min(x => x.MeasurementDate) : null;
            sensorSimpleDto.LastReadingDate = wellSensorMeasurementDtos.Any() ? wellSensorMeasurementDtos.Max(x => x.MeasurementDate) : null;
            sensorSimpleDto.WellSensorMeasurements = wellSensorMeasurementDtos;

            return sensorSimpleDto;
        }

        [HttpGet("/api/sensors/byWellID/{wellID}")]
        [ZybachViewFeature]
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

        private bool GetSensorSimpleDtoAndThrowIfNotFound(int sensorID, out SensorSimpleDto sensorSimpleDto, out ActionResult actionResult)
        {
            sensorSimpleDto = Sensors.GetByIDAsSimpleDto(_dbContext, sensorID);
            return ThrowNotFound(sensorSimpleDto, "Sensor", sensorID, out actionResult);
        }
    }
}
