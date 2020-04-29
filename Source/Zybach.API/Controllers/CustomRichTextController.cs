using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class CustomRichTextController
        : ControllerBase
    {
        private readonly ZybachDbContext _dbContext;
        private readonly ILogger<RoleController> _logger;
        private readonly KeystoneService _keystoneService;

        public CustomRichTextController(ZybachDbContext dbContext, ILogger<RoleController> logger, KeystoneService keystoneService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _keystoneService = keystoneService;
        }



        [HttpGet("customRichText/{customRichTextTypeID}")]
        public ActionResult<CustomRichTextDto> GetCustomRichText([FromRoute] int customRichTextTypeID)
        {
            var customRichTextDto = CustomRichText.GetByCustomRichTextTypeID(_dbContext, customRichTextTypeID);
            if (customRichTextDto == null)
            {
                return NotFound();
            }

            return Ok(customRichTextDto);
        }

        [HttpPut("customRichText/{customRichTextTypeID}")]
        [ContentManageFeature]
        public ActionResult<CustomRichTextDto> UpdateCustomRichText([FromRoute] int customRichTextTypeID,
            [FromBody] CustomRichTextDto customRichTextUpdateDto)
        {
            var customRichTextDto = CustomRichText.GetByCustomRichTextTypeID(_dbContext, customRichTextTypeID);
            if (customRichTextDto == null)
            {
                return NotFound();
            }

            CustomRichTextDto updatedCustomRichTextDto =
                CustomRichText.UpdateCustomRichText(_dbContext, customRichTextTypeID, customRichTextUpdateDto);

            return Ok(updatedCustomRichTextDto);
        }
    }
}
