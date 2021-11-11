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
    public class ChemigationPermitAnnualRecordController : SitkaController<ChemigationPermitAnnualRecordController>
    {
        public ChemigationPermitAnnualRecordController(ZybachDbContext dbContext, ILogger<ChemigationPermitAnnualRecordController> logger,
            KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext,
            logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/api/chemigationPermitAnnualRecords/getByPermitID/{chemigationPermitID}")]
        //[ZybachViewFeature]
        public ActionResult<List<ChemigationPermitAnnualRecordDto>> GetChemigationPermitAnnualRecordsByPermitID([FromRoute] int chemigationPermitID)
        {
            var chemigationPermitAnnualRecords = ChemigationPermitAnnualRecord.GetChemigationPermitAnnualRecordsByChemigationPermitID(_dbContext, chemigationPermitID);
            return Ok(chemigationPermitAnnualRecords);
        }


        [HttpPut("/api/chemigationPermitAnnualRecords/{chemigationPermitAnnualRecordID}")]
        //[ZybachAdminFeature]
        public ActionResult<ChemigationPermitAnnualRecordDto> UpdateChemigationPermitAnnualRecord([FromRoute] int chemigationPermitAnnualRecordID,
            [FromBody] ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto)
        {
            // TODO: figure out validation

            var chemigationPermitAnnualRecord = _dbContext.ChemigationPermitAnnualRecords.SingleOrDefault(x =>
                    x.ChemigationPermitAnnualRecordID == chemigationPermitAnnualRecordID);

            if (ThrowNotFound(chemigationPermitAnnualRecord, "ChemigationPermitAnnualRecord", chemigationPermitAnnualRecordID, out var actionResult))
            {
                return actionResult;
            }

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            var updatedChemigationPermitAnnualRecordDto =
                ChemigationPermitAnnualRecord.UpdateAnnualRecord(_dbContext, chemigationPermitAnnualRecord, chemigationPermitAnnualRecordUpsertDto);

            return Ok(updatedChemigationPermitAnnualRecordDto);
        }

        [HttpPost("/api/chemigationPermitAnnualRecords")]
        //[ZybachAdminFeature]
        public ActionResult<ChemigationPermitAnnualRecordDto> CreateChemigationPermitAnnualRecord([FromBody] ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto)
        {
            // TODO: figure out validation

            var chemigationPermitAnnualRecordDto =
                ChemigationPermitAnnualRecord.CreateAnnualRecord(_dbContext, chemigationPermitAnnualRecordUpsertDto);
            return Ok(chemigationPermitAnnualRecordDto);
        }

    }

}