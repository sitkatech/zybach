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
    public class ChemigationPermitController : SitkaController<ChemigationPermitController>
    {
        public ChemigationPermitController(ZybachDbContext dbContext, ILogger<ChemigationPermitController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/api/chemigationPermitStatuses")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationPermitStatusDto>> GetChemigationPermitStatuses()
        {
            var chemigationPermitStatusesDto = ChemigationPermitStatus.List(_dbContext);
            return Ok(chemigationPermitStatusesDto);
        }

        [HttpGet("/api/counties")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationCountyDto>> GetChemigationCounties()
        {
            var chemigationCounties = ChemigationCounty.List(_dbContext);
            return Ok(chemigationCounties);
        }

        [HttpGet("/api/chemigationPermits")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationPermitDetailedDto>> ListChemigationPermits()
        {
            var chemigationPermitDetailedDtos = ChemigationPermits.ListWithLatestAnnualRecordAsDto(_dbContext);
            return Ok(chemigationPermitDetailedDtos);
        }

        [HttpGet("/api/chemigationPermits/{chemigationPermitNumber}")]
        [ZybachViewFeature]
        public ActionResult<ChemigationPermitDto> GetChemigationPermitByPermitNumber([FromRoute] int chemigationPermitNumber)
        {
            var chemigationPermitDto = ChemigationPermits.GetChemigationPermitByNumber(_dbContext, chemigationPermitNumber);
            return RequireNotNullThrowNotFound(chemigationPermitDto, "ChemigationPermit", chemigationPermitNumber);
        }

        [HttpPost("/api/chemigationPermits")]
        [AdminFeature]
        public ActionResult<ChemigationPermitDto> CreateChemigationPermit([FromBody] ChemigationPermitNewDto chemigationPermitNewDto)
        {
            if (ChemigationPermits.IsChemigationPermitNumberUnique(_dbContext, chemigationPermitNewDto.ChemigationPermitNumber, null))
            {
                ModelState.AddModelError("ChemigationPermitNumber", "Permit Number must be unique");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chemigationPermit = ChemigationPermits.CreateNewChemigationPermit(_dbContext, chemigationPermitNewDto);
            return Ok(chemigationPermit);
        }

        [HttpPut("api/chemigationPermits/{chemigationPermitID}")]
        [AdminFeature]
        public ActionResult<ChemigationPermitDto> UpdateChemigationPermit([FromRoute] int chemigationPermitID, [FromBody] ChemigationPermitUpsertDto chemigationPermitUpsertDto)
        {
            var chemigationPermit = _dbContext.ChemigationPermits.SingleOrDefault(x => x.ChemigationPermitID == chemigationPermitID);

            if (ThrowNotFound(chemigationPermit, "ChemigationPermit", chemigationPermitID, out var actionResult))
            {
                return actionResult;
            }

            RunChemigationPermitUpsertValidation(chemigationPermitUpsertDto, chemigationPermitID);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedChemigationPermitDto = ChemigationPermits.UpdateChemigationPermit(_dbContext, chemigationPermit, chemigationPermitUpsertDto);
            return Ok(updatedChemigationPermitDto);
        }

        [HttpDelete("/api/chemigationPermits/{chemigationPermitID}")]
        [AdminFeature]
        public ActionResult DeleteChemigationPermitByID([FromRoute] int chemigationPermitID)
        {
            var chemigationPermitDto = ChemigationPermits.GetChemigationPermitByID(_dbContext, chemigationPermitID);

            if (ThrowNotFound(chemigationPermitDto, "ChemigationPermit", chemigationPermitID, out var actionResult))
            {
                return actionResult;
            }

            ChemigationPermits.DeleteByChemigationPermitID(_dbContext, chemigationPermitID);

            return Ok();
        }

        private void RunChemigationPermitUpsertValidation(ChemigationPermitUpsertDto chemigationPermitUpsertDto, int? currentID)
        {
            if (ChemigationPermits.IsChemigationPermitNumberUnique(_dbContext, chemigationPermitUpsertDto.ChemigationPermitNumber, currentID))
            {
                ModelState.AddModelError("ChemigationPermitNumber", "Permit Number must be unique");
            }
        }
    }
}