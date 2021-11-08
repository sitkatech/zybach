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
    public class ChemigationPermitController : SitkaController<ChemigationPermitController>
    {
        public ChemigationPermitController(ZybachDbContext dbContext, ILogger<ChemigationPermitController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/api/chemigationPermits")]
        [ZybachViewFeature]
        public IEnumerable<ChemigationPermitDto> GetAllChemigationPermits()
        {
            var chemigationPermitsDto = ChemigationPermit.List(_dbContext);
            return chemigationPermitsDto;
        }

        [HttpPost("/api/chemigationPermits")]
        [ZybachViewFeature]
        public ActionResult<ChemigationPermitDto> CreateChemigationPermit([FromBody] ChemigationPermitUpsertDto chemigationPermitUpsertDto)
        {
            RunChemigationPermitUpsertValidation(_dbContext, chemigationPermitUpsertDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chemigationPermit =
                ChemigationPermit.CreateNewChemigationPermit(_dbContext, chemigationPermitUpsertDto);

            return Ok(chemigationPermit);
        }

        private void RunChemigationPermitUpsertValidation(ZybachDbContext dbContext, ChemigationPermitUpsertDto chemigationPermitUpsertDto)
        {
            if (ChemigationPermit.DoesStatusExist(dbContext, chemigationPermitUpsertDto.ChemigationPermitStatusID))
            {
                ModelState.AddModelError("ChemigationPermitStatusID", "Status with that ID not found. Please enter a valid status ID.");
            }
        }

    }

}