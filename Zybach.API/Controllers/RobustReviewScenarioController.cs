using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Http;
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
    public class RobustReviewScenarioController : SitkaController<RobustReviewScenarioController>
    {
        private readonly GETService _GETService;

        public RobustReviewScenarioController(ZybachDbContext dbContext, ILogger<RobustReviewScenarioController> logger,
            KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, GETService GETService) : base(dbContext, logger, keystoneService,
            zybachConfiguration)
        {
            _GETService = GETService;
        }

        /// <summary>
        /// Comprehensive data download to support Robust Review processes
        /// </summary>
        /// <returns></returns>
        [HttpGet("/robustReviewScenario/download/robustReviewScenarioJson")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
        public List<RobustReviewDto> GetRobustReviewJsonFile()
        {
            return AgHubIrrigationUnits.GetRobustReviewDtos(_dbContext);
        }

        [HttpGet("/robustReviewScenario/checkGETAPIHealth")]
        public ActionResult<bool> CheckGETAPIHealth()
        {
            return Ok(_GETService.IsAPIResponsive().Result);
        }

        /// <summary>
        /// Trigger a new Robust Review Scenario GET Run
        /// </summary>
        /// <returns></returns>
        [HttpPost("/robustReviewScenario/new")]
        [AdminFeature]
        public ActionResult NewRobustReviewScenarioRun()
        {
            if (RobustReviewScenarioGETRunHistory.NonTerminalRunsExist(_dbContext))
            {
                return BadRequest(
                    "There may be only one Robust Review Scenario Run in progress at any given time. Please wait until the current run completes to start another.");
            }

            var userDto = UserContext.GetUserFromHttpContext(_dbContext, HttpContext);
            RobustReviewScenarioGETRunHistory.CreateNewRobustReviewScenarioGETRunHistory(_dbContext, userDto.UserID);
            BackgroundJob.Enqueue<GETStartNewRunJob>(x => x.RunJob(null));
            return Ok();
        }

        /// <summary>
        /// Return a list of all Robust Review Scenario Runs triggered by GWMA
        /// </summary>
        /// <returns></returns>
        [HttpGet("/robustReviewScenarios")]
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