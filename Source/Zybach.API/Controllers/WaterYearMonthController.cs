using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class WaterYearMonthController : SitkaController<WaterYearMonthController>
    {
        public WaterYearMonthController(ZybachDbContext dbContext, ILogger<WaterYearMonthController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/api/water-year-months")]
        [AdminFeature]
        public ActionResult<List<WaterYearMonthDto>> GetWaterYearMonths()
        {
            var waterYearMonths = WaterYearMonths.List(_dbContext);
            return Ok(waterYearMonths);
        }

        [HttpGet("/api/water-year-months/current-date-or-earlier")]
        [AdminFeature]
        public ActionResult<List<WaterYearMonthDto>> GetWaterYearMonthsForCurrentDateOrEarlier()
        {
            var waterYearMonths = WaterYearMonths.ListForCurrentDateOrEarlier(_dbContext);
            return Ok(waterYearMonths);
        }

        [HttpGet("/api/water-year-months/most-recent-sync-history")]
        [AdminFeature]
        public ActionResult<List<OpenETSyncHistoryDto>> GetMostRecentSyncHistoryForWaterYearMonthsThatHaveBeenUpdated()
        {
            return _dbContext.vOpenETMostRecentSyncHistoryForYearAndMonths
                .Include(x => x.WaterYearMonth)
                .Select(x => x.AsOpenETSyncHistoryDto()).ToList();
        }

        [HttpPut("/api/water-year-month/finalize")]
        [AdminFeature]
        public ActionResult<WaterYearMonthDto> FinalizeWaterYearMonth([FromBody] int waterYearMonthID)
        {
            var waterYearMonthDto = WaterYearMonths.GetByWaterYearMonthID(_dbContext, waterYearMonthID);
            if (ThrowNotFound(waterYearMonthDto, "Water Year Month", waterYearMonthID, out var actionResult))
            {
                return actionResult;
            }

            var finalizedWaterYearDto = WaterYearMonths.Finalize(_dbContext, waterYearMonthID);
            return Ok(finalizedWaterYearDto);
        }
    }
}