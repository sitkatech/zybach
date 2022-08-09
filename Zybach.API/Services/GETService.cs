using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using GeoJSON.Net.Geometry;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Zybach.EFModels.Entities;

namespace Zybach.API.Services
{
    public class GETService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GETService> _logger;
        private readonly WellService _wellService;
        private readonly ZybachDbContext _dbContext;
        private readonly ZybachConfiguration _zybachConfiguration;

        private readonly TimeSpan maxTimespanBeforeUpdateFailure = new TimeSpan(2, 0,  0);

        public GETService(HttpClient httpClient, ILogger<GETService> logger, WellService wellService, ZybachDbContext dbContext, IOptions<ZybachConfiguration> zybachConfiguration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _wellService = wellService;
            _dbContext = dbContext;
            _zybachConfiguration = zybachConfiguration.Value;
        }

        public async Task<bool> IsAPIResponsive()
        {
            try
            {
                var response = await _httpClient.GetAsync("Health");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when checking GET API Health. Error:{ex.Message}");
                return false;
            }
        }

        public async Task StartNewRobustReviewScenarioRun()
        {
            var historyEntry =
                RobustReviewScenarioGETRunHistory.GetNotYetStartedRobustReviewScenarioGETRunHistory(_dbContext);

            if (historyEntry == null)
            {
                return;
            }

            if (!IsAPIResponsive().Result)
            {
                RobustReviewScenarioGETRunHistory.MarkRobustReviewScenarioGETRunHistoryAsTerminalWithIntegrationFailure(_dbContext, historyEntry);
                return;
            }

            var robustReviewDtos = AgHubIrrigationUnits.GetRobustReviewDtos(_dbContext);
            var robustReviewDtosAsBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(robustReviewDtos);
            var byteArrayContent = new ByteArrayContent(robustReviewDtosAsBytes);
            byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var runNumForRunName = _dbContext.RobustReviewScenarioGETRunHistories.Count();

            var requestObject = JsonConvert.SerializeObject(new GETNewRunModel(runNumForRunName, _zybachConfiguration));

            var response = await _httpClient.PostAsync("StartRun", new MultipartFormDataContent
            {
                {new StringContent(requestObject), "\"request\""},
                {byteArrayContent, "\"files\"", "\"RobustReviewScenario.json\""}
            });

            if (!response.IsSuccessStatusCode)
            {
                RobustReviewScenarioGETRunHistory.MarkRobustReviewScenarioGETRunHistoryAsTerminalWithIntegrationFailure(_dbContext, historyEntry);
                return;
            }

            using var streamReader = new StreamReader(response.Content.ReadAsStreamAsync().Result);
            using var jsonTextReader = new JsonTextReader(streamReader);
            var responseDeserialized = new JsonSerializer().Deserialize<GETRunResponseModel>(jsonTextReader);

            historyEntry.SuccessfulStartDate = DateTime.Now;
            historyEntry.LastUpdateDate = DateTime.Now;
            historyEntry.GETRunID = responseDeserialized.RunID;
            historyEntry.StatusMessage = responseDeserialized.RunStatus.RunStatusDisplayName;
            historyEntry.StatusHexColor = responseDeserialized.RunStatus.RunStatusColor;
            _dbContext.SaveChanges();
        }

        public async Task UpdateCurrentlyRunningRunStatus()
        {
            var historyEntry =
                RobustReviewScenarioGETRunHistory.GetNonTerminalSuccessfullyStartedRobustReviewScenarioGETRunHistory(_dbContext);

            //We have nothing to check
            if (historyEntry == null)
            {
                return;
            }

            var timeSinceLastSuccessfulUpdate = DateTime.Now.Subtract(historyEntry.LastUpdateDate ?? historyEntry.CreateDate);

            if (!IsAPIResponsive().Result && timeSinceLastSuccessfulUpdate > maxTimespanBeforeUpdateFailure)
            {
                RobustReviewScenarioGETRunHistory.MarkRobustReviewScenarioGETRunHistoryAsTerminalWithIntegrationFailure(_dbContext, historyEntry);
                return;
            }

            //Right now just getting the status is a post request. API needs to be updated
            var response = await _httpClient.PostAsync("GetRunStatus",
                new StringContent(JsonConvert.SerializeObject(new GETRunDetailModel(historyEntry.GETRunID.Value, _zybachConfiguration)), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                if (timeSinceLastSuccessfulUpdate > maxTimespanBeforeUpdateFailure)
                {
                    RobustReviewScenarioGETRunHistory.MarkRobustReviewScenarioGETRunHistoryAsTerminalWithIntegrationFailure(_dbContext, historyEntry);
                }
                return;
            }

            using var streamReader = new StreamReader(response.Content.ReadAsStreamAsync().Result);
            using var jsonTextReader = new JsonTextReader(streamReader);
            var responseDeserialized = new JsonSerializer().Deserialize<GETRunResponseModel>(jsonTextReader);

            historyEntry.LastUpdateDate = DateTime.Now;
            historyEntry.IsTerminal = responseDeserialized.RunStatus.IsTerminal;
            historyEntry.StatusMessage = responseDeserialized.RunStatus.RunStatusDisplayName;
            historyEntry.StatusHexColor = responseDeserialized.RunStatus.RunStatusColor;
            _dbContext.SaveChanges();
        }

        public class GETRunResponseModel
        {
            [JsonProperty("RunId")]
            public int RunID { get; set; }
            public GETRunStatus RunStatus { get; set; }
            public string Message { get; set; }
        }

        public class GETRunStatus
        {
            public int RunStatusID { get; set; }
            public string RunStatusName { get; set; }
            public string RunStatusDisplayName { get; set; }
            public string RunStatusColor { get; set; }
            public bool IsTerminal { get; set; }
        }

        public class GETRunDetailModel
        {
            public GETRunDetailModel(int runID, ZybachConfiguration zybachConfiguration)
            {
                RunID = runID;
                //This shouldn't need to happen based on the APIM rules, but for now  we can do it manually
                CustomerID = zybachConfiguration.GET_ROBUST_REVIEW_SCENARIO_RUN_CUSTOMER_ID;
            }
            [JsonProperty("CustomerId")]
            public int CustomerID { get; set; }

            [JsonProperty("RunId")]
            public int RunID { get; set; }
        }

        public class GETNewRunModel
        {
            public GETNewRunModel(int numForRunName, ZybachConfiguration zybachConfiguration)
            {
                Name = $"GWMA Integration Run #{numForRunName}";
                Description = "This action was automatically created by the TPNRD Groundwater Manager’s Application";
                CustomerId = zybachConfiguration.GET_ROBUST_REVIEW_SCENARIO_RUN_CUSTOMER_ID;
                UserId = zybachConfiguration.GET_ROBUST_REVIEW_SCENARIO_RUN_USER_ID;
                ModelId = zybachConfiguration.GET_ROBUST_REVIEW_SCENARIO_RUN_MODEL_ID;
                ScenarioId = zybachConfiguration.GET_ROBUST_REVIEW_SCENARIO_RUN_SCENARIO_ID;
                CreateMaps = true;
                IsDifferential = true;
                //Input Volume is N/A for Custom Scenarios
                InputVolumeType = 0;
                OutputVolumeType = 1;
            }

            public int OutputVolumeType { get; set; }

            public int InputVolumeType { get; set; }

            public bool IsDifferential { get; set; }

            public bool CreateMaps { get; set; }

            public int ScenarioId { get; set; }

            public int ModelId { get; set; }

            public int UserId { get; set; }

            public int CustomerId { get; set; }

            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
