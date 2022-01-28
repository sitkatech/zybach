using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class PrintController : SitkaController<PrintController>
    {
        private readonly SitkaCaptureService.SitkaCaptureService _sitkaCaptureService;

        public PrintController(
            ZybachDbContext dbContext,
            ILogger<PrintController> logger,
            KeystoneService keystoneService,
            IOptions<ZybachConfiguration> configuration,
            SitkaCaptureService.SitkaCaptureService sitkaCaptureService
        ) : base(dbContext, logger, keystoneService, configuration)
        {
            _sitkaCaptureService = sitkaCaptureService;
        }

        [HttpPost("api/print/pdf")]
        [LoggedInUnclassifiedFeature]
        public async Task<ActionResult> PrintPdf([FromBody] CapturePostData capturePostData)
        {
            var currentUser = UserContext.GetUserFromHttpContext(_dbContext, HttpContext);

            _logger.LogInformation($"Print request from User {currentUser.FirstName} {currentUser.LastName} (ID:{currentUser.UserID}) with body {JsonSerializer.Serialize(capturePostData)}");

            try
            {
                var pdf = await _sitkaCaptureService.PrintPDF(capturePostData);
                Response.Headers.Add("Content-Disposition", "inline; filename=test.pdf");
                return File(pdf, "application/pdf");
            }
            catch (Exception ex)
            {
                return BadRequest("There was an error printing");
            }
        }

        [HttpPost("api/print/image")]
        [LoggedInUnclassifiedFeature]
        public async Task<ActionResult> PrintImage([FromBody] CapturePostData capturePostData)
        {
            var currentUser = UserContext.GetUserFromHttpContext(_dbContext, HttpContext);

            _logger.LogInformation($"Print request from User {currentUser.FirstName} {currentUser.LastName} (ID:{currentUser.UserID}) with body {JsonSerializer.Serialize(capturePostData)}");

            try
            {
                var image = await _sitkaCaptureService.PrintImage(capturePostData);
                Response.Headers.Add("Content-Disposition", "inline; filename=test.png");
                return File(image, "image/png"); // default to png for now
            }
            catch (Exception ex)
            {
                return BadRequest("There was an error printing");
            }
        }

    }
}