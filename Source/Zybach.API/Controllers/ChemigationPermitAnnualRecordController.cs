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

        [HttpGet("/api/chemigationPermits/injectionUnitTypes")]
        //[ZybachViewFeature]
        public ActionResult<List<ChemigationInjectionUnitTypeDto>> GetChemigationInjectionUnitTypes()
        {
            var chemigationInjectionUnitTypes = ChemigationPermitAnnualRecord.GetChemigationInjectionUnitTypes(_dbContext);
            return Ok(chemigationInjectionUnitTypes);
        }

        [HttpGet("/api/chemigationPermits/getByID/{chemigationPermitID}/annualRecords")]
        //[ZybachViewFeature]
        public ActionResult<List<ChemigationPermitAnnualRecordDto>> GetChemigationPermitAnnualRecordsByPermitID([FromRoute] int chemigationPermitID)
        {
            var chemigationPermitAnnualRecords = ChemigationPermitAnnualRecord.GetChemigationPermitAnnualRecordsByChemigationPermitID(_dbContext, chemigationPermitID);
            return Ok(chemigationPermitAnnualRecords);
        }

        [HttpGet("/api/chemigationPermits/getByPermitNumber/{chemigationPermitNumber}/annualRecords")]
        //[ZybachViewFeature]
        public ActionResult<List<ChemigationPermitAnnualRecordDto>> GetChemigationPermitAnnualRecordsByPermitNumber([FromRoute] int chemigationPermitNumber)
        {
            var chemigationPermitAnnualRecords = ChemigationPermitAnnualRecord.GetChemigationPermitAnnualRecordsByChemigationPermitNumber(_dbContext, chemigationPermitNumber);
            return Ok(chemigationPermitAnnualRecords);
        }

        [HttpGet("/api/chemigationPermits/getByPermitNumber/{chemigationPermitNumber}/annualRecords/getLatestRecordYear")]
        //[ZybachViewFeature]
        public ActionResult<ChemigationPermitAnnualRecordDto> GetLatestAnnualRecordByChemigationPermitNumber([FromRoute] int chemigationPermitNumber)
        {
            var latestAnnualRecordDto =
                ChemigationPermitAnnualRecord.GetLatestAnnualRecordByChemigationPermitNumber(_dbContext, chemigationPermitNumber);
            return Ok(latestAnnualRecordDto);
        }

        [HttpGet("/api/chemigationPermits/getByID/{chemigationPermitID}/annualRecords/getLatestRecordYear")]
        //[ZybachViewFeature]
        public ActionResult<ChemigationPermitAnnualRecordDto> GetLatestAnnualRecordByChemigationPermitID([FromRoute] int chemigationPermitID)
        {
            var latestAnnualRecordDto =
                ChemigationPermitAnnualRecord.GetLatestAnnualRecordByChemigationPermitID(_dbContext, chemigationPermitID);
            return Ok(latestAnnualRecordDto);
        }

        [HttpPut("/api/chemigationPermits/annualRecords/{chemigationPermitAnnualRecordID}")]
        //[ZybachAdminFeature]
        public ActionResult<ChemigationPermitAnnualRecordDto> UpdateChemigationPermitAnnualRecord([FromRoute] int chemigationPermitAnnualRecordID,
            [FromBody] ChemigationPermitAnnualRecordDto chemigationPermitAnnualRecordUpsertDto)
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

        [HttpPost("/api/chemigationPermits/annualRecords")]
        //[ZybachAdminFeature]
        public ActionResult<ChemigationPermitAnnualRecordDto> CreateChemigationPermitAnnualRecord([FromBody] ChemigationPermitAnnualRecordDto newChemigationPermitAnnualRecordDto)
        {
            // TODO: figure out validation

            var chemigationPermitAnnualRecordDto =
                ChemigationPermitAnnualRecord.CreateAnnualRecord(_dbContext, newChemigationPermitAnnualRecordDto);
            return Ok(chemigationPermitAnnualRecordDto);
        }

    }

}