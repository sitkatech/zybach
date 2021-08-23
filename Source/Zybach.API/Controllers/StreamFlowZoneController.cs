using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class StreamFlowZoneController : SitkaController<StreamFlowZoneController>
    {
        public StreamFlowZoneController(ZybachDbContext dbContext, ILogger<StreamFlowZoneController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }


        [HttpGet("/api/streamFlowZones")]
        public List<StreamFlowZoneDto> GetStreamFlowZones()
        {
            var streamFlowZoneDtos = StreamFlowZone.List(_dbContext);
            return streamFlowZoneDtos;
        }
    }
}