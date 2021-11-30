using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Text.Json;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Zybach.API.Models;
using Zybach.API.Services;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class RobustReviewScenarioController : SitkaController<RobustReviewScenarioController>
    {
        private readonly WellService _wellService;

        public RobustReviewScenarioController(ZybachDbContext dbContext, ILogger<RobustReviewScenarioController> logger,
            KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, WellService wellService) : base(dbContext, logger, keystoneService,
            zybachConfiguration)
        {
            _wellService = wellService;
        }

        /// <summary>
        /// Comprehensive data download to support Robust Review processes
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/robustReviewScenario/download/robustReviewScenarioJson")]
        public List<RobustReviewDto> GetRobustReviewJsonFile()
        {
            return GetRobustReviewDtos();
        }

        private List<RobustReviewDto> GetRobustReviewDtos()
        {
            var wellWithSensorSummaryDtos = _wellService.GetAghubAndGeoOptixWells();
            var firstReadingDateTimes = WellSensorMeasurement.GetFirstReadingDateTimes(_dbContext);
            var robustReviewDtos = wellWithSensorSummaryDtos.Select(wellWithSensorSummaryDto => CreateRobustReviewDto(wellWithSensorSummaryDto, firstReadingDateTimes)).ToList();
            return robustReviewDtos.Where(x => x != null).ToList();
        }

        private RobustReviewDto CreateRobustReviewDto(WellWithSensorSummaryDto wellWithSensorSummaryDto, Dictionary<string, DateTime> firstReadingDateTimes)
        {
            var wellRegistrationID = wellWithSensorSummaryDto.WellRegistrationID;
            if (!firstReadingDateTimes.ContainsKey(wellRegistrationID))
            {
                return null;
            }

            string dataSource;
            List<WellSensorMeasurementDto> wellSensorMeasurementDtos;
            if (wellWithSensorSummaryDto.HasElectricalData)
            {
                wellSensorMeasurementDtos = WellSensorMeasurement.GetWellSensorMeasurementsForWellByMeasurementType(
                    _dbContext,
                    wellRegistrationID, MeasurementTypeEnum.ElectricalUsage);
                dataSource = MeasurementTypes.ElectricalUsage;
            }
            else
            {
                const string continuityMeter = MeasurementTypes.ContinuityMeter;
                wellSensorMeasurementDtos =
                    WellSensorMeasurement.GetWellSensorMeasurementsForWellAndSensorsByMeasurementType(_dbContext,
                        wellRegistrationID,
                        new List<MeasurementTypeEnum>
                            {MeasurementTypeEnum.ContinuityMeter, MeasurementTypeEnum.FlowMeter},
                        wellWithSensorSummaryDto.Sensors.Where(y => y.SensorType == continuityMeter));
                dataSource = continuityMeter;
            }

            var monthlyPumpedVolume = wellSensorMeasurementDtos.GroupBy(x => x.MeasurementDate.ToString("yyyyMM"))
                .Select(x =>
                    new MonthlyPumpedVolume(x.First().ReadingYear, x.First().ReadingMonth,
                        x.Sum(y => y.MeasurementValue))).ToList();

            var point = (Point)((Feature)wellWithSensorSummaryDto.Location).Geometry;
            var robustReviewDto = new RobustReviewDto
            {
                WellRegistrationID = wellRegistrationID,
                WellTPID = wellWithSensorSummaryDto.WellTPID,
                Latitude = point.Coordinates.Latitude,
                Longitude = point.Coordinates.Longitude,
                DataSource = dataSource,
                MonthlyPumpedVolumeGallons = monthlyPumpedVolume
            };

            return robustReviewDto;
        }

        /// <summary>
        /// Comprehensive data download to support Robust Review processes
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/robust-review-scenario/new")]
        public async Task<bool> NewRobustReviewRun()
        {
            var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri("https://get-api-qa.azure-api.net/GETAzureFunctionApiQA");

            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "fb820692674a45e38186c4a2ce6e6139");
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Trace", "true");

            var robustReviewDtos = GetRobustReviewDtos();
            var robustReviewDtosAsBytes = JsonSerializer.SerializeToUtf8Bytes(robustReviewDtos);
            var byteArrayContent = new ByteArrayContent(robustReviewDtosAsBytes);
            byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var requestObject = JsonConvert.SerializeObject(new
                {
                    Name = "Test Run API Flex",  
                    CustomerId = 9, 
                    UserId = 158, 
                    ModelId = 73, 
                    ScenarioId = 19, 
                    CreateMaps = true, 
                    IsDifferential = true, 
                    InputVolumeType = 1, 
                    OutputVolumeType = 1
                });

            var response = await httpClient.PostAsync($"{httpClient.BaseAddress}/StartRun", new MultipartFormDataContent
            {
                {new StringContent(requestObject), "\"request\""},
                {byteArrayContent, "\"files\"", "\"RobustReviewScenario.json\""}
            });

            return true;
        }
    }
}