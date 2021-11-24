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
    public class ChemigationPermitAnnualRecordController : SitkaController<ChemigationPermitAnnualRecordController>
    {
        public ChemigationPermitAnnualRecordController(ZybachDbContext dbContext, ILogger<ChemigationPermitAnnualRecordController> logger,
            KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext,
            logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/api/chemigationPermits/injectionUnitTypes")]
        [ZybachViewFeature]
        public ActionResult<List<ChemigationInjectionUnitTypeDto>> GetChemigationInjectionUnitTypes()
        {
            var chemigationInjectionUnitTypes = ChemigationPermitAnnualRecord.GetChemigationInjectionUnitTypes(_dbContext);
            return Ok(chemigationInjectionUnitTypes);
        }

        [HttpGet("/api/chemigationPermits/annualRecords/annualRecordStatuses")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationPermitAnnualRecordStatusDto>> GetChemigationPermitAnnualRecordStatuses()
        {
            var chemigationPermitAnnualRecordStatusesDto = ChemigationPermitAnnualRecordStatus.List(_dbContext);
            return Ok(chemigationPermitAnnualRecordStatusesDto);
        }

        [HttpGet("/api/chemigationPermits/getByID/{chemigationPermitID}/annualRecords")]
        [ZybachViewFeature]
        public ActionResult<List<ChemigationPermitAnnualRecordDto>> GetChemigationPermitAnnualRecordsByPermitID([FromRoute] int chemigationPermitID)
        {
            var chemigationPermitAnnualRecords = ChemigationPermitAnnualRecord.GetChemigationPermitAnnualRecordsByChemigationPermitID(_dbContext, chemigationPermitID);
            return Ok(chemigationPermitAnnualRecords);
        }

        [HttpGet("/api/chemigationPermits/getByPermitNumber/{chemigationPermitNumber}/annualRecords")]
        [ZybachViewFeature]
        public ActionResult<List<ChemigationPermitAnnualRecordDto>> GetChemigationPermitAnnualRecordsByPermitNumber([FromRoute] int chemigationPermitNumber)
        {
            var chemigationPermitAnnualRecords = ChemigationPermitAnnualRecord.GetChemigationPermitAnnualRecordsByChemigationPermitNumber(_dbContext, chemigationPermitNumber);
            return Ok(chemigationPermitAnnualRecords);
        }

        [HttpGet("/api/chemigationPermits/getByPermitNumber/{chemigationPermitNumber}/annualRecords/getLatestRecordYear")]
        [ZybachViewFeature]
        public ActionResult<ChemigationPermitAnnualRecordDto> GetLatestAnnualRecordByChemigationPermitNumber([FromRoute] int chemigationPermitNumber)
        {
            var latestAnnualRecordDto =
                ChemigationPermitAnnualRecord.GetLatestAnnualRecordByChemigationPermitNumber(_dbContext, chemigationPermitNumber);

            if (ThrowNotFound(latestAnnualRecordDto, "ChemigationPermitAnnualRecord", chemigationPermitNumber, out var actionResult))
            {
                return actionResult;
            }

            return Ok(latestAnnualRecordDto);
        }

        [HttpGet("/api/chemigationPermits/getByID/{chemigationPermitID}/annualRecords/getLatestRecordYear")]
        [ZybachViewFeature]
        public ActionResult<ChemigationPermitAnnualRecordDto> GetLatestAnnualRecordByChemigationPermitID([FromRoute] int chemigationPermitID)
        {
            var latestAnnualRecordDto =
                ChemigationPermitAnnualRecord.GetLatestAnnualRecordByChemigationPermitID(_dbContext, chemigationPermitID);

            if (ThrowNotFound(latestAnnualRecordDto, "ChemigationPermitAnnualRecord", chemigationPermitID, out var actionResult))
            {
                return actionResult;
            }

            return Ok(latestAnnualRecordDto);
        }

        [HttpGet("/api/chemigationPermits/getByPermitNumber/{chemigationPermitNumber}/annualRecords/{recordYear}")]
        [ZybachViewFeature]
        public ActionResult<List<ChemigationPermitAnnualRecordDto>> GetChemigationPermitAnnualRecordByPermitNumberAndRecordYear([FromRoute] int chemigationPermitNumber, [FromRoute] int recordYear)
        {
            var chemigationPermitAnnualRecord = ChemigationPermitAnnualRecord.GetAnnualRecordByPermitNumberAndRecordYear(_dbContext, chemigationPermitNumber, recordYear);

            if (ThrowNotFound(chemigationPermitAnnualRecord, "ChemigationPermitAnnualRecord", chemigationPermitNumber, out var actionResult))
            {
                return actionResult;
            }

            return Ok(chemigationPermitAnnualRecord);
        }

        [HttpGet("/api/chemigationPermits/getByID/{chemigationPermitID}/annualRecords/{recordYear}")]
        [ZybachViewFeature]
        public ActionResult<List<ChemigationPermitAnnualRecordDto>> GetChemigationPermitAnnualRecordByPermitIDAndRecordYear([FromRoute] int chemigationPermitID, [FromRoute] int recordYear)
        {
            var chemigationPermitAnnualRecord = ChemigationPermitAnnualRecord.GetAnnualRecordByPermitIDAndRecordYear(_dbContext, chemigationPermitID, recordYear);

            if (ThrowNotFound(chemigationPermitAnnualRecord, "ChemigationPermitAnnualRecord", chemigationPermitID, out var actionResult))
            {
                return actionResult;
            }

            return Ok(chemigationPermitAnnualRecord);
        }

        [HttpPut("/api/chemigationPermits/annualRecords/{chemigationPermitAnnualRecordID}")]
        [AdminFeature]
        public ActionResult<ChemigationPermitAnnualRecordDto> UpdateChemigationPermitAnnualRecord([FromRoute] int chemigationPermitAnnualRecordID,
            [FromBody] ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto)
        {
            var chemigationPermitAnnualRecord = _dbContext.ChemigationPermitAnnualRecords.SingleOrDefault(x =>
                    x.ChemigationPermitAnnualRecordID == chemigationPermitAnnualRecordID);

            if (ThrowNotFound(chemigationPermitAnnualRecord, "ChemigationPermitAnnualRecord",
                chemigationPermitAnnualRecordID, out var actionResult))
            {
                return actionResult;
            }

            var updatedChemigationPermitAnnualRecordDto =
                ChemigationPermitAnnualRecord.UpdateAnnualRecord(_dbContext, chemigationPermitAnnualRecord, chemigationPermitAnnualRecordUpsertDto);

            return Ok(updatedChemigationPermitAnnualRecordDto);
        }

        [HttpPost("/api/chemigationPermits/{chemigationPermitID}/annualRecords")]
        [AdminFeature]
        public ActionResult<ChemigationPermitAnnualRecordDto> CreateChemigationPermitAnnualRecord([FromRoute] int chemigationPermitID, [FromBody] ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto)
        {
            if (ChemigationPermitAnnualRecord.DoesChemigationPermitAnnualRecordExistForYear(_dbContext,
                chemigationPermitID,
                chemigationPermitAnnualRecordUpsertDto.RecordYear))
            {
                ModelState.AddModelError("ChemigationPermitAnnualRecord", "Annual record already exists for this year");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chemigationPermitAnnualRecordDto =
                ChemigationPermitAnnualRecord.CreateAnnualRecord(_dbContext, chemigationPermitAnnualRecordUpsertDto, 1);
            return Ok(chemigationPermitAnnualRecordDto);
        }

    }

}