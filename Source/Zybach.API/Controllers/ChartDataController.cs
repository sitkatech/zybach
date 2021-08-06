using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
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
        private readonly InfluxDBService _influxDbService;
        private readonly GeoOptixService _geoOptixService;

        public ChartDataController(ZybachDbContext dbContext, ILogger<ChartDataController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, InfluxDBService influxDbService, GeoOptixService geoOptixService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _influxDbService = influxDbService;
            _geoOptixService = geoOptixService;
        }


        [HttpGet("/api/chartData/{wellRegistrationID}")]
        [AdminFeature]
        public async Task<WellChartDataDto> GetInstallationRecordForWell([FromRoute] string wellRegistrationID)
        {
            var wellChartDataDto = new WellChartDataDto();
            var firstReadingDateTimeForWell = await _influxDbService.GetFirstReadingDateTimeForWell(wellRegistrationID);
            if (firstReadingDateTimeForWell == null)
            {
                return wellChartDataDto;
            }

            var sensors = await _geoOptixService.GetSensorSummariesForWell(wellRegistrationID);
            var agHubWell = AgHubWell.FindByWellRegistrationIDAsWellWithSensorSummaryDto(_dbContext, wellRegistrationID);
            var hasElectricalData = agHubWell?.HasElectricalData ?? false;

            var dailyPumpedVolumes = new List<DailyPumpedVolume>();

            dailyPumpedVolumes.AddRange(await GetDailyPumpedVolumeForWellAndSensorType(sensors, "Flow Meter", firstReadingDateTimeForWell.Value));
            dailyPumpedVolumes.AddRange(await GetDailyPumpedVolumeForWellAndSensorType(sensors, "Continuity Meter", firstReadingDateTimeForWell.Value));

            if (hasElectricalData)
            {
                sensors.Add(new SensorSummaryDto() { WellRegistrationID = wellRegistrationID, SensorType = "Electrical Usage"});
                var electricalBasedFlowEstimateSeries = await _influxDbService.GetElectricalBasedFlowEstimateSeries(wellRegistrationID, firstReadingDateTimeForWell.Value);
                dailyPumpedVolumes.AddRange(electricalBasedFlowEstimateSeries);
            }

            wellChartDataDto.Sensors = sensors;
            wellChartDataDto.TimeSeries = dailyPumpedVolumes;

            return wellChartDataDto;
        }

        private async Task<List<DailyPumpedVolume>> GetDailyPumpedVolumeForWellAndSensorType(
            List<SensorSummaryDto> sensors, string sensorType, DateTime fromDate)
        {
            var sensorTypeSensors = sensors.Where(x => x.SensorType == sensorType).ToList();

            if (!sensorTypeSensors.Any())
            {
                return new List<DailyPumpedVolume>();
            }

            return await _influxDbService.GetPumpedVolumesForSensors(sensorTypeSensors.Select(x => x.SensorName).ToList(), sensorType, fromDate);
        }

    }

    public class WellChartDataDto
    {
        public List<DailyPumpedVolume> TimeSeries { get; set; }
        public List<SensorSummaryDto> Sensors { get; set; }
    }
}