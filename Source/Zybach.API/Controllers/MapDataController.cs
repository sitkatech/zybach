using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiController]
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
        public List<WellWithSensorSummaryDto> GetWellsWithSensors()
        {
            return _wellService.GetAghubAndGeoOptixWells();
        }

        [HttpGet("api/mapData/wells/withWellPressureSensor")]
        public List<WellWaterLevelMapSummaryDto> GetWellsWithWellPressureSensors()
        {
            return _wellService.GetWellPressureWellsForWaterLevelSummary();
        }
    }
}