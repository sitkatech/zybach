using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class OpenETController : SitkaController<OpenETController>
    {
        private readonly IOpenETService _openETService;

        public OpenETController(ZybachDbContext dbContext, ILogger<OpenETController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> rioConfiguration, IOpenETService openETService) : base(dbContext, logger, keystoneService, rioConfiguration)
        {
            _openETService = openETService;
        }

        [HttpGet("/openet/is-api-key-valid")]
        [AdminFeature]
        public ActionResult<bool> IsAPIKeyValid()
        {
            return Ok(_openETService.IsOpenETAPIKeyValid());
        }

        public class OpenETTokenExpirationDate
        {
            [JsonProperty("Expiration date")]
            public DateTime ExpirationDate { get; set; }
        }


        [HttpPost("/openet-sync-history/trigger-openet-google-bucket-refresh")]
        [AdminFeature]
        public ActionResult TriggerOpenETRefreshAndRetrieveJob([FromBody] int waterYearMonthID)
        {
            var openETDataTypes = OpenETDataType.All;
            foreach (var openETDataType in openETDataTypes) { 
                var triggerResponse = _openETService.TriggerOpenETGoogleBucketRefresh(waterYearMonthID, openETDataType);
                if (!triggerResponse.IsSuccessStatusCode)
                {
                    var ores = StatusCode((int)triggerResponse.StatusCode,
                        triggerResponse.Content.ReadAsStringAsync().Result);
                    return ores;
                }
            }

            return Ok();
        }

        [HttpGet("/openet-sync-history")]
        [AdminFeature]
        public ActionResult<List<OpenETSyncHistoryDto>> List()
        {
            var inProgressDtos = OpenETSyncHistory.List(_dbContext);
            return Ok(inProgressDtos);
        }
    }
}
