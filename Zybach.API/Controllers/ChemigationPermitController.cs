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
    public class ChemigationPermitController : SitkaController<ChemigationPermitController>
    {
        public ChemigationPermitController(ZybachDbContext dbContext, ILogger<ChemigationPermitController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/chemigationPermitStatuses")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationPermitStatusDto>> GetChemigationPermitStatuses()
        {
            var chemigationPermitStatusesDto = ChemigationPermitStatuses.ListAsDto(_dbContext);
            return Ok(chemigationPermitStatusesDto);
        }

        [HttpGet("/chemigationPermits")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationPermitDetailedDto>> ListChemigationPermits()
        {
            var chemigationPermitDetailedDtos = ChemigationPermits.ListWithLatestAnnualRecordAsDto(_dbContext);
            return Ok(chemigationPermitDetailedDtos);
        }

        [HttpGet("/chemigationPermits/{chemigationPermitNumber}")]
        [ZybachViewFeature]
        public ActionResult<ChemigationPermitDto> GetChemigationPermitByPermitNumberAsDto([FromRoute] int chemigationPermitNumber)
        {
            var chemigationPermitDto = ChemigationPermits.GetByPermitNumberAsDto(_dbContext, chemigationPermitNumber);
            return RequireNotNullThrowNotFound(chemigationPermitDto, "ChemigationPermit", chemigationPermitNumber);
        }

        [HttpPost("/chemigationPermits")]
        [ZybachEditFeature]
        public ActionResult<ChemigationPermitDto> CreateChemigationPermit([FromBody] ChemigationPermitNewDto chemigationPermitNewDto)
        {
            var chemigationPermit = ChemigationPermits.CreateNewChemigationPermit(_dbContext, chemigationPermitNewDto);
            return Ok(chemigationPermit);
        }

        [HttpPut("/chemigationPermits/{chemigationPermitID}")]
        [ZybachEditFeature]
        public ActionResult UpdateChemigationPermit([FromRoute] int chemigationPermitID, [FromBody] ChemigationPermitUpsertDto chemigationPermitUpsertDto)
        {
            var chemigationPermit = ChemigationPermits.GetByID(_dbContext, chemigationPermitID);
            if (ThrowNotFound(chemigationPermit, "ChemigationPermit", chemigationPermitID, out var actionResult))
            {
                return actionResult;
            }

            var well = Wells.GetByWellRegistrationID(_dbContext, chemigationPermitUpsertDto.WellRegistrationID);
            if (well == null)
            {
                ModelState.AddModelError("Well Registration ID", $"Well with Well Registration ID '{chemigationPermitUpsertDto.WellRegistrationID}' not found!");
                return BadRequest(ModelState);
            }

            chemigationPermit.ChemigationPermitStatusID = chemigationPermitUpsertDto.ChemigationPermitStatusID.Value;
            chemigationPermit.CountyID = chemigationPermitUpsertDto.CountyID.Value;
            chemigationPermit.WellID = well.WellID;
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("/chemigationPermits/{chemigationPermitID}")]
        [ZybachEditFeature]
        public ActionResult DeleteChemigationPermitByID([FromRoute] int chemigationPermitID)
        {
            var chemigationPermit = ChemigationPermits.GetByID(_dbContext, chemigationPermitID);

            if (ThrowNotFound(chemigationPermit, "ChemigationPermit", chemigationPermitID, out var actionResult))
            {
                return actionResult;
            }

            _dbContext.ChemigationPermits.Remove(chemigationPermit);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}