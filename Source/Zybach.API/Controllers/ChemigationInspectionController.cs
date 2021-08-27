using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ChemigationInspectionController : SitkaController<ChemigationInspectionController>
    {
        private readonly GeoOptixService _geoOptixService;

        public ChemigationInspectionController(ZybachDbContext dbContext, ILogger<ChemigationInspectionController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, GeoOptixService geoOptixService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _geoOptixService = geoOptixService;
        }


        [HttpGet("/api/chemigation/summaries")]
        [ZybachViewFeature]
        public async Task<List<WellInspectionSummaryDto>> GetChemigationInspections()
        {
            var chemigationInspectionDtos = ChemigationInspection.List(_dbContext);
            var wells = await _geoOptixService.GetWellSummaries();
            return wells.Select(x =>

                new WellInspectionSummaryDto
                {
                    WellRegistrationID = x.WellRegistrationID,
                    LastChemigationDate = GetMaxLastUpdatDateForProtocolName(x, chemigationInspectionDtos, "chemigation-inspection"),
                    LastNitratesDate = GetMaxLastUpdatDateForProtocolName(x, chemigationInspectionDtos, "nitrates-inspection"),
                    LastWaterLevelDate = GetMaxLastUpdatDateForProtocolName(x, chemigationInspectionDtos, "water-level-inspection"),
                    LastWaterQualityDate = GetMaxLastUpdatDateForProtocolName(x, chemigationInspectionDtos, "water-quality-inspection"),
                    PendingInspectionsCount = chemigationInspectionDtos.Count(y =>
                        x.WellRegistrationID == y.WellRegistrationID &&
                        y.Status != "Approved")
                }
            ).ToList();
        }

        private static DateTime GetMaxLastUpdatDateForProtocolName(WellSummaryDto wellSummaryDto, IEnumerable<ChemigationInspectionDto> chemigationInspectionDtos, string protocolName)
        {
            return chemigationInspectionDtos.Where(y =>
                wellSummaryDto.WellRegistrationID == y.WellRegistrationID &&
                y.ProtocolCanonicalName == protocolName).Max(x => x.LastUpdate);
        }
    }

    public class WellInspectionSummaryDto
    {
        public string WellRegistrationID { get; set; }
        public DateTime? LastChemigationDate { get; set; }
        public DateTime? LastNitratesDate { get; set; }
        public DateTime? LastWaterLevelDate { get; set; }
        public DateTime? LastWaterQualityDate { get; set; }
        public int? PendingInspectionsCount { get; set; }
    }
}