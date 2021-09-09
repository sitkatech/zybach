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
    public class SensorStatusController : SitkaController<SensorStatusController>
    {
        private readonly InfluxDBService _influxDbService;
        private readonly GeoOptixService _geoOptixService;
        private readonly WellService _wellService;


        public SensorStatusController(ZybachDbContext dbContext, ILogger<SensorStatusController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, InfluxDBService influxDbService, GeoOptixService geoOptixService, WellService wellService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _influxDbService = influxDbService;
            _geoOptixService = geoOptixService;
            _wellService = wellService;
        }


        [HttpGet("/api/sensorStatus")]
        [ZybachViewFeature]
        public async Task<List<WellWithSensorMessageAgeDto>> GetSensorMessageAges()
        {
            var wellSummariesWithSensors = (await _wellService.GetAghubAndGeoOptixWells()).Where(x => x.Sensors.Any(x=>x.SensorType != "Electrical Usage")).ToList();
            var sensorMessageAges = await _influxDbService.GetLastMessageAgeBySensor();

            return wellSummariesWithSensors.Select(well => new WellWithSensorMessageAgeDto
            {
                LandownerName = well.LandownerName,
                FieldName = well.FieldName,
                WellRegistrationID = well.WellRegistrationID,
                Location = well.Location,
                Sensors = well.Sensors.Where(x=>x.SensorType != "Electrical Usage").Select(sensor =>
                {
                    try
                    {
                        var messageAge = sensorMessageAges.ContainsKey(sensor.SensorName)
                            ? sensorMessageAges[sensor.SensorName]
                            : (int?) null;
                        return new SensorMessageAgeDto
                        {
                            SensorName = sensor.SensorName,
                            MessageAge = messageAge,
                            SensorType = sensor.SensorType
                        };
                    }
                    catch
                    {
                        var fart = 0;
                        return null;
                    }
                }).ToList()
            }).ToList();
        }
    }
}