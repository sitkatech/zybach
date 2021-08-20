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

            dailyPumpedVolumes.AddRange(GetDailyPumpedVolumeForWellAndSensorType(sensors, InfluxDBService.SensorTypes.FlowMeter, MeasurementTypeEnum.FlowMeter));
            dailyPumpedVolumes.AddRange(GetDailyPumpedVolumeForWellAndSensorType(sensors, InfluxDBService.SensorTypes.ContinuityMeter, MeasurementTypeEnum.ContinuityMeter));

            if (hasElectricalData)
            {
                sensors.Add(new SensorSummaryDto { WellRegistrationID = wellRegistrationID, SensorType = InfluxDBService.SensorTypes.ElectricalUsage});
                var wellSensorMeasurementDtos =
                    WellSensorMeasurement.GetWellSensorMeasurementsByMeasurementType(_dbContext, MeasurementTypeEnum.ElectricalUsage);
                var electricalBasedFlowEstimateSeries = wellSensorMeasurementDtos.Select(x => new DailyPumpedVolume(x.ReadingDate, x.MeasurementValue, null)).ToList();
                dailyPumpedVolumes.AddRange(electricalBasedFlowEstimateSeries);
            }

            wellChartDataDto.Sensors = sensors;
            wellChartDataDto.TimeSeries = dailyPumpedVolumes;

            return wellChartDataDto;
        }

        private IEnumerable<DailyPumpedVolume> GetDailyPumpedVolumeForWellAndSensorType(IEnumerable<SensorSummaryDto> sensors, string sensorType, MeasurementTypeEnum measurementTypeEnum)
        {
            var sensorTypeSensors = sensors.Where(x => x.SensorType == sensorType).ToList();

            if (!sensorTypeSensors.Any())
            {
                return new List<DailyPumpedVolume>();
            }

            var wellSensorMeasurementDtos = WellSensorMeasurement.GetWellSensorMeasurementsForSensorsByMeasurementType(_dbContext, measurementTypeEnum, sensorTypeSensors);
            return wellSensorMeasurementDtos.Select(x => new DailyPumpedVolume(x.ReadingDate, x.MeasurementValue, sensorType)).ToList();
        }
    }

    public class WellChartDataDto
    {
        public List<DailyPumpedVolume> TimeSeries { get; set; }
        public List<SensorSummaryDto> Sensors { get; set; }
    }
}