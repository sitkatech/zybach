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
            if (GetSensorSimpleDtoAndThrowIfNotFound(sensorID, out var sensorSimpleDto, out var actionResult)) return actionResult;

            var sensorMessageAges = await _influxDbService.GetLastMessageAgeBySensor();
            var messageAge = sensorMessageAges.ContainsKey(sensorSimpleDto.SensorName)
                ? sensorMessageAges[sensorSimpleDto.SensorName]
                : (int?)null;
            sensorSimpleDto.MessageAge = messageAge;

            var wellSensorMeasurements = WellSensorMeasurements.GetWellSensorMeasurementsImpl(_dbContext)
                .Where(x => x.SensorName == sensorSimpleDto.SensorName).ToList();

            sensorSimpleDto.FirstReadingDate = wellSensorMeasurements.Any() ? wellSensorMeasurements.Min(x => x.MeasurementDate) : null;
            sensorSimpleDto.LastReadingDate = wellSensorMeasurements.Any() ? wellSensorMeasurements.Max(x => x.MeasurementDate) : null;
            var anomalousDates = SensorAnomalies.GetAnomolousDatesBySensorName(_dbContext, sensorSimpleDto.SensorName);
            var wellSensorMeasurementDtos = WellSensorMeasurements.ZeroFillMissingDaysAsDto(wellSensorMeasurements.Where(x => !anomalousDates.Contains(x.MeasurementDate)).ToList());
            sensorSimpleDto.WellSensorMeasurements = wellSensorMeasurementDtos;
            var batteryVoltages = wellSensorMeasurements.Where(x => x.MeasurementTypeID == (int) MeasurementTypeEnum.BatteryVoltage).OrderByDescending(x => x.MeasurementDate).ToList();
            var lastVoltageReading = batteryVoltages.Any() ? batteryVoltages.FirstOrDefault()?.MeasurementValue : null;
            sensorSimpleDto.LastVoltageReading = lastVoltageReading;

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

        private bool GetSensorSimpleDtoAndThrowIfNotFound(int sensorID, out SensorSimpleDto sensorSimpleDto, out ActionResult actionResult)
        {
            sensorSimpleDto = Sensors.GetByIDAsSimpleDto(_dbContext, sensorID);
            return ThrowNotFound(sensorSimpleDto, "Sensor", sensorID, out actionResult);
        }
    }
}
