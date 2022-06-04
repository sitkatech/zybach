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

        [HttpGet("/chemigationInspections/inspectionTypes")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationInspectionTypeDto>> GetChemigationInspectionTypes()
        {
            var chemigationInspectionTypeDtos = ChemigationInspectionType.AllAsDto.OrderBy(x => x.ChemigationInspectionTypeDisplayName);
            return Ok(chemigationInspectionTypeDtos);
        }

        [HttpGet("/chemigationInspections/inspectionStatuses")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationInspectionStatusDto>> GetChemigationInspectionStatuses()
        {
            var chemigationInspectionStatusDtos = ChemigationInspectionStatus.AllAsDto.OrderBy(x => x.ChemigationInspectionStatusDisplayName);
            return Ok(chemigationInspectionStatusDtos);
        }

        [HttpGet("/chemigationInspections/failureReasons")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationInspectionFailureReasonDto>> GetChemigationInspectionFailureReasons()
        {
            var chemigationInspectionFailureReasonDtos = ChemigationInspectionFailureReasons.ListAsDto(_dbContext);
            return Ok(chemigationInspectionFailureReasonDtos);
        }

        [HttpGet("/tillageTypes")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<TillageDto>> GetTillageTypes()
        {
            var tillageTypeDtos = Tillages.ListAsDto(_dbContext);
            return Ok(tillageTypeDtos);
        }

        [HttpGet("/cropTypes")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<CropTypeDto>> GetCropTypes()
        {
            var cropTypeDtos = CropTypes.ListAsDto(_dbContext);
            return Ok(cropTypeDtos);
        }

        [HttpGet("/chemigationInspections/mainlineCheckValves")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationMainlineCheckValveDto>> GetMainlineCheckValves()
        {
            var mainlineCheckValveDtos = ChemigationMainlineCheckValves.ListAsDto(_dbContext);
            return Ok(mainlineCheckValveDtos);
        }

        [HttpGet("/chemigationInspections/lowPressureValves")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationLowPressureValveDto>> GetLowPressureValves()
        {
            var lowPressureValveDtos =
                ChemigationLowPressureValve.AllAsDto.OrderBy(x => x.ChemigationLowPressureValveDisplayName);
            return Ok(lowPressureValveDtos);
        }

        [HttpGet("/chemigationInspections/injectionValves")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationInjectionValveDto>> GetChemigationInjectionValves()
        {
            var injectionValveDtos = ChemigationInjectionValves.ListAsDto(_dbContext);
            return Ok(injectionValveDtos);
        }

        [HttpGet("/chemigationInspections/interlockTypes")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemigationInterlockTypeDto>> GetInterlockTypes()
        {
            var interlockTypeDtos = ChemigationInterlockType.AllAsDto.OrderBy(x => x.ChemigationInterlockTypeDisplayName);
            return Ok(interlockTypeDtos);
        }

        [HttpGet("/chemigationInspections")]
        [ZybachViewFeature]
        public ActionResult<List<ChemigationInspectionSimpleDto>> GetAllChemigationInspections()
        {
            var chemigationInspections =
                ChemigationInspections.ListAsDto(_dbContext);
            return Ok(chemigationInspections);
        }

        [HttpGet("/chemigationInspections/{chemigationInspectionID}")]
        [ZybachEditFeature]
        public ActionResult<ChemigationInspectionSimpleDto> GetChemigationInspectionByID([FromRoute] int chemigationInspectionID)
        {
            var chemigationInspection = ChemigationInspections.GetChemigationInspectionSimpleDtoByID(_dbContext, chemigationInspectionID);

            if (ThrowNotFound(chemigationInspection, "ChemigationInspection", chemigationInspectionID, out var actionResult))
            {
                return actionResult;
            }

            return chemigationInspection;
        }

        [HttpGet("/chemigationPermits/{chemigationPermitNumber}/latestChemigationInspection")]
        [ZybachViewFeature]
        public ActionResult<ChemigationInspectionSimpleDto> GetLatestChemigationInspectionByPermitNumber(
            [FromRoute] int chemigationPermitNumber)
        {
            var chemigationInspection = ChemigationInspections.GetLatestChemigationInspectionByPermitNumber(_dbContext, chemigationPermitNumber);

            return chemigationInspection;
        }

        [HttpPost("/chemigationPermits/annualRecords/{chemigationPermitAnnualRecordID}/createInspection")]
        [ZybachEditFeature]
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

            var chemigationInspection = ChemigationInspections.CreateChemigationInspection(_dbContext,
                chemigationInspectionUpsertDto);

            return Ok(chemigationInspection);
        }

        [HttpPut("/chemigationInspections/{chemigationInspectionID}")]
        [ZybachEditFeature]
        public ActionResult
            UpdateChemigationInspectionByAnnualRecordIDAndInspectionID([FromRoute] int chemigationInspectionID, [FromBody] ChemigationInspectionUpsertDto chemigationInspectionUpsertDto)
        {
            var chemigationInspection = _dbContext.ChemigationInspections.SingleOrDefault(x => x.ChemigationInspectionID == chemigationInspectionID);
            if (ThrowNotFound(chemigationInspection, "ChemigationInspection",
                chemigationInspectionID, out var actionResult))
            {
                return actionResult;
            }

            chemigationInspection.ChemigationInspectionStatusID =
                chemigationInspectionUpsertDto.ChemigationInspectionStatusID;
            chemigationInspection.ChemigationInspectionFailureReasonID =
                chemigationInspectionUpsertDto.ChemigationInspectionFailureReasonID;
            chemigationInspection.ChemigationInspectionTypeID =
                chemigationInspectionUpsertDto.ChemigationInspectionTypeID;
            chemigationInspection.InspectionDate = chemigationInspectionUpsertDto.InspectionDate?.AddHours(8);
            chemigationInspection.InspectorUserID = chemigationInspectionUpsertDto.InspectorUserID;
            chemigationInspection.ChemigationMainlineCheckValveID =
                chemigationInspectionUpsertDto.ChemigationMainlineCheckValveID;
            chemigationInspection.ChemigationLowPressureValveID =
                chemigationInspectionUpsertDto.ChemigationLowPressureValveID;
            chemigationInspection.ChemigationInjectionValveID =
                chemigationInspectionUpsertDto.ChemigationInjectionValveID;
            chemigationInspection.ChemigationInterlockTypeID =
                chemigationInspectionUpsertDto.ChemigationInterlockTypeID;
            chemigationInspection.HasVacuumReliefValve = chemigationInspectionUpsertDto.HasVacuumReliefValve;
            chemigationInspection.HasInspectionPort = chemigationInspectionUpsertDto.HasInspectionPort;
            chemigationInspection.TillageID = chemigationInspectionUpsertDto.TillageID;
            chemigationInspection.CropTypeID = chemigationInspectionUpsertDto.CropTypeID;
            chemigationInspection.InspectionNotes = chemigationInspectionUpsertDto.InspectionNotes;
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("/chemigationInspections/{chemigationInspectionID}")]
        [ZybachEditFeature]
        public ActionResult DeleteChemigationInspectionByID([FromRoute] int chemigationInspectionID)
        {
            var chemigationInspection = _dbContext.ChemigationInspections.SingleOrDefault(x => x.ChemigationInspectionID == chemigationInspectionID);
            if (ThrowNotFound(chemigationInspection, "ChemigationInspection",
                chemigationInspectionID, out var actionResult))
            {
                return actionResult;
            }

            _dbContext.ChemigationInspections.Remove(chemigationInspection);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}