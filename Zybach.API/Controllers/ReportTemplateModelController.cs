using System.Collections.Generic;
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
    public class ReportTemplateModelController : SitkaController<ReportTemplateModelController>
    {
        public ReportTemplateModelController(ZybachDbContext dbContext, ILogger<ReportTemplateModelController> logger,
            KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext,
            logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("reportTemplateModels")]
        [ZybachEditFeature]
        public IActionResult GetReportTemplateModels()
        {
            var reportTemplateModelDtos = ReportTemplateModel.AllAsDto;
            return Ok(reportTemplateModelDtos);
        }

        [HttpGet("reportTemplateModels/{reportTemplateModelID}/reports")]
        [ZybachEditFeature]
        public ActionResult<List<ReportTemplateDto>> ListAllReportsByModelID([FromRoute] int reportTemplateModelID)
        {
            var reportTemplateDtos =
                EFModels.Entities.ReportTemplates.ListByModelIDAsDtos(_dbContext, reportTemplateModelID);
            return reportTemplateDtos;
        }
    }
}