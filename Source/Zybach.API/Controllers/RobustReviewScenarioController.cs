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
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Zybach.API.Models;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class RobustReviewScenarioController : SitkaController<RobustReviewScenarioController>
    {
        private readonly WellService _wellService;
        private readonly GETService _GETService;

        public RobustReviewScenarioController(ZybachDbContext dbContext, ILogger<RobustReviewScenarioController> logger,
            KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, WellService wellService,  GETService GETService) : base(dbContext, logger, keystoneService,
            zybachConfiguration)
        {
            _wellService = wellService;
            _GETService = GETService;
        }

        /// <summary>
        /// Comprehensive data download to support Robust Review processes
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/robustReviewScenario/download/robustReviewScenarioJson")]
        public List<RobustReviewDto> GetRobustReviewJsonFile()
        {
            return _wellService.GetRobustReviewDtos();
        }

        [HttpGet("/api/robustReviewScenario/checkGETAPIHealth")]
        public ActionResult<bool> CheckGETAPIHealth()
        {
            return Ok(_GETService.IsAPIResponsive().Result);
        }

        /// <summary>
        /// Trigger a new Robust Review Scenario GET Run
        /// </summary>
        /// <returns></returns>
        [HttpPost("/api/robustReviewScenario/new")]
        [AdminFeature]
        public ActionResult NewRobustReviewScenarioRun()
        {
            var userDto = UserContext.GetUserFromHttpContext(_dbContext, HttpContext);
            RobustReviewScenarioGETRunHistory.CreateNewRobustReviewScenarioGETRunHistory(_dbContext, userDto.UserID);
            BackgroundJob.Enqueue<GETStartNewRunJob>(x => x.RunJob(null));
            return Ok();
        }

        /// <summary>
        /// Return a list of all Robust Review Scenario Runs triggered by GWMA
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/robustReviewScenarios")]
        [AdminFeature]
        public async Task<ActionResult<IEnumerable<RobustReviewScenarioGETRunHistoryDto>>> List()
        {
            if (RobustReviewScenarioGETRunHistory
                .GetNonTerminalSuccessfullyStartedRobustReviewScenarioGETRunHistory(_dbContext) != null)
            {
                await _GETService.UpdateCurrentlyRunningRunStatus();
            }

            return Ok(RobustReviewScenarioGETRunHistory.List(_dbContext));
        }
    }
}