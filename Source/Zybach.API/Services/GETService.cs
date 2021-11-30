using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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


        public GETService(HttpClient httpClient, ILogger<GETService> logger, WellService wellService, ZybachDbContext dbContext, IOptions<ZybachConfiguration> zybachConfiguration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _wellService = wellService;
            _dbContext = dbContext;
            _zybachConfiguration = zybachConfiguration.Value;
        }

        private async Task<TV> GetJsonFromCatalogImpl<TV>(string uri)
        {
            using var httpResponse = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            try
            {

                httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299

                var readAsStringAsync = httpResponse.Content.ReadAsStringAsync().Result;

                using var streamReader = new StreamReader(httpResponse.Content.ReadAsStreamAsync().Result);
                using var jsonTextReader = new JsonTextReader(streamReader);
                return new JsonSerializer().Deserialize<TV>(jsonTextReader);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("HttpRequestException thrown when hitting this uri: " + uri);
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<bool> StartNewRobustReviewScenarioRun()
        {
            var historyEntry =
                RobustReviewScenarioGETRunHistory.GetNotYetStartedRobustReviewScenarioGetRunHistory(_dbContext);

            if (historyEntry == null)
            {
                return false;
            }

            var robustReviewDtos = _wellService.GetRobustReviewDtos();
            var robustReviewDtosAsBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(robustReviewDtos);
            var byteArrayContent = new ByteArrayContent(robustReviewDtosAsBytes);
            byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var runNumForRunName = _dbContext.RobustReviewScenarioGETRunHistories.Count() + 1;

            var requestObject = JsonConvert.SerializeObject(new GETNewRunModel(runNumForRunName, _zybachConfiguration));

            var response = await _httpClient.PostAsync("StartRun", new MultipartFormDataContent
            {
                {new StringContent(requestObject), "\"request\""},
                {byteArrayContent, "\"files\"", "\"RobustReviewScenario.json\""}
            });

            if (!response.IsSuccessStatusCode)
            {
                historyEntry.IsTerminal = true;
                historyEntry.StatusMessage = "GET Integration was unable to start the run.";
                _dbContext.SaveChanges();
                return false;
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
            return true;
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

        public class GETNewRunModel
        {
            public GETNewRunModel(int numForRunName, ZybachConfiguration zybachConfiguration)
            {
                Name = $"GWMA Integration Run #{numForRunName}";
                CustomerId = zybachConfiguration.GET_ROBUST_REVIEW_SCENARIO_RUN_CUSTOMER_ID;
                UserId = zybachConfiguration.GET_ROBUST_REVIEW_SCENARIO_RUN_USER_ID;
                ModelId = zybachConfiguration.GET_ROBUST_REVIEW_SCENARIO_RUN_MODEL_ID;
                ScenarioId = zybachConfiguration.GET_ROBUST_REVIEW_SCENARIO_RUN_SCENARIO_ID;
                CreateMaps = true;
                IsDifferential = true;
                InputVolumeType = 1;
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
        }
    }
}