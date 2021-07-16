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
    public class ManagerDashboardController : SitkaController<ManagerDashboardController>
    {
        private readonly InfluxDBService _influxDbService;
        private readonly GeoOptixService _geoOptixService;
        private const double GALLON_TO_ACRE_INCH = 3.68266E-5;

        public ManagerDashboardController(ZybachDbContext dbContext, ILogger<ManagerDashboardController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, InfluxDBService influxDbService, GeoOptixService geoOptixService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _influxDbService = influxDbService;
            _geoOptixService = geoOptixService;
        }


        [HttpGet("districtStatistics/{year}")]
        [AdminFeature]
        public IActionResult GetDistrictStatistics([FromRoute] int year)
        {
            return new ContentResult();
            //let numberOfWellsTracked, numberOfContinuityMeters, numberOfFlowMeters, numberOfElectricalUsageEstimates;

            //// get all wells that either existed in GeoOptix as of the given year or that had electrical estimates as of the given year
            //const geoOptixWells = await this.geooptixService.getWellSummariesCreatedAsOfYear(year);
            //const aghubRegistrationIds = await this.influxService.getWellRegistrationIdsWithElectricalEstimateAsOfYear(year);
            //// combine the registration ids as a set and count to avoid counting duplicates
            //numberOfWellsTracked = new Set([...geoOptixWells.map(x => x.wellRegistrationID), ...aghubRegistrationIds]).size;

            //// get all sensors that existed in GeoOptix as of the given year
            //const sensors = await this.geooptixService.getSensorSummariesCreatedAsOfYear(year);

            //// filter by sensor type and count
            //numberOfFlowMeters = sensors.filter(x => x.sensorType === 'Flow Meter').length;
            //numberOfContinuityMeters = sensors.filter(x => x.sensorType === 'Continuity Meter').length;

            //// todo: get total number of electrical usage estimates
            //numberOfElectricalUsageEstimates = aghubRegistrationIds.length;

            //return {
            //    NumberOfWellsTracked: numberOfWellsTracked,
            //    NumberOfContinuityMeters: numberOfContinuityMeters,
            //    NumberOfElectricalUsageEstimates: numberOfElectricalUsageEstimates,
            //    NumberOfFlowMeters: numberOfFlowMeters
            //}

        }

        [HttpGet("streamFlowZonePumpingDepths")]
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

        // testing 
        [HttpGet("geoOptixWells")]
        public async Task<List<WellSummaryDto>> GetGeoOptixWells()
        {
            return await _geoOptixService.GetWellSummaries();
        }

        //[HttpGet("firstReadingDatesForSensor")]
        //public Task<Dictionary<string, DateTime>> GetFirstReadingDateTimesForSensor()
        //{
        //    return _influxDbService.GetFirstReadingDateTimesForSensor();
        //}

        //[HttpGet("pumpedVolumesForSensors")]
        //public Task<List<InfluxDBService.DailyPumpedVolume>> GetPumpedVolumesForSensors()
        //{
        //    return _influxDbService.GetPumpedVolumesForSensors(new List<string> { "PW010029", "PW010030", "PW010031" }, "Infrared", new DateTime(2000, 1, 1));
        //}

        //[HttpGet("monthlyPumpedVolumesForSensors")]
        //public Task<List<InfluxDBService.MonthlyPumpedVolume>> GetMonthlyPumpedVolumesForSensors()
        //{
        //    return _influxDbService.GetMonthlyPumpedVolumesForSensors(new List<string> { "PW010029", "PW010030", "PW010031" }, "G-191921", new DateTime(2000, 1, 1));
        //}

        //[HttpGet("firstReadingDates")]
        //public Task<Dictionary<string, DateTime>> GetFirstReadingDateTimes()
        //{
        //    return _influxDbService.GetFirstReadingDateTimes();
        //}

        //[HttpGet("lastReadingDates")]
        //public Task<Dictionary<string, DateTime>> GetLastReadingDateTimes()
        //{
        //    return _influxDbService.GetLastReadingDateTimes();
        //}

        //[HttpGet("lastMessageAgesBySensor")]
        //public Task<Dictionary<string, int>> GetLastMessageAgeBySensor()
        //{
        //    return _influxDbService.GetLastMessageAgeBySensor();
        //}


        //[HttpGet("firstReadingDateForWell/{wellRegistrationID}")]
        //public Task<DateTime?> GetFirstReadingDateTimeForWell([FromRoute] string wellRegistrationID)
        //{
        //    return _influxDbService.GetFirstReadingDateTimeForWell(wellRegistrationID);
        //}

        //[HttpGet("districtStatistics/{year}")]
        //public ActionResult<FieldDefinitionDto> GetDistrictStatistics([FromRoute] int year)
        //{
        //    var fieldDefinitionDto = FieldDefinition.GetByFieldDefinitionTypeID(_dbContext, year);
        //    return RequireNotNullThrowNotFound(fieldDefinitionDto, "FieldDefinition", year);
        //}


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