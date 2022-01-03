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
            var chemigationPermitStatusesDto = ChemigationPermitStatuses.ListAsDto(_dbContext);
            return Ok(chemigationPermitStatusesDto);
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
        public ActionResult<ChemigationPermitDto> GetChemigationPermitByPermitNumberAsDto([FromRoute] int chemigationPermitNumber)
        {
            var chemigationPermitDto = ChemigationPermits.GetByPermitNumberAsDto(_dbContext, chemigationPermitNumber);
            return RequireNotNullThrowNotFound(chemigationPermitDto, "ChemigationPermit", chemigationPermitNumber);
        }

        [HttpPost("/api/chemigationPermits")]
        [AdminFeature]
        public ActionResult<ChemigationPermitDto> CreateChemigationPermit([FromBody] ChemigationPermitNewDto chemigationPermitNewDto)
        {
            var chemigationPermit = ChemigationPermits.CreateNewChemigationPermit(_dbContext, chemigationPermitNewDto);
            return Ok(chemigationPermit);
        }

        [HttpPut("api/chemigationPermits/{chemigationPermitID}")]
        [AdminFeature]
        public ActionResult UpdateChemigationPermit([FromRoute] int chemigationPermitID, [FromBody] ChemigationPermitUpsertDto chemigationPermitUpsertDto)
        {
            var chemigationPermit = ChemigationPermits.GetByID(_dbContext, chemigationPermitID);

            if (ThrowNotFound(chemigationPermit, "ChemigationPermit", chemigationPermitID, out var actionResult))
            {
                return actionResult;
            }

            chemigationPermit.ChemigationPermitStatusID = chemigationPermitUpsertDto.ChemigationPermitStatusID;
            chemigationPermit.CountyID = chemigationPermitUpsertDto.CountyID;
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("/api/chemigationPermits/{chemigationPermitID}")]
        [AdminFeature]
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