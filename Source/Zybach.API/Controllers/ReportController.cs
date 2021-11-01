using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ReportController : SitkaController<ReportController>
    {
        public ReportController(ZybachDbContext dbContext, ILogger<ReportController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("api/reportTemplates")]
        [AdminFeature]
        public ActionResult<List<ReportTemplateDto>> ListAllReports()
        {
            var reportTemplateDtos = ReportTemplates.ListAsDtos(_dbContext);
            return reportTemplateDtos;
        }


        [HttpGet("api/reportTemplateModels")]
        [ZybachViewFeature]
        public IActionResult GetReportTemplateModels()
        {
            var reportTemplateModelDtos = ReportTemplateModel.List(_dbContext);
            return Ok(reportTemplateModelDtos);
        }


        [HttpGet("api/reportTemplates/{reportTemplateID}")]
        [AdminFeature]
        public ActionResult<ReportTemplateDto> GetReport([FromRoute] int reportTemplateID)
        {
            var reportTemplateDto = ReportTemplates.GetByReportTemplateIDAsDto(_dbContext, reportTemplateID);
            return RequireNotNullThrowNotFound(reportTemplateDto, "ReportTemplate", reportTemplateID);
        }

        [HttpPost("/api/reportTemplates/new")]
        [RequestSizeLimit(10L * 1024L * 1024L * 1024L)]
        [RequestFormLimits(MultipartBodyLengthLimit = 10L * 1024L * 1024L * 1024L)]
        [AdminFeature]
        public async Task<IActionResult> NewReportTemplate([FromForm] ReportTemplateNewDto reportTemplateNewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fileResource = await HttpUtilities.MakeFileResourceFromFormFile(reportTemplateNewDto.FileResource, _dbContext, HttpContext);

            _dbContext.FileResources.Add(fileResource);
            // _dbContext.SaveChanges();

            var reportTemplateDto = CreateNew(_dbContext, reportTemplateNewDto, fileResource);
            return Ok();
        }

        [HttpPut("api/reportTemplates/{reportTemplateID}")]
        [ZybachViewFeature]
        public async Task<ActionResult<ReportTemplateDto>> UpdateReport([FromRoute] int reportTemplateID,
            [FromForm] ReportTemplateUpdateDto reportUpdateDto)
        {
            var reportTemplateDto = ReportTemplates.GetByReportTemplateIDAsDto(_dbContext, reportTemplateID);
            if (ThrowNotFound(reportTemplateDto, "Report", reportTemplateID, out var actionResult))
            {
                return actionResult;
            }
        
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FileResource fileResource = null;
            if (reportUpdateDto.FileResource != null)
            {
                fileResource = await HttpUtilities.MakeFileResourceFromFormFile(reportUpdateDto.FileResource, _dbContext, HttpContext);

                _dbContext.FileResources.Add(fileResource);
            }
        
            var updatedReportTemplateDto = UpdateReportTemplate(_dbContext, reportTemplateID, reportUpdateDto, fileResource);
            return Ok(updatedReportTemplateDto);
        }


        private ReportTemplateDto CreateNew(ZybachDbContext dbContext, ReportTemplateNewDto reportTemplateNewDto, FileResource newFileResource)
        {
            var reportTemplateModelType = dbContext.ReportTemplateModelTypes.Single(x => x.ReportTemplateModelTypeName == "MultipleModels");
            var reportTemplate = new ReportTemplate()
            {
                FileResource = newFileResource,
                DisplayName = reportTemplateNewDto.DisplayName,
                Description = reportTemplateNewDto.Description,
                ReportTemplateModelTypeID = reportTemplateModelType.ReportTemplateModelTypeID,
                ReportTemplateModelID = reportTemplateNewDto.ReportTemplateModelID
            };
        
            dbContext.ReportTemplates.Add(reportTemplate);
            dbContext.SaveChanges();
            dbContext.Entry(reportTemplate).Reload();
            return ReportTemplates.GetByReportTemplateIDAsDto(dbContext, reportTemplate.ReportTemplateID);
        }

        private ReportTemplateDto UpdateReportTemplate(ZybachDbContext dbContext, int reportTemplateID, ReportTemplateUpdateDto reportTemplateUpdateDto, FileResource newFileResource)
        {
            var reportTemplate = dbContext.ReportTemplates
                .Include(x => x.ReportTemplateModel)
                .Include(x => x.ReportTemplateModelType)
                .Include(x => x.FileResource)
                .Include(x => x.FileResource).ThenInclude(x => x.FileResourceMimeType)
                .Include(x => x.FileResource).ThenInclude(x => x.CreateUser)
                .Include(x => x.FileResource).ThenInclude(x => x.CreateUser).ThenInclude(x => x.Role)
                .SingleOrDefault(x => x.ReportTemplateID == reportTemplateID);

            // null check occurs in calling endpoint method.
            reportTemplate.DisplayName = reportTemplateUpdateDto.DisplayName;
            reportTemplate.Description = reportTemplateUpdateDto.Description;
            reportTemplate.ReportTemplateModelID = reportTemplateUpdateDto.ReportTemplateModelID;
            if (newFileResource != null)
            {
                reportTemplate.FileResource = newFileResource;
            }

            dbContext.SaveChanges();

            return reportTemplate.AsDto();
        }
    }
}
