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
    public class ChemigationInspectionController : SitkaController<ChemigationPermitAnnualRecordController>
    {
        public ChemigationInspectionController(ZybachDbContext dbContext, ILogger<ChemigationPermitAnnualRecordController> logger,
            KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext,
            logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/api/chemigationPermits/annualRecords/chemigationInspections/inspectionTypes")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationInspectionTypeDto>> GetChemigationInspectionTypes()
        {
            var chemigationInspectionTypeDtos = ChemigationInspectionType.List(_dbContext);
            return Ok(chemigationInspectionTypeDtos);
        }

        [HttpGet("/api/chemigationPermits/annualRecords/chemigationInspections/inspectionStatuses")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationInspectionStatusDto>> GetChemigationInspectionStatuses()
        {
            var chemigationInspectionStatusDtos = ChemigationInspectionStatus.List(_dbContext);
            return Ok(chemigationInspectionStatusDtos);
        }

        [HttpGet("/api/chemigationPermits/annualRecords/chemigationInspections/failureReasons")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationInspectionFailureReasonDto>> GetChemigationInspectionFailureReasons()
        {
            var chemigationInspectionFailureReasonDtos = ChemigationInspectionFailureReason.List(_dbContext);
            return Ok(chemigationInspectionFailureReasonDtos);
        }

        [HttpGet("/api/tillageTypes")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<TillageDto>> GetTillageTypes()
        {
            var tillageTypeDtos = Tillage.List(_dbContext);
            return Ok(tillageTypeDtos);
        }

        [HttpGet("/api/cropTypes")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<CropTypeDto>> GetCropTypes()
        {
            var cropTypeDtos = CropType.List(_dbContext);
            return Ok(cropTypeDtos);
        }

        [HttpGet("/api/chemigationPermits/annualRecords/chemigationInspections/mainlineCheckValves")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationMainlineCheckValveDto>> GetMainlineCheckValves()
        {
            var mainlineCheckValveDtos = ChemigationMainlineCheckValve.List(_dbContext);
            return Ok(mainlineCheckValveDtos);
        }

        [HttpGet("/api/chemigationPermits/annualRecords/chemigationInspections/lowPressureValves")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationLowPressureValveDto>> GetLowPressureValves()
        {
            var lowPressureValveDtos = ChemigationLowPressureValve.List(_dbContext);
            return Ok(lowPressureValveDtos);
        }

        [HttpGet("/api/chemigationPermits/annualRecords/chemigationInspections/injectionValves")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationInjectionValveDto>> GetChemigationInjectionValves()
        {
            var injectionValveDtos = ChemigationInjectionValve.List(_dbContext);
            return Ok(injectionValveDtos);
        }

        [HttpGet("/api/chemigationInspections")]
        [ZybachViewFeature]
        public ActionResult<List<ChemigationInspectionSimpleDto>> GetAllChemigationInspections()
        {
            var chemigationInspections =
                ChemigationInspection.List(_dbContext);
            return Ok(chemigationInspections);
        }


        [HttpGet("/api/chemigationInspections/{chemigationInspectionID}")]
        [AdminFeature]
        public ActionResult<ChemigationInspectionSimpleDto> GetChemigationInspectionByID([FromRoute] int chemigationInspectionID)
        {
            var chemigationInspection = ChemigationInspection.GetChemigationInspectionSimpleDtoByID(_dbContext, chemigationInspectionID);

            if (ThrowNotFound(chemigationInspection, "ChemigationInspection", chemigationInspectionID, out var actionResult))
            {
                return actionResult;
            }

            return chemigationInspection;
        }

        [HttpPost("/api/chemigationPermits/annualRecords/{chemigationPermitAnnualRecordID}/createInspection")]
        [AdminFeature]
        public ActionResult<ChemigationInspectionSimpleDto>
            CreateChemigationInspectionByAnnualRecordID([FromRoute] int chemigationPermitAnnualRecordID,
                [FromBody] ChemigationInspectionUpsertDto chemigationInspectionUpsertDto)
        {
            var chemigationPermitAnnualRecord = _dbContext.ChemigationPermitAnnualRecords.SingleOrDefault(x =>
                x.ChemigationPermitAnnualRecordID == chemigationPermitAnnualRecordID);

            if (ThrowNotFound(chemigationPermitAnnualRecord, "ChemigationPermitAnnualRecord",
                chemigationPermitAnnualRecordID, out var actionResult))
            {
                return actionResult;
            }

            var chemigationInspection = ChemigationInspection.CreateChemigationInspection(_dbContext,
                chemigationInspectionUpsertDto);

            return Ok(chemigationInspection);
        }

        [HttpPut("/api/chemigationPermits/annualRecords/{chemigationPermitAnnualRecordID}/chemigationInspections/{chemigationInspectionID}")]
        [AdminFeature]
        public ActionResult<ChemigationInspectionSimpleDto>
            UpdateChemigationInspectionByAnnualRecordIDAndInspectionID([FromRoute] int chemigationPermitAnnualRecordID, [FromRoute] int chemigationInspectionID,
                [FromBody] ChemigationInspectionUpsertDto chemigationInspectionUpsertDto)
        {
            var chemigationPermitAnnualRecord = _dbContext.ChemigationPermitAnnualRecords.SingleOrDefault(x =>
                x.ChemigationPermitAnnualRecordID == chemigationPermitAnnualRecordID);

            if (ThrowNotFound(chemigationPermitAnnualRecord, "ChemigationPermitAnnualRecord",
                chemigationPermitAnnualRecordID, out var actionResult))
            {
                return actionResult;
            }

            var chemigationInspection = ChemigationInspection.UpdateChemigationInspectionByID(_dbContext,
                chemigationInspectionID, chemigationInspectionUpsertDto);

            return Ok(chemigationInspection);
        }

        [HttpDelete("/api/chemigationPermits/annualRecords/{chemigationPermitAnnualRecordID}/chemigationInspections/{chemigationInspectionID}")]
        [AdminFeature]
        public ActionResult DeleteChemigationInspectionByID([FromRoute] int chemigationPermitAnnualRecordID, [FromRoute] int chemigationInspectionID)
        {
            var chemigationInspection = ChemigationInspection.GetChemigationInspectionSimpleDtoByID(_dbContext, chemigationInspectionID);

            if (ThrowNotFound(chemigationInspection, "ChemigationInspection", chemigationInspectionID, out var actionResult))
            {
                return actionResult;
            }

            ChemigationInspection.DeleteByInspectionID(_dbContext, chemigationInspectionID);

            return Ok();
        }

    }
}