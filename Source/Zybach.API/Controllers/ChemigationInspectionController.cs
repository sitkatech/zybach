using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ChemigationInspectionController : SitkaController<ChemigationInspectionController>
    {
        public ChemigationInspectionController(ZybachDbContext dbContext, ILogger<ChemigationInspectionController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }


        [HttpGet("/api/chemigation/summaries")]
        [ZybachViewFeature]
        public List<WellInspectionSummaryDto> GetChemigationInspections()
        {
            return new List<WellInspectionSummaryDto>();
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