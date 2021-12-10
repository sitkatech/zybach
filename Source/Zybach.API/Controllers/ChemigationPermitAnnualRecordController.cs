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

            var mostRecentYearWithAnnualRecord =
                ChemigationPermitAnnualRecord.GetYearOfMostRecentChemigationPermitAnnualRecordByPermitID(_dbContext,
                    chemigationPermitID);

            chemigationPermitAnnualRecordUpsertDto.NDEEAmount = chemigationPermitAnnualRecordUpsertDto.RecordYear - mostRecentYearWithAnnualRecord == 1 ? 
                ChemigationPermitAnnualRecord.NDEEAmountEnum.Renewal : ChemigationPermitAnnualRecord.NDEEAmountEnum.New;

            var chemigationPermitAnnualRecordDto = ChemigationPermitAnnualRecord.CreateAnnualRecord(_dbContext, chemigationPermitAnnualRecordUpsertDto, chemigationPermitID);

            return Ok(chemigationPermitAnnualRecordDto);
        }


        [HttpPost("/api/chemigationPermits/annualRecordsBulkCreate/{recordYear}")]
        [AdminFeature]
        public ActionResult<int> BulkCreateChemigationPermitAnnualRecords([FromRoute] int recordYear)
        {
            var chemigationPermitDetailedDtos = ChemigationPermits.ListWithLatestAnnualRecordAsDto(_dbContext)
                .Where(x => x.ChemigationPermitStatus.ChemigationPermitStatusID == (int)ChemigationPermitStatus.ChemigationPermitStatusEnum.Active &&
                            x.LatestAnnualRecord.RecordYear == recordYear - 1)
                .ToList();

            foreach (var chemigationPermitDetailedDto in chemigationPermitDetailedDtos)
            {
                var applicatorsUpsert = MapLatestAnnualRecordApplicatorsToApplicatorUpsertDtoList(chemigationPermitDetailedDto);

                var chemicalFormulationsUpsert = MapLatestAnnualRecordChemicalFormulationsToChemicalFormulationUpsertDtoList(chemigationPermitDetailedDto);

                var chemigationPermitAnnualRecordUpsert = MapLatestChemigationPermitAnnualRecordToUpsertDto(recordYear, chemigationPermitDetailedDto, applicatorsUpsert, chemicalFormulationsUpsert);

                var chemigationPermitAnnualRecord = ChemigationPermitAnnualRecord.CreateAnnualRecordImpl(_dbContext, chemigationPermitAnnualRecordUpsert,
                    chemigationPermitDetailedDto.ChemigationPermitID);
            }

            return chemigationPermitDetailedDtos.Count();
        }

        private static ChemigationPermitAnnualRecordUpsertDto MapLatestChemigationPermitAnnualRecordToUpsertDto(int recordYear,
            ChemigationPermitDetailedDto chemigationPermitDetailedDto, List<ChemigationPermitAnnualRecordApplicatorUpsertDto> applicatorsUpsert, List<ChemigationPermitAnnualRecordChemicalFormulationUpsertDto> chemicalFormulationsUpsert)
        {
            var chemigationPermitAnnualRecordUpsert = new ChemigationPermitAnnualRecordUpsertDto()
            {
                ChemigationPermitAnnualRecordStatusID = (int)ChemigationPermitAnnualRecordStatus
                    .ChemigationPermitAnnualRecordStatusEnum.PendingPayment,
                ApplicantName = chemigationPermitDetailedDto.LatestAnnualRecord.ApplicantName,
                PivotName = chemigationPermitDetailedDto.LatestAnnualRecord.PivotName,
                RecordYear = recordYear,
                ChemigationInjectionUnitTypeID =
                    chemigationPermitDetailedDto.LatestAnnualRecord.ChemigationInjectionUnitTypeID,
                ApplicantPhone = chemigationPermitDetailedDto.LatestAnnualRecord.ApplicantPhone,
                ApplicantMobilePhone = chemigationPermitDetailedDto.LatestAnnualRecord.ApplicantMobilePhone,
                ApplicantEmail = chemigationPermitDetailedDto.LatestAnnualRecord.ApplicantEmail,
                ApplicantMailingAddress = chemigationPermitDetailedDto.LatestAnnualRecord.ApplicantMailingAddress,
                ApplicantCity = chemigationPermitDetailedDto.LatestAnnualRecord.ApplicantCity,
                ApplicantState = chemigationPermitDetailedDto.LatestAnnualRecord.ApplicantState,
                ApplicantZipCode = chemigationPermitDetailedDto.LatestAnnualRecord.ApplicantZipCode,
                NDEEAmount = ChemigationPermitAnnualRecord.NDEEAmountEnum.Renewal,
                Applicators = applicatorsUpsert,
                ChemicalFormulations = chemicalFormulationsUpsert
            };
            return chemigationPermitAnnualRecordUpsert;
        }

        private static List<ChemigationPermitAnnualRecordChemicalFormulationUpsertDto> MapLatestAnnualRecordChemicalFormulationsToChemicalFormulationUpsertDtoList(
            ChemigationPermitDetailedDto chemigationPermitDetailedDto)
        {
            var chemicalFormulationsUpsert = new List<ChemigationPermitAnnualRecordChemicalFormulationUpsertDto>();
            foreach (var chemicalFormulation in chemigationPermitDetailedDto.LatestAnnualRecord.ChemicalFormulations)
            {
                var chemicalFormulationUpsert = new ChemigationPermitAnnualRecordChemicalFormulationUpsertDto
                {
                    ChemicalFormulationID = chemicalFormulation.ChemicalFormulationID,
                    ChemicalUnitID = chemicalFormulation.ChemicalUnitID,
                    TotalApplied = null,
                    AcresTreated = chemicalFormulation.AcresTreated
                };

                chemicalFormulationsUpsert.Add(chemicalFormulationUpsert);
            }

            return chemicalFormulationsUpsert;
        }

        private static List<ChemigationPermitAnnualRecordApplicatorUpsertDto> MapLatestAnnualRecordApplicatorsToApplicatorUpsertDtoList(
            ChemigationPermitDetailedDto chemigationPermitDetailedDto)
        {
            var applicatorsUpsert = new List<ChemigationPermitAnnualRecordApplicatorUpsertDto>();
            foreach (var applicator in chemigationPermitDetailedDto.LatestAnnualRecord.Applicators)
            {
                var applicatorUpsert = new ChemigationPermitAnnualRecordApplicatorUpsertDto
                {
                    ApplicatorName = applicator.ApplicatorName,
                    CertificationNumber = applicator.CertificationNumber,
                    ExpirationYear = applicator.ExpirationYear,
                    HomePhone = applicator.HomePhone,
                    MobilePhone = applicator.MobilePhone
                };

                applicatorsUpsert.Add(applicatorUpsert);
            }

            return applicatorsUpsert;
        }
    }

}