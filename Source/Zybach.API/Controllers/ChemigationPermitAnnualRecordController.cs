using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Drawing.Charts;
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
        public ActionResult<List<ChemigationPermitAnnualRecordDto>> GetChemigationPermitAnnualRecordsByID([FromRoute] int chemigationPermitID)
        {
            var chemigationPermitAnnualRecords = ChemigationPermitAnnualRecord.GetChemigationPermitAnnualRecordsByID(_dbContext, chemigationPermitID);
            return Ok(chemigationPermitAnnualRecords);
        }

        [HttpPost("/api/chemigationPermitAnnualRecords")]
        public ActionResult<ChemigationPermitAnnualRecordDto> CreateChemigationPermitAnnualRecord([FromBody] ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto)
        {
            // TODO: figure out validation

            var chemigationPermitAnnualRecordDto =
                ChemigationPermitAnnualRecord.CreateAnnualRecord(chemigationPermitAnnualRecordUpsertDto);
            return Ok(chemigationPermitAnnualRecordDto);
        }

    }

}