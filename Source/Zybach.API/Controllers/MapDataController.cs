using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Feature;
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
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MapDataController : SitkaController<MapDataController>
    {
        private readonly GeoOptixService _geoOptixService;

        public MapDataController(ZybachDbContext dbContext, ILogger<MapDataController> logger,
            KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, GeoOptixService geoOptixService) : base(dbContext, logger, keystoneService,
            zybachConfiguration)
        {
            _geoOptixService = geoOptixService;
        }

        [HttpGet("api/mapData/wells")]
        public async Task<List<WellWithSensorSummaryDto>> GetWellsWithSensors()
        {
            var geoOptixWellDictionary = await _geoOptixService.GetWellsWithSensors();
            var lastReadingDateTimes = WellSensorMeasurement.GetLastReadingDateTimes(_dbContext);
            var firstReadingDateTimes = WellSensorMeasurement.GetFirstReadingDateTimes(_dbContext);
            var agHubWellDtos = AgHubWell.List(_dbContext);

            agHubWellDtos.ForEach(x =>
            {
                if (!geoOptixWellDictionary.ContainsKey(x.WellRegistrationID))
                {
                    geoOptixWellDictionary[x.WellRegistrationID] = GetWellWithSensorSummaryDtoFromAgHubWell(x);
                    return;
                }
                
                var geoOptixWell = geoOptixWellDictionary[x.WellRegistrationID];

                geoOptixWell.Sensors.Add(new SensorSummaryDto{SensorType = "Electrical Usage", WellRegistrationID = geoOptixWell.WellRegistrationID});
                geoOptixWell.WellTPID = x.WellTPID;
                geoOptixWell.FetchDate = x.FetchDate;
                geoOptixWell.HasElectricalData = x.HasElectricalData;
            });

            var wellWithSensorSummaryDtos = geoOptixWellDictionary.Values.ToList();

            wellWithSensorSummaryDtos.ForEach(x =>
            {
                x.LastReadingDate = lastReadingDateTimes.ContainsKey(x.WellRegistrationID)? lastReadingDateTimes[x.WellRegistrationID] : (DateTime?) null;
                x.FirstReadingDate = firstReadingDateTimes.ContainsKey(x.WellRegistrationID)? firstReadingDateTimes[x.WellRegistrationID] : (DateTime?) null;
            });

            return wellWithSensorSummaryDtos;
        }

        private WellWithSensorSummaryDto GetWellWithSensorSummaryDtoFromAgHubWell(AgHubWellDto agHubWellDto)
        {
            return new WellWithSensorSummaryDto
            {
                FetchDate = agHubWellDto.FetchDate,
                InGeoOptix = false,
                HasElectricalData = agHubWellDto.HasElectricalData,
                Location = new Feature(new Point(new Position(agHubWellDto.Longitude, agHubWellDto.Latitude))),
                WellRegistrationID = agHubWellDto.WellRegistrationID,
                WellTPID = agHubWellDto.WellTPID,
                Sensors = new List<SensorSummaryDto>{new SensorSummaryDto{SensorType = "Electrical Usage", WellRegistrationID = agHubWellDto.WellRegistrationID}}
            };
        }
    }
}