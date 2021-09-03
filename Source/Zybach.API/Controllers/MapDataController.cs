using System.Collections.Generic;
using System.Threading.Tasks;
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
    public class MapDataController : SitkaController<MapDataController>
    {
        private readonly WellService _wellService;

        public MapDataController(ZybachDbContext dbContext, ILogger<MapDataController> logger,
            KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, WellService wellService) : base(dbContext, logger, keystoneService,
            zybachConfiguration)
        {
            _wellService = wellService;
        }

        [HttpGet("api/mapData/wells")]
        public async Task<List<WellWithSensorSummaryDto>> GetWellsWithSensors()
        {
            return await _wellService.GetAghubAndGeoOptixWells();
        }
    }
}