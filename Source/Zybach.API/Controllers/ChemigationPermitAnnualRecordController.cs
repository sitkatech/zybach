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
            var chemigationInjectionUnitTypes = ChemigationInjectionUnitTypes.ListAsDto(_dbContext);
            return Ok(chemigationInjectionUnitTypes);
        }

        [HttpGet("/api/chemigationPermits/annualRecords/annualRecordStatuses")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationPermitAnnualRecordStatusDto>> GetChemigationPermitAnnualRecordStatuses()
        {
            var chemigationPermitAnnualRecordStatusesDto = ChemigationPermitAnnualRecordStatus.List(_dbContext);
            return Ok(chemigationPermitAnnualRecordStatusesDto);
        }

        [HttpGet("/api/chemigationPermits/annualRecords")]
        [ZybachViewFeature]
        public ActionResult<List<ChemigationPermitAnnualRecordDetailedDto>> GetAllChemigationPermitAnnualRecords()
        {
            var chemigationPermitAnnualRecords =
                ChemigationPermitAnnualRecords.ListAsDetailedDto(_dbContext);
            return Ok(chemigationPermitAnnualRecords);
        }

        [HttpGet("/api/chemigationPermits/{chemigationPermitNumber}/annualRecords")]
        [ZybachViewFeature]
        public ActionResult<List<ChemigationPermitAnnualRecordDetailedDto>> GetChemigationPermitAnnualRecordsByPermitNumber([FromRoute] int chemigationPermitNumber)
        {
            var chemigationPermitAnnualRecords = ChemigationPermitAnnualRecords.ListByChemigationPermitNumberAsDetailedDto(_dbContext, chemigationPermitNumber);
            return Ok(chemigationPermitAnnualRecords);
        }

        [HttpGet("/api/chemigationPermits/{chemigationPermitNumber}/getLatestRecordYear")]
        [ZybachViewFeature]
        public ActionResult<ChemigationPermitAnnualRecordDetailedDto> GetLatestAnnualRecordByChemigationPermitNumber([FromRoute] int chemigationPermitNumber)
        {
            var latestAnnualRecordDto =
                ChemigationPermitAnnualRecords.GetLatestByChemigationPermitNumberAsDetailedDto(_dbContext, chemigationPermitNumber);

            if (ThrowNotFound(latestAnnualRecordDto, "ChemigationPermitAnnualRecord", chemigationPermitNumber, out var actionResult))
            {
                return actionResult;
            }

            return Ok(latestAnnualRecordDto);
        }

        [HttpGet("/api/chemigationPermits/{chemigationPermitNumber}/{recordYear}")]
        [ZybachViewFeature]
        public ActionResult<ChemigationPermitAnnualRecordDetailedDto> GetChemigationPermitAnnualRecordByPermitNumberAndRecordYear([FromRoute] int chemigationPermitNumber, [FromRoute] int recordYear)
        {
            var chemigationPermitAnnualRecord = ChemigationPermitAnnualRecords.GetByPermitNumberAndRecordYearAsDetailedDto(_dbContext, chemigationPermitNumber, recordYear);

            if (ThrowNotFound(chemigationPermitAnnualRecord, "ChemigationPermitAnnualRecord", chemigationPermitNumber, out var actionResult))
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

            ChemigationPermitAnnualRecords.UpdateAnnualRecord(_dbContext, chemigationPermitAnnualRecord, chemigationPermitAnnualRecordUpsertDto);
            return Ok();
        }

        [HttpPost("/api/chemigationPermits/{chemigationPermitNumber}/annualRecords")]
        [AdminFeature]
        public ActionResult<ChemigationPermitAnnualRecordDto> CreateChemigationPermitAnnualRecord([FromRoute] int chemigationPermitNumber, [FromBody] ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto)
        {
            var chemigationPermitAnnualRecords = ChemigationPermitAnnualRecords.ListByChemigationPermitNumber(_dbContext, chemigationPermitNumber);
            if (!chemigationPermitAnnualRecords.Any())
            {
                return BadRequest($"No Annual Records found for Permit # {chemigationPermitNumber}");
            }
            if (chemigationPermitAnnualRecords.Any(x => x.RecordYear == chemigationPermitAnnualRecordUpsertDto.RecordYear))
            {
                ModelState.AddModelError("ChemigationPermitAnnualRecord", "Annual record already exists for this year");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mostRecentChemigationPermitAnnualRecord = chemigationPermitAnnualRecords.OrderByDescending(x => x.RecordYear).First();

            chemigationPermitAnnualRecordUpsertDto.NDEEAmount = chemigationPermitAnnualRecordUpsertDto.RecordYear - mostRecentChemigationPermitAnnualRecord.RecordYear == 1 ? 
                ChemigationPermitAnnualRecords.NDEEAmounts.Renewal : ChemigationPermitAnnualRecords.NDEEAmounts.New;

            ChemigationPermitAnnualRecords.CreateAnnualRecord(_dbContext, chemigationPermitAnnualRecordUpsertDto, mostRecentChemigationPermitAnnualRecord.ChemigationPermitID);
            return Ok();
        }


        [HttpPost("/api/chemigationPermits/annualRecordsBulkCreate/{recordYear}")]
        [AdminFeature]
        public ActionResult<int> BulkCreateChemigationPermitAnnualRecords([FromRoute] int recordYear)
        {
            var chemigationPermitsRenewed = ChemigationPermits.BulkCreateRenewalRecords(_dbContext, recordYear);
            return Ok(chemigationPermitsRenewed);
        }
    }

}