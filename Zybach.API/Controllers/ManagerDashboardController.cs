using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Models;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class ManagerDashboardController : SitkaController<ManagerDashboardController>
    {
        private readonly WellService _wellService;
        private const double GALLON_TO_ACRE_INCH = 3.68266E-5;

        public ManagerDashboardController(ZybachDbContext dbContext, ILogger<ManagerDashboardController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, WellService wellService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _wellService = wellService;
        }


        [HttpGet("/managerDashboard/districtStatistics")]
        [ZybachViewFeature]
        public DistrictStatisticsDto GetDistrictStatistics()
        {
            var allWells = Wells.ListAsWellWithSensorSimpleDto(_dbContext);
            return new DistrictStatisticsDto
            { 
                NumberOfWellsTracked = allWells.Count,
                NumberOfContinuityMeters = allWells.Where(x => x.Sensors.Any(y => y.SensorTypeID == SensorType.ContinuityMeter.SensorTypeID)).Select(x => x.WellRegistrationID).Distinct().Count(),
                NumberOfAgHubWellsWithConnectedElectricalMeters = allWells.Where(x => x.WellConnectedMeter).Select(x => x.WellRegistrationID).Distinct().Count(),
                NumberOfFlowMeters = allWells.Where(x => x.Sensors.Any(y => y.SensorTypeID == SensorType.FlowMeter.SensorTypeID)).Select(x => x.WellRegistrationID).Distinct().Count()
            };
        }

        [HttpGet("/managerDashboard/streamFlowZonePumpingDepths")]
        public List<AnnualStreamFlowZonePumpingDepthDto> GetStreamFlowZonePumpingDepths()
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
                annualStreamFlowZonePumpingDepths.Add(CreateAnnualStreamFlowZonePumpingDepthDto(year, streamFlowZoneWellMap));
            }
            return annualStreamFlowZonePumpingDepths;

        }

        private AnnualStreamFlowZonePumpingDepthDto CreateAnnualStreamFlowZonePumpingDepthDto(int year, List<StreamFlowZoneWellsDto> streamFlowZoneWellMap)
        {
            // Step 2. Get the total pumped volume for each well.
            // This is represented as a mapping from Well Registration IDs to pumped volumes
            var pumpedVolumes = WellSensorMeasurements
                .GetWellSensorMeasurementsByMeasurementTypeAndYear(_dbContext, MeasurementTypeEnum.ElectricalUsage, year)
                .GroupBy(x => x.WellRegistrationID).ToDictionary(x => x.Key, x => x.Sum(y => y.MeasurementValue));

            // Step 3. For each StreamFlowZone, calculate the pumping depth
            var streamFlowZonePumpingDepthDtos = new List<StreamFlowZonePumpingDepthDto>();
            foreach (var streamFlowZoneWellsDto in streamFlowZoneWellMap)
            {
                if (!streamFlowZoneWellsDto.Wells.Any())
                {
                    streamFlowZonePumpingDepthDtos.Add(
                        new StreamFlowZonePumpingDepthDto(streamFlowZoneWellsDto.StreamFlowZone.StreamFlowZoneID, 0, 0, 0));
                }
                else
                {
                    var wellRegistrationIDsWithinStreamFlowZone = streamFlowZoneWellsDto.Wells.Select(x => x.WellRegistrationID.ToUpper());
                    var totalIrrigatedAcres = streamFlowZoneWellsDto.Wells
                        .Where(x => wellRegistrationIDsWithinStreamFlowZone.Contains(x.WellRegistrationID))
                        .SelectMany(x => x.IrrigatedAcresPerYear).Where(x => x.Year == year).Sum(x => x.Acres);
                    var totalVolume = pumpedVolumes.Where(x => wellRegistrationIDsWithinStreamFlowZone.Contains(x.Key)).Sum(x => x.Value);

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

        public StreamFlowZonePumpingDepthDto(int streamFlowZoneID, double pumpingDepth, double totalIrrigatedAcres, double totalPumpedVolume)
        {
            StreamFlowZoneID = streamFlowZoneID;
            PumpingDepth = pumpingDepth;
            TotalIrrigatedAcres = totalIrrigatedAcres;
            TotalPumpedVolume = totalPumpedVolume;
        }

        public int StreamFlowZoneID { get; set; }
        public double PumpingDepth { get; set; }
        public double TotalIrrigatedAcres { get; set; }
        public double TotalPumpedVolume { get; set; }

    }
}