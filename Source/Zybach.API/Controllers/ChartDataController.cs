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
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ChartDataController : SitkaController<ChartDataController>
    {
        private readonly GeoOptixService _geoOptixService;

        public ChartDataController(ZybachDbContext dbContext, ILogger<ChartDataController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, GeoOptixService geoOptixService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _geoOptixService = geoOptixService;
        }


        [HttpGet("/api/chartData/{wellRegistrationID}")]
        [AdminFeature]
        public async Task<WellChartDataDto> GetInstallationRecordForWell([FromRoute] string wellRegistrationID)
        {
            var wellChartDataDto = new WellChartDataDto();
            var sensors = await _geoOptixService.GetSensorSummariesForWell(wellRegistrationID);
            var agHubWell = AgHubWell.FindByWellRegistrationIDAsWellWithSensorSummaryDto(_dbContext, wellRegistrationID);
            var hasElectricalData = agHubWell?.HasElectricalData ?? false;

            var dailyPumpedVolumes = new List<DailyPumpedVolume>();

            dailyPumpedVolumes.AddRange(GetDailyPumpedVolumeForWellAndSensorType(wellRegistrationID, sensors, InfluxDBService.SensorTypes.FlowMeter, MeasurementTypeEnum.FlowMeter));
            dailyPumpedVolumes.AddRange(GetDailyPumpedVolumeForWellAndSensorType(wellRegistrationID, sensors, InfluxDBService.SensorTypes.ContinuityMeter, MeasurementTypeEnum.ContinuityMeter));

            if (hasElectricalData)
            {
                sensors.Add(new SensorSummaryDto { WellRegistrationID = wellRegistrationID, SensorType = InfluxDBService.SensorTypes.ElectricalUsage});
                var wellSensorMeasurementDtos = WellSensorMeasurement.GetWellSensorMeasurementsForWellByMeasurementType(_dbContext, wellRegistrationID, MeasurementTypeEnum.ElectricalUsage);
                var electricalBasedFlowEstimateSeries = CreateDailyPumpedVolumesAndZeroFillMissingDays(wellSensorMeasurementDtos, InfluxDBService.SensorTypes.ElectricalUsage);
                dailyPumpedVolumes.AddRange(electricalBasedFlowEstimateSeries);
            }

            wellChartDataDto.Sensors = sensors;
            wellChartDataDto.TimeSeries = dailyPumpedVolumes;

            return wellChartDataDto;
        }

        private IEnumerable<DailyPumpedVolume> GetDailyPumpedVolumeForWellAndSensorType(string wellRegistrationID,
            IEnumerable<SensorSummaryDto> sensors, string sensorType, MeasurementTypeEnum measurementTypeEnum)
        {
            var sensorTypeSensors = sensors.Where(x => x.SensorType == sensorType).ToList();

            if (!sensorTypeSensors.Any())
            {
                return new List<DailyPumpedVolume>();
            }

            var wellSensorMeasurementDtos = WellSensorMeasurement.GetWellSensorMeasurementsForWellAndSensorsByMeasurementType(_dbContext, wellRegistrationID, measurementTypeEnum, sensorTypeSensors);
            return CreateDailyPumpedVolumesAndZeroFillMissingDays(wellSensorMeasurementDtos, sensorType);
        }

        private static IEnumerable<DailyPumpedVolume> CreateDailyPumpedVolumesAndZeroFillMissingDays(
            List<WellSensorMeasurementDto> wellSensorMeasurementDtos, string sensorType)
        {
            var measurementValues = wellSensorMeasurementDtos.ToDictionary(
                x => new DateTime(x.ReadingDate.Year, x.ReadingDate.Month, x.ReadingDate.Day).ToShortDateString(), x => x.MeasurementValue);
            var startDate = wellSensorMeasurementDtos.Min(x => x.ReadingDate);
            var endDate = DateTime.Today;
            var list = Enumerable.Range(0, (endDate - startDate).Days + 1)
                .ToList();
            var dailyPumpedVolumes = list.Select(a =>
            {
                var dateTime = startDate.AddDays(a);
                var gallons = measurementValues.ContainsKey(dateTime.ToShortDateString()) ? measurementValues[dateTime.ToShortDateString()] : 0;
                return new DailyPumpedVolume(dateTime, gallons, sensorType);
            });
            return dailyPumpedVolumes;
        }
    }

    public class WellChartDataDto
    {
        public List<DailyPumpedVolume> TimeSeries { get; set; }
        public List<SensorSummaryDto> Sensors { get; set; }
    }
}