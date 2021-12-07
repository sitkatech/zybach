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

        [HttpGet("/api/chemigationPermits/annualRecords")]
        [ZybachViewFeature]
        public ActionResult<List<ChemigationPermitAnnualRecordDto>> GetAllChemigationPermitAnnualRecords()
        {
            var chemigationPermitAnnualRecords =
                ChemigationPermitAnnualRecord.GetAllChemigationPermitAnnualRecords(_dbContext);
            return Ok(chemigationPermitAnnualRecords);
        }

        [HttpGet("/api/chemigationPermits/{chemigationPermitNumber}/annualRecords")]
        [ZybachViewFeature]
        public ActionResult<List<ChemigationPermitAnnualRecordDetailedDto>> GetChemigationPermitAnnualRecordsByPermitNumber([FromRoute] int chemigationPermitNumber)
        {
            var chemigationPermitAnnualRecords = ChemigationPermitAnnualRecord.ListByChemigationPermitNumberAsDetailedDto(_dbContext, chemigationPermitNumber);
            return Ok(chemigationPermitAnnualRecords);
        }

        [HttpGet("/api/chemigationPermits/{chemigationPermitNumber}/getLatestRecordYear")]
        [ZybachViewFeature]
        public ActionResult<ChemigationPermitAnnualRecordDetailedDto> GetLatestAnnualRecordByChemigationPermitNumber([FromRoute] int chemigationPermitNumber)
        {
            var latestAnnualRecordDto =
                ChemigationPermitAnnualRecord.GetLatestByChemigationPermitNumberAsDetailedDto(_dbContext, chemigationPermitNumber);

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
            var chemigationPermitAnnualRecord = ChemigationPermitAnnualRecord.GetByPermitNumberAndRecordYearAsDetailedDto(_dbContext, chemigationPermitNumber, recordYear);

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
            
            var chemigationPermitAnnualRecordDto = ChemigationPermitAnnualRecord.CreateAnnualRecord(_dbContext, chemigationPermitAnnualRecordUpsertDto, chemigationPermitID);
            return Ok(chemigationPermitAnnualRecordDto);
        }


        [HttpPost("/api/chemigationPermits/annualRecordsBulkCreate/{recordYear}")]
        [AdminFeature]
        public ActionResult<int> BulkCreateChemigationAnnualRecords([FromRoute] int recordYear)
        {
            var chemigationPermitDetailedDtos = ChemigationPermits.ListWithLatestAnnualRecordAsDto(_dbContext)
                .Where(x => x.ChemigationPermitStatus.ChemigationPermitStatusID == (int)ChemigationPermitStatus.ChemigationPermitStatusEnum.Active &&
                            x.LatestAnnualRecord.RecordYear == recordYear - 1)
                .ToList();

            foreach (var chemigationPermitDetailedDto in chemigationPermitDetailedDtos)
            {
                //var chemigationPermitAnnualRecordUpsertDto = new ChemigationPermitAnnualRecordUpsertDto();
                //chemigationPermitAnnualRecordUpsertDto.ChemigationPermitAnnualRecordStatusID = (int)ChemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusEnum.PendingPayment;
                //chemigationPermitAnnualRecordUpsertDto.ChemigationInjectionUnitTypeID = chemigationPermitDetailedDto.ChemigationPermit.ChemigationInjectionUnitTypeID;
                //chemigationPermitAnnualRecordUpsertDto.RecordYear = this.newRecordYear;
                //chemigationPermitAnnualRecordUpsertDto.PivotName = chemigationPermitDetailedDto.ChemigationPermitAnnualRecord.PivotName;
                //chemigationPermitAnnualRecordUpsertDto.ApplicantFirstName = chemigationPermitDetailedDto.ChemigationPermitAnnualRecord.ApplicantFirstName;
                //chemigationPermitAnnualRecordUpsertDto.ApplicantLastName = chemigationPermitDetailedDto.ChemigationPermitAnnualRecord.ApplicantLastName;
                //chemigationPermitAnnualRecordUpsertDto.ApplicantMailingAddress = chemigationPermitDetailedDto.ChemigationPermitAnnualRecord.ApplicantMailingAddress;
                //chemigationPermitAnnualRecordUpsertDto.ApplicantCity = chemigationPermitDetailedDto.ChemigationPermitAnnualRecord.ApplicantCity;
                //chemigationPermitAnnualRecordUpsertDto.ApplicantState = chemigationPermitDetailedDto.ChemigationPermitAnnualRecord.ApplicantState;
                //chemigationPermitAnnualRecordUpsertDto.ApplicantZipCode = chemigationPermitDetailedDto.ChemigationPermitAnnualRecord.ApplicantZipCode;
                //chemigationPermitAnnualRecordUpsertDto.ApplicantPhone = chemigationPermitDetailedDto.ChemigationPermitAnnualRecord.ApplicantPhone;
                //chemigationPermitAnnualRecordUpsertDto.ApplicantMobilePhone = chemigationPermitDetailedDto.ChemigationPermitAnnualRecord.ApplicantMobilePhone;
                //chemigationPermitAnnualRecordUpsertDto.ApplicantEmail = chemigationPermitDetailedDto.ChemigationPermitAnnualRecord.ApplicantEmail;
                //chemigationPermitAnnualRecordUpsertDto.DateReceived = chemigationPermitDetailedDto.ChemigationPermitAnnualRecord.DateReceived;
                //chemigationPermitAnnualRecordUpsertDto.DatePaid = chemigationPermitDetailedDto.ChemigationPermitAnnualRecord.DatePaid;
            }
            // foreach chemigationPermitDetailedDto, create an annual record
            // use the InitializeModel from the typescript in add-annual-record; we need to create them as ChemigationPermitAnnualRecordUpsertDto so we can reuse CreateAnnualRecord
            // make saving total applied nullable
            // return count created, which is just chemigationPermitDetailedDtos.Count

            return 1;
        }
    }

}