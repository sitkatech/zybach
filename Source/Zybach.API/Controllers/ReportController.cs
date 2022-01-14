using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.ReportTemplates;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class ReportController : SitkaController<ReportController>
    {
        public ReportController(ZybachDbContext dbContext, ILogger<ReportController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("api/reportTemplates")]
        [AdminFeature]
        public ActionResult<List<ReportTemplateDto>> ListAllReports()
        {
            var reportTemplateDtos = EFModels.Entities.ReportTemplates.ListAsDtos(_dbContext);
            return reportTemplateDtos;
        }

        [HttpGet("api/reportTemplatesByModelID/{reportTemplateModelID}")]
        [AdminFeature]
        public ActionResult<List<ReportTemplateDto>> ListAllReportsByModelID([FromRoute] int reportTemplateModelID)
        {
            var reportTemplateDtos = EFModels.Entities.ReportTemplates.ListByModelIDAsDtos(_dbContext, reportTemplateModelID);
            return reportTemplateDtos;
        }

        [HttpGet("api/reportTemplateModels")]
        [AdminFeature]
        public IActionResult GetReportTemplateModels()
        {
            var reportTemplateModelDtos = ReportTemplateModel.List(_dbContext);
            return Ok(reportTemplateModelDtos);
        }


        [HttpGet("api/reportTemplates/{reportTemplateID}")]
        [AdminFeature]
        public ActionResult<ReportTemplateDto> GetReport([FromRoute] int reportTemplateID)
        {
            var reportTemplateDto = EFModels.Entities.ReportTemplates.GetByReportTemplateIDAsDto(_dbContext, reportTemplateID);
            return RequireNotNullThrowNotFound(reportTemplateDto, "ReportTemplate", reportTemplateID);
        }

        [HttpPost("/api/reportTemplates/new")]
        [RequestSizeLimit(10L * 1024L * 1024L * 1024L)]
        [RequestFormLimits(MultipartBodyLengthLimit = 10L * 1024L * 1024L * 1024L)]
        [AdminFeature]
        public async Task<ActionResult<ReportTemplateDto>> NewReportTemplate([FromForm] ReportTemplateNewDto reportTemplateNewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_dbContext.ReportTemplates.Any(x => x.DisplayName.Equals(reportTemplateNewDto.DisplayName)))
            {
                return BadRequest($"Report Template with Name '{reportTemplateNewDto.DisplayName}' already exists.");
            }
            var fileResource = await HttpUtilities.MakeFileResourceFromFormFile(reportTemplateNewDto.FileResource, _dbContext, HttpContext);

            _dbContext.FileResources.Add(fileResource);

            var reportTemplate = CreateNew(_dbContext, reportTemplateNewDto, fileResource);

            ReportTemplateGenerator.ValidateReportTemplate(reportTemplate, out var reportIsValid, out var errorMessage, out var sourceCode, _dbContext, _logger);
            if (!reportIsValid)
            {
                var errorMessageAndSourceCode = $"{errorMessage} \n <pre style='max-height: 300px; overflow: scroll;'>{sourceCode}</pre>";
                return BadRequest(errorMessageAndSourceCode);
            }

            _dbContext.ReportTemplates.Add(reportTemplate);
            _dbContext.SaveChanges();
            _dbContext.Entry(reportTemplate).Reload();

            var reportTemplateDto = EFModels.Entities.ReportTemplates.GetByReportTemplateIDAsDto(_dbContext, reportTemplate.ReportTemplateID);
            return Ok(reportTemplateDto);
        }

        [HttpPut("api/reportTemplates/{reportTemplateID}")]
        [RequestSizeLimit(10L * 1024L * 1024L * 1024L)]
        [RequestFormLimits(MultipartBodyLengthLimit = 10L * 1024L * 1024L * 1024L)]
        [AdminFeature]
        public async Task<ActionResult<ReportTemplateDto>> UpdateReport([FromRoute] int reportTemplateID,
            [FromForm] ReportTemplateUpdateDto reportUpdateDto)
        {
            var reportTemplate = EFModels.Entities.ReportTemplates.GetByReportTemplateIDWithTracking(_dbContext, reportTemplateID);
            if (ThrowNotFound(reportTemplate, "ReportTemplate", reportTemplateID, out var actionResult))
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

            var updatedReportTemplate = UpdateReportTemplate(_dbContext, reportTemplate, reportUpdateDto, fileResource);

            ReportTemplateGenerator.ValidateReportTemplate(updatedReportTemplate, out var reportIsValid, out var errorMessage, out var sourceCode, _dbContext, _logger);
            if (!reportIsValid)
            {
                var errorMessageAndSourceCode = $"{errorMessage} \n <pre style='max-height: 300px; overflow: scroll;'>{sourceCode}</pre>";
                return BadRequest(errorMessageAndSourceCode);
            }

            _dbContext.SaveChanges();
            _dbContext.Entry(reportTemplate).Reload();

            var reportTemplateDto = EFModels.Entities.ReportTemplates.GetByReportTemplateIDAsDto(_dbContext, reportTemplate.ReportTemplateID);
            return Ok(reportTemplateDto);
        }

        private ReportTemplate CreateNew(ZybachDbContext dbContext, ReportTemplateNewDto reportTemplateNewDto, FileResource newFileResource)
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

            return reportTemplate;
        }

        private ReportTemplate UpdateReportTemplate(ZybachDbContext dbContext, ReportTemplate reportTemplate, ReportTemplateUpdateDto reportTemplateUpdateDto, FileResource newFileResource)
        {
            // null check occurs in calling endpoint method.
            reportTemplate.DisplayName = reportTemplateUpdateDto.DisplayName;
            reportTemplate.Description = reportTemplateUpdateDto.Description;
            reportTemplate.ReportTemplateModelID = reportTemplateUpdateDto.ReportTemplateModelID;
            if (newFileResource != null)
            {
                reportTemplate.FileResource = newFileResource;
            }

            return reportTemplate;
        }

        [HttpPost("/api/reportTemplates/generateReports")]
        [AdminFeature]
        public ActionResult GenerateReports([FromBody] GenerateReportsDto generateReportsDto)
        {
            var reportTemplateID = generateReportsDto.ReportTemplateID;
            var reportTemplate = EFModels.Entities.ReportTemplates.GetByReportTemplateID(_dbContext, reportTemplateID);

            var selectedModelIDs = generateReportsDto.ModelIDList;
            if (selectedModelIDs == null)
            {
                return RequireNotNullThrowNotFound(generateReportsDto,
                    "GenerateReportsDto", selectedModelIDs);
            }

            var reportTemplateGenerator = new ReportTemplateGenerator(reportTemplate, selectedModelIDs);
            return GenerateAndDownload(reportTemplateGenerator, reportTemplate);
        }

        private ActionResult GenerateAndDownload(ReportTemplateGenerator reportTemplateGenerator, ReportTemplate reportTemplate)
        {
            reportTemplateGenerator.Generate(_dbContext);
            var fileData = System.IO.File.ReadAllBytes(reportTemplateGenerator.GetCompilePath());
            var stream = new MemoryStream(fileData);
            return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{reportTemplate.DisplayName} Report");
        }
    }
}
