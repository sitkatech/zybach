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
    public class CustomRichTextController : SitkaController<CustomRichTextController>
    {
        public CustomRichTextController(ZybachDbContext dbContext, ILogger<CustomRichTextController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/customRichText/{customRichTextTypeID}")]
        public ActionResult<CustomRichTextDto> GetCustomRichText([FromRoute] int customRichTextTypeID)
        {
            var customRichTextDto = CustomRichTexts.GetByCustomRichTextTypeID(_dbContext, customRichTextTypeID);
            return RequireNotNullThrowNotFound(customRichTextDto, "CustomRichText", customRichTextTypeID);
        }

        [HttpPut("/customRichText/{customRichTextTypeID}")]
        [ZybachViewFeature]
        public ActionResult<CustomRichTextDto> UpdateCustomRichText([FromRoute] int customRichTextTypeID, [FromBody] CustomRichTextDto customRichTextUpdateDto)
        {
            var customRichTextDto = CustomRichTexts.GetByCustomRichTextTypeID(_dbContext, customRichTextTypeID);
            if (ThrowNotFound(customRichTextDto, "CustomRichText", customRichTextTypeID, out var actionResult))
            {
                return actionResult;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedCustomRichTextDto =
                CustomRichTexts.UpdateCustomRichText(_dbContext, customRichTextTypeID, customRichTextUpdateDto);
            return Ok(updatedCustomRichTextDto);
        }
    }
}
