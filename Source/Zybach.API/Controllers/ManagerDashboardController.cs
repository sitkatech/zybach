﻿using System;
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
    public class ManagerDashboardController : SitkaController<ManagerDashboardController>
    {
        private readonly InfluxDBService _influxDbService;
        private readonly GeoOptixService _geoOptixService;
        private readonly GeoOptixSearchService _geoOptixSearchService;
        private const double GALLON_TO_ACRE_INCH = 3.68266E-5;

        public ManagerDashboardController(ZybachDbContext dbContext, ILogger<ManagerDashboardController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, InfluxDBService influxDbService, GeoOptixService geoOptixService, GeoOptixSearchService geoOptixSearchService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _influxDbService = influxDbService;
            _geoOptixService = geoOptixService;
            _geoOptixSearchService = geoOptixSearchService;
        }


        [HttpGet("/api/managerDashboard/districtStatistics/{year}")]
        [AdminFeature]
        public async Task<DistrictStatisticsDto> GetDistrictStatistics([FromRoute] int year)
        {
            // get all wells that either existed in GeoOptix as of the given year or that had electrical estimates as of the given year
            var geoOptixWells = await _geoOptixService.GetWellSummariesCreatedAsOfYear(year);
            var aghubRegistrationIds = await _influxDbService.GetWellRegistrationIDsWithElectricalEstimateAsOfYear(year);
            // combine the registration ids as a set and count to avoid counting duplicates
            var numberOfWellsTracked = geoOptixWells.Select(x => x.WellRegistrationID)
                .Union(aghubRegistrationIds, StringComparer.InvariantCultureIgnoreCase).Count();

            // get all sensors that existed in GeoOptix as of the given year
            var sensors = await _geoOptixService.GetSensorSummariesCreatedAsOfYear(year);

            // filter by sensor type and count
            var numberOfFlowMeters = sensors.Count(x => x.SensorType == "Flow Meter");
            var numberOfContinuityMeters = sensors.Count(x => x.SensorType == "Continuity Meter");

            // todo: get total number of electrical usage estimates
            var numberOfElectricalUsageEstimates = aghubRegistrationIds.Count;

            return new DistrictStatisticsDto
            { 
                NumberOfWellsTracked = numberOfWellsTracked,
                NumberOfContinuityMeters = numberOfContinuityMeters,
                NumberOfElectricalUsageEstimates = numberOfElectricalUsageEstimates,
                NumberOfFlowMeters = numberOfFlowMeters
            };
        }

        public class DistrictStatisticsDto
        {
            public int NumberOfWellsTracked { get; set; }
            public int NumberOfContinuityMeters { get; set; }
            public int NumberOfElectricalUsageEstimates { get; set; }
            public int NumberOfFlowMeters { get; set; }
        }

        [HttpGet("/api/managerDashboard/streamFlowZonePumpingDepths")]
        public async Task<List<AnnualStreamFlowZonePumpingDepthDto>> GetStreamFlowZonePumpingDepths()
        {
            // Currently, we are only accounting for electrical data when color-coding the SFZ map;
            // hence, we can confine our attention to the aghub wells and the electrical estimate time series

            var currentYear = DateTime.Today.Year;
            var years = Enumerable.Range(2019, currentYear - 2019 + 1);

            // Step 1. Get a mapping from SFZs to Wells
            var streamFlowZoneWellMap = StreamFlowZone.ListStreamFlowZonesAndWellsWithinZone(_dbContext);
            var annualStreamFlowZonePumpingDepths = new List<AnnualStreamFlowZonePumpingDepthDto>();
            foreach (var year in years)
            {
                annualStreamFlowZonePumpingDepths.Add(await CreateAnnualStreamFlowZonePumpingDepthDto(year, streamFlowZoneWellMap));
            }
            return annualStreamFlowZonePumpingDepths;

        }

        private async Task<AnnualStreamFlowZonePumpingDepthDto> CreateAnnualStreamFlowZonePumpingDepthDto(int year, List<StreamFlowZoneWellsDto> streamFlowZoneWellMap)
        {
            // Step 2. Get the total pumped volume for each well.
            // This is represented as a mapping from Well Registration IDs to pumped volumes
            var pumpedVolumes = await _influxDbService.GetAnnualEstimatedPumpedVolumeByWellForYear(year);

            // Step 3. For each StreamFlowZone, calculate the pumping depth
            var streamFlowZonePumpingDepthDtos = new List<StreamFlowZonePumpingDepthDto>();
            foreach (var streamFlowZoneWellsDto in streamFlowZoneWellMap)
            {
                if (!streamFlowZoneWellsDto.AgHubWells.Any())
                {
                    streamFlowZonePumpingDepthDtos.Add(
                        new StreamFlowZonePumpingDepthDto(streamFlowZoneWellsDto.StreamFlowZone.StreamFlowZoneID, 0, 0, 0));
                }
                else
                {
                    var wellIDs = streamFlowZoneWellsDto.AgHubWells.Select(x => x.AgHubWellID);
                    var wellRegistrationIDs = streamFlowZoneWellsDto.AgHubWells.Select(x => x.WellRegistrationID.ToUpper());
                    var totalIrrigatedAcres = _dbContext.AgHubWellIrrigatedAcres
                        .Where(x => wellIDs.Contains(x.AgHubWellID) && x.IrrigationYear == year).Sum(x => x.Acres);
                    var totalVolume = pumpedVolumes.Where(x => wellRegistrationIDs.Contains(x.Key)).Sum(x => x.Value);

                    // todo: this is reporting in gallons/acres right now and we probably want acre-inch per acre
                    streamFlowZonePumpingDepthDtos.Add(new StreamFlowZonePumpingDepthDto(
                        streamFlowZoneWellsDto.StreamFlowZone.StreamFlowZoneID,
                        GALLON_TO_ACRE_INCH * totalVolume / totalIrrigatedAcres, totalIrrigatedAcres,
                        GALLON_TO_ACRE_INCH * totalVolume));
                }
            }

            return new AnnualStreamFlowZonePumpingDepthDto(year, streamFlowZonePumpingDepthDtos);
        }
    }

    public class AnnualStreamFlowZonePumpingDepthDto
    {
        public AnnualStreamFlowZonePumpingDepthDto()
        {
        }

        public AnnualStreamFlowZonePumpingDepthDto(int year, List<StreamFlowZonePumpingDepthDto> streamFlowZonePumpingDepths)
        {
            Year = year;
            StreamFlowZonePumpingDepths = streamFlowZonePumpingDepths;
        }

        public int Year { get; set; }
        public List<StreamFlowZonePumpingDepthDto> StreamFlowZonePumpingDepths { get; set; }
    }
    public class StreamFlowZonePumpingDepthDto
    {
        public StreamFlowZonePumpingDepthDto()
        {
        }

        public StreamFlowZonePumpingDepthDto(int streamFlowZoneFeatureID, double pumpingDepth, double totalIrrigatedAcres, double totalPumpedVolume)
        {
            StreamFlowZoneFeatureID = streamFlowZoneFeatureID;
            PumpingDepth = pumpingDepth;
            TotalIrrigatedAcres = totalIrrigatedAcres;
            TotalPumpedVolume = totalPumpedVolume;
        }

        public int StreamFlowZoneFeatureID { get; set; }
        public double PumpingDepth { get; set; }
        public double TotalIrrigatedAcres { get; set; }
        public double TotalPumpedVolume { get; set; }

    }
}