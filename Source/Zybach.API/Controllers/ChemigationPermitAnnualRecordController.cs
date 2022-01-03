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

        [HttpGet("/api/chemigationInjectionUnitTypes")]
        [ZybachViewFeature]
        public ActionResult<List<ChemigationInjectionUnitTypeDto>> GetChemigationInjectionUnitTypes()
        {
            var chemigationInjectionUnitTypes = ChemigationInjectionUnitTypes.ListAsDto(_dbContext);
            return Ok(chemigationInjectionUnitTypes);
        }

        [HttpGet("/api/chemigationPermitAnnualRecordStatuses")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationPermitAnnualRecordStatusDto>> GetChemigationPermitAnnualRecordStatuses()
        {
            var chemigationPermitAnnualRecordStatusesDto = ChemigationPermitAnnualRecordStatus.List(_dbContext);
            return Ok(chemigationPermitAnnualRecordStatusesDto);
        }

        [HttpGet("/api/chemigationPermitAnnualRecords")]
        [ZybachViewFeature]
        public ActionResult<List<ChemigationPermitAnnualRecordDetailedDto>> GetAllChemigationPermitAnnualRecords()
        {
            var chemigationPermitAnnualRecords =
                ChemigationPermitAnnualRecords.ListAsDetailedDto(_dbContext);
            return Ok(chemigationPermitAnnualRecords);
        }

        [HttpPost("/api/chemigationPermitAnnualRecords/{recordYear}")]
        [AdminFeature]
        public ActionResult<int> BulkCreateChemigationPermitAnnualRecords([FromRoute] int recordYear)
        {
            var chemigationPermitsRenewed = ChemigationPermits.BulkCreateRenewalRecords(_dbContext, recordYear);
            return Ok(chemigationPermitsRenewed);
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

        [HttpGet("/api/chemigationPermits/{chemigationPermitNumber}/annualRecords")]
        [ZybachViewFeature]
        public ActionResult<List<ChemigationPermitAnnualRecordDetailedDto>> GetChemigationPermitAnnualRecordsByPermitNumber([FromRoute] int chemigationPermitNumber)
        {
            var chemigationPermitAnnualRecords = ChemigationPermitAnnualRecords.ListByChemigationPermitNumberAsDetailedDto(_dbContext, chemigationPermitNumber);
            return Ok(chemigationPermitAnnualRecords);
        }

        [HttpPost("/api/chemigationPermits/{chemigationPermitID}/annualRecords")]
        [AdminFeature]
        public ActionResult<ChemigationPermitAnnualRecordDto> CreateChemigationPermitAnnualRecord([FromRoute] int chemigationPermitID, [FromBody] ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto)
        {
            var chemigationPermit = ChemigationPermits.GetByID(_dbContext, chemigationPermitID);

            if (ThrowNotFound(chemigationPermit, "ChemigationPermit", chemigationPermitID, out var actionResult))
            {
                return actionResult;
            }

            var chemigationPermitAnnualRecords = ChemigationPermitAnnualRecords.ListByChemigationPermitID(_dbContext, chemigationPermitID);
            if (chemigationPermitAnnualRecords.Any(x => x.RecordYear == chemigationPermitAnnualRecordUpsertDto.RecordYear))
            {
                ModelState.AddModelError("ChemigationPermitAnnualRecord", "Annual record already exists for this year");
                return BadRequest(ModelState);
            }

            var mostRecentChemigationPermitAnnualRecord = chemigationPermitAnnualRecords.OrderByDescending(x => x.RecordYear).FirstOrDefault();

            chemigationPermitAnnualRecordUpsertDto.NDEEAmount = mostRecentChemigationPermitAnnualRecord != null && chemigationPermitAnnualRecordUpsertDto.RecordYear - mostRecentChemigationPermitAnnualRecord.RecordYear == 1 ?
                ChemigationPermitAnnualRecords.NDEEAmounts.Renewal : ChemigationPermitAnnualRecords.NDEEAmounts.New;

            ChemigationPermitAnnualRecords.CreateAnnualRecord(_dbContext, chemigationPermitAnnualRecordUpsertDto, chemigationPermitID);
            return Ok();
        }

        [HttpPut("/api/chemigationPermitAnnualRecords/{chemigationPermitAnnualRecordID}")]
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
    }
}