using System;
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
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ChemigationPermitController : SitkaController<ChemigationPermitController>
    {
        public ChemigationPermitController(ZybachDbContext dbContext, ILogger<ChemigationPermitController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/api/chemigationPermits/permitStatuses")]
        //[ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationPermitStatusDto>> GetChemigationPermitStatuses()
        {
            var chemigationPermitStatusesDto = ChemigationPermitStatus.List(_dbContext);
            return Ok(chemigationPermitStatusesDto);
        }

        [HttpGet("/api/chemigationPermits")]
        //[ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationPermitDto>> GetAllChemigationPermits()
        {
            var chemigationPermitsDto = ChemigationPermit.List(_dbContext);
            return Ok(chemigationPermitsDto);
        }

        [HttpGet("/api/chemigationPermits/getByID/{chemigationPermitID}")]
        //[ZybachViewFeature]
        public ActionResult<ChemigationPermitDto> GetChemigationPermitByID([FromRoute] int chemigationPermitID)
        {
            var chemigationPermitDto = ChemigationPermit.GetChemigationPermitByID(_dbContext, chemigationPermitID);
            return RequireNotNullThrowNotFound(chemigationPermitDto, "ChemigationPermit", chemigationPermitID);
        }

        [HttpGet("/api/chemigationPermits/getByPermitNumber/{chemigationPermitNumber}")]
        //[ZybachViewFeature]
        public ActionResult<ChemigationPermitDto> GetChemigationPermitByPermitNumber([FromRoute] int chemigationPermitNumber)
        {
            var chemigationPermitDto = ChemigationPermit.GetChemigationPermitByNumber(_dbContext, chemigationPermitNumber);
            return RequireNotNullThrowNotFound(chemigationPermitDto, "ChemigationPermit", chemigationPermitNumber);
        }

        [HttpPost("/api/chemigationPermits")]
        //[AdminFeature]
        public ActionResult<ChemigationPermitDto> CreateChemigationPermit([FromBody] ChemigationPermitNewDto chemigationPermitNewDto)
        {
            //RunChemigationPermitUpsertValidation(chemigationPermitNewDto, null);

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            var chemigationPermit =
                ChemigationPermit.CreateNewChemigationPermit(_dbContext, chemigationPermitNewDto);

            return Ok(chemigationPermit);
        }

        [HttpPut("api/chemigationPermits/{chemigationPermitID}")]
        //[AdminFeature]
        public ActionResult<ChemigationPermitDto> UpdateChemigationPermit([FromRoute] int chemigationPermitID, [FromBody] ChemigationPermitUpsertDto chemigationPermitUpsertDto)
        {
            var chemigationPermit = _dbContext.ChemigationPermits.SingleOrDefault(x => x.ChemigationPermitID == chemigationPermitID);

            if (ThrowNotFound(chemigationPermit, "ChemigationPermit", chemigationPermitID, out var actionResult))
            {
                return actionResult;
            }

            var currentID = chemigationPermit.ChemigationPermitID;

            RunChemigationPermitUpsertValidation(chemigationPermitUpsertDto, currentID);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedChemigationPermitDto = ChemigationPermit.UpdateChemigationPermit(_dbContext, chemigationPermit, chemigationPermitUpsertDto);

            return Ok(updatedChemigationPermitDto);
        }

        [HttpDelete("/api/chemigationPermits/{chemigationPermitID}")]
        //[AdminFeature]
        public ActionResult DeleteChemigationPermitByID([FromRoute] int chemigationPermitID)
        {
            var chemigationPermitDto = ChemigationPermit.GetChemigationPermitByID(_dbContext, chemigationPermitID);

            if (ThrowNotFound(chemigationPermitDto, "ChemigationPermit", chemigationPermitID, out var actionResult))
            {
                return actionResult;
            }

            ChemigationPermit.DeleteByChemigationPermitID(_dbContext, chemigationPermitID);

            return Ok();
        }

        private void RunChemigationPermitUpsertValidation(ChemigationPermitUpsertDto chemigationPermitUpsertDto, int? currentID)
        {
            if (ChemigationPermit.IsChemigationPermitNumberUnique(_dbContext, chemigationPermitUpsertDto.ChemigationPermitNumber, currentID))
            {
                ModelState.AddModelError("ChemigationPermitNumber", "Permit Number must be unique");
            }

        }

    }

}