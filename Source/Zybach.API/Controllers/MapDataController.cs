﻿using System;
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
    public class MapDataController : SitkaController<MapDataController>
    {
        private readonly InfluxDBService _influxDbService;
        private readonly GeoOptixService _geoOptixService;

        public MapDataController(ZybachDbContext dbContext, ILogger<MapDataController> logger,
            KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration,
            InfluxDBService influxDbService, GeoOptixService geoOptixService) : base(dbContext, logger, keystoneService,
            zybachConfiguration)
        {
            _influxDbService = influxDbService;
            _geoOptixService = geoOptixService;
        }

        [HttpGet("api/mapData/wells")]
        public async Task<List<WellWithSensorSummaryDto>> GetWellsWithSensors()
        {
            var geoOptixWellDictionary = await _geoOptixService.GetWellsWithSensors();
            var lastReadingDateTimes = await this._influxDbService.GetLastReadingDateTimes();
            var firstReadingDateTimes = await this._influxDbService.GetFirstReadingDateTimes();
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
                Location = new Feature(new Point(new Position(agHubWellDto.Latitude, agHubWellDto.Longitude))),
                WellRegistrationID = agHubWellDto.WellRegistrationID,
                WellTPID = agHubWellDto.WellTPID,
                Sensors = new List<SensorSummaryDto>{new SensorSummaryDto{SensorType = "Electrical Usage", WellRegistrationID = agHubWellDto.WellRegistrationID}}
            };
        }
    }
}